using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            Dictionary<char, char> mainDic = new Dictionary<char, char>();

            for (int i = 97; i <= 122; i++)
            {
                mainDic.Add((char)i, ' ');
            }
            Dictionary<char, char> key = new Dictionary<char, char>();

            for (int i = 0; i < plainText.Length; i++)
            {

                if (key.ContainsKey(plainText[i]) == false)
                    key[plainText[i]] = cipherText.ToLower()[i];


            }
            Dictionary<char, char> sortedKey = new Dictionary<char, char>();

            sortedKey = key.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var s in sortedKey.Keys)
            {
                if (mainDic.Keys.Contains(s))
                {
                    mainDic[s] = sortedKey[s];
                }
            }


            List<char> restOfAlpha = new List<char>();
            for (char ch = 'a'; ch <= 'z'; ch++)
            {
                restOfAlpha.Add(ch);
            }
            foreach (char value in mainDic.Values)
            {
                if (restOfAlpha.Contains(value) && value != ' ')
                {
                    restOfAlpha.Remove(value);
                }
            }
            foreach (char m in mainDic.Keys.ToList())
            {
                if (mainDic[m] == ' ')
                {
                    mainDic[m] = restOfAlpha[0];
                    restOfAlpha.RemoveAt(0);
                }
            }



            char start = sortedKey.Keys.Min();
            List<char> mainDickeys = new List<char>(mainDic.Keys);
            int startIndx = mainDickeys.IndexOf(start);
            string finalKey = "";

            for (int i = 0; i < mainDic.Count; i++)
            {
                int currIndx = (startIndx + i) % (mainDic.Count);
                finalKey = finalKey + mainDic[mainDickeys[currIndx]];
            }
            return finalKey;
        }

        public string Decrypt(string cipherText, string key)
        {
            Dictionary<char, char> table = new Dictionary<char, char>();
            List<int> arr = new List<int>();

            for (int i = 0; i < 26; i++)
            {
                table.Add(key[i], ' ');
            }
            int k = 0;
            for (int i = 97; i <= 122; i++)
            {

                table[key[k]] = (char)i;
                arr.Add(i);
                k++;

            }
            cipherText = cipherText.ToLower();
            string plain = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                plain += table[cipherText[i]];
            }
            return plain;
        }

        public string Encrypt(string plainText, string key)
        {
            if (key == null)
                return plainText;
            Dictionary<char, char> table = new Dictionary<char, char>();
            List<int> arr = new List<int>();
            for (int i = 97; i <= 122; i++)
            {

                table.Add((char)i, ' ');
                arr.Add(i);
            }
            for (int i = 0; i < 26; i++)
            {
                table[(char)arr[i]] = key[i];
            }

            string cipher = "";
            for (int j = 0; j < plainText.Length; j++)
            {
                char c = plainText[j];
                cipher += table[plainText[j]];
            }

            return cipher;
        }







        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	=
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        /// 

        public string AnalyseUsingCharFrequency(string cipher)
        {
            string plain = "";
            Dictionary<char, int> frequencyMap = new Dictionary<char, int>();
            List<char> alphabet = new List<char>() {
                'e', 't', 'a', 'o', 'i', 'n', 's', 'r', 'h','l',
                'd', 'c', 'u', 'm', 'f', 'p', 'g', 'w', 'y', 'b',
                'v', 'k', 'x', 'j', 'q', 'z'
            };

            // Count the frequency of each letter
            foreach (char c in cipher)
            {

                if (frequencyMap.ContainsKey(c))
                {
                    frequencyMap[c]++;
                }
                else
                {
                    frequencyMap[c] = 1;
                }

            }
            // sort dictionary
            List<KeyValuePair<char, int>> list = frequencyMap.ToList();

            list.Sort((x, y) => y.Value.CompareTo(x.Value));

            Dictionary<char, int> sortedDictionary = new Dictionary<char, int>();
            foreach (var k in list)
            {
                sortedDictionary.Add(k.Key, k.Value);
            }
            int index = 0;
            foreach (char c in cipher)
            {
                index = -1;
                foreach (var k in sortedDictionary.Keys)
                {
                    index++;
                    if (k == c)
                    {
                        plain += alphabet[index];

                    }

                }
            }

            return plain;
        }
    }
}