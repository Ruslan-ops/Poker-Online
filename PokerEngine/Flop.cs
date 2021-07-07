using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Flop : Round
    {
        public override RoundType RoundType => RoundType.Flop;
        public override Position StartPosition => Position.SmallBlind;
        public override int AdditionalCardsOnBoardAmount => 3;
        public override string Name => "Flop";

        public override Round Next
        {
            get
            {
                return new Turn(); ;
            }
            protected set
            {
                Next = value;
            }
        }
        public Flop()
        {
        }
    }
}
