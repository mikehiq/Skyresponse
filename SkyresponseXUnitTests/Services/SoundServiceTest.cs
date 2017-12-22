using System;
using Moq;
using Xunit;

namespace SkyresponseXUnitTests.Services
{
    public class SoundServiceTest : IClassFixture<TestsFixture>
    {
        private readonly TestsFixture _fixture;

        public SoundServiceTest(TestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void PlaySoundTest()
        {
            //Act
            _fixture.SoundService.PlaySound();

            //Assert
            _fixture.SoundWrapper.Verify(x => x.Play(It.IsAny<string>(), Guid.Empty), Times.Once);
        }

        [Fact]
        public void SavePathTest()
        {
            //Arrange
            _fixture.PersistanceManager.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            _fixture.SoundService.SavePath(_fixture.Path);

            //Assert
            _fixture.PersistanceManager.Verify(x => x.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SetOutputDeviceTest()
        {
            //Arrange
            _fixture.PersistanceManager.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            //Act
            _fixture.SoundService.SetOutputDevice(Guid.Empty);

            //Assert
            _fixture.PersistanceManager.Verify(x => x.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void HasDeviceNotSetTest()
        {
            //Arrange
            _fixture.PersistanceManager.Setup(x => x.HasValue(It.IsAny<string>())).Verifiable();

            //Act
            var deviceSet = _fixture.SoundService.HasDeviceSet;

            //Assert
            Assert.False(deviceSet);
        }

        [Fact]
        public void HasCustomPathNotSetTest()
        {
            //Arrange
            _fixture.PersistanceManager.Setup(x => x.HasValue(It.IsAny<string>())).Verifiable();

            //Act
            var customPathSet = _fixture.SoundService.HasCustomPathSet;

            //Assert
            Assert.False(customPathSet);
        }
    }
}
