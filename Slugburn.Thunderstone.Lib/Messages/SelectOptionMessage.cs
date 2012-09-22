using Slugburn.Thunderstone.Lib.Abilities;

namespace Slugburn.Thunderstone.Lib.Messages
{
    public class SelectOptionMessage
    {
        public string Caption { get; set; }

        public string Message { get; set; }

        public string[] Options { get; set; }

        public static SelectOptionMessage From(SelectOptionArg arg)
        {
            return new SelectOptionMessage
                       {
                           Caption = arg.Caption,
                           Message = arg.Message,
                           Options = arg.Options,
                       };
        }
    }
}