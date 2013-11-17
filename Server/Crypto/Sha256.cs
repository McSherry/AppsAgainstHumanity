using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AppsAgainstHumanity.Server.Crypto
{
    /// <summary>
    /// A class to generate SHA-256 hashes.
    /// </summary>
    public static class Sha256
    {
        private static SHA256CryptoServiceProvider sha256csp = new SHA256CryptoServiceProvider();

        /// <summary>
        /// Generate a SHA-256 hash from the given bytes.
        /// </summary>
        /// <param name="input">Bytes to generate a hash from.</param>
        /// <returns>A SHA-256 hash.</returns>
        public static byte[] Generate(byte[] input)
        {
            sha256csp.Initialize();
            return sha256csp.ComputeHash(input);
        }
        /// <summary>
        /// Generate a SHA-256 hash from a given UTF-8 string.
        /// </summary>
        /// <param name="input">UTF-8 string to generate a hash from.</param>
        /// <returns>A SHA-256 hash.</returns>
        public static byte[] Generate(string input)
        {
            return Generate(UTF8Encoding.UTF8.GetBytes(input));
        }
    }
}
