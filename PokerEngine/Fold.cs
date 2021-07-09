using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public class Fold : Move
    {
        public override MoveAlias Alias => MoveAlias.Fold;

        public Fold()
        {
        }
        internal override void Make(Player player)
        {
            player.Fold();
        }
    }
}
