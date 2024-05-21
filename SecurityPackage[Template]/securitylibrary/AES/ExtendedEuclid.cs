using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            ///throw new NotImplementedException();
            int ret = 0;
            int[,] mat = new int[20, 7];

            mat[0, 1] = 1;
            mat[0, 3] = baseN;
            mat[0, 5] = 1;
            mat[0, 6] = number;


            int j = 1;
            while (j < 20)
            {
                //Q
                mat[j, 0] = mat[j - 1, 3] / mat[j - 1, 6];
                //A2
                mat[j, 1] = mat[j - 1, 4];
                //A2
                mat[j, 2] = mat[j - 1, 5];
                //A3
                mat[j, 3] = mat[j - 1, 6];
                //B1
                mat[j, 4] = mat[j - 1, 1] - (mat[j, 0] * mat[j - 1, 4]);
                //B2
                mat[j, 5] = mat[j - 1, 2] - (mat[j, 0] * mat[j - 1, 5]);
                //B3
                mat[j, 6] = mat[j - 1, 3] % mat[j - 1, 6];

                if (mat[j, 6] == 1 || mat[j, 6] == 0)
                    break;
                j++;
            }

            if (mat[j, 6] == 1)
            {
                if (mat[j, 5] < 0)
                    ret = mat[j, 5] + baseN;
                else
                    ret = mat[j, 5];
            }

            else
                ret = -1;

            return ret;
        }
    }
}
