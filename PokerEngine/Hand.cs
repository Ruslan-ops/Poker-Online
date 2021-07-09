using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public class Hand : IEnumerable<Card>
    {
        //internal static readonly Hand Empty = new Hand(Card.Empty, Card.Empty);
        internal Card FirstCard { get; private set; }
        internal Card SecondCard { get; private set; }

        public Hand(Card first, Card second)
        {
            FirstCard = first;
            SecondCard = second;
        }

        public override string ToString()
        {
            return $"{FirstCard.ToString()} || {SecondCard.ToString()}";
        }
        public void Show()
        {
            FirstCard.Show();
            SecondCard.Show();
        }

        public IEnumerator<Card> GetEnumerator()
        {
            yield return FirstCard;
            yield return SecondCard;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
