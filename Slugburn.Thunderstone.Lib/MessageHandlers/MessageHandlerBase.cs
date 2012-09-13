using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public abstract class MessageHandlerBase : IMessageHandler
    {
        protected MessageHandlerBase(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }

        public abstract void Handle(Message message);
    }
}
