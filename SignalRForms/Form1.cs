using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.IdentityModel.Tokens;
using SignalRChat.Domain;
using System.Net;
using System.Net.Http.Headers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SignalRForms
{
    public partial class Form1 : Form
    {
        private string _secretKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFsZXhkIiwic3ViIjoiYWxleGQiLCJqdGkiOiJlNGRhOTYyZiIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjQwMjUxIiwiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNDciLCJodHRwOi8vbG9jYWxob3N0OjUwODgiLCJodHRwczovL2xvY2FsaG9zdDo3MjUyIl0sIm5iZiI6MTcxMDE3MDk1MiwiZXhwIjoxNzE4MTE5NzUyLCJpYXQiOjE3MTAxNzA5NTMsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.FO1al65zlUVJ-scB6K3W8ZkXH88peeCV4H7UkN6v0Ew";
        private HubConnection _hubConnection;
        public Form1()
        {
            InitializeComponent();

        }

        //private async Task HubConnection_Closed(Exception? arg)
        //{
        //    await _hubConnection.StartAsync();
        //    lbMessages.Invoke(new Action(() =>
        //      {
        //          lbMessages.Items.Add($"Connection started {DateTime.Now}");
        //      }));
        //}


        private async void Form1_Load(object sender, EventArgs e)
        {

            _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7252/chathub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(_secretKey);
            })
            // by default tries attempts at 0,2,10,30 seconds
            .WithAutomaticReconnect(new[] {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(5)})
            .WithKeepAliveInterval(TimeSpan.FromSeconds(5))
            .Build();

            _hubConnection.On<string>("LoginResponse", (jwtToken) =>
            {
                _secretKey = jwtToken;
            });

            _hubConnection.On<string>("ConnectionDetailsResponse", (details) =>
            {
                lbMessages.Invoke(new Action(() =>
                {
                    lbMessages.Items.Add($"Id conexiune:{details}");
                }));
            });

            _hubConnection.On<string, string>("ReceiveMessage", (user, Message) =>
            {
                var newMessage = $"{user}:{Message}";
                lbMessages.Invoke(new Action(() =>
               {
                   lbMessages.Items.Add(newMessage);
               }));
            });
            _hubConnection.Reconnecting += reconnecting =>
            {
                lbMessages.Invoke(new Action(() =>
                {
                    lbMessages.Items.Add($"reconnecting -PID:{System.Diagnostics.Process.GetCurrentProcess().Id}-date:{DateTime.Now}");
                }));

                return Task.CompletedTask;
            };
            _hubConnection.Reconnected += async reconnected =>
            {
                await _hubConnection.InvokeAsync("ConnectionDetails");
                lbMessages.Invoke(new Action(() =>
                {
                    lbMessages.Items.Add($"reconnected!-PID:{System.Diagnostics.Process.GetCurrentProcess().Id}-date:{DateTime.Now}");
                }));
            };
            _hubConnection.Closed += async closed =>
            {
                lbMessages.BeginInvoke((Action)(() =>
                {
                    lbMessages.Items.Add($"Connection lost - PID: {System.Diagnostics.Process.GetCurrentProcess().Id} - Date: {DateTime.Now}");
                }));

                await Task.Delay(1000);

                await _hubConnection.StartAsync();
            };

            _hubConnection.On<TCPRequestDTO>("StartScaleResponse", (tcprequest) =>
            {
                lbMessages.Invoke(new Action(() =>
                {
                   lbMessages.Items.Add(tcprequest.Response);
                }));
            });

            try
            {
                await _hubConnection.StartAsync();
                await _hubConnection.InvokeAsync("Login", "ScaleApp");
                if (_hubConnection.State == HubConnectionState.Connected)
                {
                    lbMessages.Items.Add("conectat");
                }
            }
            catch (Exception ex)
            {
                lbMessages.Invoke(new Action(() =>
                {
                    lbMessages.Items.Add(ex.Message);
                }));
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                TCPRequestDTO request = new TCPRequestDTO
                {
                    Address = IPAddress.Parse("127.0.0.1"),
                    Port = 143
                };
                await _hubConnection.InvokeAsync("StartScale", request);

                await _hubConnection.InvokeAsync("SendMessage", txtUser.Text, txtMessage.Text);
            }
            catch (Exception ex)
            {
                lbMessages.Invoke(new Action(() => { lbMessages.Items.Add(ex.Message); }));

            }
        }
    }
}
