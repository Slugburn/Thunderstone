using System;
using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib
{
    public class GameSession
    {
        private readonly List<Player> _players = new List<Player>();

        public void Join(Player player)
        {
            _players.Add(player);
            player.Session = this;
        }

        public GameSetup Setup { get; set; }

        public List<Player> GetPlayers()
        {
            return new List<Player>(_players);
        }

        public void SendAll(Action<Player> action)
        {
            _players.Each(action);
        }
    }
}
