using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public class Bet : BettingMove
    {
        public override MoveAlias Alias => MoveAlias.Bet;

        public Bet(int size)
        {
            BetSize = size;
        }

        internal override void Make(Player player)
        {
            player.Bet(BetSize);
        }
    }
}
