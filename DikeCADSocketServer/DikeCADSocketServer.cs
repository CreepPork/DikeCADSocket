using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CitizenFX.Core;
using Fleck;

namespace DikeCADSocketServer
{
    public class DikeCADSocketServer : BaseScript
    {
        private static readonly List<IWebSocketConnection> Sockets = new List<IWebSocketConnection>();

        public DikeCADSocketServer()
        {
            EventHandlers.Add("dike:receiveData", new Action<string>(OnReceiveData));

            CreateWebSocketServer();
        }

        private static void CreateWebSocketServer()
        {
            WebSocketServer server = new WebSocketServer("ws://127.0.0.1:8080");
            
            server.Start(socket =>
            {
                socket.OnOpen += () =>
                {
                    socket.Send("Connection established.");
                    Debug.WriteLine("Socket opened!");
                    
                    Sockets.Add(socket);
                };
            });
        }

        private static void OnReceiveData(string json)
        {
            Debug.WriteLine(json);

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
    }
}