using System;
using System.Collections.Generic;
using Skyresponse.Services;

namespace Skyresponse.Wrappers.SoundWrappers
{
    public interface ISoundWrapper
    {
        void Play(string path, Guid device);
        IEnumerable<DeviceInfo> DeviceList { get; }
        Guid DefaultDevice { get; }
    }
}