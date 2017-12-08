using System;
using System.Windows.Forms;
using Skyresponse.Forms;
using Unity;

namespace Skyresponse
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var unityContainer = Startup.BuildUnityContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(unityContainer.Resolve<MainForm>());
        }
    }
}
