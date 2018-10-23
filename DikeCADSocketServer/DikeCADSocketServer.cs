using System;
using System.Collections.Generic;
using System.Threading;
using CitizenFX.Core;
using Fleck;

namespace DikeCADSocketServer
{
    public class DikeCADSocketServer : BaseScript
    {
        // Socket, isOpen
        private static readonly Dictionary<IWebSocketConnection, bool> Sockets = new Dictionary<IWebSocketConnection, bool>();

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
                    
                    Sockets.Add(socket, true);
                };
            });
        }

        private static void OnReceiveData(string json)
        {
            Debug.WriteLine(json);

            new Thread(() =>
            {
                foreach (KeyValuePair<IWebSocketConnection,bool> socket in Sockets)
                {
                    // If socket is open
                    if (socket.Value)
                    {
                        socket.Key.Send(json);
                    }
                }
            }).Start();
        }
    }
}