using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class Turn : Round
    {
        public override RoundType RoundType => RoundType.Turn;
        public override Position StartPosition => Position.SmallBlind;
        public override int AdditionalCardsOnBoardAmount => 1;
        public override string Name => "Turn";

        public override Round Next
        {
            get
            {
                return new River(); ;
            }
            protected set
            {
                Next = value;
            }
        }
        public Turn()
        {
        }
    }
}
