using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerEngine
{
    public class Table
    {
        public event Action DealFinishedEvent;
        public event Action<IEnumerable<Player>> ShowDownEvent;
        public event Action<IEnumerable<WonPotInfo>> WinnersGotPotEvent;
        public readonly int StartStack;
        public readonly int MaxPlayersAmount;
        public bool IsDeal { get; private set; }
        public int CurrentMaxBetSize => _dealer.CurrentMaxBetSize;
        public int SmallBlindSize => _dealer.SmallBlindSize;
        public int BigBlindSize => _dealer.BigBlindSize;
        public int PotSize => _dealer.GetPotSize();
        public int PlayersAmount => _playersCircle.Amount;
        public int SeatsAmount => _playersCircle.SeatsAmount;
        public string CurrentRound => _currentRound != null ? _currentRound.Name : throw new InvalidOperationException("Deal hasn't started yet");
        private Round _currentRound;
        private Dealer _dealer;
        private PlayersSittingInCircle _playersCircle;
        private MoveOrder _moveOrderAtRound;

        public Table(int maxPlayersAmount, int startStack, int smallBlindSize)
        {
            MaxPlayersAmount = maxPlayersAmount;
            StartStack = startStack;
            IsDeal = false;
            _dealer = new Dealer(smallBlindSize);
            _currentRound = new Preflop();
            _playersCircle = new PlayersSittingInCircle(MaxPlayersAmount);
            _moveOrderAtRound = new MoveOrder(_playersCircle);
            _moveOrderAtRound.PlayersRanOutEvent += ContinueOrStartNewRound;
            _moveOrderAtRound.EverybodyMadeAllInEvent += SkipRoundsToShowDown;
            _playersCircle.PlayerMadeFoldEvent += CheckPlayersInDealAmount;
            //_playersCircle.PlayerMadeAllInEvent += CheckPlayersWithChipsAmount;
        }

        private void CheckPlayersWithChipsAmount()
        {
            var playersWithChips = _playersCircle.Where(p => p.IsInDeal && p.HasChips);
            if (playersWithChips.Count() == 0)
            {
                SkipRoundsToShowDown();
            }
            else if (playersWithChips.Count() == 1)
            {
                if (CurrentMaxBetSize <= GetBetSizeOf(playersWithChips.First()))
                {
                    SkipRoundsToShowDown();
                }
            }
        }

        private void SkipRoundsToShowDown()
        {
            while (IsDeal)
            {
                StartNewRound();
            }
        }

        private void CheckPlayersInDealAmount()
        {
            var playersInDeal = _playersCircle.Where(p => p.IsInDeal);
            if (playersInDeal.Count() == 1)
            {
                GiveAllPotTo(playersInDeal.First());
            }
        }

        private void GiveAllPotTo(Player theOnlyPlayer)
        {
            WonPotInfo award = new WonPotInfo { Winner = theOnlyPlayer, WonPotSize = PotSize };
            _dealer.GiveChipsFromPotTo(theOnlyPlayer, PotSize);
            WinnersGotPotEvent?.Invoke(new WonPotInfo[] { award });
            FinishThisDeal();
        }

        public void StartNewDeal()
        {
            //Console.WriteLine("***NEW DEAL***");
            _currentRound = new Preflop();
            _dealer.Reset();
            DealCardsToPlayers();
            _playersCircle.AssignNewButton();
            _moveOrderAtRound.Update(_currentRound);
            BetBlinds();
            IsDeal = true;
        }

        private void BetBlinds()
        {
            Player smallBlind = _playersCircle.GetSmallBlind();
            Player bigBlind = _playersCircle.GetBigBlind();
            smallBlind.BetSmallBlind();
            bigBlind.BetBigBlind();
        }

        internal void StartNewRound()
        {
            if (_currentRound is River)
            {
                ShowDown();
                FinishThisDeal();
                return;
            }
            _currentRound = _currentRound.Next;
            _moveOrderAtRound.Update(_currentRound);
            _dealer.PutNewCardsOnBoard(_currentRound);
        }

        private void FinishThisDeal()
        {
            _moveOrderAtRound.Clear();
            _currentRound = null;
            _dealer.Reset();
            IsDeal = false;
            DealFinishedEvent?.Invoke();
        }

        private void ShowDown()
        {
            IEnumerable<Player> playersInShowDown = _playersCircle.Where(player => player.IsInDeal);
            Combination maxCombination = playersInShowDown.Max(player => player.Combination);
            List<Player> winners = playersInShowDown.Where(player => player.Combination.CompareTo(maxCombination) == 0).ToList();
            var awards = new List<WonPotInfo>();
            foreach (Player winner in winners)
            {
                int wonChips = PotSize / winners.Count;
                _dealer.GiveChipsFromPotTo(winner, wonChips);
                awards.Add(new WonPotInfo { Winner = winner, WonPotSize = wonChips });

            }
            if (PotSize > 0)
            {
                _dealer.GiveChipsFromPotTo(winners.First(), PotSize);
            }
            ShowDownEvent(playersInShowDown);
            WinnersGotPotEvent?.Invoke(awards);

        }

        public int GetBetSizeOf(Player player)
        {
            return _dealer.GetBetSizeInRoundOf(player);
        }

        public Player GetPlayer(int seatNumber)
        {
            return _playersCircle.GetPlayer(seatNumber);
        }

        private void DealCardsToPlayers()
        {
            var playersWithChips = _playersCircle.Where(p => p.HasChips);
            foreach (Player p in playersWithChips)
            {
                _dealer.DealNewHandTo(p);
            }
        }

        private void ContinueOrStartNewRound()
        {
            Player firstAtRound = _moveOrderAtRound.FirstPlayerAtCurrentRound;
            _moveOrderAtRound.Update(firstAtRound);
            if (_moveOrderAtRound.IsEmpty || _moveOrderAtRound.Count == 1)
            {
                SkipRoundsToShowDown();
            }
            else if (GetBetSizeOf(_moveOrderAtRound.GetWhoseMove()) == CurrentMaxBetSize)
            {
                StartNewRound();
            }
            /*
            Player firstAtRound = _moveOrderAtRound.FirstPlayerAtCurrentRound;
            Player firstAndInDeal = _playersCircle.GetFirstInclusiveAfter(firstAtRound, player => player.IsInDeal);

            if (GetBetSizeOf(firstAndInDeal) < CurrentMaxBetSize)
            {
                _moveOrderAtRound.Update(firstAndInDeal);
            }
            else
            {
                StartNewRound();
            }
            */
        }

        public void AddPlayer(string playerName)
        {
            Player player = new Player(StartStack, _dealer, playerName);
            _playersCircle.Add(player);
        }

        internal void DeletePlayer(string playerName)
        {
            Player playerToDelete = _playersCircle.Get(playerName);
            playerToDelete.Fold();
            _playersCircle.Delete(playerName);
        }

        public Player GetWhoseMove()
        {
            if (_moveOrderAtRound.IsEmpty)
            {
                throw new InvalidOperationException("Deal hasn't started yet");
            }
            else
            {
                return _moveOrderAtRound.GetWhoseMove();
            }
        }

        public void MakeMove(Player player, Move move)
        {
            if (player == GetWhoseMove())
            {
                _dealer.CheckThatMoveIsAllowed(player, move);
                player.MakeMove(move);
            }
            else
            {
                throw new Exception("It's not a turn of this player");
            }
        }


        public void TakeBreak()
        {

        }

        public IEnumerable<Card> GetBoardCards()
        {
            return _dealer.GetBoardCards();
        }

        public IEnumerable<Player> GetPlayers()
        {
            return _playersCircle;
        }
    }
}
