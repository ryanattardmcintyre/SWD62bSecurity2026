using System.Security.Cryptography;
using System.Text;

namespace Presentation.Helpers
{
    public class HashingHelper
    {
        public  byte[] Hash(byte [] input)
        {
            using (var sha512 = SHA512.Create())
            {
                byte[] digest = sha512.ComputeHash(input);
                return digest;
            }
        }

        public string Hash(string input)
        {
            //convert from string to byte array
            //if input is a string which can be human readable then we use Encoding.UTF8.GetBytes(input) to convert it to byte array
            //if input is a cryptographic string then we use Convert.FromBase64String(input) to convert it to byte array
           
            byte[] inputAsBytes = Encoding.UTF8.GetBytes(input);
            byte[] digest = Hash(inputAsBytes);

            //convert from byte array to string
            string result = Convert.ToBase64String(digest);
            return result;
        }

        public byte[] HashWithSalt(string input)
        {
            //its important that the salt doesn't change because if it does it will produce a different
            //digest everytime

            byte[] inputAsBytes = Encoding.UTF8.GetBytes(input);
            byte[] saltAsBytes = new byte[] { 50, 12, 110, 120, 35, 80, 167, 200, 201, 220, 255 };

            HMACSHA512 hmac = new HMACSHA512(saltAsBytes);
            byte[] digest = hmac.ComputeHash(inputAsBytes);
            return digest;
        }


    }
}
