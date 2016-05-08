using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MusicPlayer.Security
{
    public class SkyNet
    {
        internal const string Inputkey = "QUO875OP-HFSC-GEC6-LOPW-OKH8NHJ2ZXC3";
        RijndaelManaged aesAlg;

        public SkyNet(string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(Inputkey, saltBytes);

            aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

        }

        public string EncryptRijndael(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
            Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        public string DecryptRijndael(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return null;

            if (!IsBase64String(cipherText))
                return null;

            string text;

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;
        }
    }
}