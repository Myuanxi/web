using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using dms.Models;
using Microsoft.EntityFrameworkCore;

public class ChatWebSocketHandler
{
    private readonly DmsContext _context;

    public ChatWebSocketHandler(DmsContext context)
    {
        _context = context;
    }

    private static ConcurrentDictionary<WebSocket, string> _sockets = new ConcurrentDictionary<WebSocket, string>();

    public async Task HandleAsync(WebSocket socket)
    {
        _sockets.TryAdd(socket, null);

        var buffer = new byte[1024 * 4];
        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var messageParts = message.Split('|');
            if (messageParts.Length == 3)
            {
                var senderId = int.Parse(messageParts[0]);
                var receiverId = int.Parse(messageParts[1]);
                var chatMessage = messageParts[2];

                // 保存聊天记录到数据库
                var chatRecord = new ChatMessage
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Message = chatMessage,
                    Timestamp = DateTime.Now
                };
                _context.ChatMessages.Add(chatRecord);
                await _context.SaveChangesAsync();

                // 广播消息
                await BroadcastMessageAsync($"{senderId}|{chatMessage}");
            }

            result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        _sockets.TryRemove(socket, out _);
        await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }

    private async Task BroadcastMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sockets.Keys)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
