using System;
using Moq;
using NUnit.Framework;
using Skyresponse.Persistence;
using Skyresponse.Services.Sound;
using Skyresponse.Wrappers.SoundWrappers;

namespace SkyresponseUnitTest.Services.Sound
{
    [TestFixture]
    public class SoundServiceTest
    {
        private Mock<IPersistenceManager> _persistanceManager;
        private Mock<ISoundWrapper> _soundWrapper;
        private SoundService _soundService;
        private const string Path = @"..\..\Media\Sounds\Level2.mp3";

        [SetUp]
        public void Init()
        {
            _persistanceManager = new Mock<IPersistenceManager>();
            _soundWrapper = new Mock<ISoundWrapper>();
            _soundService = new SoundService(_persistanceManager.Object, _soundWrapper.Object);

            _persistanceManager.Setup(x => x.Read(It.IsAny<string>())).Returns(Path);
        }

        [Test]
        public void PlaySoundTest()
        {
            //Act
            _soundService.PlaySound();

            //Assert
            _soundWrapper.Verify(x => x.Play(It.IsAny<string>(), Guid.Empty), Times.Once);
        }

        [Test]
        public void SavePathTest()
        {
            //Arrange
            _persistanceManager.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            _soundService.SavePath(Path);

            //Assert
            _persistanceManager.Verify(x => x.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void SetOutputDeviceTest()
        {
            //Arrange
            _persistanceManager.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            _soundService.SetOutputDevice(Guid.Empty);

            //Assert
            _persistanceManager.Verify(x => x.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
