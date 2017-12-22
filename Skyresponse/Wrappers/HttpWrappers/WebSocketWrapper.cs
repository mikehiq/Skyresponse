using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Skyresponse.Wrappers.HttpWrappers
{
    public class WebSocketWrapper : IWebSocketWrapper
    {
        private const int ReceiveChunkSize = 1024;
        private const int SendChunkSize = 1024;

        private ClientWebSocket _ws;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

        private Action<WebSocketWrapper> _onConnected;
        private Action<string, WebSocketWrapper> _onMessage;
        private Action<WebSocketWrapper> _onDisconnected;

        public WebSocketWrapper()
        {
            _cancellationToken = _cancellationTokenSource.Token;
        }

        /// <inheritdoc />
        /// <summary>
        /// Connects to the WebSocket server.
        /// </summary>
        /// <returns></returns>
        public WebSocketWrapper Connect(string uri)
        {
            _ws = new ClientWebSocket {Options = {KeepAliveInterval = TimeSpan.FromSeconds(20)}};
            try
            {
                ConnectAsync(uri);
            }
            catch (Exception ex)
            {
                var exception = new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex);
                throw exception;
            }
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Set the Action to call when the connection has been established.
        /// </summary>
        /// <param name="onConnect">The Action to call.</param>
        /// <returns></returns>
        public WebSocketWrapper OnConnect(Action<WebSocketWrapper> onConnect)
        {
            _onConnected = onConnect;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Set the Action to call when the connection has been terminated.
        /// </summary>
        /// <param name="onDisconnect">The Action to call</param>
        /// <returns></returns>
        public WebSocketWrapper OnDisconnect(Action<WebSocketWrapper> onDisconnect)
        {
            _onDisconnected = onDisconnect;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Set the Action to call when a messages has been received.
        /// </summary>
        /// <param name="onMessage">The Action to call.</param>
        /// <returns></returns>
        public WebSocketWrapper OnMessage(Action<string, WebSocketWrapper> onMessage)
        {
            _onMessage = onMessage;
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Send a message to the WebSocket server.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void SendMessage(string message)
        {
            SendMessageAsync(message);
        }

        private async void SendMessageAsync(string message)
        {
            if (_ws.State != WebSocketState.Open)
            {
                throw new Exception("Connection is not open.");
            }

            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var messagesCount = (int) Math.Ceiling((double) messageBuffer.Length / SendChunkSize);

            for (var i = 0; i < messagesCount; i++)
            {
                var offset = (SendChunkSize * i);
                var count = SendChunkSize;
                var lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > messageBuffer.Length)
                {
                    count = messageBuffer.Length - offset;
                }

                await _ws.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Text,
                    lastMessage, _cancellationToken);
            }
        }

        private async void ConnectAsync(string uri)
        {
            var connectUri = new Uri(uri);
            try
            {
                await _ws.ConnectAsync(connectUri, _cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            await CallOnConnected();
            StartListen();
        }

        private async void StartListen()
        {
            var buffer = new byte[ReceiveChunkSize];

            try
            {
                while (_ws.State == WebSocketState.Open)
                {
                    var stringResult = new StringBuilder();


                    WebSocketReceiveResult result;
                    do
                    {
                        result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty,
                                CancellationToken.None);
                            await CallOnDisconnected();
                        }
                        else
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    await CallOnMessage(stringResult);

                }
            }
            catch (Exception)
            {
                await CallOnDisconnected();
            }
            finally
            {
                _ws.Dispose();
            }
        }

        private async Task CallOnMessage(StringBuilder stringResult)
        {
            if (_onMessage != null)
                await RunInTask(() => _onMessage(stringResult.ToString(), this));
        }

        private async Task CallOnDisconnected()
        {
            if (_onDisconnected != null)
                await RunInTask(() => _onDisconnected(this));
        }

        private async Task CallOnConnected()
        {
            if (_onConnected != null)
                await RunInTask(() => _onConnected(this));
        }

        private static async Task RunInTask(Action action)
        {
            await Task.Factory.StartNew(action);
        }
    }
}