using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly string _secretKey = "cheie_complexa_secreta_exemplu123456123465123456123456";
        public readonly string _jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFsZXhkIiwic3ViIjoiYWxleGQiLCJqdGkiOiJlNGRhOTYyZiIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjQwMjUxIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNDciLCJodHRwOi8vbG9jYWxob3N0OjUwODgiLCJodHRwczovL2xvY2FsaG9zdDo3MjUyIl0sIm5iZiI6MTcxMDE3MDk1MiwiZXhwIjoxNzE4MTE5NzUyLCJpYXQiOjE3MTAxNzA5NTMsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.FO1al65zlUVJ-scB6K3W8ZkXH88peeCV4H7UkN6v0Ew";

        private List<string> _allowedUsers = new List<string> { "ScaleApp", "PrinterApp" };
        public override Task OnConnectedAsync()
        {

            return base.OnConnectedAsync();
        }


        public async Task Login(string username)
        {
            if (_allowedUsers.Contains(username))
                await Clients.Caller.SendAsync("LoginResponse", _jwt);
        }

        public async Task ConnectionDetails()
        {
            await Clients.Caller.SendAsync("ConnectionDetailsResponse", this.Context.ConnectionId);
        }

        [Authorize]
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

            //exemple trimitere 
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            //await Clients.Caller.SendAsync("ReceiveMessage", user, message);
            //await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);
        }

        public async Task StartScale(TCPRequestDTO tcprequest)
        {
            //await Clients.Client(string.Empty).SendAsync()
            //incercare pornire cantar
            try
            {
                var ipEndPoint = new IPEndPoint(tcprequest.Address, tcprequest.Port);

                using TcpClient client = new();
                await client.ConnectAsync(ipEndPoint);
                await using NetworkStream stream = client.GetStream();

                var buffer = new byte[1_024];
                int received = await stream.ReadAsync(buffer);

                tcprequest.Response  = Encoding.UTF8.GetString(buffer, 0, received);
                await Clients.Caller.SendAsync("StartScaleResponse", tcprequest);
            }
            catch (Exception ex)
            {
                tcprequest.Error = ex.InnerException.Message;
                await Clients.Caller.SendAsync("StartScaleResponse", tcprequest);
            }

        }

        [Authorize("Worker")]
        public async Task GetScaleInformation(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        [Authorize("Administrator")]
        public async Task UpdateScaleInfo(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", username) }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}