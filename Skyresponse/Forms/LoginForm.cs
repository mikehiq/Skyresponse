using System;
using System.Windows.Forms;
using Skyresponse.Wrappers.DialogWrappers;

namespace Skyresponse.Forms
{
    public partial class LoginForm : Form, ILoginForm
    {
        private readonly IDialogWrapper _dialogWrapper;
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginForm(IDialogWrapper dialogWrapper)
        {
            InitializeComponent();
            _dialogWrapper = dialogWrapper;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usernameTextBox.Text) || string.IsNullOrEmpty(passwordTextBox.Text))
            {
                _dialogWrapper.ShowMessageBox(@"Please provide Username and Password", @"Critical Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UserName = usernameTextBox.Text;
            Password = passwordTextBox.Text;
            passwordTextBox.Clear();
            usernameTextBox.Clear();

            DialogResult = DialogResult.OK;
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
