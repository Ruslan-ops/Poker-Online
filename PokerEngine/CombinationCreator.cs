using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerEngine
{
    class CombinationCreator
    {
        public static Combination Create(IEnumerable<Card> cardsCollection)
        {
            List<Card> cards = cardsCollection.ToList();
            switch (cards.Count)
            {
                case 2:
                    {
                        return CreateForTwoCards(cards);
                    }
                default:
                    {
                        return CreateForMoreThanFiveCards(cards);
                    }
            }
        }
        //internal static Combination Create(Hand hand, Board board)
        //{
        //    List<Card> cards = new List<Card> { hand.FirstCard, hand.SecondCard };
        //    foreach (Card card in board)
        //    {
        //        cards.Add(card);
        //    }
        //    return Create(cards);
        //    switch (cards.Count)
        //    {
        //        case 2:
        //            {
        //                return CreateForTwoCards(cards);
        //                break;
        //            }
        //        default:
        //            {
        //                return CreateForMoreThanFiveCards(cards);
        //                break;
        //            }
        //    }

        //}

        private static Combination CreateForTwoCards(IEnumerable<Card> cards)
        {
            var duplicatesCollection = GetOrderedDuplicatesCollection(cards);
            var biggestCollection = duplicatesCollection.First();
            if (biggestCollection.Count() == 2)
            {
                return new Combination(biggestCollection, CombinationType.OnePair);
            }
            else
            {
                IEnumerable<Card> orderedCards = cards.OrderByDescending(card => card.Rank);
                return new Combination(orderedCards, CombinationType.High);
            }
        }

        private static Combination CreateForMoreThanFiveCards(IEnumerable<Card> cards)
        {
            var cardsOfSameSuit = cards.GroupBy(card => card.Suit).OrderByDescending(group => group.Count()).First(); //Max(group => group.Count());
            if (cardsOfSameSuit.Count() < 5)
            {
                return CreateForNotFlush(cards);
            }
            else
            {
                return GetFlushOrCheckMax(cards, cardsOfSameSuit);
            }
        }

        private static Combination CreateForNotFlush(IEnumerable<Card> cards)
        {
            var duplicatesCollection = GetOrderedDuplicatesCollection(cards);
            var biggestCollection = duplicatesCollection.First();
            switch (biggestCollection.Count())
            {
                case 1:
                    return GetStraightOrHigh(cards);
                case 2:
                    return GetStraightOrCheckOtherPair(cards);
                case 3:
                    return GetFullHouseOrCheckStraight(cards);
                case 4:
                    return new Combination(biggestCollection.Concat(duplicatesCollection[1]), CombinationType.FourOfKind);
                default:
                    throw new Exception("Strange error: found more than 4 same rank cards");
            }
        }

        private static Combination GetFlushOrCheckMax(IEnumerable<Card> allCards, IEnumerable<Card> cardsOfSameSuit)
        {
            List<Card> probableStraight = new List<Card>();
            if (TryFindStraight(cardsOfSameSuit, probableStraight))
            {
                return GetRoyalFlushOrStraightFlush(probableStraight);
            }
            else
            {
                return new Combination(cardsOfSameSuit.OrderByDescending(card => card.Rank), CombinationType.Flush);
            }
        }

        private static Combination GetRoyalFlushOrStraightFlush(List<Card> straightFlush)
        {
            if (straightFlush[0].Rank == Rank.Ace)
            {
                return new Combination(straightFlush, CombinationType.RoyalFlush);
            }
            else
            {
                return new Combination(straightFlush, CombinationType.StraightFlush);
            }
        }

        private static Combination GetFullHouseOrCheckStraight(IEnumerable<Card> cards)
        {
            var duplicatesCollection = GetOrderedDuplicatesCollection(cards);
            if (duplicatesCollection[1].Count() >= 2)
            {
                return new Combination(duplicatesCollection[0].Concat(duplicatesCollection[1]), CombinationType.FullHouse);
            }
            else
            {
                return GetStraightOrThreeOfKind(cards, duplicatesCollection[0].Concat(duplicatesCollection[1].Concat(duplicatesCollection[2])));
            }
        }

        private static Combination GetStraightOrThreeOfKind(IEnumerable<Card> cards, IEnumerable<Card> threeOfKind)
        {
            List<Card> probableStraight = new List<Card>();
            if (TryFindStraight(cards, probableStraight))
            {
                return new Combination(probableStraight, CombinationType.Straight);
            }
            else
            {
                return new Combination(threeOfKind, CombinationType.ThreeOfKind);
            }
        }

        private static Combination GetStraightOrHigh(IEnumerable<Card> cards)
        {
            List<Card> probableStraight = new List<Card>();
            if (TryFindStraight(cards, probableStraight))
            {
                return new Combination(probableStraight, CombinationType.Straight);
            }
            else
            {
                return new Combination(cards.OrderByDescending(card => card.Rank), CombinationType.High);
            }
        }

        private static bool TryFindStraight(IEnumerable<Card> cards, List<Card> probableStraight)
        {
            var orderedUniqueCards = cards.OrderByDescending(card => card.Rank).Distinct(new CardComparerByRank()).ToList();
            if (orderedUniqueCards.Count < 5)
            {
                return false;
            }
            Card maxInOrdered = orderedUniqueCards[0];
            for (int i = 0; i < orderedUniqueCards.Count - 2; i++)
            {
                maxInOrdered = orderedUniqueCards[i];
                int nextRank = (int)maxInOrdered.Rank - 1;
                Card nextInOrdered = orderedUniqueCards[i + 1];
                probableStraight.Add(maxInOrdered);
                while (nextInOrdered.Rank == (Rank)nextRank)
                {
                    probableStraight.Add(nextInOrdered);
                    i++;
                    if (i + 1 >= orderedUniqueCards.Count)
                    {
                        break;
                    }
                    nextInOrdered = orderedUniqueCards[i + 1];
                    nextRank--;
                }
                if (probableStraight.Last().Rank == Rank.Two && orderedUniqueCards[0].Rank == Rank.Ace)
                {
                    probableStraight.Add(orderedUniqueCards[0]);
                }
                if (probableStraight.Count > 4)
                {
                    return true;
                }
                probableStraight.Clear();
            }
            return false;

        }

        private static Combination GetStraightOrCheckOtherPair(IEnumerable<Card> cards)
        {
            List<Card> probableStraight = new List<Card>();
            if (TryFindStraight(cards, probableStraight))
            {
                return new Combination(probableStraight, CombinationType.Straight);
            }
            else
            {
                return GetOnePairOrTwoPairs(cards);
            }
        }

        private static Combination GetOnePairOrTwoPairs(IEnumerable<Card> cards)
        {
            var duplicatesCollection = GetOrderedDuplicatesCollection(cards);
            if (duplicatesCollection[1].Count() == 2)
            {
                return CreateTwoPairs(duplicatesCollection[0], duplicatesCollection[1], duplicatesCollection[2]);
            }
            else
            {
                IEnumerable<Card> exceptOfPair = cards.Where(card => card.Rank != duplicatesCollection[0].Key).OrderByDescending(card => card.Rank);
                return new Combination(duplicatesCollection[0].Concat(exceptOfPair), CombinationType.OnePair);
            }
        }

        private static Combination CreateTwoPairs(IEnumerable<Card> firstPair, IEnumerable<Card> secondPair, IEnumerable<Card> rest)
        {
            return new Combination(firstPair.Concat(secondPair).Concat(rest), CombinationType.TwoPairs);
        }

        private static List<IGrouping<Rank, Card>> GetOrderedDuplicatesCollection(IEnumerable<Card> cards)
        {
            return cards
                        .GroupBy(card => card.Rank)
                        .OrderByDescending(group => group.Count())
                        .ThenByDescending(group => group.Key)
                        .ToList();
        }
    }
}
