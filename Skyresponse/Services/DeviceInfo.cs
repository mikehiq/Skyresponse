using System;

namespace Skyresponse.Services
{
    public class DeviceInfo
    {
        public DeviceInfo()
        {

        }

        public DeviceInfo(Guid guid, string deviceName)
        {
            Guid = guid;
            DeviceName = deviceName;
        }

        public Guid Guid { get; set; }
        public string DeviceName { get; set; }

        public override string ToString()
        {
            return DeviceName;
        }
    }
}
