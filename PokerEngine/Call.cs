using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public class Call : BettingMove
    {
        public override MoveAlias Alias => MoveAlias.Call;


        public Call()
        {
        }

        internal override void Make(Player player)
        {
            BetSize = player.Call();
        }
    }
}
