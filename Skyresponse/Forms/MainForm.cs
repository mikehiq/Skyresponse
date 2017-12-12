using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Skyresponse.Api;
using Skyresponse.DialogWrappers;
using Skyresponse.Properties;
using Skyresponse.Services;

namespace Skyresponse.Forms
{
    public interface IMainForm
    {
    }

    public partial class MainForm : Form, IMainForm
    {
        private readonly IDialogWrapper _fileDialog;
        private readonly ISkyresponseApi _skyresponseApi;
        private readonly ISoundService _soundService;
        private NotifyIcon _notifyIcon;
        private IEnumerable<DeviceInfo> _deviceList;

        public MainForm(IDialogWrapper fileDialog, ISkyresponseApi skyresponseApi, ISoundService soundService)
        {
            InitializeComponent();
            _fileDialog = fileDialog;
            _skyresponseApi = skyresponseApi;
            _soundService = soundService;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Minimized; //start application minimized
            PopulateDeviceComboBox();
            GetDropdown();
            CreateNotifyIcon();
            _skyresponseApi.InitAsync();
        }

        private void PopulateDeviceComboBox()
        {
            _deviceList = _soundService.DeviceList;
            deviceOutComboBox.Items.AddRange(_deviceList.ToArray());
        }

        private void GetDropdown()
        {
            deviceOutComboBox.DropDownWidth = GetDropDownWidth(); //TODO: fixa bredden på dropdownlist!!!
        }

        private int GetDropDownWidth()
        {
            List<int> deviceNames = new List<int>();
            foreach (var deviceInfo in _deviceList)
            {
                var deviceNameLength = deviceInfo.DeviceName.Length;
                deviceNames.Add(deviceNameLength);
            }
            return deviceNames.Max();
        }

        private void BrowseSoundFile_Click(object sender, EventArgs e)
        {
            _fileDialog.ShowFileDialog();
            string path = _fileDialog.FileName;
            _soundService.SavePath(path);
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            Exit(sender, e);
        }

        private void CreateNotifyIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = Resources.Skyresponse_white_small,
                ContextMenu = new ContextMenu(),
                Visible = true
            };
            _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Browse", BrowseSoundFile_Click));

            var deviceMenu = new MenuItem("Device");
            deviceMenu.MenuItems.AddRange(_deviceList.Select(d => new MenuItem(d.DeviceName)).ToArray());
            _notifyIcon.ContextMenu.MenuItems.Add(deviceMenu);

            _notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", Exit));

            ShowInTaskbar = false;
            _notifyIcon.Text = @"Skyresponse ljudmonitor";
            _notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            Activate();
            WindowState = FormWindowState.Normal;
        }

        private void deviceOutComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combobox = sender as ComboBox;
            var deviceInfo = combobox.SelectedItem as DeviceInfo;
            _soundService.SetOutputDevice(deviceInfo.Guid);
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        /// <inheritdoc />
        /// <summary>
        /// override CreateParams property to disable the close icon in the system/window and the alt+F4 keystroke
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                const int csNoclose = 0x200;

                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= csNoclose;
                return cp;
            }
        }
    }
}
