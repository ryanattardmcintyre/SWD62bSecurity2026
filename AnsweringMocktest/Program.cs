using System.Security.Cryptography;
using System.Text;

string privateKeyPath = "C:\\Users\\attar\\Downloads\\keys3\\privateKey.txt";
string publicKey1Path = "C:\\Users\\attar\\Downloads\\keys3\\publicKey1.txt";
string publicKey2Path = "C:\\Users\\attar\\Downloads\\keys3\\publickey2.txt";
string publicKey3Path = "C:\\Users\\attar\\Downloads\\keys3\\publicKey3.txt";


string privateKeyStr = System.IO.File.ReadAllText(privateKeyPath);
string publicKey1Str = System.IO.File.ReadAllText(publicKey1Path);
string publicKey2Str = System.IO.File.ReadAllText(publicKey2Path);
string publicKey3Str = System.IO.File.ReadAllText(publicKey3Path);


byte[] privateKeyBytes = Convert.FromBase64String(privateKeyStr);
byte[] publicKey1Bytes = Convert.FromBase64String(publicKey1Str);
byte[] publicKey2Bytes = Convert.FromBase64String(publicKey2Str);
byte[] publicKey3Bytes = Convert.FromBase64String(publicKey3Str);



RSA mySigningAlg = RSA.Create();

SHA256 myHashingAlgorithm = SHA256.Create();
//byte[] testMessage = Encoding.UTF8.GetBytes("TestMessage");
//byte[] digest = myHashingAlgorithm.ComputeHash(testMessage);


using (RSA privateKeyRsa = RSA.Create())
using (RSA publicKeyRsa = RSA.Create())
{
    privateKeyRsa.ImportRSAPrivateKey(privateKeyBytes, out _);
    publicKeyRsa.ImportRSAPublicKey(publicKey1Bytes, out _);
    // Test the keys by signing and verifying a message
    byte[] testMessage = Encoding.UTF8.GetBytes("TestMessage");
    //Answer1: sign testMessage here

    byte [] digest = myHashingAlgorithm.ComputeHash(testMessage);

    byte[] signature =  privateKeyRsa.SignHash(digest, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    //Answer2: verify signature produced here

    bool result = publicKeyRsa.VerifyHash(digest, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

}

using (RSA privateKeyRsa = RSA.Create())
using (RSA publicKeyRsa = RSA.Create())
{
    privateKeyRsa.ImportRSAPrivateKey(privateKeyBytes, out _);
    publicKeyRsa.ImportRSAPublicKey(publicKey2Bytes, out _);
    // Test the keys by signing and verifying a message
    byte[] testMessage = Encoding.UTF8.GetBytes("TestMessage");
    //Answer1: sign testMessage here

    byte[] digest = myHashingAlgorithm.ComputeHash(testMessage);

    byte[] signature = privateKeyRsa.SignHash(digest, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    //Answer2: verify signature produced here

    bool result = publicKeyRsa.VerifyHash(digest, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

}

using (RSA privateKeyRsa = RSA.Create())
using (RSA publicKeyRsa = RSA.Create())
{
    privateKeyRsa.ImportRSAPrivateKey(privateKeyBytes, out _);
    publicKeyRsa.ImportRSAPublicKey(publicKey3Bytes, out _);
    // Test the keys by signing and verifying a message
    byte[] testMessage = Encoding.UTF8.GetBytes("TestMessage");
    //Answer1: sign testMessage here

    byte[] digest = myHashingAlgorithm.ComputeHash(testMessage);

    byte[] signature = privateKeyRsa.SignHash(digest, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    //Answer2: verify signature produced here

    bool result = publicKeyRsa.VerifyHash(digest, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

}

