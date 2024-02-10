using IPV6ConfigBlazorWA;
using IPV6ConfigBlazorWA.Model;
using IPV6ConfigBlazorWA.Model.DataModel;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazorBootstrap();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddHttpClient<IIPV6ConfigSetupService, IPV6ConfigSetupService>
//    ("BaseUrl", client => client.BaseAddress = new Uri("https://localhost:7239/api/"));

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddTransient<IIPV6ConfigSetupService<IPV6_PrimarySubnetModel>, IPV6ConfigSetupService<IPV6_PrimarySubnetModel>>();
builder.Services.AddTransient<IIPV6ConfigSetupService<IPV6_DivisionSubnet32Model>, IPV6ConfigSetupService<IPV6_DivisionSubnet32Model>>();
builder.Services.AddTransient<IIPV6ConfigSetupService<IPV6_UserTypeSubnet36Model>, IPV6ConfigSetupService<IPV6_UserTypeSubnet36Model>>();
builder.Services.AddTransient<IIPV6ConfigSetupService<IPV6_CitySubnet44Model>, IPV6ConfigSetupService<IPV6_CitySubnet44Model>>();
builder.Services.AddTransient<IIPV6ConfigSetupService<IPV6_BTSSubnet48Model>, IPV6ConfigSetupService<IPV6_BTSSubnet48Model>>();
builder.Services.AddTransient<IIPV6ConfigSetupService<IPV6_ParentSubnet56Model>, IPV6ConfigSetupService<IPV6_ParentSubnet56Model>>();
builder.Services.AddTransient<IIPV6ConfigSetupService<IPV6_CustomerSubnet64Model>, IPV6ConfigSetupService<IPV6_CustomerSubnet64Model>>();






await builder.Build().RunAsync();
