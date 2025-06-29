using Api.Hubs;
using Api.Hubs.Trackers;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSignalR(options =>
{
	options.EnableDetailedErrors = true;
});

builder.Services.AddSingleton<BoardTracker>();
builder.Services.AddSingleton<MatchTracker>();

var app = builder.Build();
app.Use(async (context, next) =>
{
	try
	{
		await next();
	}
	catch (Exception ex)
	{
		var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "Unhandled exception");
		throw;
	}
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ChatHub>("/hub");
app.MapHub<MatchmakingHub>("/hub/matchmaking");

app.Run();