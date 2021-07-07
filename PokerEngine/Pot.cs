using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Pot
    {
        public int Size { get; private set; }

        public Pot()
        {
            Size = 0;
        }

        internal void AddСhipsFrom(Dealer dealer)
        {
            Size += dealer.PlayerChips;
        }

        internal void GiveChipsTo(Dealer dealer)
        {
            Size -= dealer.PlayerChips;
        }
    }
}
