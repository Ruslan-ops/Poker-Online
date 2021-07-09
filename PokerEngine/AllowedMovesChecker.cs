using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class AllowedMovesChecker
    {
        private const int MovesCount = 4;
        private BetsLog _betsLog;


        public AllowedMovesChecker(BetsLog betsLogAtRound)
        {
            _betsLog = betsLogAtRound;
        }

        public void CheckValidationFor(Player player, Move move)
        {
            List<MoveAlias> allowedMoves = GetAllowedMovesFor(player);
            if (allowedMoves.Contains(move.Alias))
            {
                return;
            }
            throw new Exception("This move is forbidden");
        }

        public List<MoveAlias> GetAllowedMovesFor(Player player)
        {
            List<MoveAlias> allowedMoves = new List<MoveAlias>();
            allowedMoves.Add(GetFirstAllowedFor(player));
            allowedMoves.Add(GetSecondAllowedFor(player));
            allowedMoves.Add(MoveAlias.Fold);
            allowedMoves.Add(MoveAlias.AllIn);
            return allowedMoves;
        }

        private MoveAlias GetFirstAllowedFor(Player player)
        {
            if (_betsLog.MaxBetSizeInRound > _betsLog.GetBetSizeInRoundOf(player))
            {
                return MoveAlias.Call;
            }
            else
            {
                return MoveAlias.Check;
            }
        }
        private MoveAlias GetSecondAllowedFor(Player player)
        {
            if (_betsLog.MaxBetSizeInRound > 0)
            {
                return MoveAlias.Raise;
            }
            else
            {
                return MoveAlias.Bet;
            }
        }

    }
}
