using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public class Seat
    {
        public bool IsFree => Player == null;
        public readonly int Number; 
        public Player Player { get; private set; }

        public Seat(int number, Player player)
        {
            Player = player;
            Number = number;
        }
        public Seat(int number) : this(number, null) { }

        public void Add(Player player)
        {
            if (IsFree)
            {
                Player = player;
            }
            else
            {
                throw new Exception("The seat is occupied");
            }
        }

        public void RemovePlayer()
        {
            Player = null;
        }
    }
}
