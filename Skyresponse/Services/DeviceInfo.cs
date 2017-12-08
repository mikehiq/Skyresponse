using System;

namespace Skyresponse.Services
{
    public class DeviceInfo
    {
        public DeviceInfo()
        {

        }

        public DeviceInfo(Guid id, string deviceName)
        {
            Id = id;
            DeviceName = deviceName;
        }

        public Guid Id { get; set; }
        public string DeviceName { get; set; }

        public override string ToString()
        {
            return DeviceName;
        }
    }
}
