using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MicrotikBlazorWA;
using MicrotikBlazorWA.Socket;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7219/api/") });
builder.Services.AddScoped<ISocketService, SocketSerivce>();

await builder.Build().RunAsync();
