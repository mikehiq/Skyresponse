using System;
using Moq;
using NUnit.Framework;
using Skyresponse.Forms;
using Skyresponse.Wrappers.DialogWrappers;

namespace SkyresponseUnitTest.Login
{
    [TestFixture]
    public class LoginFormTest
    {
        private Mock<IDialogWrapper> _dialogWrapper;
        private LoginForm _loginForm;

        [SetUp]
        public void Init()
        {
            _dialogWrapper = new Mock<IDialogWrapper>();
            _loginForm = new LoginForm(_dialogWrapper.Object);
        }

        [Test]
        public void TestMethod1()
        {

        }
    }
}
