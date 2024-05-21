using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {

        public string permutations(int x, string key)
        {
            int[,] PC_1 = new int[8, 7] {
                { 57, 49, 41, 33, 25, 17, 9 },
                { 1, 58, 50, 42, 34, 26, 18 },
                { 10, 2, 59, 51, 43, 35, 27 },
                { 19, 11, 3, 60, 52, 44, 36 },
                { 63, 55, 47, 39, 31, 23, 15 },
                { 7, 62, 54, 46, 38, 30, 22 },
                { 14, 6, 61, 53, 45, 37, 29 },
                { 21, 13, 5, 28, 20, 12, 4 } };

            int[,] PC_2 = new int[8, 6] {
                { 14, 17, 11, 24, 1, 5 },
                { 3, 28, 15, 6, 21, 10 },
                { 23, 19, 12, 4, 26, 8 },
                { 16, 7, 27, 20, 13, 2 },
                { 41, 52, 31, 37, 47, 55 },
                { 30, 40, 51, 45, 33, 48 },
                { 44, 49, 39, 56, 34, 53 },
                { 46, 42, 50, 36, 29, 32 } };
            string resl = "";
            if (x == 1)
            {

                int row = PC_1.GetLength(0);
                int cols = PC_1.GetLength(1);
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        resl += key[PC_1[i, j] - 1];
                    }
                }
            }
            else
            {
                int row = PC_2.GetLength(0);
                int cols = PC_2.GetLength(1);
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        resl += key[PC_2[i, j] - 1];
                    }
                }
            }
            return resl;
        }
        public void shift(string c, string d, List<string> nc, List<string> nd)
        {

            for (int i = 1; i <= 16; i++)
            {
                string t1 = "", t2 = "";
                if (i == 1 || i == 2 || i == 9 || i == 16)
                {
                    t1 = t1 + c[0];
                    c = c.Remove(0, 1);
                    c += t1;
                    t2 = t2 + d[0];
                    d = d.Remove(0, 1);
                    d += t2;
                }
                else
                {
                    t1 = t1 + c[0] + c[1];
                    c = c.Remove(0, 2);
                    c += t1;
                    t2 = t2 + d[0] + d[1];
                    d = d.Remove(0, 2);
                    d += t2;
                }
                nc.Add(c);
                nd.Add(d);
            }

        }
        public List<string> get_keys(string key)
        {
            List<string> res_keys = new List<string>();
            List<string> nc = new List<string>();
            List<string> nd = new List<string>();
            List<string> k_shift = new List<string>();
            string res = "";

            //convert to binary
            string k_binary = Convert.ToString(Convert.ToInt64(key, 16), 2).PadLeft(64, '0');
            // perform pc_1
            string k_per = permutations(1, k_binary);
            // Console.WriteLine("k_per" + k_per);
            //split to 2 equle parts
            string c = k_per.Substring(0, 28);
            string d = k_per.Substring(28, 28);
            // perfom shifting
            shift(c, d, nc, nd);
            // put 2 [arts as one string
            for (int i = 0; i < 16; i++)
            {
                k_shift.Add(nc[i] + nd[i]);
            }

            //perfom pc_2
            for (int i = 0; i < 16; i++)
            {
                res = permutations(2, k_shift[i]);
                res_keys.Add(res);
                res = "";
            }
            return res_keys;
        }

        public static string HexToBinary(string hexString)
        {
            // Convert hexadecimal string to byte array
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            // Convert byte array to binary string
            string binaryString = "";
            foreach (byte b in bytes)
            {
                binaryString += Convert.ToString(b, 2).PadLeft(8, '0');
            }
            return binaryString;
        }
        public string IP(string plainText)
        {
            string PlainText = HexToBinary(plainText.Substring(2));
            string Plain = "";
            int[] ip = {58, 50, 42, 34, 26, 18, 10, 2,
                        60, 52, 44, 36, 28, 20, 12, 4,
                        62, 54, 46, 38, 30, 22, 14, 6,
                        64, 56, 48, 40, 32, 24, 16, 8,
                        57, 49, 41, 33, 25, 17, 9, 1,
                        59, 51, 43, 35, 27, 19, 11, 3,
                        61, 53, 45, 37, 29, 21, 13, 5,
                        63, 55, 47, 39, 31, 23, 15, 7};
            for (int i = 0; i < ip.Length; i++)
            {
                Plain += PlainText[ip[i] - 1];

            }
            return Plain;

        }
        public string FunctionOnRight(int[] arr, string Right, string Key)
        {
            string NewRight = "";
            string NewR = "";

            for (int i = 0; i < arr.Length; i++)
            {
                NewRight += Right[arr[i] - 1];

            }
            for (int n = 0; n < NewRight.Length; n++)
            {
                if (int.Parse(NewRight[n].ToString()) == int.Parse(Key[n].ToString()))
                {
                    NewR += "0";
                }
                else
                {
                    NewR += "1";
                }
            }
            return NewR;
        }
        public static string FunctionOnRightKey(string Right)
        {
            string NewRight = "";
            int z = 0;
            int[,,] sBox8 = new int[8, 4, 16]
{
    // First
    {
        {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
        {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
        {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
        {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
    },
    // Second 
    {
        {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
        {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
        {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
        {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
    },
    // Third 
    {
       {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
       {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
       {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
       {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
    },
    // Fourth 
    {
          {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
          {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
          {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
          {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
    },
    // Fifth 
    {
            {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
            {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
            {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
            {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
    },
    // Sixth 
    {
       {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
       {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
       {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
       {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
    },
    // seven 
    {
       {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
       {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
       {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
       {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
    },
    // eight 
    {
       {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
       {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
       {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
       {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
    }
};

            for (int i = 0; i < Right.Length; i += 6)
            {
                string substring = Right.Substring(i, 6);
                int rownum = Convert.ToInt32(substring[0].ToString() + substring[5].ToString(), 2);
                int columnum = Convert.ToInt32(substring.Substring(1, 4), 2);


                int[,] sbox = new int[4, 16];
                for (int h = 0; h < 4; h++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        sbox[h, j] = sBox8[z, h, j];
                    }
                }
                int num = sbox[rownum, columnum];
                string bin = Convert.ToString(num, 2);
                bin = bin.PadLeft(4, '0');
                NewRight += bin;
                z++;
            }
            return NewRight;
        }

        public string Round(string plainText, string Key)
        {

            //tabels
            //E bit selection
            int[] E_bitSelection = {
                32, 1, 2, 3, 4, 5,
                4, 5, 6, 7, 8, 9,
                8, 9, 10, 11, 12, 13,
                12, 13, 14, 15, 16, 17,
                16, 17, 18, 19, 20, 21,
                20, 21, 22, 23, 24, 25,
                24, 25, 26, 27, 28, 29,
                28, 29, 30, 31, 32, 1
            };
            //p table
            int[] P = {
                16,  7, 20, 21, 29, 12, 28, 17,
                1, 15, 23, 26, 5, 18, 31, 10,
                2,  8, 24, 14, 32, 27,  3,  9,
                19, 13, 30,  6,22, 11,  4, 25
            };

            int lenght = plainText.Length / 2;
            // new left
            string Right = plainText.Substring(lenght);
            string Left = plainText.Substring(0, lenght);
            // Console.WriteLine( Right);
            // Console.WriteLine(  Left);

            string NewRight1 = FunctionOnRight(E_bitSelection, Right, Key);

            string NewRight2 = FunctionOnRightKey(NewRight1);

            string NewRight3 = FunctionOnRight(P, NewRight2, Left);

            //return array, first index has left and sec has right
            string concatenatedString = string.Concat(Right, NewRight3);
            //  Console.WriteLine( concatenatedString);

            return concatenatedString;
        }

        public string Inv_IP(string plainText)
        {

            int lenght = plainText.Length / 2;
            string Right = plainText.Substring(lenght);
            string Left = plainText.Substring(0, lenght);
            plainText = string.Concat(Right, Left);
            string Plain = "";
            int[] IP_inverse = {
                    40,  8, 48, 16, 56, 24, 64, 32,
                    39,  7, 47, 15, 55, 23, 63, 31,
                    38,  6, 46, 14, 54, 22, 62, 30,
                    37,  5, 45, 13, 53, 21, 61, 29,
                    36,  4, 44, 12, 52, 20, 60, 28,
                    35,  3, 43, 11, 51, 19, 59, 27,
                    34,  2, 42, 10, 50, 18, 58, 26,
                    33,  1, 41,  9, 49, 17, 57, 25
                };

            for (int i = 0; i < IP_inverse.Length; i++)
            {
                Plain += plainText[IP_inverse[i] - 1];

            }
            return Plain;


        }


        public override string Decrypt(string cipherText, string key)
        {

            List<string> roundKeys = get_keys(key);
            roundKeys.Reverse();
            string round = IP(cipherText);
            //16 rounds
            for (int i = 0; i < 16; i++)
            {
                round = Round(round, roundKeys[i]);

            }
            string finalcipher = Inv_IP(round);
            string hexCipher = "0x";
            hexCipher += Convert.ToString(Convert.ToInt64(finalcipher, 2), 16).ToUpper();
            while (hexCipher.Length != 18)
            {
                hexCipher = hexCipher.Insert(2, "0");
            }
            Console.WriteLine("Hexadecimal Cipher:" + hexCipher);

            return hexCipher;
        }

        public override string Encrypt(string plainText, string key)
        {

            List<string> roundKeys = get_keys(key);
            string round = IP(plainText);
            //16 rounds
            for (int i = 0; i < 16; i++)
            {
                round = Round(round, roundKeys[i]);

            }
            string finalcipher = Inv_IP(round);
            string hexCipher = "0x";
            hexCipher += Convert.ToString(Convert.ToInt64(finalcipher, 2), 16).ToUpper();
            while (hexCipher.Length != 18)
            {
                hexCipher = hexCipher.Insert(2, "0");
            }
            Console.WriteLine("Hexadecimal Cipher:" + hexCipher);

            return hexCipher;
        }
    }
}