using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CitizenFX.Core;
using Fleck;
using Newtonsoft.Json;

namespace DikeCADSocketServer
{
    public class DikeCADSocketServer : BaseScript
    {
        private static readonly List<IWebSocketConnection> Sockets = new List<IWebSocketConnection>();

        public DikeCADSocketServer()
        {
            EventHandlers.Add("dike:receiveData", new Action<string>(OnReceiveData));
            EventHandlers["playerDropped"] += new Action<string, Player>(OnPlayerDropped);

            CreateWebSocketServer();
        }

        private static void OnPlayerDropped(string reason, [FromSource] Player player)
        {
            string json = JsonConvert.SerializeObject(new PlayerDropped(reason, int.Parse(player.Handle)));
            
            OnReceiveData(json);
        }

        private static void CreateWebSocketServer()
        {
            WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8080");
            
            server.Start(socket =>
            {
                socket.OnOpen += () =>
                {
                    Sockets.Add(socket);
                };
            });
        }

        private static void OnReceiveData(string json)
        {
            new Thread(() =>
            {
                foreach (IWebSocketConnection socket in Sockets.ToList())
                {
                    if (socket.IsAvailable)
                    {
                        socket.Send(json);
                    }
                    else
                    {
                        Sockets.Remove(socket);
                    }
                }
            }).Start();
        }
        
        private class PlayerDropped
        {
            public PlayerDropped(string reason, int id)
            {
                Reason = reason;
                ID = id;
            }

            public string Reason { get; set; }
            public int ID { get; set; }
        }
    }
}