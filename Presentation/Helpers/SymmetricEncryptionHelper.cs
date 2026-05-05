using System.Security.Cryptography;
using System.Text;

namespace Presentation.Helpers
{

    public class SymmetricParameters
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
    }

    public class SymmetricEncryptionHelper
    {

        public static SymmetricParameters GenerateSymmetricParameters(SymmetricAlgorithm algorithm)
        {
            //approach 1:
            //we can generate a random key and Iv using the selected algorithm instance

            algorithm.GenerateKey();
            algorithm.GenerateIV();

            return new SymmetricParameters
            {
                Key = algorithm.Key,
                IV = algorithm.IV
            };
        }

        public static SymmetricParameters GenerateSymmetricParameters(
            SymmetricAlgorithm algorithm, string userInput)
        {
            //approach 2:
            //we can use a user's input to generate the key and iv
            //when? when you want to encrypt data owned by a specific user in a different way ensuring
            //      more security because if there was a break for a particular user becasue his/her password was discovered
            //      that doesn't compromise other users' data


            byte[] userInputBytes = Encoding.UTF8.GetBytes(userInput);
            //salt is a random value added to the user input to make the derived key more secure
            byte[] salt = new byte[16]; // 16 bytes salt
            salt = new byte[] { 0x1A, 0x2B, 0x3C, 0x4D, 0x5E, 0x6F, 0x7A, 0x8B, 0x9C, 0xAD, 0xBE, 0xCF, 0xD1, 0xE2, 0xF3, 0x04 }; // Example salt

            Rfc2898DeriveBytes myKeyGenerator = new Rfc2898DeriveBytes(userInputBytes, salt, 1000, new HashAlgorithmName("SHA512")); // 16 bytes salt
            SymmetricParameters parameters = new SymmetricParameters()
            {
                Key = myKeyGenerator.GetBytes(algorithm.KeySize / 8),
                IV = myKeyGenerator.GetBytes(algorithm.BlockSize / 8)
            };
            return parameters;
        }


        public static string Encrypt(string plainText, SymmetricParameters keys, SymmetricAlgorithm alg)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            alg.Key = keys.Key;
            alg.IV = keys.IV;

            MemoryStream msIn = new MemoryStream(plainTextBytes);
            MemoryStream msOut = new MemoryStream(); //cipher is going to be stored in there

            using (CryptoStream cryptoStream = 
                new CryptoStream(msIn, alg.CreateEncryptor(), CryptoStreamMode.Read))
            {
                    cryptoStream.CopyTo(msOut);
            }

            byte[] cipher = msOut.ToArray();

            return Convert.ToBase64String(cipher);
        }

        public static string Decrypt(string cipherText, SymmetricParameters keys, SymmetricAlgorithm alg)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            alg.Key = keys.Key;
            alg.IV = keys.IV;

            MemoryStream msIn = new MemoryStream(cipherBytes);
            MemoryStream msOut = new MemoryStream(); //clear text is going to be stored in there

            using (CryptoStream cryptoStream =
                new CryptoStream(msIn, alg.CreateDecryptor(), CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(msOut);
            }

            byte[] clearTextBytes = msOut.ToArray();

            return Encoding.UTF8.GetString(clearTextBytes);
        }
    }
}
