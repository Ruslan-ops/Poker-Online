using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Check : Move
    {
        public override MoveAlias Alias => MoveAlias.Check;

        public Check()
        {
        }

        internal override void Make(Player player)
        {
            player.Check();
        }
    }
}
