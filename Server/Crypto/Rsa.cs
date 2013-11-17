using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AppsAgainstHumanity.Server.Crypto
{
    using BytePair = KeyValuePair<byte[], byte[]>;
    using QuadPair = KeyValuePair<BytePair, BytePair>;

    public class Rsa
    {
        public static QuadPair GenerateKeyPair()
        {
            BytePair rsaPriv,rsaPubl;
            using (var rsa = new RSACryptoServiceProvider(2048) { PersistKeyInCsp = false })
            {
                // Public key is { exponent, modulus }
                rsaPubl = new BytePair(rsa.ExportParameters(false).Exponent, rsa.ExportParameters(false).Modulus);
                // Public key is { private exponent, modulus }
                rsaPriv = new BytePair(rsa.ExportParameters(true).D, rsa.ExportParameters(true).Modulus);
            }

            return new KeyValuePair<BytePair, BytePair>(rsaPriv, rsaPubl);
        }

        public byte[] PublicKeyEncrypt(byte[] plaintext, BytePair publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider() { PersistKeyInCsp = false };

            rsa.ImportParameters(
                new RSAParameters()
                {
                    Exponent = publicKey.Key,
                    Modulus = publicKey.Value
                }
            );

            return rsa.Encrypt(plaintext, false);
        }
        public byte[] PrivateKeyDecrypt(byte[] ciphertext, BytePair privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider() { PersistKeyInCsp = false };

            rsa.ImportParameters(
                new RSAParameters()
                {
                    D = privateKey.Key,
                    Modulus = privateKey.Value
                }
            );

            return rsa.Decrypt(ciphertext, false);
        }

        // NOTE: UNTESTED
        // THIS MAY NOT, AND LIKELY WILL NOT WORK
        // USED AS A WORKAROUND FOR MICROSOFT'S SIGN/VERIFY METHODS
        // ATTEMPTS ENCRYPTION WITH PUBLIC KEY (RATHER THAN PRIVATE AS ABOVE)
        // ATTEMPTS DECRYPTION WITH PRIVATE KEY (RATHER THAN PUBLIC AS ABOVE)
        public byte[] Sign(byte[] plaintext, BytePair privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider() { PersistKeyInCsp = false };

            rsa.ImportParameters(
                new RSAParameters()
                {
                    Exponent = privateKey.Key,
                    Modulus = privateKey.Value
                }
            );

            return rsa.Encrypt(plaintext, false);
        }
        public bool Verify(byte[] ciphertext, BytePair publicKey, byte[] hashValue)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider() { PersistKeyInCsp = false };

            rsa.ImportParameters(
                new RSAParameters()
                {
                    D = publicKey.Key,
                    Modulus = publicKey.Value
                }
            );

            return rsa.Decrypt(ciphertext, false).SequenceEqual(hashValue);
        }
    }
}
