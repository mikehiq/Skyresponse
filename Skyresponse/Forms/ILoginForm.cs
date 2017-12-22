using System.Windows.Forms;

namespace Skyresponse.Forms
{
    public interface ILoginForm
    {
        DialogResult ShowDialog();
        string UserName { get; set; }
        string Password { get; set; }
        bool Created { get; }
    }
}