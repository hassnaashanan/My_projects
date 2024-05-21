using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SecurityLibrary.DiffieHellman
{


    public class DiffieHellman
    {

      public  int power(int f, int s, int sf)
        {
            //throw new NotImplementedException();
            int rs = 1, k = 0;
            while ( k < s)
            {
                rs = (rs * f) % sf;
                k++;
            }
            return rs;
        }

       public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            //throw new NotImplementedException();
            List<int> res = new List<int>();
            int ya=0, yb=0, ka=0, kb = 0;
            if (xa<q && xb<q)
            {
                ya = power(alpha, xa, q);
                yb = power(alpha, xb, q);

                ka = power(yb, xa, q);
                kb = power(ya, xb, q);
            }

            res.Add(ka);
            res.Add(kb);
            return res;

        }
    }
}