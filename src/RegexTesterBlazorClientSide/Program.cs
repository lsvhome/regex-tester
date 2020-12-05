using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("RegexUtilsTests")]


namespace RegexTesterBlazorClientSide
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

#if DEBUG
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener { Name = "consoleLog" });
#endif
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
