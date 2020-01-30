using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Tshirt.Classes
{
    public class SHA
    {
        public static string GenerateSHA256String(string inputString)
        {
            byte[] hash = null;
            try
            {
                SHA256 sha256 = SHA256Managed.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(inputString);
                hash = sha256.ComputeHash(bytes);
            }
            catch (Exception)
            {
            }
            return GetStringFromHash(hash);

        }
        public static string GenerateSHA512String(string inputString)
        {
            byte[] hash = null;
            try
            {
                SHA512 sha512 = SHA512Managed.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(inputString);
                hash = sha512.ComputeHash(bytes);
            }
            catch (Exception)
            {
            }
            return GetStringFromHash(hash);

        }
        public static string GenerateSHA1String(string inputString)
        {
            byte[] hash = null;
            try
            {
                SHA1 sha1 = SHA1Managed.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(inputString);
                hash = sha1.ComputeHash(bytes);
            }
            catch (Exception)
            {
            }
            return GetStringFromHash(hash);

        }
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("X2"));
                }
            }
            catch (Exception)
            {
            }
            return result.ToString();
        }
    }
}