using System.Collections.Concurrent;
using Slugburn.Thunderstone.Lib;

namespace Slugburn.Thunderstone.Web
{
    public class PlayerStore
    {

        private readonly ConcurrentDictionary<string, Player> _players = new ConcurrentDictionary<string, Player>();

        static PlayerStore()
        {
            Instance = new PlayerStore();
        }

        public static PlayerStore Instance { get; private set; }

        public void Store(Player player)
        {
            _players.TryAdd(player.Id, player);
        }

        public Player Get(string id)
        {
            Player player;
            return _players.TryGetValue(id, out player) ? player : null;
        }
    }
}