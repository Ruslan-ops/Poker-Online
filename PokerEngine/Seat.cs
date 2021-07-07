using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Seat
    {
        public bool IsFree => Player == null;
        public Player Player { get; private set; }

        public Seat(Player player)
        {
            Player = player;
        }
        public Seat() : this(null) { }

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
