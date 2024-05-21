using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public char[,] Matrixenc(int rows, int cols, string plainText)
        {
            char[,] matrix = new char[rows, cols];

            int z = 0;
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (z < plainText.Length)
                    {
                        matrix[j, i] = plainText[z];
                        z++;
                    }
                    else
                    {

                        matrix[j, i] = 'x';
                    }
                }
            }

            return matrix;
        }
        public char[,] Matrixdec(int rows, int cols, string plainText)
        {
            char[,] matrix = new char[rows, cols];

            int z = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (z < plainText.Length)
                    {
                        matrix[i, j] = plainText[z];
                        z++;
                    }
                    else
                    {

                        matrix[i, j] = 'x';
                    }
                }
            }

            return matrix;
        }
        public string Stringenc(char[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            string extractedString = "";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {

                    extractedString += matrix[i, j];
                }

            }

            return extractedString;
        }
        public string Stringdec(char[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            string extractedString = "";

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {

                    extractedString += matrix[j, i];
                }

            }

            return extractedString;
        }

        public int Analyse(string plainText, string cipherText)
        {
            int key = 2;
            for (int i = 0; i < 3; i++)
            {
                string cipher = Encrypt(plainText, key);
                Console.WriteLine(cipher);
                Console.WriteLine(cipherText);
                if (cipher.ToUpper() == cipherText.ToUpper())
                {
                    return key;
                }
                else
                {
                    key++;
                }
            }
            return key;


        }

        public string Decrypt(string cipherText, int key)
        {
            int col = (int)Math.Ceiling((double)cipherText.Length / key);
            char[,] matrix = Matrixdec(key, col, cipherText);
            string plain = Stringdec(matrix);
            Console.WriteLine(plain);
            Console.WriteLine(plain.Length);
            plain = plain.Replace("x", "");
            Console.WriteLine(plain);
            return plain;
        }

        public string Encrypt(string plainText, int key)
        {
            int col = (int)Math.Ceiling((double)plainText.Length / key);
            char[,] matrix = Matrixenc(key, col, plainText);
            string cipher = Stringenc(matrix);
            Console.WriteLine(cipher);
            Console.WriteLine(cipher.Length);
            cipher = cipher.Replace("x", "");
            Console.WriteLine(cipher);
            return cipher;

        }
    }
}
