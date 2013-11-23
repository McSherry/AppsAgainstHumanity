using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AppsAgainstHumanity.Server.Crypto
{
    using BytePair = KeyValuePair<byte[], byte[]>;
    using StringPair = KeyValuePair<string, string>;

    public class Rsa
    {
        public static StringPair GenerateKeyPair()
        {
            StringPair rsaKeys;
            using (var rsa = new RSACryptoServiceProvider(2048) { PersistKeyInCsp = false })
            {
                // Public key
                rsaKeys = new StringPair(
                    rsa.ToXmlString(false), // Public Key
                    rsa.ToXmlString(true) // Private Key
                );
            }

            return rsaKeys;
        }

        public byte[] PublicKeyEncrypt(byte[] plaintext, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider() { PersistKeyInCsp = false };

            rsa.FromXmlString(publicKey);

            return rsa.Encrypt(plaintext, false);
        }
        public byte[] PrivateKeyDecrypt(byte[] ciphertext, string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider() { PersistKeyInCsp = false };

            rsa.FromXmlString(privateKey);

            return rsa.Decrypt(ciphertext, false);
        }

        public byte[] Sign(byte[] plaintext, string privateKey)
        {
            return this.PrivateKeyDecrypt(plaintext, privateKey);
        }
        public bool Verify(byte[] ciphertext, string publicKey, byte[] hashValue)
        {

            return this.PublicKeyEncrypt(ciphertext, publicKey).SequenceEqual(hashValue);
        }
    }
}
