using System;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class MoveOrder
    {
        public event Action PlayersRanOutEvent;
        public event Action EverybodyMadeAllInEvent;

        public bool IsEmpty => _playersOrder.Count == 0;
        public int Count => _playersOrder.Count;
        public Player FirstPlayerAtCurrentRound { get; private set; }
        private Queue<Player> _playersOrder;
        private PlayersSittingInCircle _players;

        public MoveOrder(PlayersSittingInCircle players)
        {
            _playersOrder = new Queue<Player>();
            _players = players;
            _players.PlayerMadeMoveEvent += GiveMoveToNextPlayer;
        }

        public void Update(Round newRound)
        {
            Player firstPlayer = FindFirstPlayerAtRound(newRound);
            Update(firstPlayer);
        }

        public void Update(Player firstPlayer)
        {
            _playersOrder = _players.ToQueue(firstPlayer, p => (p.IsInDeal) && p.HasChips);
            /*if (_playersOrder.Count == 1)
            {
                EverybodyMadeAllInEvent?.Invoke();
            }*/
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

        private void CheckConditions()
        {

        }

        private void GiveMoveToNextPlayer()
        {
            if (IsEmpty)
            {
                return;
            }
            _playersOrder.Dequeue();
            if (IsEmpty)
            {
                PlayersRanOutEvent?.Invoke();
            }
            /*if (IsEmpty)
            {
                return;
            }
            else
            {
                _playersOrder.Dequeue();
                if (IsEmpty)
                {
                    PlayersRanOutEvent?.Invoke();
                }
            }*/
        }
    }
}
