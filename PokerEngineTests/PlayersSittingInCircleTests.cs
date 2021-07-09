using NUnit.Framework;
using NUnit.Framework.Constraints;
using PokerEngine;
using System;
using System.Linq;

namespace PokerEngineTests
{
    public class PlayersSittingInCircleTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddFullPlayersCircle()
        {
            int seatsCount = 5;
            int playersCount = 5;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
                players.Add(ps[i]);
            }
            Assert.AreEqual(playersCount, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            for(int i = 0; i < players.Amount; i++)
            {
                Assert.AreSame(ps[i], players.ElementAt(i));
            }
        }

        [Test]
        public void AddPlayersCircle3()
        {
            int seatsCount = 5;
            int playersCount = 3;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
                players.Add(ps[i]);
            }
            Assert.AreEqual(playersCount, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            for (int i = 0; i < players.Amount; i++)
            {
                Assert.AreSame(ps[i], players.ElementAt(i));
            }
        }

        [Test]
        public void AddPlayersCircle1()
        {
            int seatsCount = 5;
            int playersCount = 1;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
                players.Add(ps[i]);
            }
            Assert.AreEqual(playersCount, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            for (int i = 0; i < players.Amount; i++)
            {
                Assert.AreSame(ps[i], players.ElementAt(i));
            }
        }

        [Test]
        public void AddFullPlayersRandomly()
        {
            int seatsCount = 5;
            int playersCount = 5;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
            }
            players.Add(ps[0], 4);
            players.Add(ps[1], 1);
            players.Add(ps[2], 3);
            players.Add(ps[3], 0);
            players.Add(ps[4], 2);
            Assert.AreEqual(playersCount, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            Assert.AreSame(players.Get(0), ps[3]);
            Assert.AreSame(players.Get(1), ps[1]);
            Assert.AreSame(players.Get(2), ps[4]);
            Assert.AreSame(players.Get(3), ps[2]);
            Assert.AreSame(players.Get(4), ps[0]);
        }

        [Test]
        public void AddNotFullPlayersRandomly()
        {
            int seatsCount = 6;
            int playersCount = 4;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
            }
            players.Add(ps[0], 3);
            players.Add(ps[1], 5);
            players.Add(ps[2], 1);
            players.Add(ps[3], 4);
            Assert.AreEqual(playersCount, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            Assert.Throws<Exception>(() => players.Get(0));
            Assert.Throws<Exception>(() => players.Get(2));
            Assert.AreSame(ps[0], players.Get(3) );
            Assert.AreSame(ps[1], players.Get(5) );
            Assert.AreSame(ps[2], players.Get(1) );
            Assert.AreSame(ps[3], players.Get(4) );

            Assert.AreSame(players.Get(1), players.ElementAt(0));
            Assert.AreSame(players.Get(3), players.ElementAt(1));
            Assert.AreSame(players.Get(4), players.ElementAt(2));
            Assert.AreSame(players.Get(5), players.ElementAt(3));
        }

        [Test]
        public void AddPlayerWhenAllSeatsOccupied()
        {
            int seatsCount = 4;
            int playersCount = 4;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player oddPlayer = new Player(300, null, "Odd");
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
            }
            players.Add(ps[0], 3);
            players.Add(ps[1], 2);
            players.Add(ps[2], 1);
            players.Add(ps[3], 0);
            Assert.AreEqual(playersCount, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            Assert.Throws<Exception>(() => players.Add(oddPlayer));
        }

        [Test]
        public void AddPlayersOnOccupiedSeat()
        {
            int seatsCount = 6;
            int playersCount = 4;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player oddPlayer = new Player(300, null, "Odd");
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
            }
            players.Add(ps[0], 3);
            players.Add(ps[1], 5);
            players.Add(ps[2], 1);
            players.Add(ps[3], 4);
            Assert.AreEqual(playersCount, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            Assert.Throws<Exception>(() => players.Get(0));
            Assert.Throws<Exception>(() => players.Get(2));
            Assert.Throws<Exception>(() => players.Add(oddPlayer, 3));
        }

        [Test]
        public void DeletePlayerRandomly()
        {
            int seatsCount = 7;
            int playersCount = 4;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
            }
            players.Add(ps[0], 3);
            players.Add(ps[1], 5);
            players.Add(ps[2], 1);
            players.Add(ps[3], 4);
            players.DeleteAt(4);
            players.Delete("num-0");
            Assert.AreEqual(playersCount - 2, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            Assert.Throws<Exception>(() => players.Get(4));
            Assert.Throws<Exception>(() => players.Get(3));
        }

        [Test]
        public void DeleteFirstPlayer()
        {
            int seatsCount = 7;
            int playersCount = 4;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
            }
            players.Add(ps[0], 3);
            players.Add(ps[1], 5);
            players.Add(ps[2], 1);
            players.Add(ps[3], 4);
            players.DeleteAt(1);
            players.Delete("num-0");
            Assert.AreEqual(playersCount - 2, players.Amount);
            Assert.AreEqual(seatsCount, players.SeatsAmount);
            Assert.Throws<Exception>(() => players.Get(1));
            Assert.Throws<Exception>(() => players.Get(3));
        }

        [Test]
        public void AssignNewButtonWithManyPlayers()
        {
            int seatsCount = 7;
            int playersCount = 4;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
            }
            players.Add(ps[0], 3);
            players.Add(ps[1], 5);
            players.Add(ps[2], 1);
            players.Add(ps[3], 4);
            Assert.AreSame(players.Get(1), players.First());
            Assert.AreEqual(playersCount, players.Amount);
            players.AssignNewButton();
            Assert.AreSame(players.Get(3), players.First());
            Assert.AreEqual(playersCount, players.Amount);
        }

        [Test]
        public void AssignNewButtonWithTwoPlayers()
        {
            int seatsCount = 6;
            int playersCount = 2;
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
            }
            players.Add(ps[0], 4);
            players.Add(ps[1], 2);
            Assert.AreSame(players.Get(2), players.First());
            Assert.AreEqual(playersCount, players.Amount);
            players.AssignNewButton();
            Assert.AreSame(players.Get(4), players.First());
            Assert.AreEqual(playersCount, players.Amount);
            players.AssignNewButton();
            Assert.AreSame(players.Get(2), players.First());
            Assert.AreEqual(playersCount, players.Amount);
        }

        private PlayersSittingInCircle InitCircle(int seatsCount, int playersCount)
        {
            PlayersSittingInCircle players = new PlayersSittingInCircle(seatsCount);
            Player[] ps = new Player[playersCount];
            for (int i = 0; i < ps.Length; i++)
            {
                ps[i] = new Player(400, null, $"num-{i}");
                players.Add(ps[i]);
            }
            return players;
        }


    }
}