using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Moq;
using NUnit.Framework;
using Skyresponse.Forms;
using Skyresponse.Persistence;
using Skyresponse.Services.User;
using Skyresponse.Wrappers.DialogWrappers;
using Skyresponse.Wrappers.HttpWrappers;

namespace SkyresponseUnitTest.Services.User
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IPersistenceManager> _persistanceManager;
        private Mock<ILoginForm> _loginForm;
        private Mock<IHttpWrapper> _httpRequest;
        private Mock<IDialogWrapper> _dialogWrapper;
        private UserService _userService;
        private const string Username = "hiq@srd";
        private const string Password = "secretpassword";
        private const string Granttype = "password";

        [SetUp]
        public void Init()
        {
            _persistanceManager = new Mock<IPersistenceManager>();
            _loginForm = new Mock<ILoginForm>();
            _httpRequest = new Mock<IHttpWrapper>();
            _dialogWrapper = new Mock<IDialogWrapper>();
            _userService = new UserService(_persistanceManager.Object, _loginForm.Object, _httpRequest.Object, _dialogWrapper.Object);
        }

        [Test]
        public void GivenUserInfoSaved_ReturnUserInfo()
        {
            MockUserInfo(Username, Password);

            var resultDictionary = _userService.GetUserInfoDictionary();
            Assert.That(resultDictionary, Is.EquivalentTo(CreateLoginInfoDictionary()));
        }

        [Test]
        public void GivenLoginFormCreated_DontCallShowDialog()
        {
            MockUserInfo(Username, Password);

            _loginForm.Setup(x => x.Created).Returns(true);
            _userService.GetUserInfoDictionary();

            _loginForm.Verify(x => x.ShowDialog(), Times.Never);
        }

        [Test]
        public void GivenNoUserInfoSaved_GetFromLoginForm()
        {
            MockUserInfo(string.Empty, string.Empty);

            _loginForm.Setup(x => x.Created).Returns(false);
            _loginForm.Setup(x => x.ShowDialog()).Returns(DialogResult.OK);
            _loginForm.Setup(x => x.UserName).Returns(Username);
            _loginForm.Setup(x => x.Password).Returns(Password);

            var resultDictionary = _userService.GetUserInfoDictionary();
            Assert.That(resultDictionary, Is.EquivalentTo(CreateLoginInfoDictionary()));
        }

        [Test]
        public void LoginTest()
        {
            _httpRequest.Setup(x => x.GetAccessToken(It.IsAny<Dictionary<string, string>>()))
                .Throws(new UnauthorizedAccessException());

            _userService.GetAccessTokenResponse();

            _persistanceManager.Verify(x => x.ClearUserInfo(), Times.Once);
            _dialogWrapper.Verify(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButtons>(), It.IsAny<MessageBoxIcon>()), Times.Once);
        }

        [Test]
        public void SaveUserInfoTest()
        {
            //Arrange
            _persistanceManager.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            _persistanceManager.Setup(x => x.SaveSecure(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            _userService.SaveUserInfo();

            //Assert
            _persistanceManager.Verify(x => x.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _persistanceManager.Verify(x => x.SaveSecure(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        private void MockUserInfo(string username, string password)
        {
            _persistanceManager.Setup(x => x.Read(It.IsAny<string>())).Returns(username);
            _persistanceManager.Setup(x => x.ReadSecure(It.IsAny<string>())).Returns(password);
        }

        private static Dictionary<string, string> CreateLoginInfoDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                { "UserName", Username },
                { "Password", Password },
                { "grant_type", Granttype }
            };
            return dict;
        }
    }
}
