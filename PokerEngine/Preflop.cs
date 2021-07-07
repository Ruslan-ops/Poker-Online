using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Preflop : Round
    {
        public override RoundType RoundType => RoundType.Preflop;
        public override Position StartPosition => Position.UTG;
        public override int AdditionalCardsOnBoardAmount => 0;
        public override string Name => "Preflop";
        public override Round Next
        {
            get
            {
                return new Flop();
            }
            protected set
            {
                Next = value;
            }
        }
        public Preflop()
        {
        }

    }
}
