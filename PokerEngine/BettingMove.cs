using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public abstract class BettingMove : Move
    {
        public int BetSize { get; protected set; }
    }
}
