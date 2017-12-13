using System;
using System.Configuration;
using System.Security.Principal;
using Microsoft.Win32;

namespace Skyresponse.Services
{
    public class StartUpManager
    {
        private static readonly string AssemblyLocation = ConfigurationManager.AppSettings["AssemblyLocation"];
        private static readonly string SubKey = ConfigurationManager.AppSettings["SubKey"];

        public static void AddApplicationToCurrentUserStartup()
        {
            using (var key = Registry.CurrentUser.OpenSubKey($"{SubKey}", true))
            {
                key.SetValue("Skyresponse", $"{AssemblyLocation}");
            }
        }

        public static void AddApplicationToAllUserStartup()
        {
            using (var key = Registry.LocalMachine.OpenSubKey($"{SubKey}", true))
            {
                key.SetValue("Skyresponse", $"{AssemblyLocation}");
            }
        }

        public static void RemoveApplicationFromCurrentUserStartup()
        {
            using (var key = Registry.CurrentUser.OpenSubKey($"{SubKey}", true))
            {
                key.DeleteValue("Skyresponse", false);
            }
        }

        public static void RemoveApplicationFromAllUserStartup()
        {
            using (var key = Registry.LocalMachine.OpenSubKey($"{SubKey}", true))
            {
                key.DeleteValue("Skyresponse", false);
            }
        }

        public static bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;

            try
            {
                //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {

                isAdmin = false;

            }
            catch (Exception)

            {
                isAdmin = false;
            }
            return isAdmin;
        }
    }
}
