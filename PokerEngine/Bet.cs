using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Bet : Move
    {
        public readonly int Size;
        public override MoveAlias Alias => MoveAlias.Bet;

        internal Bet(int size)
        {
            Size = size;
        }

        internal override void Make(Player player)
        {
            player.Bet(Size);
        }
    }
}
