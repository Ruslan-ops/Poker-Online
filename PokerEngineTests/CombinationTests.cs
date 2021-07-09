using NUnit.Framework;
using NUnit.Framework.Constraints;
using PokerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
namespace PokerEngineTests
{
    class CombinationTests
    {
        [Test]
        public void TwoCardsOnePair()
        {
            Card[] cards = new Card[] {new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Ace, Suit.Hearts)};
            Combination combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.OnePair, combination.Type);
            Assert.AreEqual(2, combination.Size);
        }

        [Test]
        public void TwoCardsHigh()
        {
            Card[] cards = new Card[] { new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.King, Suit.Hearts) };
            Combination combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.High, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.Seven, Suit.Diamonds));
            Assert.AreEqual(2, combination.Size);
        }

        [Test]
        public void InvalidCardsAmount()
        {
            Card[] cards1 = new Card[] { new Card(Rank.Seven, Suit.Diamonds) };
            Card[] cards2 = new Card[] { new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.King, Suit.Hearts), new Card(Rank.King, Suit.Spades) };
            Card[] cards3 = new Card[] { new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.King, Suit.Hearts), new Card(Rank.King, Suit.Spades), new Card(Rank.Queen, Suit.Diamonds) };
            Card[] cards4 = new Card[] { };

            Assert.Throws<ArgumentException>(() => Combination.FromCards(cards1));
            Assert.Throws<ArgumentException>(() => Combination.FromCards(cards2));
            Assert.Throws<ArgumentException>(() => Combination.FromCards(cards3));
            Assert.Throws<ArgumentException>(() => Combination.FromCards(cards4));
        }

        [Test]
        public void StraightOrHigh()
        {
            Card[] cards = new Card[] { new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.King, Suit.Hearts), new Card(Rank.Six, Suit.Spades), new Card(Rank.Queen, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Ace, Suit.Hearts)};
            Combination combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.High, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Ace, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Queen, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Seven, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Six, Suit.Spades));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());

            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.King, Suit.Hearts), new Card(Rank.Nine, Suit.Spades), new Card(Rank.Queen, Suit.Diamonds),
                                        new Card(Rank.Jack, Suit.Diamonds), new Card(Rank.Ace, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.Straight, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Ace, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Queen, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Jack, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Ten, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Three, Suit.Spades), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Ace, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.Straight, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Five, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.Four, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Three, Suit.Spades));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Two, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Ace, Suit.Hearts));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[] {new Card(Rank.Five, Suit.Hearts), new Card(Rank.Three, Suit.Spades), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Six, Suit.Diamonds), new Card(Rank.Ace, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.Straight, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Six, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.Five, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Four, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Three, Suit.Spades));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Two, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            for(int i = 2; i <= 10; i++)
            {
                List<Card> cardsList = new List<Card> { };
                for(int j = 0; j < 5; j++)
                {
                    if(j%2 == 0)
                    {
                        cardsList.Add(new Card((Rank)(i + j), Suit.Clubs));
                    }
                    else
                    {
                        cardsList.Add(new Card((Rank)(i + j), Suit.Diamonds));
                    }
                }
                combination = Combination.FromCards(cardsList);
                Assert.AreEqual(CombinationType.Straight, combination.Type);
                Assert.AreEqual(combination.ElementAt(0).Rank, (Rank)(i+4));
                Assert.AreEqual(combination.ElementAt(1).Rank, (Rank)(i+3));
                Assert.AreEqual(combination.ElementAt(2).Rank, (Rank)(i+2));
                Assert.AreEqual(combination.ElementAt(3).Rank, (Rank)(i+1));
                Assert.AreEqual(combination.ElementAt(4).Rank, (Rank)(i));
                Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
                Assert.AreEqual(5, combination.Size);
            }
           
        }

        [Test]
        public void StraightOrPairOrTwoPairs()
        {
            Card[] cards = new Card[] { new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.King, Suit.Hearts), new Card(Rank.Six, Suit.Spades), new Card(Rank.Queen, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Ace, Suit.Hearts)};
            Combination combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.High, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Ace, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Queen, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Seven, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Six, Suit.Spades));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());

            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Three, Suit.Spades), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.King, Suit.Hearts), new Card(Rank.Ten, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.OnePair, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Five, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Four, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Three, Suit.Spades), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Ace, Suit.Hearts), new Card(Rank.Ten, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.Straight, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Five, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.Four, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Three, Suit.Spades));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Two, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Ace, Suit.Hearts));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[]{ new Card(Rank.Five, Suit.Diamonds), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Three, Suit.Spades), new Card(Rank.Four, Suit.Clubs),
                                        new Card(Rank.Two, Suit.Spades), new Card(Rank.Six, Suit.Diamonds), new Card(Rank.Six, Suit.Diamonds)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.Straight, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Six);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Five);
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Four, Suit.Clubs));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Three, Suit.Spades));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Two, Suit.Spades));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Three, Suit.Spades), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Four, Suit.Spades), new Card(Rank.King, Suit.Hearts), new Card(Rank.Ten, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.TwoPairs, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Four);
            Assert.AreEqual(combination.ElementAt(3).Rank, Rank.Four);
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Two, Suit.Hearts), new Card(Rank.Two, Suit.Spades), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Four, Suit.Spades), new Card(Rank.King, Suit.Hearts), new Card(Rank.Ten, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.TwoPairs, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Four);
            Assert.AreEqual(combination.ElementAt(3).Rank, Rank.Four);
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Two, Suit.Hearts), new Card(Rank.Two, Suit.Spades), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Four, Suit.Spades), new Card(Rank.King, Suit.Hearts), new Card(Rank.Ten, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.TwoPairs, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Four);
            Assert.AreEqual(combination.ElementAt(3).Rank, Rank.Four);
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

        }

        [Test]
        public void FullHouseOrStraightOrThreeOfKind()
        {
            Card[] cards = new Card[] { new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.Seven, Suit.Hearts), new Card(Rank.Queen, Suit.Spades), new Card(Rank.Queen, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Seven, Suit.Diamonds)};
            Combination combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.FullHouse, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Seven);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Seven);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Seven);
            Assert.AreEqual(combination.ElementAt(3).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(4).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());

            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.Seven, Suit.Hearts), new Card(Rank.Queen, Suit.Spades), new Card(Rank.Queen, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Queen, Suit.Clubs)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.FullHouse, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(3).Rank, Rank.Seven);
            Assert.AreEqual(combination.ElementAt(4).Rank, Rank.Seven);
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());

            cards = new Card[] { new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.Seven, Suit.Hearts), new Card(Rank.Queen, Suit.Spades), new Card(Rank.Queen, Suit.Diamonds),
                                        new Card(Rank.Seven, Suit.Clubs), new Card(Rank.Queen, Suit.Clubs)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.FullHouse, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(3).Rank, Rank.Seven);
            Assert.AreEqual(combination.ElementAt(4).Rank, Rank.Seven);
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());

            cards = new Card[]{ new Card(Rank.Five, Suit.Diamonds), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Five, Suit.Spades), new Card(Rank.Four, Suit.Clubs),
                                        new Card(Rank.Two, Suit.Spades), new Card(Rank.Six, Suit.Diamonds), new Card(Rank.Three, Suit.Diamonds)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.Straight, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Six);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Five);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Four);
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Three, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Two, Suit.Spades));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Three, Suit.Spades), new Card(Rank.Ten, Suit.Clubs),
                                        new Card(Rank.Four, Suit.Spades), new Card(Rank.King, Suit.Hearts), new Card(Rank.Ten, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.ThreeOfKind, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Ten);
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.King, Suit.Hearts));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Five, Suit.Hearts));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);
        }

        [Test]
        public void FourOfKind()
        {
            Card[] cards = new Card[]{ new Card(Rank.Seven, Suit.Diamonds), new Card(Rank.Seven, Suit.Hearts), new Card(Rank.Queen, Suit.Spades), new Card(Rank.Queen, Suit.Diamonds),
                                        new Card(Rank.Seven, Suit.Clubs), new Card(Rank.Queen, Suit.Clubs),  new Card(Rank.Queen, Suit.Hearts)};
            Combination combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.FourOfKind, combination.Type);
            Assert.AreEqual(combination.ElementAt(0).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(1).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(2).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(3).Rank, Rank.Queen);
            Assert.AreEqual(combination.ElementAt(4).Rank, Rank.Seven);
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
        }

        [Test]
        public void Flush()
        {
            Card[] cards =  { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Five, Suit.Diamonds), new Card(Rank.Three, Suit.Spades), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Ten, Suit.Hearts)};
            Combination combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.Flush, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Ace, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.Ten, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Five, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Four, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Two, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
        }

        [Test]
        public void StraightFlushOrRoyalFlush()
        {
            Card[] cards =  { new Card(Rank.Ten, Suit.Diamonds), new Card(Rank.Five, Suit.Diamonds), new Card(Rank.Three, Suit.Diamonds), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Ten, Suit.Hearts)};
            Combination combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.StraightFlush, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Five, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.Four, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Three, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Two, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Ace, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());

            cards = new Card[] { new Card(Rank.Six, Suit.Diamonds), new Card(Rank.Five, Suit.Diamonds), new Card(Rank.Three, Suit.Diamonds), new Card(Rank.Four, Suit.Diamonds),
                                        new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Ten, Suit.Hearts)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.StraightFlush, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Six, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.Five, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Four, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Three, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Two, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);

            cards = new Card[] { new Card(Rank.Jack, Suit.Diamonds), new Card(Rank.King, Suit.Diamonds), new Card(Rank.Nine, Suit.Diamonds), new Card(Rank.Queen, Suit.Diamonds),
                                        new Card(Rank.Eight, Suit.Diamonds), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Ten, Suit.Diamonds)};
            combination = Combination.FromCards(cards);
            Assert.AreEqual(CombinationType.RoyalFlush, combination.Type);
            Assert.AreEqual(combination.ElementAt(0), new Card(Rank.Ace, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(1), new Card(Rank.King, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(2), new Card(Rank.Queen, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(3), new Card(Rank.Jack, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAt(4), new Card(Rank.Ten, Suit.Diamonds));
            Assert.AreEqual(combination.ElementAtOrDefault(5), new Card());
            Assert.AreEqual(5, combination.Size);
        }
    }
}
