namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public interface IMessageHandler
    {
        string Id { get; }
        void Handle(Message message);
    }
}
