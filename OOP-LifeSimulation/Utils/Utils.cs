using System;
using System.Security.Cryptography;

namespace OOP_LifeSimulation
{
    public class Utils
    {
        public static int GetRandomInt(int mod)
        {
            var provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];
            provider.GetBytes(byteArray);
            return BitConverter.ToUInt16(byteArray, 0) % mod;
        }
        
        public static int GetNextInt32(RNGCryptoServiceProvider rnd)
        {
            byte[] randomInt = new byte[4];
            rnd.GetBytes(randomInt);
            return Convert.ToInt32(randomInt[0]);
        }
    }
}