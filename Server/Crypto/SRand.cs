using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel;

namespace AppsAgainstHumanity.Server.Crypto
{
    public class SRand
    {
        private static RandomNumberGenerator RNG = RandomNumberGenerator.Create();
        public static implicit operator ulong(SRand s)
        {
            byte[] rndData = new byte[8];
            RNG.GetBytes(rndData);

            return BitConverter.ToUInt64(rndData, 0);
        }
        public static implicit operator int(SRand s)
        {
            byte[] rndData = new byte[4];
            RNG.GetBytes(rndData);

            return BitConverter.ToInt32(rndData, 0);
        }
    }
}
