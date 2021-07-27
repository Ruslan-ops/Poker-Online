using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


[assembly: InternalsVisibleTo("PokerEngineTests")]
namespace PokerEngine
{
    public class Table
    {
        public event Action DealStartedEvent;
        public event Action DealFinishedEvent;
        public event Action NewRoundStartedEvent;
        public event Action<IEnumerable<Player>> ShowDownEvent;
        public event Action<IEnumerable<WonPotInfo>> WinnersGotPotEvent;
        public readonly int MinStartStack;
        public readonly int MaxStartStack;
        public readonly int MaxPlayersAmount;
        public bool IsDeal { get; private set; }
        public int CurrentMaxBetSize => _dealer.CurrentMaxBetSize;
        public int SmallBlindSize => _dealer.SmallBlindSize;
        public int BigBlindSize => _dealer.BigBlindSize;
        public int PotSize => _dealer.GetPotSize();
        public int PlayersAmount => _playersCircle.Amount;
        public int SeatsAmount => _playersCircle.SeatsAmount;
        public Player BigBlind => _playersCircle.GetBigBlind();
        public Player SmallBlind => _playersCircle.GetSmallBlind();
        public Player Button => _playersCircle.GetButton();
        public RoundType CurrentRound => _currentRound != null ? _currentRound.RoundType : throw new InvalidOperationException("Deal hasn't started yet");
        public IEnumerable<Seat> Seats { get; set; }
        private Round _currentRound;
        private Dealer _dealer;
        private PlayersSittingInCircle _playersCircle;
        private MoveOrder _moveOrderAtRound;

        public Table(int maxPlayersAmount, int minStartStack, int maxStartStack, int smallBlindSize)
        {
            if(maxPlayersAmount < 2 || minStartStack > maxStartStack || minStartStack <= 0 || smallBlindSize <= 0)
            {
                throw new ArgumentException("Invalid arguments for creating the table");
            }
            MaxPlayersAmount = maxPlayersAmount;
            MinStartStack = minStartStack;
            MaxStartStack = maxStartStack;
            IsDeal = false;
            _dealer = new Dealer(smallBlindSize);
            _currentRound = new Preflop();
            _playersCircle = new PlayersSittingInCircle(MaxPlayersAmount);
            _moveOrderAtRound = new MoveOrder(_playersCircle);
            _moveOrderAtRound.PlayersRanOutEvent += ContinueOrStartNewRound;
            _playersCircle.PlayerMadeFoldEvent += CheckPlayersInDealAmount;
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
            if(_playersCircle.Where(p => p.HasChips).Count() > 1)
            {
                _currentRound = new Preflop();
                _dealer.Reset();
                DealCardsToPlayers();
                _playersCircle.AssignNewButton();
                _moveOrderAtRound.Update(_currentRound);
                BetBlinds();
                IsDeal = true;
                DealStartedEvent?.Invoke();
            }
            else
            {
                throw new Exception("There are too few players to start new deal");
            }
        }

        public List<MoveAlias> GetAllowedMovesFor(Player player)
        {
            if(_playersCircle.Contains(player) && player.IsInDeal)
            {
                return _dealer.GetAllowedMovesFor(player);
            }
            throw new Exception("No such player in the deal");
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
            NewRoundStartedEvent?.Invoke();
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
            int wonChips = PotSize / winners.Count;
            foreach (Player winner in winners)
            {
                _dealer.GiveChipsFromPotTo(winner, wonChips);
                awards.Add(new WonPotInfo { Winner = winner, WonPotSize = wonChips });

            }
            if (PotSize > 0)
            {
                _dealer.GiveChipsFromPotTo(winners.First(), PotSize);
            }
            ShowDownEvent?.Invoke(playersInShowDown);
            WinnersGotPotEvent?.Invoke(awards);
        }

        public int GetBetSizeOf(Player player)
        {
            return _dealer.GetBetSizeInRoundOf(player);
        }

        public Player GetPlayer(int seatNumber)
        {
            return _playersCircle.Get(seatNumber);
        }

        public Player GetPlayer(string playerName)
        {
            return _playersCircle.Get(playerName);
        }

        public Seat GetSeatOf(string playerName)
        {
            return _playersCircle.Seats.Single(seat => seat.Player == GetPlayer(playerName));
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
            bool isEverybodyMadeAllIn = _moveOrderAtRound.IsEmpty || _moveOrderAtRound.Count == 1 && GetBetSizeOf(_moveOrderAtRound.GetWhoseMove()) == CurrentMaxBetSize;
            if (isEverybodyMadeAllIn)
            {
                SkipRoundsToShowDown();
            }
            else if (GetBetSizeOf(_moveOrderAtRound.GetWhoseMove()) == CurrentMaxBetSize)
            {
                StartNewRound();
            }
        }

        public void AddPlayer(string playerName)
        {
            AddPlayer(playerName, MinStartStack);
        }

        public void AddPlayer(string playerName, int stackSize)
        {
            if(stackSize >= MinStartStack && stackSize <= MaxStartStack)
            {
                
                Player player = new Player(stackSize, _dealer, playerName);
                _playersCircle.Add(player);
            }
            else
            {
                throw new ArgumentException($"Start stack must be from {MinStartStack} to {MaxStartStack}");
            }
        }

        public void AddPlayer(string playerName, int stackSize, int seatNumber)
        {
            if (stackSize >= MinStartStack && stackSize <= MaxStartStack)
            {
                Player player = new Player(stackSize, _dealer, playerName);
                _playersCircle.Add(player, seatNumber);
            }
            else
            {
                throw new ArgumentException($"Start stack must be from {MinStartStack} to {MaxStartStack}");
            }
        }

        public void DeletePlayer(string playerName)
        {
            Player playerToDelete = _playersCircle.Get(playerName);
            playerToDelete.Fold();
            _moveOrderAtRound.Delete(playerToDelete);
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
                move.Make(player);
                if(!_moveOrderAtRound.IsEmpty)
                {
                    _moveOrderAtRound.GiveMoveToNextPlayer();
                }
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
