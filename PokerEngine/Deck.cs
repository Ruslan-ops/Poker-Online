using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Deck
    {
        private List<Card> _cards;
        private const int SuitsAmount = 4;
        private const int RankAmount = 13;
        public int Size { get; private set; }

        public Deck()
        {
            Size = 52;
            _cards = new List<Card>(Size);
            for (int i = 1; i <= SuitsAmount; i++)
            {
                _cards.Add(new Card(Rank.Ace, (Suit)i));
                for (int j = 2; j <= RankAmount; j++)
                {
                    _cards.Add(new Card((Rank)j, (Suit)i));
                }
            }
        }

        public void RandomShuffle()
        {
            Random random = new Random();
            Card tmp;
            for (int i = 0; i < Size; i++)
            {
                int from = random.Next(Size);
                tmp = _cards[from];
                int to = random.Next(Size);
                _cards[from] = _cards[to];
                _cards[to] = tmp;
            }
        }

        public Card TakeTopCard()
        {
            int topIndex = Size - 1;
            Size--;
            return _cards[topIndex];
        }
        public void Show()
        {
            for (int i = 0; i < Size; i++)
            {
                Console.WriteLine($"{i}. {_cards[i].Rank} of {_cards[i].Suit}");
            }
        }

        public void Reset()
        {
            Size = 52;
        }
    }
}
