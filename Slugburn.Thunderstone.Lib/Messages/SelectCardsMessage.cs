namespace Slugburn.Thunderstone.Lib.Messages
{
    public class SelectCardsMessage
    {
        public string Caption { get; set; }

        public string Message { get; set; }

        public object Cards { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }
    }
}
