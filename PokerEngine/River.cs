using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class River : Round
    {
        public override RoundType RoundType => RoundType.River;
        public override Position StartPosition => Position.SmallBlind;
        public override int AdditionalCardsOnBoardAmount => 1;
        public override string Name => "River";

        public override Round Next
        {
            get
            {
                return new Preflop();
            }
            protected set
            {
                Next = value;
            }
        }
        public River()
        {
        }
    }
}
