using System.Windows.Forms;

namespace Skyresponse.DialogWrappers
{
    public interface IDialogWrapper
    {
        DialogResult ShowFileDialog();
        DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons button, MessageBoxIcon icon);
        string FileName { get; set; }
    }

    /// <summary>
    /// Wrapper class for dialog windows
    /// </summary>
    public class DialogWrapper : IDialogWrapper
    {
        private readonly OpenFileDialog _fileDialog;

        public string FileName
        {
            get => _fileDialog.FileName;
            set => _fileDialog.FileName = value;
        }

        public DialogWrapper()
        {
            _fileDialog = new OpenFileDialog { Filter = @"Audio Files (.mp3)|*.mp3" };
        }

        /// <summary>
        /// Creates an OpenFileDialog window
        /// </summary>
        /// <returns></returns>
        public DialogResult ShowFileDialog()
        {
            return _fileDialog.ShowDialog();
        }

        /// <summary>
        /// Creates a MessageBox window
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public DialogResult ShowMessageBox(string text, string caption = "", MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            return MessageBox.Show(text, caption, button, icon);
        }
    }
}
