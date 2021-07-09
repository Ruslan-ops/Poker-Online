using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public class Call : Move
    {
        public override MoveAlias Alias => MoveAlias.Call;

        public Call()
        {
        }

        internal override void Make(Player player)
        {
            player.Call();
        }
    }
}
