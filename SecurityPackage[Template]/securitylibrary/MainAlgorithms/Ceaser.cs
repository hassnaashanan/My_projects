using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {

        public Dictionary<char, int> table()
        {
            Dictionary<char, int> charValueTable = new Dictionary<char, int>();
            int i = 0;
            for (char c = 'a'; c <= 'z'; c++)
            {
                charValueTable.Add(c, i);
                i++;
            }
            return charValueTable;

        }

        public string Encrypt(string plainText, int key)
        {
            Dictionary<char, int> Table = table();

            char[] characters = new char[plainText.Length];
            for (int i = 0; i < plainText.Length; i++)
            {
                int v = Table[plainText[i]];
                int eq = (v + key) % (26);

                foreach (var ch in Table)
                {
                    if (ch.Value == eq)
                    {
                        characters[i] = ch.Key;
                        break;
                    }
                }

            }
            string Text = string.Concat(characters);
            return Text;
        }

        public string Decrypt(string cipherText, int key)
        {
            Dictionary<char, int> Table = table();
            cipherText = cipherText.ToLower();
            char[] characters = new char[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {
                int v = Table[cipherText[i]];
                int eq = (v - key + 26) % 26;

                foreach (var ch in Table)
                {
                    if (ch.Value == eq)
                    {
                        characters[i] = ch.Key;
                        break;
                    }
                }

            }
            string Text = string.Concat(characters);
            return Text;
        }

        public int Analyse(string plainText, string cipherText)
        {
            int Index(char letter)
            {
                return char.ToLower(letter) - 97;
            }

            int key = (Index(cipherText[0]) - Index(plainText[0]));
            if (key < 0)
            {
                key = key + 26;
                return key;
            }
            return key % 26;
        }
    }
}