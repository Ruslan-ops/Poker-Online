using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerEngine
{
    public class Player
    {
        public event Action MadeFoldEvent;
        public string Name { get; private set; }
        public Hand Hand { get; private set; }
        public Combination Combination { get; private set; }
        private Dealer _dealer;
        public int Stack { get; private set; }
        public bool HasChips => Stack > 0;
        public bool IsInDeal { get; private set; }

        internal void Check()
        {

        }

        internal Player(int stack, Dealer dealer, string name)
        {
            Hand = null;// Hand.Empty;
            _dealer = dealer;
            Name = name;
            Stack = stack;
            IsInDeal = false;
        }

        internal void TakeNewHandFromDealer()
        {
            Hand = _dealer.GetHand();
            IsInDeal = true;
            Combination = CombinationCreator.Create(Hand);
            _dealer.PutNewCardsEvent += UpdateCombination;
        }
        internal void TakeNewHandFromDealer(Hand hand)
        {
            Hand = hand;
            IsInDeal = true;
            Combination = CombinationCreator.Create(Hand);
            _dealer.PutNewCardsEvent += UpdateCombination;
        }

        private void UpdateCombination()
        {
            if (IsInDeal)
            {
                Combination = CombinationCreator.Create(Hand.Concat(_dealer.GetBoardCards()));
            }
        }

        internal void Fold()
        {
            Hand = null;// Hand.Empty;
            _dealer.PutNewCardsEvent -= UpdateCombination;
            IsInDeal = false;
            MadeFoldEvent?.Invoke();
        }


        private void CheckNonNegative(int chipsAmount)
        {
            if (_dealer.PlayerChips < 0)
            {
                throw new Exception("Chips amount must be positive number");
            }
        }


        internal void GiveChipsToDealer()
        {
            CheckNonNegative(_dealer.PlayerChips);
            Stack -= _dealer.PlayerChips;
            /*if (Stack == 0)
            {
                MadeAllInEvent?.Invoke();
            }*/
        }

        internal void TakeChipsFromDealer()
        {
            CheckNonNegative(_dealer.PlayerChips);
            Stack += _dealer.PlayerChips;
        }

        internal void Bet(int betSize)
        {
            if (betSize >= _dealer.BigBlindSize)
            {
                _dealer.PutChipsInPotFrom(this, betSize);
            }
            else
            {
                throw new Exception("Too small bet size");
            }
        }

        internal void BetSmallBlind()
        {
            _dealer.PutChipsInPotFrom(this, _dealer.SmallBlindSize);
        }

        internal void BetBigBlind()
        {
            _dealer.PutChipsInPotFrom(this, _dealer.BigBlindSize);
        }


        internal void AllIn()
        {
            _dealer.PutChipsInPotFrom(this, Stack);
        }

        internal void Call()
        {
            int callSize = _dealer.CurrentMaxBetSize - _dealer.GetBetSizeInRoundOf(this);
            _dealer.PutChipsInPotFrom(this, callSize);
        }

        internal void Raise(double coef)
        {
            int betSize = (int)(coef * _dealer.CurrentMaxBetSize);
            _dealer.PutChipsInPotFrom(this, betSize);

        }


        /*internal void MakeMove(Move move)
        {
            move.Make(this);
            MadeMoveEvent?.Invoke();
        }*/



        public void ShowHand()
        {
            Hand.Show();
        }
    }
}
