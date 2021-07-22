using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Dealer
    {
        public event Action PutNewCardsEvent;
        public int PlayerChips { get; private set; }
        public int CurrentMaxBetSize => _betsLogAtRound.MaxBetSizeInRound;
        public int SmallBlindSize { get; private set; }
        public int BigBlindSize => 2 * SmallBlindSize;

        private Deck _deck;
        private Board _board;
        private Pot _pot;
        private BetsLog _betsLogAtRound;
        private AllowedMovesChecker _allowedMovesUnit;

        public Dealer(int smallBlindSize)
        {
            _deck = new Deck();
            _pot = new Pot();
            _board = new Board();
            _betsLogAtRound = new BetsLog();
            _allowedMovesUnit = new AllowedMovesChecker(_betsLogAtRound);
            SmallBlindSize = smallBlindSize;
        }

        private void ShuffleDeck()
        {
            _deck.RandomShuffle();
        }

        public Hand GetHand()
        {
            return new Hand(_deck.TakeTopCard(), _deck.TakeTopCard());
        }

        public IEnumerable<Card> GetBoardCards()
        {
            return _board;
        }

        public Card GetCard()
        {
            return _deck.TakeTopCard();
        }

        public int GetPotSize()
        {
            return _pot.Size;
        }

        public void DealNewHandTo(Player player)
        {
            player.TakeNewHandFromDealer();
        }

        internal void Reset()
        {
            TakeBackAllCards();
            _betsLogAtRound.Clear();
        }

        private void TakeBackAllCards()
        {
            _board.Clear();
            _deck.Reset();
            ShuffleDeck();
        }

        public void PutNewCardsOnBoard(Round newRound)
        {
            for (int i = 0; i < newRound.AdditionalCardsOnBoardAmount; i++)
            {
                _board.PutNewCard(this);
            }
            _betsLogAtRound.Clear();
            PutNewCardsEvent();
        }

        public void PutChipsInPotFrom(Player player, int betSize)
        {
            if (player.Stack >= betSize)
            {
                if (betSize >= 0)
                {
                    PlayerChips = betSize;
                    _betsLogAtRound.Add(player, betSize);
                    player.GiveChipsToDealer();
                    _pot.AddСhipsFrom(this);
                    PlayerChips = 0;
                }
                else
                {
                    throw new Exception("Invalid bet size");
                }
            }
            else
            {
                throw new Exception("Player doesn't have enough chips to make this bet");
            }
        }

        internal void CheckThatMoveIsAllowed(Player player, Move move)
        {
            _allowedMovesUnit.CheckValidationFor(player, move);
        }

        public void GiveChipsFromPotTo(Player player, int chips)
        {
            if (_pot.Size >= chips)
            {
                if (chips >= 0)
                {
                    PlayerChips = chips;
                    _pot.GiveChipsTo(this);
                    player.TakeChipsFromDealer();
                    PlayerChips = 0;
                }
                else
                {
                    throw new Exception("Invalid chips amount");
                }
            }
            else
            {
                throw new Exception("Stack doesn't have enough chips");
            }
        }

        public int GetBetSizeInRoundOf(Player player)
        {
            return _betsLogAtRound.GetBetSizeInRoundOf(player);
        }
    }
}
