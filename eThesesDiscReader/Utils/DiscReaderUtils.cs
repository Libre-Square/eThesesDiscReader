using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eThesesDiscReader.Utils
{
    public class DiscReaderUtils
    {
        public static byte[] shaDigest(string text)
        {
            SHA512 sha = new SHA512Managed();
            return sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));
        }

        public static bool compareDigests(byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null || array1.Length != array2.Length)
                return false;

            for (int i=0; i<array1.Length; i++)
            {
                if (array1[i] != array2[i])
                    return false;
            }
            return true;
        }
    }
}
