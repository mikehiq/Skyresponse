using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Skyresponse.Api;
using Skyresponse.Persistence;
using Skyresponse.Properties;
using Skyresponse.Services;
using Skyresponse.Services.Sound;
using Skyresponse.Wrappers.DialogWrappers;

namespace Skyresponse.Systemtray
{
    public interface ISystemTrayApplicationContext
    {
    }

    public class SystemTrayApplicationContext : ApplicationContext, ISystemTrayApplicationContext
    {
        private readonly IDialogWrapper _fileDialog;
        private readonly ISkyresponseApi _skyresponseApi;
        private readonly ISoundService _soundService;
        private readonly IPersistenceManager _persistenceManager;
        private NotifyIcon _notifyIcon;
        private IEnumerable<DeviceInfo> _deviceList;
        MenuItem _defaultMenuItem;
        MenuItem _customMenuItem;

        public SystemTrayApplicationContext(IDialogWrapper fileDialog, ISkyresponseApi skyresponseApi, ISoundService soundService, IPersistenceManager persistenceManager)
        {
            _fileDialog = fileDialog;
            _skyresponseApi = skyresponseApi;
            _soundService = soundService;
            _persistenceManager = persistenceManager;
            Init();
        }

        private void Init()
        {
            _skyresponseApi.InitAsync();
            PopulateDeviceList();
            CreateNotifyIcon();
        }

        private void CreateNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Resources.Skyresponse_white_small,
                ContextMenu = new ContextMenu(),
                Visible = true
            };
            var soundMenu = new MenuItem("Sound");
            _defaultMenuItem = new MenuItem("Default", SoundMenu_Click) { Checked = !_soundService.HasCustomPathSet };
            soundMenu.MenuItems.Add(_defaultMenuItem);
            _customMenuItem = new MenuItem("Custom", SoundMenu_Click) { Checked = _soundService.HasCustomPathSet };
            soundMenu.MenuItems.Add(_customMenuItem);
            _notifyIcon.ContextMenu.MenuItems.Add(soundMenu);

            var deviceMenu = new MenuItem("Device");
            deviceMenu.MenuItems.AddRange(_deviceList.Select(d => new MenuItem(d.DeviceName, SelectedDeviceOut_Click) { Checked = IsDeviceChecked(d)}).ToArray());
            _notifyIcon.ContextMenu.MenuItems.Add(deviceMenu);

            _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Logout", Logout));

            _notifyIcon.Text = @"Skyresponse ljudmonitor";
        }

        #region Click events
        private void SoundMenu_Click(object sender, EventArgs e)
        {
            if (sender == _customMenuItem)
            {
                _customMenuItem.Checked = true;
                _defaultMenuItem.Checked = false;
                BrowseSoundFile();
            }
            else
            {
                _defaultMenuItem.Checked = true;
                _customMenuItem.Checked = false;
                _soundService.SavePath(string.Empty);
            }
        }

        private bool IsDeviceChecked(DeviceInfo deviceInfo)
        {
            return _soundService.Device == deviceInfo.Guid;
        }

        private void SelectedDeviceOut_Click(object sender, EventArgs e)
        {
            var item = (MenuItem)sender;
            UncheckAllMenuItems(item);
            var deviceInfo = _deviceList.First(d => d.DeviceName.Equals(item.Text));
            item.Checked = true;
            _soundService.SetOutputDevice(deviceInfo.Guid);
        }

        private void Logout(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _notifyIcon.Visible = false;
            _persistenceManager.ClearUserInfo();
            Application.Restart();
            Environment.Exit(0);
        }
        #endregion

        private void BrowseSoundFile()
        {
            _fileDialog.ShowFileDialog();
            string path = _fileDialog.FileName;
            _soundService.SavePath(path);
        }

        private static void UncheckAllMenuItems(MenuItem item)
        {
            foreach (var parentObject in item.Parent.MenuItems)
            {
                var menuItem = (MenuItem)parentObject;
                menuItem.Checked = false;
            }
        }

        private void PopulateDeviceList()
        {
            _deviceList = _soundService.DeviceList;
        }
    }
}
