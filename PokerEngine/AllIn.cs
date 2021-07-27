using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public class AllIn : BettingMove
    {
        public override MoveAlias Alias => MoveAlias.AllIn;

        public AllIn()
        {
        }
        internal override void Make(Player player)
        {
            BetSize = player.AllIn();
        }
    }
}
