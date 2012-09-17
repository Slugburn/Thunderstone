namespace Slugburn.Thunderstone.Lib.Events
{
    internal class MonsterDefeated
    {
        public MonsterDefeated(Player player, Card monster)
        {
            Player = player;
            Monster = monster;
        }

        public Player Player { get; set; }

        public Card Monster { get; set; }
    }
}