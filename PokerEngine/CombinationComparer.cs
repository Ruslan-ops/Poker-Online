using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerEngine
{
    static class CombinationComparer
    {
        public static int Compare(Combination x, Combination y)
        {
            if (x.Type > y.Type)
            {
                return 1;
            }
            else if (x.Type < y.Type)
            {
                return -1;
            }
            else
            {
                return CompareCombinationsOfTheSameType(x, y);
            }
        }

        private static int CompareCombinationsOfTheSameType(Combination x, Combination y)
        {
            foreach ((Card cardX, Card cardY) in x.Zip(y, (i1, i2) => (i1, i2)))
            {
                if (cardX.CompareTo(cardY) != 0)
                {
                    return cardX.CompareTo(cardY);
                }
            }
            return 0;
        }
    }
}
