using System.Security.Cryptography;

namespace Presentation.Helpers
{
    public class DigitalSigningHelper
    {
        //no need to create a method which generates the Asymmetric parameters
        //because we can use the AsymmetricEncryptionHelper class method!

        public static string SignData(MemoryStream dataToSign,
            string privateKey)
        {
            //1. creating the algorithm
            RSA myAlg = RSA.Create();

            //2. import the private key
            myAlg.FromXmlString(privateKey);

            //3. hash the data
            byte[] hash = SHA256.Create().ComputeHash(dataToSign);

            //4. digitally sign the hashed data
            byte[] signature = 
                myAlg.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signature);

        }


        //True means untouched/original
        //False means the data was tampered with
        public static bool VerifySignature(MemoryStream dataToVerify, string signature, string publicKey)
        {
            //1. creating the algorithm
            RSA myAlg = RSA.Create();

            //2. import the public key
            myAlg.FromXmlString(publicKey);

            //3. hash the data
            byte[] hash = SHA256.Create().ComputeHash(dataToVerify);

            //4. digitally verify the hashed data
            bool decision = myAlg.VerifyHash(hash, 
                Convert.FromBase64String(signature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return decision;
        }

    }
}
