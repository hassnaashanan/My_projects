using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public char[,] getmat(string text, int col, int row, int n = 0)
        {
            char[,] mat = new char[row, col];
            int l = 0;
            if (n == 0)
            {
                for (int r = 0; r < row; r++)
                {
                    for (int c = 0; c < col; c++)
                    {
                        if (l < text.Length)
                            mat[r, c] = text[l];
                        else
                            mat[r, c] = 'x';
                        l++;
                    }
                }
            }
            else
            {
                for (int r = 0; r < col; r++)
                {
                    for (int c = 0; c < row; c++)
                    {
                        if (l < text.Length)
                            mat[c, r] = text[l];

                        l++;
                    }
                }
            }
            return mat;
        }
        public int getindex(string text, char a)
        {
            int x = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == a)
                { x = i; break; }
            }
            return x;
        }
        public int get_no_col(string plain, string cipher)
        {
            int columns = 0;

            int x = 0, y = 0;

            x = getindex(plain, cipher[9]);
            y = getindex(plain, cipher[10]);


            columns = Math.Abs(y - x);
            return columns;

        }
        public List<int> Analyse(string plainText, string cipherText)
        {

            cipherText = cipherText.ToLower();
            List<int> key = new List<int>();
            int col = get_no_col(plainText, cipherText);
            int row = (int)Math.Ceiling((double)plainText.Length / col);
            char[,] matplain = getmat(plainText, col, row);
            char[,] matcipher = getmat(cipherText, col, row, 1);

            int k = 0;
            for (int n = 0; n < col; n++)
            {
                for (int c = 0; c < col; c++)
                {
                    for (int r = 0; r < row; r++)
                    {
                        if (matplain[r, n] == matcipher[r, c])
                            k++;
                    }
                    if (k == row)
                        key.Add(c + 1);
                    k = 0;
                }
            }
            if (key.Count < col)
            {
                for (int i = 0; i < col + 1; i++)
                {
                    key.Add(0);
                }
            }
            return key;
        }
        public string Decrypt(string cipherText, List<int> key)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            int cols = key.Count();

            int Np = cipherText.Length;
            int rows;
            if (Np % cols != 0)
                rows = (int)(Np / cols) + 1;
            else
                rows = (int)(Np / cols);

            char[,] mat = new char[rows, cols];

            int c = (key[0] - 1) * rows;
            for (int j = 0; j < cols; j++)
            {
                c = (key[j] - 1) * rows;
                for (int i = 0; i < rows; i++)
                {
                    if (c < Np)
                        mat[i, j] = cipherText[c];
                    c++;
                }
            }


            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(mat[i, j] + " ");
                }
                Console.WriteLine();
            }
            char[] plain = new char[cols * rows];
            int k = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    plain[k] = mat[i, j];
                    k++;
                }
            }
            Console.WriteLine(plain);
            string plaintext = new string(plain);
            return plaintext;
        }
        ///
        public string Encrypt(string plainText, List<int> key)
        {
            //throw new NotImplementedException();
            string ciphertext = "";
            plainText = plainText.ToLower();
            int cols = key.Count();

            int Np = plainText.Length;

            int rows = (int)(Np / cols) + 1;
            bool f = false;
            while (f == false)
            {
                if (plainText.Length % cols != 0)
                    plainText += "x";
                else
                    f = true;
            }
            Dictionary<int, string> mat = new Dictionary<int, string>();

            foreach (var i in key)
                mat.Add(i, "");

            int c = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (c < plainText.Length)
                    {
                        mat[key[j]] += plainText[c]; // Map key to column and append character
                        c++;
                    }
                }
            }
            foreach (KeyValuePair<int, string> keyValuePair in mat)
            {
                Console.WriteLine($"Key: {keyValuePair.Key}, Value: {keyValuePair.Value}");
            }

            for (int i = 1; i <= cols; i++)
            {
                ciphertext += (mat[i]);
            }
            Console.WriteLine(ciphertext);
            return ciphertext;
        }
    }
}
