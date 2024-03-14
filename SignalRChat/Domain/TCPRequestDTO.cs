using System.Net;

namespace SignalRChat.Domain
{
    public class TCPRequestDTO : Request
    {
        public TCPRequestDTO()
        {
            this.Type = Request.RequestTypes.TCPRequest;
        }
        public IPAddress Address { get;set; }
        public int Port { get; set; }   
        public string PostData { get; set; }
        public int Timeout { get; set; } = 30;
        public int WaitTime { get; set; }
    }
}
