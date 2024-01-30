using System.Security.Cryptography;

namespace RSALib
{
    //openssl genpkey -algorithm RSA -out private_key.pem
    //openssl rsa -pubout -in private_key.pem -out public_key.pem
    public static class RSATools
    {
        private static RSA GetKey(string path)
        {
            var file = File.ReadAllText(path);
            var rsa = RSA.Create();
            rsa.ImportFromPem(file);
            return rsa;
        }

        public static RSA GetRSAPrivateKey() 
        {
            return GetKey("rsa/private_key.pem");
        }
        public static RSA GetRSAPublicKey() 
        {
            return GetKey("rsa/public_key.pem");
        }
    }
}
