using System.Windows.Forms;

namespace Skyresponse.Wrappers.DialogWrappers
{
    public interface IDialogWrapper
    {
        DialogResult ShowFileDialog();
        DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons button, MessageBoxIcon icon);
        string FileName { get; set; }
    }
}