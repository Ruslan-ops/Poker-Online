using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Board : IEnumerable<Card>
    {
        private const int MaxCardsAmount = 5;
        private List<Card> _cards;
        public int CardsAmount { get { return _cards.Count; } private set { } }

        public Board()
        {
            _cards = new List<Card>(0);
        }

        public void Clear()
        {
            _cards = new List<Card>(0);
        }

        public void PutNewCard(Dealer dealer)
        {
            if (CardsAmount < MaxCardsAmount)
            {
                _cards.Add(dealer.GetCard());
            }
            else
            {
                throw new Exception("Board overflow");
            }
        }

        public List<Card> ToCardsList()
        {
            return new List<Card>(_cards);
        }
        

        public IEnumerator<Card> GetEnumerator()
        {
            foreach (Card card in _cards)
            {
                yield return card;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
