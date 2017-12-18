using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Skyresponse.DialogWrappers;
using Skyresponse.Forms;
using Skyresponse.HttpWrappers;
using Skyresponse.Persistence;
using Skyresponse.Services;

namespace Skyresponse.Api
{
    public interface ISkyresponseApi
    {
        Task InitAsync();
        bool IsLoggedIn { get; set; }
    }

    public class SkyresponseApi : ISkyresponseApi
    {
        private readonly IPersistenceManager _persistenceManager;
        private readonly IHttpWrapper _httpRequest;
        private readonly IWebSocketWrapper _webSocket;
        private readonly ISoundService _soundService;
        private readonly IDialogWrapper _dialog;
        private readonly ILoginForm _loginForm;
        private readonly System.Timers.Timer _timer;
        private readonly List<string> _alreadyPlayed;
        private const string UsernameKey = "UserName";
        private const string PasswordKey = "Password";
        private static readonly string WebSocketUrl = ConfigurationManager.AppSettings["WebSocketUrl"];
        private string _accesstoken;

        public bool IsLoggedIn { get; set; }

        public SkyresponseApi(IPersistenceManager persistenceManager, ILoginForm loginForm, IHttpWrapper httpRequest, IWebSocketWrapper webSocket, ISoundService soundService, IDialogWrapper dialog)
        {
            _persistenceManager = persistenceManager;
            _loginForm = loginForm;
            _httpRequest = httpRequest;
            _webSocket = webSocket;
            _soundService = soundService;
            _dialog = dialog;
            _alreadyPlayed = new List<string>();
            _timer = new System.Timers.Timer { Interval = 10000 };
            _timer.Elapsed += OnTimeUp;
        }

        /// <summary>
        /// Init method
        /// </summary>
        /// <returns></returns>
        public async Task InitAsync()
        {
            var username = _persistenceManager.Read(UsernameKey);
            var password = _persistenceManager.ReadSecure(PasswordKey);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                var result = DialogResult.None;
                if (!_loginForm.Created)
                {
                    result = _loginForm.ShowDialog();
                }
                if (result.Equals(DialogResult.OK))
                {
                    username = _loginForm.UserName;
                    password = _loginForm.Password;
                }
            }
            await Login(username, password);
        }

        /// <summary>
        /// Login to SkyResponse API and get access token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task Login(string username, string password)
        {
            var accessToken = string.Empty;
            var dict = new Dictionary<string, string>
            {
                {UsernameKey, username},
                {PasswordKey, password},
                {"grant_type", "password"}
            };

            try
            {
                accessToken = await _httpRequest.GetAccessToken(dict);
            }
            catch (UnauthorizedAccessException)
            {
                _persistenceManager.ClearUserInfo();
                _dialog.ShowMessageBox("Incorrect user name or password!", @"Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error); //TODO: flytta bort härifrån, FULT!
                await InitAsync();
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                IsLoggedIn = true;
                _accesstoken = accessToken;
                _persistenceManager.Save(UsernameKey, username);
                _persistenceManager.SaveSecure(PasswordKey, password);

                try
                {
                    await _httpRequest.RegisterForPush(_accesstoken);
                }
                catch (HttpRequestException)
                {
                    ReConnect();
                }

                try
                {
                    var webSocketUrl = string.Concat(WebSocketUrl, _accesstoken);
                    _webSocket.Connect(webSocketUrl);
                }
                catch (WebSocketException)
                {
                    ReConnect();
                }

                _webSocket.OnMessage(OnMessageAsync);
                _webSocket.OnDisconnect(OnDisconnect);
                _timer.Stop();
            }
        }

        /// <summary>
        /// Try to reconnect
        /// </summary>
        /// <param name="webSocketWrapper"></param>
        private void OnDisconnect(WebSocketWrapper webSocketWrapper)
        {
            ReConnect();
        }

        /// <summary>
        /// Calls OnTimeUp when timer runs out
        /// </summary>
        private void ReConnect()
        {
            _timer.Enabled = true;
            _timer.Start();
        }

        /// <summary>
        /// Calls InitAsync
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnTimeUp(object sender, EventArgs e)
        {
            await InitAsync();
        }

        /// <summary>
        /// Play sound when recieving message with active alarm
        /// </summary>
        /// <param name="message"></param>
        /// <param name="socketWrapper"></param>
        private async void OnMessageAsync(string message, WebSocketWrapper socketWrapper)
        {
            var webSocketDefinition = new { globalAlarmId = "", active = true }; //active = true is active alarm
            var webSocketMessage = message.GetJsonObject(webSocketDefinition);
            if (!_alreadyPlayed.Contains(webSocketMessage.globalAlarmId))
            {
                _alreadyPlayed.Add(webSocketMessage.globalAlarmId);
                var alarmInfo = await _httpRequest.GetAlarmInfo(webSocketMessage.globalAlarmId, _accesstoken);
                if (webSocketMessage.active && alarmInfo > 0)
                {
                    _soundService.PlaySound();
                }
            }
        }
    }
}
