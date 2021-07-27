using NUnit.Framework;
using NUnit.Framework.Constraints;
using PokerEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerEngineTests
{
    class DealTests
    {
        Table table;
        Player first;
        Player second;
        Player third;

        [SetUp]
        public void Setup()
        {
            table = new Table(5, 10000, 10000, 100);
            table.AddPlayer("player_1", 10000, 1);
            table.AddPlayer("player_2", 10000, 3);
            table.AddPlayer("player_3", 10000, 4);
            first = table.GetPlayer(1);
            second = table.GetPlayer(3);
            third = table.GetPlayer(4);
            table.StartNewDeal();
            first.TakeNewHandFromDealer(new Hand(new Card(Rank.Five, Suit.Clubs), new Card(Rank.Four, Suit.Clubs)));
            second.TakeNewHandFromDealer(new Hand(new Card(Rank.Five, Suit.Clubs), new Card(Rank.Four, Suit.Clubs)));
            third.TakeNewHandFromDealer(new Hand(new Card(Rank.Five, Suit.Clubs), new Card(Rank.Four, Suit.Clubs)));

        }

        [Test]
        public void EverybodyFoldAtPreflop()
        {
            Assert.IsTrue(table.IsDeal);
            Assert.AreEqual(table.GetPlayers().ElementAt(0), second);
            Assert.AreEqual(table.GetPlayers().ElementAt(1), third);
            Assert.AreEqual(table.GetPlayers().ElementAt(2), first);
            Assert.AreEqual(table.GetWhoseMove(), second);
            Assert.AreEqual(table.PotSize, table.SmallBlindSize + table.BigBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.BigBlind), table.BigBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.SmallBlind), table.SmallBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.Button), 0);
            Player actor = table.GetWhoseMove();
            Assert.AreEqual(actor.Stack, 10000);
            table.MakeMove(actor, new Fold());
            Assert.AreEqual(table.GetPlayers().Where(p => p.IsInDeal).Count(), 2);
            Assert.AreEqual(table.GetWhoseMove(), third);
            actor = table.GetWhoseMove();
            Assert.Throws<Exception>(() => table.MakeMove(second, new Fold()));
            Assert.Throws<Exception>(() => table.MakeMove(first, new Fold()));
            table.MakeMove(actor, new Fold());
            Assert.IsFalse(table.IsDeal);
            Assert.AreEqual(first.Stack, 10100);
            Assert.AreEqual(second.Stack, 10000);
            Assert.AreEqual(third.Stack, 9900);
        }


        [Test]
        public void EverybodyAllinAtPreflop()
        {
            table.ShowDownEvent += (players) =>
            {
                Assert.AreEqual(players.Count(), 3);
            };

            table.WinnersGotPotEvent += (awards) =>
            {
                Assert.AreEqual(awards.Count(), 3);
                var winners = awards.Select(award => award.Winner);
                foreach (var winner in winners)
                {
                    Assert.IsTrue(winner.Combination.CompareTo(winners.First().Combination) == 0);
                }
                foreach (var award in awards)
                {
                    Assert.AreEqual(award.WonPotSize, 10000);
                }
            };
            Assert.IsTrue(table.IsDeal);
            Assert.AreEqual(table.GetPlayers().ElementAt(0), second);
            Assert.AreEqual(table.GetPlayers().ElementAt(1), third);
            Assert.AreEqual(table.GetPlayers().ElementAt(2), first);
            Assert.AreEqual(table.GetWhoseMove(), second);
            Assert.AreEqual(table.PotSize, table.SmallBlindSize + table.BigBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.BigBlind), table.BigBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.SmallBlind), table.SmallBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.Button), 0);
            Player actor = table.GetWhoseMove();
            Assert.AreEqual(actor.Stack, 10000);
            table.MakeMove(actor, new AllIn());
            Assert.AreEqual(table.GetPlayers().Where(p => p.IsInDeal).Count(), 3);
            Assert.AreEqual(table.GetPlayers().Where(p => p.HasChips).Count(), 2);
            Assert.AreEqual(table.GetWhoseMove(), third);
            actor = table.GetWhoseMove();
            Assert.Throws<Exception>(() => table.MakeMove(second, new Fold()));
            Assert.Throws<Exception>(() => table.MakeMove(first, new Fold()));
            table.MakeMove(actor, new AllIn());
            Assert.AreEqual(table.GetPlayers().Where(p => p.IsInDeal).Count(), 3);
            Assert.AreEqual(table.GetPlayers().Where(p => p.HasChips).Count(), 1);
            Assert.AreEqual(table.GetWhoseMove(), first);
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new AllIn());
            Assert.IsFalse(table.IsDeal);
            Assert.AreEqual(third.Stack, 10000);
            Assert.AreEqual(second.Stack, 10000);
            Assert.AreEqual(first.Stack, 10000);
        }

        [Test]
        public void SimpleDealWithShowDown()
        {
            table.ShowDownEvent += (players) =>
            {
                Assert.AreEqual(players.Count(), 3);
            };

            table.WinnersGotPotEvent += (awards) =>
            {
                Assert.AreEqual(awards.Count(), 3);
                var winners = awards.Select(award => award.Winner);
                foreach (var winner in winners)
                {
                    Assert.IsTrue(winner.Combination.CompareTo(winners.First().Combination) == 0);
                }
                foreach (var award in awards)
                {
                    Assert.AreEqual(award.WonPotSize, 1400);
                }
            };
            Assert.IsTrue(table.IsDeal);
            Assert.AreEqual(table.GetPlayers().ElementAt(0), second);
            Assert.AreEqual(table.GetPlayers().ElementAt(1), third);
            Assert.AreEqual(table.GetPlayers().ElementAt(2), first);
            Assert.AreEqual(table.GetWhoseMove(), second);
            Assert.AreEqual(table.PotSize, table.SmallBlindSize + table.BigBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.BigBlind), table.BigBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.SmallBlind), table.SmallBlindSize);
            Assert.AreEqual(table.GetBetSizeOf(table.Button), 0);
            Player actor = table.GetWhoseMove();
            Assert.AreEqual(actor.Stack, 10000);
            table.MakeMove(actor, new Call());
            Assert.AreEqual(table.GetWhoseMove(), third);
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Call());
            Assert.AreEqual(table.GetPlayers().Where(p => p.IsInDeal).Count(), 3);
            Assert.AreEqual(table.GetPlayers().Where(p => p.HasChips).Count(), 3);
            Assert.AreEqual(table.GetWhoseMove(), first);
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Check());
            Assert.AreEqual(RoundType.Flop, table.CurrentRound);
            Assert.AreEqual(third, table.GetWhoseMove());
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Bet(400));
            Assert.AreEqual(table.CurrentMaxBetSize, 400);
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Raise(2.0));
            Assert.AreEqual(table.CurrentMaxBetSize, 800);
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Call());
            Assert.AreEqual(RoundType.Flop, table.CurrentRound);
            Assert.AreEqual(table.CurrentMaxBetSize, 800);
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Call());
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Check());
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Check());
            Assert.AreEqual(RoundType.Turn, table.CurrentRound);
            Assert.AreEqual(table.CurrentMaxBetSize, 0);
            Assert.AreEqual(third, table.GetWhoseMove());

            actor = table.GetWhoseMove();
            Assert.Throws<Exception>(() =>
            {
                table.MakeMove(actor, new Bet(100));
            });
            table.MakeMove(actor, new Bet(400));
            Assert.AreEqual(table.CurrentMaxBetSize, 400);
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Call());
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Call());
            Assert.AreEqual(RoundType.River, table.CurrentRound);
            Assert.AreEqual(table.CurrentMaxBetSize, 0);
            Assert.AreEqual(third, table.GetWhoseMove());

            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Check());
            Assert.AreEqual(table.CurrentMaxBetSize, 0);
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Check());
            actor = table.GetWhoseMove();
            table.MakeMove(actor, new Check());

            Assert.IsFalse(table.IsDeal);
            Assert.AreEqual(third.Stack, 10000);
            Assert.AreEqual(second.Stack, 10000);
            Assert.AreEqual(first.Stack, 10000);
        }
    }
} 
