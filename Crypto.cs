using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasktServer
{
    public static class Cryptography
    {
        public static Tuple<string, string> CreateKeyPair()
        {
            System.Security.Cryptography.CspParameters cspParams = new System.Security.Cryptography.CspParameters { ProviderType = 1 };

            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(1024, cspParams);

            string publicKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(false));
            string privateKey = Convert.ToBase64String(rsaProvider.ExportCspBlob(true));

            return new Tuple<string, string>(privateKey, publicKey);


        }
        public static byte[] Encrypt(string publicKey, string data)
        {
            System.Security.Cryptography.CspParameters cspParams = new System.Security.Cryptography.CspParameters { ProviderType = 1 };
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(publicKey));

            byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

            return encryptedBytes;
        }
        public static string Decrypt(string privateKey, byte[] encryptedBytes)
        {
            System.Security.Cryptography.CspParameters cspParams = new System.Security.Cryptography.CspParameters { ProviderType = 1 };
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

            rsaProvider.ImportCspBlob(Convert.FromBase64String(privateKey));

            byte[] plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

            string plainText = System.Text.Encoding.UTF8.GetString(plainBytes, 0, plainBytes.Length);

            return plainText;
        }
    }
}
