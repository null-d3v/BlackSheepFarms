using Microsoft.AspNetCore.HttpOverrides;
using WebMarkupMin.AspNetCoreLatest;

var webApplicationBuilder = WebApplication.CreateBuilder(args);
webApplicationBuilder.Configuration
    .AddJsonFile("appsettings.json", false, false)
    .AddEnvironmentVariables();

webApplicationBuilder.Logging
    .ClearProviders()
    .AddJsonConsole();

webApplicationBuilder.Services
    .AddSingleton(webApplicationBuilder.Configuration);

webApplicationBuilder.Services
    .AddOptions();

webApplicationBuilder.Services
    .Configure<ForwardedHeadersOptions>(
        options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
webApplicationBuilder.Services
    .AddResponseCompression();
webApplicationBuilder.Services
    .AddWebOptimizer(
        options =>
        {
            options.EnableDiskCache = false;
        },
        !webApplicationBuilder.Environment.IsDevelopment(),
        !webApplicationBuilder.Environment.IsDevelopment());
webApplicationBuilder.Services
    .AddWebMarkupMin()
    .AddHtmlMinification()
    .AddHttpCompression();

webApplicationBuilder.Services.AddRouting(
    routeOptions => routeOptions.LowercaseUrls = true);

var mvcBuilder = webApplicationBuilder.Services
    .AddControllersWithViews();
if (webApplicationBuilder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

using var webApplication = webApplicationBuilder.Build();

if (webApplicationBuilder.Environment.IsDevelopment())
{
    webApplication.UseDeveloperExceptionPage();
}
else
{
    webApplication
        .UseExceptionHandler("/error/500");
    webApplication
        .UseStatusCodePagesWithReExecute("/error/{0}");
}

webApplication
    .UseResponseCompression()
    .UseWebOptimizer()
    .UseWebMarkupMin()
    .UseStaticFiles()
    .UseRouting();

webApplication
    .UseForwardedHeaders();

webApplication.MapControllers();

await webApplication
    .RunAsync()
    .ConfigureAwait(false);