using System;
using System.Collections.Generic;
using System.Configuration;
using Skyresponse.Persistence;
using Skyresponse.SoundWrappers;

namespace Skyresponse.Services
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
    }

    public class SoundService : ISoundService
    {
        private readonly IPersistenceManager _persistenceManager;
        private readonly ISoundWrapper _soundWrapper;
        private static readonly string SoundPath = ConfigurationManager.AppSettings["SoundPath"];
        private const string PathSettingsKey = "Path";
        private const string DeviceSettingsKey = "Device";
        private string _path;

        public SoundService(IPersistenceManager persistenceManager, ISoundWrapper soundWrapper)
        {
            _persistenceManager = persistenceManager;
            _soundWrapper = soundWrapper;
            LoadOutputDevice();
            LoadPath();
        }

        public void PlaySound()
        {
            _soundWrapper.Play(_path, Device);
        }

        public void SavePath(string path)
        {
            _persistenceManager.Save(PathSettingsKey, path);
            _path = path;
        }

        public void SetOutputDevice(Guid guid)
        {
            Device = guid;
            _persistenceManager.Save(DeviceSettingsKey, guid.ToString());
        }

        public IEnumerable<DeviceInfo> DeviceList => _soundWrapper.DeviceList;

        public bool HasCustomPathSet => _persistenceManager.HasValue(PathSettingsKey);

        public bool HasDeviceSet => _persistenceManager.HasValue(DeviceSettingsKey);

        public Guid Device { get; private set; }

        private void LoadOutputDevice()
        {
            // Load or default
            var deviceString = _persistenceManager.Read(DeviceSettingsKey);
            Device = !string.IsNullOrWhiteSpace(deviceString) ? Guid.Parse(deviceString) : _soundWrapper.DefaultDevice;
        }

        private void LoadPath()
        {
            _path = _persistenceManager.Read(PathSettingsKey);

            if (string.IsNullOrWhiteSpace(_path))
                _path = SoundPath;
        }
    }
}
