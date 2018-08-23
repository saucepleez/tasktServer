using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tasktServer.Models
{
    public class SocketConnectionModel
    {
        public string PublicKey { get; set; }
        public WebSocket WebSocket { get; set; }
        public PingRequestModel PingRequest { get; set; } = new PingRequestModel();
        public async Task SendMessageAsync(String data)
        {
            var encoded = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            await WebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }


    }
    public class PingRequestModel{
        //potentially move to a list of requests for multiples
        public DateTime RequestSent { get; set; }
        public bool AwaitingPingReply { get; set; }
        public bool ReadyForUIReporting { get; set; }
        public string ClientStatus { get; set; }
        public PingRequestModel()
        {
            AwaitingPingReply = true;
            ReadyForUIReporting = false;
        }
    }


}
