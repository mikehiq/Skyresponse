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
        private readonly AccessTokenResponse _accessTokenResponse = new AccessTokenResponse();

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
        private Dictionary<string, string> GetUserInfoDictionary()
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

            return new Dictionary<string, string>
            {
                {UsernameKey, username},
                {PasswordKey, password},
                {"grant_type", "password"}
            };
        }

        //TODO: Ändra metod ovan och använd nedan property istället, senare!
        public Dictionary<string, string> UserInfoDictionary
        {
            get
            {
                if (_userInfoDictionary == null)
                {
                    _userInfoDictionary = GetUserInfoDictionary();
                }
                return _userInfoDictionary;
            }
            //TODO: maybe add a setter here instead of setting dictionary to null at logout
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
                _accessTokenResponse.AccessToken = await _httpRequest.GetAccessToken(UserInfoDictionary);
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
                ClearUserInfo();
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

        public void ClearUserInfo()
        {
            _persistenceManager.Save(UsernameKey, string.Empty);
            _persistenceManager.Save(PasswordKey, string.Empty);
            _userInfoDictionary = null; //is this really good use? Should I use a setter instead?!?!
        }

        private void ShowErrorDialog()
        {
            _dialogWrapper.ShowMessageBox("Incorrect user name or password!", @"Skyresponse", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowConnectionLostDialog()
        {
            _dialogWrapper.ShowMessageBox("Connection lost!\nPress OK to reconnect.", @"Skyresponse", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
