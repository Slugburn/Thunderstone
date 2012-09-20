using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Thunderstone.Lib.Messages
{
    public class StartTurnMessage
    {

        public IEnumerable<string> AvailableActions { get; set; }

        public static StartTurnMessage From(IEnumerable<PlayerAction> validActions)
        {
            return new StartTurnMessage {AvailableActions = validActions.Select(x=>x.ToString())};
        }

    }
}
