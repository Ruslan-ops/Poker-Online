using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    public enum MoveAlias : byte
    {
        Bet,
        Raise,
        AllIn,
        Call,
        Check,
        Fold
    }
    public abstract class Move
    {
        public abstract MoveAlias Alias { get; }
        internal abstract void Make(Player player);
    }
}
