using Moq;
using Skyresponse.Persistence;
using Skyresponse.Services;
using Skyresponse.SoundWrappers;

namespace SkyresponseXUnitTests.Services
{
    public class TestsFixture
    {
        public Mock<IPersistenceManager> PersistanceManager;
        public Mock<ISoundWrapper> SoundWrapper;
        public SoundService SoundService;
        public string Path;

        public TestsFixture()
        {
            PersistanceManager = new Mock<IPersistenceManager>();
            SoundWrapper = new Mock<ISoundWrapper>();
            SoundService = new SoundService(PersistanceManager.Object, SoundWrapper.Object);
            PersistanceManager.Setup(x => x.Read(It.IsAny<string>())).Returns(Path);
            Path = @"..\..\Media\Sounds\Level2.mp3";
        }
    }
}
