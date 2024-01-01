using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Azure.Amqp.Framing;

namespace GrubHubClone.Common.ServerSentEvents;

public class ServerSentEventsService : IServerSentEventsService
{
    private readonly Dictionary<string, StreamWriter> _connectedClients;
    private readonly Dictionary<string, TaskCompletionSource<bool>> _eventTasks;
    private readonly Dictionary<string, Queue<string>> _messages;

    public ServerSentEventsService()
    {
        _connectedClients = new Dictionary<string, StreamWriter>();
        _eventTasks = new();
        _messages = new Dictionary<string, Queue<string>>();
    }

    public async Task<StreamWriter> Init(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Accept") && !context.Request.Headers["Accept"].Contains("text/event-stream"))
        {
            throw new HttpRequestException("Missing headers \"Accept:text/event-stream\".");
        }

        if (!context.Request.Headers.ContainsKey("ClientId") && string.IsNullOrEmpty(context.Request.Headers["ClientId"]))
        {
            throw new HttpRequestException("Missing headers \"ClientId\".");
        }

        string clientId = context.Request.Headers["ClientId"]!;

        lock (_connectedClients)
        {
            if (_connectedClients.ContainsKey(clientId))
            {
                return _connectedClients[clientId];
            }
        }

        var writer = new StreamWriter(context.Response.Body, Encoding.UTF8);

        lock (_connectedClients)
        {
            _connectedClients.Add(clientId, writer);
        }

        lock (_eventTasks)
        {
            _eventTasks.Add(clientId, new TaskCompletionSource<bool>());
        }

        context.Response.Headers.Add("Content-Type", "text/event-stream");
        context.Response.Headers.Add("Cache-Control", "no-cache");
        context.Response.Headers.Add("Connection", "keep-alive");

        await writer.WriteLineAsync("event: info");
        await writer.WriteLineAsync("data: Connected");
        await writer.WriteLineAsync();
        await writer.FlushAsync();

        return writer;
    }

    public bool HasMessage(string clientId)
    {
        lock (_messages)
        {
            return _messages.ContainsKey(clientId);
        }
    }

    public void AddMessageToQueue(string clientId, string message)
    {
        lock (_messages)
        {
            if (!_messages.ContainsKey(clientId)) 
            {
                _messages.Add(clientId, new Queue<string>());
            }
            _messages[clientId].Enqueue(message);
        }
    }

    public async Task SendQueuedMessagesToClient(string clientId)
    {
        if (!HasMessage(clientId)) return;

        Queue<string> queue;

        lock (_messages)
        {
            queue = _messages[clientId];
        }

        while(queue.Count > 0)
        {
            var message = queue.Dequeue();
            await SendEventToClient(clientId, message);
        }

        lock (_messages)
        {
            if (!queue.Any())
            {
                _messages.Remove(clientId);
            }
        }
    }

    public void RemoveClient(string clientId)
    {
        lock (_connectedClients)
        {
            _connectedClients.Remove(clientId);
        }

        lock (_eventTasks)
        {
            _eventTasks.Remove(clientId);
        }

        lock (_messages)
        {
            _messages.Remove(clientId);
        }
    }

    public async Task SendEventToClient<T>(string clientId, T data)
    {
        StreamWriter clientConnection;

        lock (_connectedClients)
        {
            clientConnection = _connectedClients[clientId];
        }

        if (clientConnection == null) return;

        var payload = JsonSerializer.Serialize<T>(data);

        await clientConnection.WriteLineAsync($"event: {clientId}");
        await clientConnection.WriteLineAsync($"data: {payload}");
        await clientConnection.WriteLineAsync();

        await clientConnection.FlushAsync();

        lock (_eventTasks)
        {
            var eventTask = _eventTasks[clientId];
            eventTask.SetResult(true);
        }
    }

    public async Task CloseConnection(string clientId) 
    {
        StreamWriter clientConnection;

        lock (_connectedClients)
        {
            clientConnection = _connectedClients[clientId];
        }

        if (clientConnection == null) return;

        await clientConnection.WriteLineAsync($"event: {clientId}");
        await clientConnection.WriteLineAsync($"data: Connection closed");
        await clientConnection.WriteLineAsync();

        await clientConnection.FlushAsync();
    }

    public bool ClientIsConnected(string id)
    {
        bool isConnected;

        lock (_connectedClients)
        {
            isConnected = _connectedClients.ContainsKey(id);
        }

        return isConnected;
    }

    public TaskCompletionSource<bool> GetTaskCompletion(string clientId)
    {
        lock (_eventTasks)
        {
            if (!_eventTasks.ContainsKey(clientId))
            {
                throw new KeyNotFoundException();
            }

            TaskCompletionSource<bool> eventTask = _eventTasks[clientId];

            if (eventTask.Task.IsCompleted)
            {
                _eventTasks[clientId] = new TaskCompletionSource<bool>();
            }

            return eventTask;
        }
    }
}
