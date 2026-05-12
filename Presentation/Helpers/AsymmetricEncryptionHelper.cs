using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.Helpers
{
    public class AsymmetricParameters
    {     
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
    public class AsymmetricEncryptionHelper
    {
        public static AsymmetricParameters GenerateKeys()
        {
            RSA myAlg = RSA.Create();

            // Key generation logic here
            return new AsymmetricParameters
            {
                PublicKey = myAlg.ToXmlString(false), // Export public key
                PrivateKey = myAlg.ToXmlString(true) // Export private key
            };
        }

        public static byte[] Encrypt(byte[] data, string publicKey)
        {
            RSA myAlg = RSA.Create();
            myAlg.FromXmlString(publicKey); // Import public key

            byte[] cipher = myAlg.Encrypt(data, RSAEncryptionPadding.Pkcs1); // Encrypt data using public key

            return cipher; // Return encrypted data as byte array
        }

        public static byte[] Decrypt(byte[] cipher, string privateKey)
        {
            RSA myAlg = RSA.Create();
            myAlg.FromXmlString(privateKey); // Import public key

            byte[] clearBytes = myAlg.Decrypt(cipher, RSAEncryptionPadding.Pkcs1); // Encrypt data using public key

            return clearBytes; // Return encrypted data as byte array
        }

        public static string Encrypt(string data, string publicKey)
        {
            byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data); // converting human input data
            byte[] cipherBytes = Encrypt(dataBytes, publicKey); // Encrypt byte array
            return Convert.ToBase64String(cipherBytes); // Converting encrypted data
        }

        public static string Decrypt(string cipher, string privateKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipher); // Converting encrypted data
            byte[] clearBytes = Decrypt(cipherBytes, privateKey); // Decrypt byte array
            return System.Text.Encoding.UTF8.GetString(clearBytes); // converting human readable data
        }

        //exercise: try to encrypt and decrypt a file using
        //- symmetric
        //- asymmetric encryption

    }


}
