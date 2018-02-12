using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Skyresponse.Api;
using Skyresponse.Properties;
using Skyresponse.Services;
using Skyresponse.Services.Sound;
using Skyresponse.Services.User;
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
        private readonly IUserService _userService;
        private NotifyIcon _notifyIcon;
        private IEnumerable<DeviceInfo> _deviceList;
        private MenuItem _defaultMenuItem;
        private MenuItem _customMenuItem;

        public SystemTrayApplicationContext(IDialogWrapper fileDialog, ISkyresponseApi skyresponseApi, ISoundService soundService, IUserService userService)
        {
            _fileDialog = fileDialog;
            _skyresponseApi = skyresponseApi;
            _soundService = soundService;
            _userService = userService;
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
                _soundService.LoadPath();
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
            _userService.ClearUserInfo();
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
