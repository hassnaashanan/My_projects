using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int Encrypt(int p, int q, int M, int e)
        {
            int c = 0;
            int n = p * q;
            Console.WriteLine(n);
            if (e > 7)
            {
                int result = 1;
                for (int i = 0; i < e; i++)
                {
                    result = (result * M) % n;
                }
                c = result;
            }
            else
            {
                c = (int)(Math.Pow(M, e) % n);
            }
            Console.WriteLine(c);
            return c;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int n = p * q;
            int pi = (p - 1) * (q - 1);
            AES.ExtendedEuclid ex = new AES.ExtendedEuclid();
            int d = ex.GetMultiplicativeInverse(e, pi);

            int dec = modEx(C, d, n);
            return dec;
        }
        public int modEx(int a, int b, int n)
        {
            long res = 1;
            for (int i = 0; i < b; i++)
            {
                res = (res * a) % n;
            }
            return (int)res;
        }
    }
}
