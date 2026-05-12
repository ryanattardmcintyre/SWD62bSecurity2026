using System.Security.Cryptography;

namespace Presentation.Helpers
{
    public class HybridEncryptionHelper
    {
        public static MemoryStream EncryptData(MemoryStream msIn, string publicKey)
        { 
            //1. import the public key
            RSA rSA = RSA.Create();
            rSA.FromXmlString(publicKey);

            //2. generate the symmetric keys
            var myParams = SymmetricEncryptionHelper.GenerateSymmetricParameters(Aes.Create());


            //An exercise: create an overload of the SymmetricEncryptionHelper.Encrypt() to accept a memorystream
            //3. encrypt the data using the symmetric keys
            string textToEncrypt = System.Text.Encoding.UTF8.GetString(msIn.ToArray());
            string cipherAsText = SymmetricEncryptionHelper.Encrypt(textToEncrypt, myParams, Aes.Create());

            //4. encrypt the symmetric keys using the public key
            byte[] encSecretKey = AsymmetricEncryptionHelper.Encrypt(myParams.Key, publicKey);
            byte[] encIv =AsymmetricEncryptionHelper.Encrypt(myParams.IV, publicKey);

            //5. we store the encKey & encIv & enc (File) data into one MemoryStream and return it
            MemoryStream msOut = new MemoryStream();
            msOut.Write(encSecretKey, 0, encSecretKey.Length);
            msOut.Write(encIv, 0, encIv.Length);

            //deciding on the conversion of the cipher text to byte array - you must consult what was the last conversion
            //did before obtaining cipherAsText. if it was Convert.ToBase64String() then you should do Convert.FromBase64String() to get the byte array of the cipher text
            MemoryStream msCipher = new MemoryStream(Convert.FromBase64String(cipherAsText));
            msCipher.CopyTo(msOut);

            msOut.Position = 0;
            return msOut;
        }

        public static MemoryStream Decrypt(MemoryStream msCipherIn, string privateKey)
        {
            //Exercise: implement the decryption logic for the above encryption method

            //1. create an instance of the RSA algorithm and import the private key
            //2. open msCipherIn and first read the
            //   encrypted key (128bytes, when you'are reading you read msCipherIn.Read(encKey, 0, 128))
            //   encrypted iv (128bytes, when you'are reading you read msCipherIn.Read(encIv, 0, 128))
            //   encrypted data (read what's left - that would be the cipher text)

            //3. decrypt the encrypted key and iv using the private key i.e. Asymmetric

            //4. decrypt the cipher text using the decrypted key and iv i.e. Symmetric

            //5. return the decrypted data packaged into a MemoryStream

            return new MemoryStream();
        }
    }
}
