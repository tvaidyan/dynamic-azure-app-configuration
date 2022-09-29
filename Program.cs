using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAzureAppConfiguration();

var connectionString = builder.Configuration.GetConnectionString("AzureAppConfiguration");

builder.Host.ConfigureAppConfiguration(options =>
{
    options.AddAzureAppConfiguration(config =>
    {
        config.Connect(connectionString)
        .ConfigureRefresh(refresh =>
        {
            refresh.Register("version", refreshAll: true)
            .SetCacheExpiration(TimeSpan.FromSeconds(10));
        });
    });
});

builder.Services.Configure<HotColorOfTheMoment>(builder.Configuration.GetSection(nameof(HotColorOfTheMoment)));
var app = builder.Build();
app.UseAzureAppConfiguration();

app.MapGet("/", (IOptionsSnapshot<HotColorOfTheMoment> colorConfig) => $"Hello World!  Hot Color of the Moment: {colorConfig.Value.Color}");

app.Run();

public class HotColorOfTheMoment
{
    public string Color { get; set; } = "Red";
}