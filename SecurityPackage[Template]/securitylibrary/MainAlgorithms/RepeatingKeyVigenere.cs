using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public char[,] creatTable()
        {
            char[,] table = new char[26, 26];

            List<char> alphabet = new List<char>() {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i','j',
                'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                'u', 'v', 'w', 'x', 'y', 'z'
            };
            int a;
            for (int i = 0; i < 26; i++)
            {
                a = 0;
                a = a + i;
                for (int x = 0; x < 26; x++)
                {
                    table[i, x] = alphabet[a % 26];
                    a++;
                }
            }
            return table;
        }
        public string keyEdit(string plainText, string key)
        {
            string KeyStream = "";
            if (plainText.Length > key.Length)
            {

                for (int i = 0; i < plainText.Length; i++)
                {
                    KeyStream += key[i % key.Length];
                }

            }
            else
            {
                KeyStream = key;
            }
            return KeyStream;
        }
        public int getIndex(char alpha)
        {
            Dictionary<char, int> alphabets = new Dictionary<char, int>();
            int index;
            for (int i = 0; i < 26; i++)
            {
                char letter = (char)('a' + i);
                alphabets.Add(letter, i);
            }
            index = alphabets[alpha];
            return index;
        }
        public string Analyse(string plainText, string cipherText)
        {
            string repeatedKey = Decrypt(cipherText, plainText);
            string key = "";

            for (int i = 0; i < repeatedKey.Length; i++)
            {
                key += repeatedKey[i];
                if (repeatedKey[i + 1] == repeatedKey[0] && i + 1 < repeatedKey.Length)
                {
                    if (repeatedKey[i + 2] == repeatedKey[1] && i + 2 < repeatedKey.Length)
                    {
                        return key;
                    }

                }
            }
            return key;

        }


        public string Decrypt(string cipherText, string key)
        {
            char[,] table = creatTable();
            string KeyStream = keyEdit(cipherText, key);
            string plain = "";
            cipherText = cipherText.ToLower();
            int ik;

            Dictionary<int, char> alphabets = new Dictionary<int, char>();
            int index;
            for (int i = 0; i < 26; i++)
            {
                char letter = (char)('a' + i);
                alphabets.Add(i, letter);
            }

            for (int i = 0; i < KeyStream.Length; i++)
            {
                ik = getIndex(KeyStream[i]);

                for (int x = 0; x < 26; x++)
                {

                    if (table[x, ik] == cipherText[i])
                    {


                        plain += alphabets[x];
                    }

                }
            }


            return plain;
        }

        public string Encrypt(string plainText, string key)
        {
            char[,] table = creatTable();
            string KeyStream = keyEdit(plainText, key);
            string cipher = "";
            int ik, ip;

            for (int i = 0; i < KeyStream.Length; i++)
            {
                ik = getIndex(KeyStream[i]);
                ip = getIndex(plainText[i]);
                cipher += table[ik, ip];
            }

            return cipher;
        }
    }
}

