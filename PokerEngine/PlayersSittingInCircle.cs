using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerEngine
{
    class PlayersSittingInCircle : IEnumerable<Player>
    {
        //public event Action PlayerMadeMoveEvent;
        public event Action PlayerMadeFoldEvent;
        //public event Action PlayerMadeAllInEvent;

        public int Amount => _players.Size;
        public readonly int SeatsAmount;
        private const int MinPlayersAmount = 2;
        private const int MaxPlayersAmount = 10;
        private Ring<Player> _players;
        private readonly List<Seat> _seats;



        public PlayersSittingInCircle(int seatsAmount)
        {
            if (seatsAmount < MinPlayersAmount || seatsAmount > MaxPlayersAmount)
            {
                throw new Exception($"Seats amount must be from {MinPlayersAmount} to {MaxPlayersAmount}");
            }
            SeatsAmount = seatsAmount;
            _players = new Ring<Player>();
            _seats = new List<Seat>(SeatsAmount);
            for (int i = 0; i < SeatsAmount; i++)
            {
                _seats.Add(new Seat());
            }
        }

        private void PlacePlayerOnSeat(Player player, int seatNumber)
        {
            _seats[seatNumber].Add(player);
            int indexInPlayers = 0;
            for (int i = 0; i < seatNumber; i++)
            {
                if (!_seats[i].IsFree)
                {
                    indexInPlayers++;
                }
            }
            _players.Insert(indexInPlayers, player);
            player.MadeFoldEvent += PlayerMadeFoldEvent;


        }

        public void Add(Player player, int seatNumber)
        {
            if (seatNumber < 0 || seatNumber >= SeatsAmount)
            {
                throw new Exception("No such seat number");
            }
            if (_seats[seatNumber].IsFree)
            {
                PlacePlayerOnSeat(player, seatNumber);
                return;
            }
            else
            {
                throw new Exception("This seat is occupied");
            }
        }
        public void Add(Player player)
        {
            if (Amount < SeatsAmount)
            {
                for (int i = 0; i < SeatsAmount; i++)
                {
                    if (_seats[i].IsFree)
                    {
                        PlacePlayerOnSeat(player, i);
                        return;
                    }
                }
            }
            else
            {
                throw new Exception("All seats are occupied");
            }
        }


        private Seat GetSeatWith(Player player)
        {
            Seat requiredSeat = _seats.Where(x => x.Player == player).FirstOrDefault();
            return requiredSeat ?? throw new Exception("No such player");
        }

        private Seat GetSeatWith(string playerName)
        {
            Player requaredPlayer = Get(playerName);
            return GetSeatWith(requaredPlayer);
        }

        public Player Get(string name)
        {
            Player requiredPlayer = _players.Where(x => x.Name == name).FirstOrDefault();
            return requiredPlayer ?? throw new Exception("No such player");
        }

        public Player Get(int seatNumber)
        {
            if (seatNumber < 0 || seatNumber >= SeatsAmount)
            {
                throw new Exception("No such seat number");
            }
            if (_seats[seatNumber].IsFree)
            {
                throw new Exception("There is no player on the seat");
            }
            else
            {
                return _seats[seatNumber].Player;
            }
        }

        public void Delete(string playerName)
        {
            Seat seatWithDealetingPlayer = GetSeatWith(playerName);
            DeleteAt(seatWithDealetingPlayer);
        }

        private void DeleteAt(Seat seatWithDealetingPlayer)
        {
            Player playerToDelete = seatWithDealetingPlayer.Player;
            _players.Remove(playerToDelete);
            seatWithDealetingPlayer.RemovePlayer();
            playerToDelete.MadeFoldEvent -= PlayerMadeFoldEvent;
        }
        public void DeleteAt(int seatNumber)
        {
            if (seatNumber < 0 || seatNumber >= SeatsAmount)
            {
                throw new Exception("No such seat number");
            }
            Seat seatWithDealetingPlayer = _seats[seatNumber];
            DeleteAt(seatWithDealetingPlayer);
        }

        public Player this[int index]
        {
            get
            {
                return _players[index];
            }
            set
            {
                _players[index] = value;
            }
        }
        public Queue<Player> ToQueue(Player firstPlayer, Func<Player, bool> predicate)
        {
            return _players.ToQueue(firstPlayer, predicate);
        }

        public Player GetButton()
        {
            return _players[(int)Position.Button];
        }

        public Player GetSmallBlind()
        {
            if (Amount == 2)
            {
                return GetButton();
            }
            else
            {
                return _players[(int)Position.SmallBlind];
            }
        }

        public Player GetBigBlind()
        {
            if (Amount == 2)
            {
                return _players[1];
            }
            else
            {
                return _players[(int)Position.BigBlind];
            }
        }


        public Player GetNextAfter(Player current)
        {
            return _players.GetNextAfter(current);
        }
        public void AssignNewButton()
        {
            _players.Spin();
        }



        public IEnumerator<Player> GetEnumerator()
        {
            foreach (Player player in _players)
            {
                yield return player;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
