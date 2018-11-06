using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AzureChat.Startup))]

namespace AzureChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapAzureSignalR(this.GetType().FullName);
        }
    }
}
