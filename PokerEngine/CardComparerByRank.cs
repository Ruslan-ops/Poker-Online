using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class CardComparerByRank : IEqualityComparer<Card>
    {
        public bool Equals(Card x, Card y)
        {
            return x.Rank == y.Rank;
        }

        public int GetHashCode(Card card)
        {
            return card.Rank.GetHashCode();
        }
    }
}
