using System;
using NAudio.Wave;

namespace Skyresponse.SoundWrappers
{
    public interface ISoundWrapper
    {
        Mp3FileReader FileReader(string path);
        DirectSoundOut DeviceSoundOut(Guid device);
    }

    public class SoundWrapper : ISoundWrapper
    {
        public Mp3FileReader FileReader(string path)
        {
            return new Mp3FileReader(path);
        }

        public DirectSoundOut DeviceSoundOut(Guid device)
        {
            return new DirectSoundOut(device);
        }
    }
}
