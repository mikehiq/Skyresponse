using System;
using System.Collections.Generic;

namespace Skyresponse.Services.Sound
{
    public interface ISoundService
    {
        void PlaySound();
        void SavePath(string path);
        void SetOutputDevice(Guid guid);
        IEnumerable<DeviceInfo> DeviceList { get; }
        bool HasCustomPathSet { get; }
        bool HasDeviceSet { get; }
        Guid Device { get; }
        void LoadPath();
    }
}