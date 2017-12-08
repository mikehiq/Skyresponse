using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace Skyresponse.Persistence
{
    public interface IPersistenceManager
    {
        void SaveSecure(string key, string value);
        string ReadSecure(string key);
        void Save(string key, string value);
        string Read(string key);
    }

    /// <summary>
    /// Persistence class for safe saving of parameters
    /// </summary>
    public class PersistenceManager : IPersistenceManager
    {
        private static readonly byte[] Entropy = Encoding.Unicode.GetBytes(string.Empty);

        /// <summary>
        /// Encrypts and saves a string as SecureString
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SaveSecure(string key, string value)
        {
            var secureString = ToSecureString(value);
            var encryptString = EncryptString(secureString);
            Save(key, encryptString);
        }

        /// <summary>
        /// Returns a decrypted SecureString as a string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadSecure(string key)
        {
            var encryptedString = Read(key);
            var decryptString = DecryptString(encryptedString);
            var insecureString = ToInsecureString(decryptString);
            return insecureString;
        }

        /// <summary>
        /// Saves key and value to a config file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Save(string key, string value)
        {
            Properties.Settings.Default[key] = value;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Returns value from a config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns>The value</returns>
        public string Read(string key)
        {
            var value = Properties.Settings.Default[key].ToString();
            return value;
        }

        /// <summary>
        /// Encrypts the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>An encrypted string</returns>
        private static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), Entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the input string
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <returns>A decrypted SecureString</returns>
        private static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), Entropy, DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        /// <summary>
        /// Converts a string to SecureString
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A SecureString</returns>
        private static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>
        /// Converts a SecureString to string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A string</returns>
        private static string ToInsecureString(SecureString input)
        {
            string returnValue;
            IntPtr pointer = Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = Marshal.PtrToStringBSTR(pointer);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(pointer);
            }
            return returnValue;
        }
    }
}
