namespace SignalRChat.Domain
{
    public class Request
    {
        public enum RequestTypes
        {
            TCPRequest
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public RequestTypes Type { get; set; }

        public string Response { get; set; }
        public string Error { get; set; }

    }
}
