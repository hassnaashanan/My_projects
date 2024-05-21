using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            List<List<int>> cipherFinalCol = new List<List<int>>();
            List<List<int>> plainFinalCol = new List<List<int>>();
            List<List<int>> cipherRow = new List<List<int>>();
            List<List<int>> plainRow = new List<List<int>>();


            for (int i = 0; i < plainText.Count; i = i + 2)
            {
                List<int> plainCol = new List<int>();
                List<int> cipherCol = new List<int>();

                plainCol.Add(plainText[i]);
                plainCol.Add(plainText[i + 1]);
                cipherCol.Add(cipherText[i]);
                cipherCol.Add(cipherText[i + 1]);

                cipherFinalCol.Add(cipherCol);
                plainFinalCol.Add(plainCol);
            }

            for (int i = 0; i < (plainText.Count / 2) - 1; i++)
            {
                for (int j = i + 1; j < (plainText.Count / 2); j++)
                {
                    List<int> cipherColToRow = new List<int>();
                    List<int> plainColToRow = new List<int>();

                    cipherColToRow.Add(cipherFinalCol[i].ElementAt(0));
                    cipherColToRow.Add(cipherFinalCol[i].ElementAt(1));
                    cipherColToRow.Add(cipherFinalCol[j].ElementAt(0));
                    cipherColToRow.Add(cipherFinalCol[j].ElementAt(1));

                    plainColToRow.Add(plainFinalCol[i].ElementAt(0));
                    plainColToRow.Add(plainFinalCol[i].ElementAt(1));
                    plainColToRow.Add(plainFinalCol[j].ElementAt(0));
                    plainColToRow.Add(plainFinalCol[j].ElementAt(1));
                    plainRow.Add(plainColToRow);
                    cipherRow.Add(cipherColToRow);

                }
            }


            for (int i = 0; i < plainRow.Count; i++)
            {
                List<int> finalPlainRow = new List<int>();
                for (int j = 0; j < 4; j++)
                {
                    if (plainRow[i].ElementAt(j) < 0)
                    {
                        finalPlainRow.Insert(j, plainRow[i].ElementAt(j) + 26);

                    }
                    else if (plainRow[i].ElementAt(j) >= 26)
                    {
                        finalPlainRow.Insert(j, (plainRow[i].ElementAt(j) % 26));
                    }

                    else
                        finalPlainRow.Insert(j, plainRow[i].ElementAt(j));
                }

                int calcFinalDet(int d)
                {
                    if (d == 1 || d == -1)
                    {
                        return d;
                    }
                    else
                    {
                        int a = d;
                        int b = 26;
                        int x0 = 1, x1 = 0, y0 = 0, y1 = 1;

                        while (b != 0)
                        {
                            int q = a / b;
                            int tmp = b;
                            b = a % b;
                            a = tmp;

                            int nx = x0 - q * x1;
                            int ny = y0 - q * y1;

                            x0 = x1;
                            x1 = nx;
                            y0 = y1;
                            y1 = ny;
                        }

                        return x0 < 0 ? x0 + 26 : x0;
                    }
                }

                int det = finalPlainRow[0] * finalPlainRow[3] - finalPlainRow[1] * finalPlainRow[2];
                int finalDet = calcFinalDet(det);
                List<int> invPlain = new List<int>();
                int[] indices = { 3, 1, 2, 0 };
                for (int x = 0; x < 4; x++)
                {
                    int y = indices[x];
                    if (y == 1 || y == 2)
                    {

                        invPlain.Insert(x, finalPlainRow[y] * finalDet * -1);
                    }
                    else
                    {
                        invPlain.Insert(x, finalPlainRow[y] * finalDet);

                    }
                    y--;

                }



                List<int> kpInvMatrix = new List<int>();
                int ind = 0;
                kpInvMatrix.Insert(0, (cipherRow[i].ElementAt(ind) * invPlain[ind]) + (cipherRow[i].ElementAt(ind + 2) * invPlain[ind + 1]));
                kpInvMatrix.Insert(1, (cipherRow[i].ElementAt(ind) * invPlain[ind + 2]) + (cipherRow[i].ElementAt(ind + 2) * invPlain[ind + 3]));
                kpInvMatrix.Insert(2, (cipherRow[i].ElementAt(ind + 1) * invPlain[ind]) + (cipherRow[i].ElementAt(ind + 3) * invPlain[ind + 1]));
                kpInvMatrix.Insert(3, (cipherRow[i].ElementAt(ind + 1) * invPlain[ind + 2]) + (cipherRow[i].ElementAt(ind + 3) * invPlain[ind + 3]));


                List<int> key = kpInvMatrix.Select(elm =>
                {
                    if (elm < 0)
                    {
                        return elm + 26;
                    }
                    else if (elm >= 26)
                    {
                        return elm % 26;
                    }
                    else
                    {
                        return elm;
                    }
                }).ToList();
                List<int> cipher = new List<int>();
                cipher = Encrypt(plainText, key);
                bool areEqual = true;

                for (int w = 0; w < cipher.Count; w++)
                {
                    if (cipher[i] != cipherText[i])
                    {
                        areEqual = false;
                        break;
                    }
                }

                if (areEqual == true)
                {
                    return key;
                }
            }
            throw new InvalidAnlysisException();
        }
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int[,] f = listToArr(key);
            int det;
            int[,] ad;
            if (f.GetLength(0) == 2)
            {
                det = f[0, 0] * f[1, 1] - f[0, 1] * f[1, 0];
            }
            else
            {

                det = calcDet(f);
            }
            int res = calcMod26(det);
            if (f.GetLength(0) == 2)
            {
                int temp = 0;
                temp = f[0, 0];
                f[0, 0] = f[1, 1];
                f[1, 1] = temp;
                f[0, 1] *= -1;
                f[1, 0] *= -1;
                ad = f;
            }
            else
            {

                ad = calcAdjoint(f);
            }
            int len = ad.GetLength(0);
            for (int i = 0; i < ad.GetLength(0); i++)
            {
                for (int j = 0; j < ad.GetLength(1); j++)
                {
                    ad[i, j] = calcMod26(ad[i, j]);
                }
            }
            det = calcMod26(det);
            det = inv(det);
            for (int i = 0; i < ad.GetLength(0); i++)
            {
                for (int j = 0; j < ad.GetLength(1); j++)
                {
                    ad[i, j] = calcMod26(ad[i, j] * det);
                }
            }
            var cc = ad;
            int[,] ct = new int[len, 1];
            int y = 0;
            List<int> list = new List<int>();
            while (true)
            {
                int j = 0;
                for (; y < len; y++)
                {
                    ct[j, 0] = cipherText[y];
                    j++;
                }
                int[,] pl = matrixMul(ad, ct);
                for (int q = 0; q < pl.GetLength(0); q++)
                {
                    for (int e = 0; e < pl.GetLength(1); e++)
                    {
                        if (pl[q, e] != -1)
                            list.Add(pl[q, e]);

                    }
                }
                if (f.GetLength(0) == 2)
                    len += 2;
                else
                    len += 3;
                if (y == cipherText.Count)
                {
                    break;
                }
            }
            return list;
        }
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int matColSize = (int)Math.Sqrt(key.Count);
            int[,] keyMat = new int[matColSize, matColSize];
            List<int> cipher = new List<int>();
            int keyStart = 0;
            for (int i = 0; i < matColSize; i++)
            {
                for (int j = 0; j < matColSize; j++)
                {
                    keyMat[i, j] = key[keyStart];
                    keyStart++;
                }
            }


            int plainRows = 0;
            if (plainText.Count % matColSize != 0)
            {
                plainRows++;
            }

            plainRows = plainText.Count / matColSize;


            int[,] plainMat = new int[matColSize, plainRows];
            int startKey = 0;
            foreach (var i in Enumerable.Range(0, plainRows))
            {
                foreach (var j in Enumerable.Range(0, matColSize))
                {
                    if (startKey < plainText.Count)
                    {
                        plainMat[j, i] = plainText[startKey];
                        startKey++;

                    }
                    else
                    {
                        plainMat[j, i] = 0;

                    }
                }
            }


            int[,] cipherMatrix = new int[matColSize, plainRows];
            for (int i = 0; i < matColSize; i++)
            {
                for (int j = 0; j < plainRows; j++)
                {
                    int sum = 0;
                    for (int c = 0; c < matColSize; c++)
                    {
                        sum += keyMat[i, c] * plainMat[c, j];
                    }
                    cipherMatrix[i, j] = sum % 26;
                }
            }

            for (int i = 0; i < plainRows; i++)
            {
                for (int j = 0; j < matColSize; j++)
                {
                    cipher.Add(cipherMatrix[j, i]);
                }
            }

            return cipher;
        }
        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {
            int[,] plain = listToColMatrix(plain3);
            int[,] cipher = listToColMatrix(cipher3);
            int det = calcDet(plain);
            int res = calcMod26(det);
            int[,] ad = calcAdjoint(plain);
            int len = ad.GetLength(0);
            for (int i = 0; i < ad.GetLength(0); i++)
            {
                for (int j = 0; j < ad.GetLength(1); j++)
                {
                    ad[i, j] = calcMod26(ad[i, j]);
                }
            }
            det = calcMod26(det);
            det = inv(det);
            for (int i = 0; i < ad.GetLength(0); i++)
            {
                for (int j = 0; j < ad.GetLength(1); j++)
                {
                    ad[i, j] = calcMod26(ad[i, j] * det);
                }
            }
            int[,] ct = new int[len, 1];
            int y = 0;
            List<int> list = new List<int>();
            int[,] pl = matrixMul(cipher, ad);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var e = calcMod26(pl[i, j]);
                    list.Add(pl[i, j]);
                }
            }
            //while (true)
            //{
            //    int j = 0;
            //    for (; y < len; y++)
            //    {
            //        ct[j, 0] = cipher3[y];
            //        j++;
            //    }
            //    int[,] pl = matrixMul(ad,cipher);
            //    for (int q = 0; q < pl.GetLength(0); q++)
            //    {
            //        for (int e = 0; e < pl.GetLength(1); e++)
            //        {
            //            if (pl[q, e] != -1)
            //                list.Add(pl[e, q]);

            //        }
            //    }
            //    len += 3;
            //    if (y == cipher3.Count)
            //    {
            //        break;
            //    }
            //}
            var ciph = Encrypt(plain3, list);
            bool areEqual = true;
            for (int w = 0; w < ciph.Count; w++)
            {
                if (ciph[w] != cipher3[w])
                {
                    areEqual = false;
                    break;
                }
            }

            if (areEqual == true)
            {
                return list;
            }
            throw new InvalidAnlysisException();

        }

        public int[,] listToColMatrix(List<int> l)
        {
            int[,] arr = new int[3, 3];
            arr = new int[3, 3];
            arr[0, 0] = l[0];
            arr[1, 0] = l[1];
            arr[2, 0] = l[2];
            arr[0, 1] = l[3];
            arr[1, 1] = l[4];
            arr[2, 1] = l[5];
            arr[0, 2] = l[6];
            arr[1, 2] = l[7];
            arr[2, 2] = l[8];
            return arr;
        }


        //// Function to convert a list to a matrix of specified size
        //private List<List<int>> ConvertToMatrix(List<int> list, int size)
        //{
        //    List<List<int>> matrix = new List<List<int>>();
        //    for (int i = 0; i < list.Count; i += size)
        //    {
        //        matrix.Add(list.GetRange(i, size));
        //    }
        //    return matrix;
        //}

        //// Function to calculate the determinant of a 3x3 matrix
        //private int Determinant3x3(List<List<int>> matrix)
        //{
        //    return matrix[0][0] * (matrix[1][1] * matrix[2][2] - matrix[1][2] * matrix[2][1]) -
        //           matrix[0][1] * (matrix[1][0] * matrix[2][2] - matrix[1][2] * matrix[2][0]) +
        //           matrix[0][2] * (matrix[1][0] * matrix[2][1] - matrix[1][1] * matrix[2][0]);
        //}

        //// Function to find the inverse of a 3x3 matrix
        //private List<List<int>> MatrixInverse(List<List<int>> matrix)
        //{
        //    int det = Determinant3x3(matrix);
        //    int detInv = ModInverse(det, 26);

        //    if (det == 0 || detInv == 0)
        //        throw new InvalidOperationException("Matrix is singular; it does not have an inverse.");

        //    List<List<int>> inverse = new List<List<int>>();

        //    inverse.Add(new List<int> {
        //    (matrix[1][1] * matrix[2][2] - matrix[1][2] * matrix[2][1]) * detInv % 26,
        //    (matrix[0][2] * matrix[2][1] - matrix[0][1] * matrix[2][2]) * detInv % 26,
        //    (matrix[0][1] * matrix[1][2] - matrix[0][2] * matrix[1][1]) * detInv % 26
        //});
        //    inverse.Add(new List<int> {
        //    (matrix[1][2] * matrix[2][0] - matrix[1][0] * matrix[2][2]) * detInv % 26,
        //    (matrix[0][0] * matrix[2][2] - matrix[0][2] * matrix[2][0]) * detInv % 26,
        //    (matrix[0][2] * matrix[1][0] - matrix[0][0] * matrix[1][2]) * detInv % 26
        //});
        //    inverse.Add(new List<int> {
        //    (matrix[1][0] * matrix[2][1] - matrix[1][1] * matrix[2][0]) * detInv % 26,
        //    (matrix[0][1] * matrix[2][0] - matrix[0][0] * matrix[2][1]) * detInv % 26,
        //    (matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0]) * detInv % 26
        //});

        //    return inverse;
        //}

        //// Function to find the modular multiplicative inverse
        //private int ModInverse(int a, int m)
        //{
        //    a = a % m;
        //    for (int x = 1; x < m; x++)
        //    {
        //        if ((a * x) % m == 1)
        //            return x;
        //    }
        //    return 0;
        //}

        //// Function to multiply two matrices
        //private List<int> MatrixMultiply(List<List<int>> matrix1, List<List<int>> matrix2)
        //{
        //    List<int> result = new List<int>();
        //    for (int i = 0; i < matrix1.Count; i++)
        //    {
        //        for (int j = 0; j < matrix2[0].Count; j++)
        //        {
        //            int sum = 0;
        //            for (int k = 0; k < matrix2.Count; k++)
        //            {
        //                sum += matrix1[i][k] * matrix2[k][j];
        //            }
        //            result.Add(sum);
        //        }
        //    }
        //    return result;
        //}


        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }

        public int[,] listToArr(List<int> l)
        {
            int[,] arr;
            if (l.Count == 4)
            {
                arr = new int[2, 2];
                arr[0, 0] = l[0];
                arr[0, 1] = l[1];
                arr[1, 0] = l[2];
                arr[1, 1] = l[3];
            }
            else
            {
                arr = new int[3, 3];
                arr[0, 0] = l[0];
                arr[0, 1] = l[1];
                arr[0, 2] = l[2];
                arr[1, 0] = l[3];
                arr[1, 1] = l[4];
                arr[1, 2] = l[5];
                arr[2, 0] = l[6];
                arr[2, 1] = l[7];
                arr[2, 2] = l[8];
            }

            return arr;
        }

        public int calcDet(int[,] m)
        {
            int determinant = 0;

            bool[] usedCol = new bool[m.GetLength(0)];

            for (int r = 0; r < m.GetLength(0); r++)
            {
                int s;
                if (r % 2 == 0)
                {
                    s = 1;
                }
                else
                {
                    s = -1;
                }

                for (int c = 0; c < m.GetLength(0); c++)
                {
                    if (usedCol[c])
                        continue;

                    determinant = determinant + (s * m[r, c] * minorDet(m, r, c));
                    usedCol[c] = true;
                    break;
                }

                Array.Clear(usedCol, 0, usedCol.Length);
            }
            return determinant;
        }

        public int minorDet(int[,] m, int rToDel, int cToDel)
        {
            int l = m.GetLength(0);

            int[,] minorM = new int[l - 1, l - 1];
            for (int i = 0, r = 0; i < l; i++)
            {
                if (i == rToDel)
                {
                    continue;
                }
                for (int j = 0, c = 0; j < l; j++)
                {
                    if (j == cToDel)
                    {
                        continue;
                    }

                    minorM[r, c] = m[i, j];
                    c++;
                }

                r++;
            }


            return minorM[0, 0] * minorM[1, 1] - minorM[0, 1] * minorM[1, 0];
        }

        public int minorForAdjoint(int[,] m, int rToDel, int cTodel)
        {
            int l = m.GetLength(0);
            int[,] minor = new int[l - 1, l - 1];

            int r = 0;
            for (int i = 0; i < l; i++)
            {
                if (i == rToDel)
                    continue;

                int c = 0;
                for (int j = 0; j < l; j++)
                {
                    if (j == cTodel)
                        continue;

                    minor[r, c] = m[i, j];
                    c++;
                }

                r++;
            }

            return detForAdjoint(minor);
        }
        public int calcMod26(int n)
        {
            int x = n % 26;
            int y;
            if (x < 0)
            {
                y = x + 26;
            }
            else
            {
                y = x;
            }
            return y;
        }
        public int[,] calcAdjoint(int[,] m)
        {
            int ln = m.GetLength(0);
            int[,] adj = new int[ln, ln];

            for (int i = 0; i < ln; i++)
            {
                for (int j = 0; j < ln; j++)
                {
                    int co = (int)Math.Pow(-1, i + j) * minorForAdjoint(m, i, j);
                    adj[j, i] = co;
                }
            }

            return adj;
        }
        public int detForAdjoint(int[,] m)
        {
            int len = m.GetLength(0);
            int det = 0;


            if (len == 2)
            {
                det = m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0];
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    det += m[0, i] * minorForAdjoint(m, 0, i);
                }
            }

            return det;
        }
        public int inv(int w, int n = 26)
        {
            int naux = n;

            int y = 0;
            int x = 1;

            if (n == 1)
            {
                return 0;
            }

            while (w > 1)
            {
                int q = w / n;

                int temp = n;

                n = w % n;

                w = temp;

                temp = y;
                y = x - q * y;
                x = temp;
            }

            if (x < 0)
            {
                x += naux;
            }
            int u = 0;
            if (w == 1)
            {
                u = x;
            }
            else
            {
                u = -1;
            }
            return u;
        }
        public int[,] matrixMul(int[,] mA, int[,] mB)
        {
            int aRows = mA.GetLength(0);
            int aCols = mA.GetLength(1);
            int bRows = mB.GetLength(0);
            int bCols = mB.GetLength(1);


            int[,] res = new int[aRows, bRows];
            for (int i = 0; i < aRows; i++)
            {
                for (int j = 0; j < bRows; j++)
                {
                    res[i, j] = -1;
                }
            }

            for (int i = 0; i < aRows; i++)
            {
                for (int j = 0; j < bCols; j++)
                {
                    int sum = 0;

                    for (int u = 0; u < aCols; u++)
                    {
                        sum += mA[i, u] * mB[u, j];
                    }

                    res[i, j] = calcMod26(sum);
                }
            }

            return res;
        }
        public char numTOChar(int num)
        {
            int j = 0;
            Dictionary<int, char> table = new Dictionary<int, char>();
            for (int i = 97; i <= 122; i++)
            {
                table.Add(j, (char)i);
                j++;
            }
            return table[num];
        }
    }
}

