namespace Slugburn.Thunderstone.Lib.Abilities
{
    public class SelectOptionArg
    {
        public string Caption { get; set; }
        public string Message { get; set; }
        public string[] Options { get; set; }

        public SelectOptionArg(string caption, string message, params string[] options)
        {
            Caption = caption;
            Message = message;
            Options = options;
        }
    }
}
