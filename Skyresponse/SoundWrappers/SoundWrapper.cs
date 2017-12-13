using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using Skyresponse.Services;

namespace Skyresponse.SoundWrappers
{
    public interface ISoundWrapper
    {
        void Play(string path, Guid device);
        IEnumerable<DeviceInfo> DeviceList { get; }
        Guid DefaultDevice { get; }
    }

    public class SoundWrapper : ISoundWrapper
    {
        public void Play(string path, Guid device)
        {
            var fileReader = FileReader(path);
            var soundOut = DeviceSoundOut(device);
            soundOut.Init(fileReader);
            soundOut.Play();
        }

        public IEnumerable<DeviceInfo> DeviceList
        {
            get
            {
                var directSoundDevices = DirectSoundOut.Devices;
                return directSoundDevices.Select(d => new DeviceInfo(d.Guid, d.Description));
            }
        }

        public Guid DefaultDevice => DirectSoundOut.DSDEVID_DefaultPlayback;

        private static Mp3FileReader FileReader(string path)
        {
            return new Mp3FileReader(path);
        }

        private static DirectSoundOut DeviceSoundOut(Guid device)
        {
            return new DirectSoundOut(device);
        }
    }
}
