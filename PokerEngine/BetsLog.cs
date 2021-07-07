using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PokerEngine
{
    class BetsLog : IEnumerable<Player>
    {
        public int MaxBetSizeInRound { get; private set; }
        private Dictionary<Player, int> _playerBetsizePairs; //private Dictionary<Player, BetSizes> _playerBetsizePairs;
        public BetsLog()
        {
            MaxBetSizeInRound = 0;
            _playerBetsizePairs = new Dictionary<Player, int>();  //_playerBetsizePairs = new Dictionary<Player, BetSizes>();

        }

        public void Add(Player maker, int betSize)
        {
            if (_playerBetsizePairs.ContainsKey(maker))
            {
                _playerBetsizePairs[maker] += betSize;
                /*BetSizes newBetSizes = _playerBetsizePairs[maker];
                newBetSizes.BetSizeInRound += betSize;
                newBetSizes.BetSizeInDeal += betSize;
                _playerBetsizePairs[maker] = newBetSizes;*/
            }
            else
            {
                _playerBetsizePairs.Add(maker, betSize);
                //_playerBetsizePairs.Add(maker, new BetSizes(betSize, betSize));
            }
            if (_playerBetsizePairs[maker] > MaxBetSizeInRound)
            {
                MaxBetSizeInRound = _playerBetsizePairs[maker];
            }
        }

        private void CheckContains(Player player)
        {
            if (!_playerBetsizePairs.ContainsKey(player))
            {
                throw new Exception("No such player in bet log");
            }

        }

        public int GetBetSizeInRoundOf(Player player)
        {
            if (_playerBetsizePairs.ContainsKey(player))
            {
                return _playerBetsizePairs[player];
            }
            else
            {
                return 0;
            }
        }

        /* public int GetBetSizeInDealOf(Player player)
         {
             if (_playerBetsizePairs.ContainsKey(player))
             {
                 return _playerBetsizePairs[player].BetSizeInDeal;
             }
             else
             {
                 throw new Exception("No such player in bets log. The player didn't make any bet");
             }
         }*/

        public void Clear() //ClearCurrentRound()
        {
            MaxBetSizeInRound = 0;
            _playerBetsizePairs.Clear();
            /*foreach(var pair in _playerBetsizePairs)
            {
                pair.Value.ResetRound();
            }*/
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return _playerBetsizePairs.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
