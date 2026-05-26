using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using System.Security.Cryptography;

namespace Presentation.Controllers
{
    public class TestController : Controller
    {

        public IActionResult TestMockTest()
        {
           string cipher=
                SymmetricEncryptionHelper.Decrypt("xslyhz/gjwoGk6zUizNFIQ==", new SymmetricParameters
            {
                Key = new byte[] { 229, 40, 188, 60, 221, 98, 193, 19, 22, 116, 225, 239, 224, 217, 11, 146, 36, 89, 20, 80, 216, 6, 98, 237, 28, 46, 89, 161, 88, 8, 253, 129 },
                IV = new byte[] { 212, 197, 20, 130, 243, 43, 210, 203, 169, 193, 14, 101, 206, 98, 59, 92 }
            }, Aes.Create());

            return Content("done");
        }

        public IActionResult TestDigitalSigning()
        {
            RSA myAlg = RSA.Create();
            var myKeys = AsymmetricEncryptionHelper.GenerateKeys();
            MemoryStream fileData = new MemoryStream();
            string myFile = @"C:\Users\attar\source\repos\SWD62bSecurity2026\Presentation\file1.txt";
            using (var fileStream = new FileStream(myFile, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(fileData);
            }
            fileData.Position = 0;
            string signature = DigitalSigningHelper.SignData(fileData, myKeys.PrivateKey);

            //-------------------------------------------------------

            MemoryStream fileData2 = new MemoryStream();
            using (var fileStream = new FileStream(myFile, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(fileData2);
            }
            fileData2.Position = 0;
            bool isOriginal = DigitalSigningHelper.VerifySignature(fileData2, signature, myKeys.PublicKey);

            return Content($"Is the original file intact? {isOriginal}");

        }

        public IActionResult TestAsymmetric()
        {
            string plainText = "Hello world";
            var myKeys = AsymmetricEncryptionHelper.GenerateKeys();

            var myCipherMs = HybridEncryptionHelper.EncryptData(
                new MemoryStream(System.Text.Encoding.UTF8.GetBytes(plainText)),
                myKeys.PublicKey);



            string cipherText = AsymmetricEncryptionHelper.Encrypt(plainText, myKeys.PublicKey);

            //-------------------------------------------------------------------

            string decryptedText =
                AsymmetricEncryptionHelper.Decrypt(cipherText, myKeys.PrivateKey);

            return Content($"Plain Text: {plainText}\n" +
                           $"Cipher Text: {cipherText}\n" +
                           $"Decrypted Text: {decryptedText}");


        }

        public IActionResult TestSymmetric()
        {
            string plainText = "Hello world";

            var myAlgorithm = Aes.Create();


            var myKeys = SymmetricEncryptionHelper.GenerateSymmetricParameters(
                myAlgorithm);

            string cipherText = SymmetricEncryptionHelper.Encrypt(plainText, myKeys, myAlgorithm);


            //-------------------------------------------------------

            string decryptedText = SymmetricEncryptionHelper.Decrypt(cipherText, myKeys, myAlgorithm);

            return Content($"Plain Text: {plainText}\n" +
                           $"Cipher Text: {cipherText}\n" +
                           $"Decrypted Text: {decryptedText}");

        }


        public IActionResult HashWorksheet()
        {
            /*
             * Message A
Transfer 250 EUR to account 458923
SHA-512 Digest:
b4Ff8n/2dWtxSGl/X8qzxF/DwfFabzEVmo23SftOw01RZNmkGQMUwCqCyb0cToKyjIEVNsoI0sWZqrUHsIg60A==
Message B
Transfer 2500 EUR to account 458923
SHA-512 Digest:
XEWsgeU/mmxf5URp9hm4+Ae0IrH1MRMu0nwGZ3Kb4JaQfVUdXeZIxfiPtvxRv0Kp4Vm98DLBpx3wrGpNgmeTPg==
Message C
Transfer 250 EUR to account 458923.
SHA-512 Digest:
fpMvTBCMXiaaCs6cKNvxZRcDwYIgsdjIZ8LEns7eiC3fPhaMmdbeJHtQ8nb0WNFmjGcuZd2o8GBdaV3egqD8mQ==
*/

            HashingHelper myHashingHelper = new HashingHelper();
           string digest1 = myHashingHelper.Hash("Transfer 250 EUR to account 458923");
            string digest2 = myHashingHelper.Hash("Transfer 2500 EUR to account 458923");
            string digest3 = myHashingHelper.Hash("Transfer 250 EUR to account 458923.");

            bool decision1 = digest1 == "b4Ff8n/2dWtxSGl/X8qzxF/DwfFabzEVmo23SftOw01RZNmkGQMUwCqCyb0cToKyjIEVNsoI0sWZqrUHsIg60A==";
            bool decision2 = digest2 == "XEWsgeU/mmxf5URp9hm4+Ae0IrH1MRMu0nwGZ3Kb4JaQfVUdXeZIxfiPtvxRv0Kp4Vm98DLBpx3wrGpNgmeTPg==";
            bool decision3 = digest3 == "fpMvTBCMXiaaCs6cKNvxZRcDwYIgsdjIZ8LEns7eiC3fPhaMmdbeJHtQ8nb0WNFmjGcuZd2o8GBdaV3egqD8mQ==";

            return Content($"Message A is {(decision1? "tampered": "original")}\n" +
                           $"Message B is {(decision2? "tampered": "original")}\n" +
                           $"Message C is {(decision3? "tampered": "original")}");
        }


        public IActionResult TestWeakAlgo()
        {
            string input = "SECURITY";
          int sum = 0;
   
          foreach (char c in input) { 
                sum += c;
                     }
        int weakHash = sum % 256;

            int sum2 = 0;
            var input2 = "YTRUICES";
            foreach (char c in input2)
            {
                sum2 += c;
            }
            int weakHash2 = sum2 % 256;


            //------------------------ hash both inputs using the Hash method using sha512

                HashingHelper myHashingHelper = new HashingHelper();
            var digest1 = myHashingHelper.Hash(input);
            var digest2 = myHashingHelper.Hash(input2);


            return Content($"Weak Hash 1: {weakHash}, Weak Hash 2: {weakHash2}\n" +
                           $"SHA-512 Digest 1: {digest1}\n" +
                           $"SHA-512 Digest 2: {digest2}");
        }
    }
}
