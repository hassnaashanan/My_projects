using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        /// 

        static long mod(long baseValue, long exponent, long modulus)
        {
            long res = 1;

            for (long i = exponent; i > 0; i--)
            {
                res = (res * baseValue) % modulus;
            }

            return res % modulus;
        }


        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            List<long> cipher = new List<long>();
            long c1 = mod(alpha, k, q);
            long K = mod(y, k, q);
            long c2 = (K * m) % q;
            cipher.Add(c1);
            cipher.Add(c2);
            return cipher;

        }

        public int Decrypt(int c1, int c2, int x, int q)
        {
            AES.ExtendedEuclid ex = new AES.ExtendedEuclid();
            int k = (int)mod(c1, x, q);
            int KInv = ex.GetMultiplicativeInverse(k, q);
            int m = (c2 * KInv) % q;
            return m;

        }
    }
}
