using System;

namespace Skyresponse.Wrappers.HttpWrappers
{
    public interface IWebSocketWrapper
    {
        /// <summary>
        /// Connects to the WebSocket server.
        /// </summary>
        /// <returns></returns>
        WebSocketWrapper Connect(string uri);

        /// <summary>
        /// Set the Action to call when the connection has been established.
        /// </summary>
        /// <param name="onConnect">The Action to call.</param>
        /// <returns></returns>
        WebSocketWrapper OnConnect(Action<WebSocketWrapper> onConnect);

        /// <summary>
        /// Set the Action to call when the connection has been terminated.
        /// </summary>
        /// <param name="onDisconnect">The Action to call</param>
        /// <returns></returns>
        WebSocketWrapper OnDisconnect(Action<WebSocketWrapper> onDisconnect);

        /// <summary>
        /// Set the Action to call when a messages has been received.
        /// </summary>
        /// <param name="onMessage">The Action to call.</param>
        /// <returns></returns>
        WebSocketWrapper OnMessage(Action<string, WebSocketWrapper> onMessage);

        /// <summary>
        /// Send a message to the WebSocket server.
        /// </summary>
        /// <param name="message">The message to send</param>
        void SendMessage(string message);
    }
}