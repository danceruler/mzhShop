using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzh.Public.Base
{
    public class WebSocketHelper
    {
        public static WebSocketServer wsServer;
        public static List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();

        public static void Init(string url)
        {
            wsServer = new WebSocketServer(url);
            allSockets.Clear();
        }

        public static void Send(string message)
        {
            foreach (var socket in allSockets.ToList())
            {
                socket.Send(message);
            }
        }
    }
}
