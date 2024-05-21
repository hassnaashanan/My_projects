using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
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
            int v = 0;
            if (plainText == "a") { return key; }
            else if (plainText.Length > key.Length)
            {

                for (int i = 0; i < plainText.Length; i++)
                {
                    if (i < key.Length)
                    {
                        KeyStream += key[i];
                    }
                    else
                    {
                        KeyStream += plainText[v];
                        v++;
                    }
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

            string autoKey = Decrypt(cipherText, plainText);

            string key = "";

            for (int i = 0; i < autoKey.Length; i++)
            {
                key += autoKey[i];
                if (autoKey[i + 1] == plainText[0] && i + 1 < cipherText.Length)
                {
                    if (autoKey[i + 2] == plainText[1] && i + 2 < autoKey.Length)
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
            string KeyStream = keyEdit("a", key);
            string plain = "";
            cipherText = cipherText.ToLower();
            int ik;
            int ip = 0;
            Dictionary<int, char> alphabets = new Dictionary<int, char>();
            int index;
            for (int i = 0; i < 26; i++)
            {
                char letter = (char)('a' + i);
                alphabets.Add(i, letter);
            }

            for (int i = 0; i < cipherText.Length; i++)
            {
                if (i < KeyStream.Length)
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
                else
                {
                    ik = getIndex(plain[ip]);

                    for (int x = 0; x < 26; x++)
                    {

                        if (table[x, ik] == cipherText[i])
                        {

                            plain += alphabets[x];
                        }

                    }
                    ip++;

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
