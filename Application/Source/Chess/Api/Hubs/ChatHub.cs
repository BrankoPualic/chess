using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public interface IChatClient
{
	Task ReceiveMessage(long username, string message);
}

public class ChatHub : Hub<IChatClient>
{
	public async Task SendMessage(long username, string message) => await Clients.All.ReceiveMessage(username, message);
}