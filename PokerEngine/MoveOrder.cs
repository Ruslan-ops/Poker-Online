using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class MoveOrder
    {
        public event Action PlayersRanOutEvent;
        public bool IsEmpty => _playersOrder.Count == 0;
        public int Count => _playersOrder.Count;
        public Player FirstPlayerAtCurrentRound { get; private set; }
        private Queue<Player> _playersOrder;
        private PlayersSittingInCircle _players;

        public MoveOrder(PlayersSittingInCircle players)
        {
            _playersOrder = new Queue<Player>();
            _players = players;
        }

        public void Update(Round newRound)
        {
            Player firstPlayer = FindFirstPlayerAtRound(newRound);
            Update(firstPlayer);
        }

        public void Update(Player firstPlayer)
        {
            _playersOrder = _players.ToQueue(firstPlayer, p => (p.IsInDeal) && p.HasChips);
        }
        private Player FindFirstPlayerAtRound(Round newRound)
        {
            Player firstPlayer;
            if (_players.Amount == 2 && newRound.StartPosition == Position.UTG)
            {
                firstPlayer = _players.GetButton();
            }
            else
            {
                int firstIndex = (int)newRound.StartPosition % _players.Amount;
                firstPlayer = _players[firstIndex];
            }
            FirstPlayerAtCurrentRound = firstPlayer;
            return firstPlayer;
        }

        public Player GetWhoseMove()
        {
            return _playersOrder.Peek();
        }

        public void Clear()
        {
            _playersOrder.Clear();
        }

        public void GiveMoveToNextPlayer()
        {
            _playersOrder.Dequeue();
            if (IsEmpty)
            {
                PlayersRanOutEvent?.Invoke();
            }  
        }
    }
}
