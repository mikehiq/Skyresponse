using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Skyresponse.Api;
using Skyresponse.DialogWrappers;
using Skyresponse.Properties;
using Skyresponse.Services;

namespace Skyresponse.Systemtray
{
    public class SystemTrayApplicationContext : ApplicationContext
    {
        private readonly IDialogWrapper _fileDialog;
        private readonly ISkyresponseApi _skyresponseApi;
        private readonly ISoundService _soundService;
        private NotifyIcon _notifyIcon;
        private IEnumerable<DeviceInfo> _deviceList;
        MenuItem _defaultMenuItem;
        MenuItem _customMenuItem;

        public SystemTrayApplicationContext(IDialogWrapper fileDialog, ISkyresponseApi skyresponseApi, ISoundService soundService)
        {
            _fileDialog = fileDialog;
            _skyresponseApi = skyresponseApi;
            _soundService = soundService;
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
            _defaultMenuItem = new MenuItem("Default", SoundMenu_Click);
            soundMenu.MenuItems.Add(_defaultMenuItem);
            _defaultMenuItem.Checked = true;
            _customMenuItem = new MenuItem("Custom", SoundMenu_Click);
            soundMenu.MenuItems.Add(_customMenuItem);
            _notifyIcon.ContextMenu.MenuItems.Add(soundMenu);

            var deviceMenu = new MenuItem("Device");
            deviceMenu.MenuItems.AddRange(_deviceList.Select(d => new MenuItem(d.DeviceName, SelectedDeviceOut_Click)).ToArray());
            _notifyIcon.ContextMenu.MenuItems.Add(deviceMenu);

            _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", Exit));

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
                _soundService.SavePath(ConfigurationManager.AppSettings["SoundPath"]);
            }
        }

        private void SelectedDeviceOut_Click(object sender, EventArgs e)
        {
            var item = sender as MenuItem;
            UncheckAllMenuItems(item);
            var deviceInfo = _deviceList.First(d => d.DeviceName.Equals(item.Text));
            item.Checked = true;
            _soundService.SetOutputDevice(deviceInfo.Guid);
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _notifyIcon.Visible = false;
            Application.Exit();
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
                var menuItem = parentObject as MenuItem;
                menuItem.Checked = false;
            }
        }

        private void PopulateDeviceList()
        {
            _deviceList = _soundService.DeviceList;
        }
    }
}
