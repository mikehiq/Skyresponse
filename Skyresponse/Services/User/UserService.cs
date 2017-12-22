using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Skyresponse.Forms;
using Skyresponse.Persistence;
using Skyresponse.Wrappers.DialogWrappers;
using Skyresponse.Wrappers.HttpWrappers;

namespace Skyresponse.Services.User
{
    public class UserService : IUserService
    {
        private readonly IPersistenceManager _persistenceManager;
        private readonly ILoginForm _loginForm;
        private readonly IHttpWrapper _httpRequest;
        private readonly IDialogWrapper _dialogWrapper;
        private const string UsernameKey = "UserName";
        private const string PasswordKey = "Password";
        private Dictionary<string, string> _userInfoDictionary;
        private AccessTokenResponse _accessTokenResponse = new AccessTokenResponse();

        public UserService(IPersistenceManager persistenceManager, ILoginForm loginForm, IHttpWrapper httpRequest, IDialogWrapper dialogWrapper)
        {
            _persistenceManager = persistenceManager;
            _loginForm = loginForm;
            _httpRequest = httpRequest;
            _dialogWrapper = dialogWrapper;
        }
        /// <summary>
        /// Returns a dictionary with user info
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetUserInfoDictionary()
        {
            var username = _persistenceManager.Read(UsernameKey);
            var password = _persistenceManager.ReadSecure(PasswordKey);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                var result = DialogResult.None;
                if (!_loginForm.Created)
                    result = _loginForm.ShowDialog();

                if (result.Equals(DialogResult.OK))
                {
                    username = _loginForm.UserName;
                    password = _loginForm.Password;
                }
            }

            return _userInfoDictionary = new Dictionary<string, string>
            {
                {UsernameKey, username},
                {PasswordKey, password},
                {"grant_type", "password"}
            };
        }

        /// <inheritdoc />
        /// <summary>
        /// Return access token
        /// </summary>
        /// <returns></returns>
        public async Task GetAccessTokenResponse()
        {
            try
            {
                _accessTokenResponse.AccessToken = await _httpRequest.GetAccessToken(GetUserInfoDictionary());
                _accessTokenResponse.Success = true;
            }
            catch (HttpRequestException)
            {
                _accessTokenResponse.Success = false;
                ShowConnectionLostDialog();
            }
            catch (UnauthorizedAccessException)
            {
                _accessTokenResponse.Success = false;
                _persistenceManager.ClearUserInfo();
                ShowErrorDialog();
            }
        }

        public async Task<string> GetAccessToken()
        {
            while (true)
            {
                await GetAccessTokenResponse();
                if (_accessTokenResponse.Success)
                    return _accessTokenResponse.AccessToken;
            }
        }

        public void SaveUserInfo()
        {
            _persistenceManager.Save(UsernameKey, _userInfoDictionary[UsernameKey]);
            _persistenceManager.SaveSecure(PasswordKey, _userInfoDictionary[PasswordKey]);
        }

        private void ShowErrorDialog()
        {
            _dialogWrapper.ShowMessageBox("Incorrect user name or password!", @"Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowConnectionLostDialog()
        {
            _dialogWrapper.ShowMessageBox("Connection lost!\nPress OK to reconnect.", @"Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
