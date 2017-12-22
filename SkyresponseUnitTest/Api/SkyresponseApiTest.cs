using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Moq;
using NUnit.Framework;
using Skyresponse.Api;
using Skyresponse.Forms;
using Skyresponse.Persistence;
using Skyresponse.Services;
using Skyresponse.Services.Sound;
using Skyresponse.Services.User;
using Skyresponse.Wrappers.DialogWrappers;
using Skyresponse.Wrappers.HttpWrappers;

namespace SkyresponseUnitTest.Api
{
    [TestFixture]
    public class SkyresponseApiTest
    {
        private Mock<IHttpWrapper> _httpRequest;
        private Mock<ISoundService> _soundService;
        private Mock<IWebSocketWrapper> _webSocket;
        private Mock<IUserService> _userService;
        private SkyresponseApi _skyresponseApi;
        private string username = "hiq@srd";
        private string password = "secretpassword";
        private string granttype = "password";

        [SetUp]
        public void Before()
        {
            _httpRequest = new Mock<IHttpWrapper>();
            _soundService = new Mock<ISoundService>();
            _webSocket = new Mock<IWebSocketWrapper>();
            _userService = new Mock<IUserService>();

            _skyresponseApi = new SkyresponseApi(_httpRequest.Object, _webSocket.Object, _soundService.Object, _userService.Object);
        }

        //[Test]
        //public void LoginTest()
        //{
        //    _httpRequest.Setup(x => x.GetAccessToken(It.IsAny<Dictionary<string, string>>()))
        //        .Throws(new UnauthorizedAccessException());

        //    _skyresponseApi.Login(CreateLoginInfoDictionary());

        //    _persistanceManager.Verify(x => x.ClearUserInfo(), Times.Once);
        //    _dialog.Verify(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButtons>(), It.IsAny<MessageBoxIcon>()), Times.Once);
        //}

        private Dictionary<string, string> CreateLoginInfoDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                { "UserName", username },
                { "Password", password },
                { "grant_type", granttype }
            };
            return dict;
        }
    }
}
