using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Skyresponse.Api;
using Skyresponse.Services.Sound;
using Skyresponse.Services.User;
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
        //private string username = "hiq@srd";
        //private string password = "secretpassword";
        //private string granttype = "password";

        [SetUp]
        public void Before()
        {
            _httpRequest = new Mock<IHttpWrapper>();
            _soundService = new Mock<ISoundService>();
            _webSocket = new Mock<IWebSocketWrapper>();
            _userService = new Mock<IUserService>();

            _skyresponseApi = new SkyresponseApi(_httpRequest.Object, _webSocket.Object, _soundService.Object, _userService.Object);
        }

        [Test]
        public void LoginTest()
        {
            _httpRequest.Setup(x => x.GetAccessToken(It.IsAny<Dictionary<string, string>>()))
                .Throws(new UnauthorizedAccessException());

            _skyresponseApi.Login();
        }
    }
}
