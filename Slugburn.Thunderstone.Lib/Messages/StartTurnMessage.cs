namespace Slugburn.Thunderstone.Lib.Messages
{
    public class StartTurnMessage
    {
        public bool Village { get; set; }
        public bool Dungeon { get; set; }
        public bool Prepare { get; set; }
        public bool Rest { get; set; }
    }
}
