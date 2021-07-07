using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public enum Rank : byte //todo remove public 
    {
        //Empty = 0,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    public enum Suit : byte //todo remove public 
    {
        //Empty = 0,
        Hearts = 1,
        Diamonds,
        Clubs,
        Spades
    }
    public struct Card
    {
        //internal static readonly Card Empty = new Card(Rank.Empty, Suit.Empty);
        internal Rank Rank { get; private set; }
        internal Suit Suit { get; private set; }

        public Card(Rank rank, Suit suit) //todo make internal
        {
            Rank = rank;
            Suit = suit;
        }

        public override string ToString()
        {
            return $"{this.Rank} of {this.Suit}";
        }
        public int CompareTo(Card other)
        {
            return this.Rank.CompareTo(other.Rank);
        }

        public static bool operator >(Card c1, Card c2)
        {
            return c1.Rank > c2.Rank;
        }

        public static bool operator <(Card c1, Card c2)
        {
            return c1.Rank < c2.Rank;
        }

        public void Show()
        {
            Console.WriteLine($"{this.Rank} of {this.Suit}");
        }
    }
}
