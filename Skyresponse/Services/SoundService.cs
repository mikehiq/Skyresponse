using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NAudio.Wave;
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
    }

    public class SoundService : ISoundService
    {
        private readonly IPersistenceManager _persistenceManager;
        private readonly ISoundWrapper _soundWrapper;
        private static readonly string SoundPath = ConfigurationManager.AppSettings["SoundPath"];
        private const string PathSettingsKey = "Path";
        private const string DeviceSettingsKey = "Device";
        private Guid _device;
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
            var fileReader = _soundWrapper.FileReader(_path);
            var soundOut = _soundWrapper.DeviceSoundOut(_device);
            soundOut.Init(fileReader);
            soundOut.Play();
        }

        public void SavePath(string path)
        {
            _persistenceManager.Save(PathSettingsKey, path);
            _path = path;
        }

        public void SetOutputDevice(Guid guid)
        {
            _device = guid;
            _persistenceManager.Save(DeviceSettingsKey, guid.ToString());
        }

        public IEnumerable<DeviceInfo> DeviceList
        {
            get
            {
                var directSoundDevices = DirectSoundOut.Devices;
                return directSoundDevices.Select(d => new DeviceInfo(d.Guid, d.Description));
            }
        }

        private void LoadOutputDevice()
        {
            // Load or default
            var deviceString = _persistenceManager.Read(DeviceSettingsKey);
            _device = !string.IsNullOrWhiteSpace(deviceString) ? Guid.Parse(deviceString) : DirectSoundOut.DSDEVID_DefaultPlayback;
        }

        private void LoadPath()
        {
            _path = _persistenceManager.Read(PathSettingsKey);

            if (string.IsNullOrWhiteSpace(_path))
                _path = SoundPath;
        }
    }
}
