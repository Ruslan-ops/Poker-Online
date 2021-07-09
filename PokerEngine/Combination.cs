using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerEngine
{
    public enum CombinationType : byte
    {
        High,
        OnePair,
        TwoPairs,
        ThreeOfKind,
        Straight,
        Flush,
        FullHouse,
        FourOfKind,
        StraightFlush,
        RoyalFlush
    }
    public class Combination : IEnumerable<Card>, IComparable
    {
        public int Size => _evaluetedCards.Count;
        public CombinationType Type { get; private set; }
        private List<Card> _evaluetedCards;

        internal Combination(IEnumerable<Card> valueableCards, CombinationType type)
        {
            _evaluetedCards = valueableCards.Take(5).ToList();
            Type = type;
            if (_evaluetedCards.Count != 2 && _evaluetedCards.Count < 5)
            {
                throw new ArgumentException("Cards amount in combination must be ethier 2 or bigger than 4");
            }
        }

        public static Combination FromCards(IEnumerable<Card> cards)
        {
            if(cards.Count() == 2 || cards.Count() >= 5)
            {
                return CombinationCreator.Create(cards);
            }
            else
            {
                throw new ArgumentException("Cards amount in combination must be ethier 2 or bigger than 4");
            }
        }

        public int CompareTo(object other)
        {
            if (other is Combination comb)
            {
                return CombinationComparer.Compare(this, comb);
            }
            else
            {
                throw new ArgumentException("You try compare a combination with not combination");
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{this.Type} | ");
            foreach (Card card in this)
            {
                sb.Append(card + ", ");
            }
            return sb.ToString();
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _evaluetedCards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
