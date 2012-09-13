using System;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class Message
    {
        public Message(Player player, string id, string body)
        {
            Player = player;
            Id = id;
            Body = body;

            Game = player.Game;
        }

        public Player Player { get; set; }

        public string Id { get; set; }

        public string Body { get; set; }

        public Game Game { get; set; }
    }
}