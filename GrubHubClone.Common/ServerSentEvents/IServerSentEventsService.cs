using Microsoft.AspNetCore.Http;

namespace GrubHubClone.Common.ServerSentEvents
{
    public interface IServerSentEventsService
    {
        void AddMessageToQueue(string clientId, string message);
        bool ClientIsConnected(string id);
        TaskCompletionSource<bool> GetTaskCompletion(string clientId);
        bool HasMessage(string clientId);
        Task<StreamWriter> Init(HttpContext context);
        void RemoveClient(string clientId);
        Task SendEventToClient<T>(string clientId, T data);
        Task CloseConnection(string clientId);
        Task SendQueuedMessagesToClient(string clientId);
    }
}