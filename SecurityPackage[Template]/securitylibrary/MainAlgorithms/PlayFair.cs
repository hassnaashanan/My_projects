using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public char[,] GenerateMatrix(string key)
        {
            char[,] matrix = new char[5, 5];
            key = key.ToLower();

            //make a list with alphabet - key
            List<char> alphabet = new List<char>() {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                'u', 'v', 'w', 'x', 'y', 'z'
            };
            foreach (char c in key)
            {
                alphabet.Remove(c);
            }
            //make a unique alphabet key

            string uniqe_key = "";

            foreach (char c in key)
            {
                if (uniqe_key.IndexOf(c) == -1 && c != 'j')
                {
                    uniqe_key += c;
                }
            }
            // fill matrix
            int alen = 0;
            int klen = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (klen < uniqe_key.Length)
                    {
                        matrix[i, j] = uniqe_key[klen];
                        klen++;
                    }
                    else
                    {
                        matrix[i, j] = alphabet[alen];
                        alen++;
                    }
                }

            }
            Console.WriteLine("");
            Console.WriteLine("Matrix:");
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine(); // Move to the next row
            }
            return matrix;
        }
        public string plain_text(string plainText)
        {
            string plain = "";
            for (int i = 0; i < plainText.Length; i++)
            {

                plain += plainText[i];


                if (i + 1 < plainText.Length && plain.Length % 2 == 1 && plainText[i] == plainText[i + 1])
                {

                    plain += 'x';
                }
            }
            if (plain.Length % 2 != 0)
                plain += 'x';
            return plain;
        }
        public int[] rowColumnIndex(char[,] matrix, char X)
        {
            int row = 0; int column = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (matrix[i, j] == X) { row = i; column = j; }
                }
            }

            return new int[] { row, column };
        }
        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            string plainText = "";
            char[,] matrix = GenerateMatrix(key);

            int row1, row2, col1, col2;

            for (int c = 0; c < cipherText.Length; c += 2)
            {
                char fst = cipherText[c];
                char sec = cipherText[c + 1];
                int[] result1 = rowColumnIndex(matrix, fst);
                row1 = result1[0]; col1 = result1[1];
                int[] result2 = rowColumnIndex(matrix, sec);
                row2 = result2[0]; col2 = result2[1];
                if (row1 == row2)
                {

                    plainText += matrix[row1, (col1 + 4) % 5];
                    plainText += matrix[row2, (col2 + 4) % 5];

                }
                else if (col1 == col2)
                {

                    plainText += matrix[(row1 + 4) % 5, col1];
                    plainText += matrix[(row2 + 4) % 5, col2];

                }
                else { plainText += matrix[row1, col2]; plainText += matrix[row2, col1]; }

            }
            string plain = "";

            for (int i = 0; i < plainText.Length; i++)
            {

                if (i > 0 && i < plainText.Length - 1 && plainText[i] == 'x')
                {
                    if (plainText[i - 1] == plainText[i + 1] && i % 2 != 0)

                        continue;
                }
                plain += plainText[i];

            }


            if (plain[plain.Length - 1] == 'x')
                plain = plain.Remove(plain.Length - 1, 1);

            Console.WriteLine("");
            Console.WriteLine(plain);

            return plain;
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = "";
            char[,] matrix = GenerateMatrix(key);
            string plain_Text = plain_text(plainText);


            int row1, row2, col1, col2;

            for (int c = 0; c < plain_Text.Length; c += 2)
            {
                char fst = plain_Text[c];
                char sec = plain_Text[c + 1];
                int[] result1 = rowColumnIndex(matrix, fst);
                row1 = result1[0]; col1 = result1[1];
                int[] result2 = rowColumnIndex(matrix, sec);
                row2 = result2[0]; col2 = result2[1];

                if (row1 == row2) { cipherText += matrix[row1, (col1 + 1) % 5]; cipherText += matrix[row1, (col2 + 1) % 5]; }
                else if (col1 == col2) { cipherText += matrix[(row1 + 1) % 5, col1]; cipherText += matrix[(row2 + 1) % 5, col1]; }
                else { cipherText += matrix[row1, col2]; cipherText += matrix[row2, col1]; }


            }
            Console.WriteLine("");
            Console.WriteLine(cipherText);
            return cipherText.ToUpper();
        }
    }
}