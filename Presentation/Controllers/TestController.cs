using Microsoft.AspNetCore.Mvc;
using Presentation.Helpers;
using System.Security.Cryptography;

namespace Presentation.Controllers
{
    public class TestController : Controller
    {

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
