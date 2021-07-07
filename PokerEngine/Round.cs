using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public enum RoundType : byte
    {
        Preflop,
        Flop,
        Turn,
        River,
    }
    abstract class Round
    {
        public abstract Round Next { get; protected set; }
        public abstract RoundType RoundType { get; }
        public abstract string Name { get; }
        public abstract Position StartPosition { get; }
        public abstract int AdditionalCardsOnBoardAmount { get; }

    }
}
