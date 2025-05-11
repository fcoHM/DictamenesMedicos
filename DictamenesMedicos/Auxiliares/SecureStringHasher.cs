using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DictamenesMedicos.Auxiliares
{
    public static class SecureStringHasher
    {
        public static string SecureStringToString(SecureString secureString)
        {
            IntPtr bstr = IntPtr.Zero;
            try
            {
                bstr = Marshal.SecureStringToBSTR(secureString);
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                if (bstr != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstr);
            }
        }

        public static string HashPasswordFromSecureString(SecureString securePassword)
        {
            string plainTextPassword = SecureStringToString(securePassword);
            return HashPassword(plainTextPassword); // Funcion de hashing
        }

        // Usando PBKDF2
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static SecureString ConvertToSecureString(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }

            return secure;
        }

        public static string ConvertToUnsecureString(SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException(nameof(secureString));

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString); // Limpia la memoria de forma segura
            }
        }
    }
}
