using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using SignalRChat.Hubs;

namespace SignalRChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Vezi aici lista full optiuni server https://learn.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-8.0&tabs=dotnet#configure-server-options
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR(hubOptions =>
             {
                 hubOptions.EnableDetailedErrors = true;
                 hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
                 hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
             });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer();

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ScalesCommunicationAuthPolicy", policy =>
            //    {
            //        policy.Requirements.Add(new ScaleCommunicationRequirement());
            //    });
            //});


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapHub<ChatHub>("/chathub", options =>
            {
                options.Transports =
                    HttpTransportType.WebSockets |
                    HttpTransportType.LongPolling;

                //Dacă un token expiră în timpul duratei de viață a unei conexiuni, în mod implicit conexiunea continuă să funcționeze.
                //Conexiunile LongPolling și ServerSentEvent eșuează la cererile ulterioare dacă nu trimit noi tokenuri de acces.
                //Pentru ca conexiunile să se închidă atunci când token-ul de autentificare expiră, setează
                options.CloseOnAuthenticationExpiration = true;
            }
            );
            app.Run();
        }
    }
}
