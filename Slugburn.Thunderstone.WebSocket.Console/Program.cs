using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Slugburn.Thunderstone.Lib;
using Slugburn.Thunderstone.Lib.MessageHandlers;

namespace Slugburn.Thunderstone.WebSocket.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var messageHandlers = GetInstancesOfType<IMessageHandler>();
            var program = new Program(messageHandlers);
            program.Start();
        }

        private static T[] GetInstancesOfType<T>()
        {
            return typeof (Game).Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof (T).IsAssignableFrom(type))
                .Select(x => x.GetConstructor(new Type[0]))
                .Where(constructor => constructor != null)
                .Select(constructor => constructor.Invoke(new object[0]))
                .Cast<T>()
                .ToArray();
        }

        private readonly ConcurrentDictionary<Guid, Player> _players = new ConcurrentDictionary<Guid, Player>();
        private readonly Dictionary<string, IMessageHandler> _messageHandlers;

        public Program(IEnumerable<IMessageHandler> messageHandlers)
        {
            _messageHandlers = messageHandlers.ToDictionary(x => x.Id);
        }

        private void Start()
        {
            using (var server = new WebSocketServer("ws://localhost:8181"))
            {
                server.Start(socket =>
                                 {
                                     socket.OnOpen = () => OnSocketOpen(socket);
                                     socket.OnClose = () => OnSocketClose(socket);
                                     socket.OnMessage = msg => OnMessage(msg, socket);
                                 });
                Console.Read();
            }
        }

        private void OnSocketOpen(IWebSocketConnection socket)
        {
            var info = socket.ConnectionInfo;
            var player = new Player(info.Id, (messageId, body) => SendMessage(socket, messageId, body));
            _players.TryAdd(info.Id, player);

            var session = new GameSession();
            session.Join(player);

            Console.WriteLine("Socket opened:");
            Console.WriteLine("\tClientIPAddress = {0}", info.ClientIpAddress);
            Console.WriteLine("\tCookies:");
            info.Cookies.ToList().ForEach(pair => Console.WriteLine("\t\t{0} = {1}", pair.Key, pair.Value));
            Console.WriteLine("\tHost = {0}", info.Host);
            Console.WriteLine("\tId = {0}", info.Id);
            Console.WriteLine("\tOrigin = {0}", info.Origin);
            Console.WriteLine("\tPath = {0}", info.Path);
            Console.WriteLine("\tSubProtocol = {0}", info.SubProtocol);
        }

        private static void SendMessage(IWebSocketConnection socket, string messageId, object body)
        {
            socket.Send(JsonConvert.SerializeObject(new { Id = messageId, Body = body }));
        }

        private void OnSocketClose(IWebSocketConnection socket)
        {
            Player removed;
            var playerId = socket.ConnectionInfo.Id;
            _players.TryRemove(playerId, out removed);
        }

        private void OnMessage(string message, IWebSocketConnection socket)
        {
            var playerId = socket.ConnectionInfo.Id;

            var jobject = JObject.Parse(message);
            var messageId = (string)jobject["id"];
            var body = jobject["body"] == null ? null : jobject["body"].ToString();
            Console.WriteLine("Message received from socket {0}: {1}", playerId, messageId);

            Player player;
            IMessageHandler handler;
            if (!_players.TryGetValue(playerId, out player) || !_messageHandlers.TryGetValue(messageId, out handler))
                return;

            handler.Handle(new Message(player, messageId, body));
        }

    }
}
