using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using dms.Models;

public class ChatWebSocketHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ChatWebSocketHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task HandleWebSocketAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await HandleWebSocketCommunication(webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }

    private async Task HandleWebSocketCommunication(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DmsContext>();

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var parts = message.Split('|');
                var senderId = int.Parse(parts[0]);
                var receiverId = int.Parse(parts[1]);
                var chatMessage = new ChatMessage
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Message = parts[2],
                    Timestamp = DateTime.Now
                };

                context.ChatMessages.Add(chatMessage);
                await context.SaveChangesAsync();
            }

            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}
