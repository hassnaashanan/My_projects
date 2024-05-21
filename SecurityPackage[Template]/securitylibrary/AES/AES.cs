using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        public string HexToBinary(string hexString)
        {

            int hexValue = Convert.ToInt32(hexString, 16);


            string binaryString = Convert.ToString(hexValue, 2).PadLeft(hexString.Length * 4, '0');

            return binaryString;
        }
        public string BinaryToHex(string binaryString)
        {

            int binaryValue = Convert.ToInt32(binaryString, 2);

            string hexString = binaryValue.ToString("X2");

            return hexString;
        }
        public string Xor(string binary1, string binary2)
        {
            if (binary1.Length != 8)
            {
                binary1 = binary1.PadLeft(8, '0');
            }
            if (binary2.Length != 8)
            {
                binary2 = binary2.PadLeft(8, '0');
            }

            StringBuilder resultBuilder = new StringBuilder();

            for (int i = 0; i < binary1.Length; i++)
            {
                resultBuilder.Append(binary1[i] == binary2[i] ? '0' : '1');
            }

            return resultBuilder.ToString();
        }
        public string[,] ToMatrix(string Matrix)
        {
            string[,] Mat = new string[4, 4];
            int k = 2;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    Mat[j, i] = Matrix[k].ToString() + Matrix[k + 1];
                    k = k + 2;

                }

            }
            return Mat;
        }
        public string[,] CopyMatrixToLargerMatrix(string[,] sourceMatrix, string[,] destinationMatrix, int startingColumn)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    destinationMatrix[i, startingColumn + j] = sourceMatrix[i, j];
                }
            }
            return destinationMatrix;
        }
        public string[,] CopyMatrixToSmallMatrix(string[,] sourceMatrix, string[,] destinationMatrix, int startingColumn)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    destinationMatrix[i, j] = sourceMatrix[i, startingColumn + j];
                }
            }
            return destinationMatrix;
        }

        public string[,] HexToStateM(string text)
        {
            text = text.Replace("0x", "");


            string[,] s = new string[4, 4];
            int w = 0;
            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    s[r, c] = text.Substring(w, 2);
                    w += 2;
                }
            }
            return s;
        }

        public string[,] ShiftR(string[,] stateM)
        {
            for (int r = 1; r < 4; r++)
            {
                for (int s = 0; s < r; s++)
                {
                    string x = stateM[r, 0];
                    for (int c = 0; c < 3; c++)
                    {
                        stateM[r, c] = stateM[r, c + 1];
                    }
                    stateM[r, 3] = x;
                }
            }
            return stateM;
        }
        public string[,] ShiftL(string[,] stateM)
        {
            for (int r = 1; r < 4; r++)
            {
                for (int s = 0; s < r; s++)
                {
                    string x = stateM[r, 3];
                    for (int c = 3; c > 0; c--)
                    {
                        stateM[r, c] = stateM[r, c - 1];
                    }
                    stateM[r, 0] = x;
                }
            }
            return stateM;
        }
        public static string[,] AddRoundKey(string[,] sM, string[,] rKey)
        {


            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                {
                    byte stateByte = Convert.ToByte(sM[r, c], 16);
                    byte roundKeyByte = Convert.ToByte(rKey[r, c], 16);

                    sM[r, c] = (stateByte ^ roundKeyByte).ToString("X2");
                }
            }
            return sM;
        }
        public string[,] Mix_Columns(string[,] matrix, string[,] state_matrix)
        {
            string[,] resultMatrix = new string[4, 4];


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    StringBuilder bin = new StringBuilder("00000000");
                    for (int k = 0; k < 4; k++)
                    {
                        string res = "00000000";
                        if (state_matrix[i, k] == "01")
                        {
                            res = HexToBinary(matrix[k, j]);
                        }
                        else
                        {
                            string elm2 = HexToBinary(matrix[k, j]);
                            string shiftedNumber = elm2.Substring(1) + "0";
                            if (elm2[0] == '1') { shiftedNumber = Xor(shiftedNumber, "00011011"); }
                            if (state_matrix[i, k] == "02")
                            {
                                res = shiftedNumber;
                            }
                            else
                            {
                                string lastor = Xor(shiftedNumber, elm2);
                                res = lastor;
                            }
                        }
                        bin = new StringBuilder(Xor(bin.ToString(), res));
                    }
                    resultMatrix[i, j] = BinaryToHex(bin.ToString());
                }
            }
            return resultMatrix;
        }
        public string[,] INV_Mix_Columns(string[,] matrix)
        {
            string[,] resultMatrix = new string[4, 4];
            string[,] state_matrix2 = {
                { "0E", "0B", "0D", "09" },
                { "09", "0E", "0B", "0D" },
                { "0D", "09", "0E", "0B" },
                { "0B", "0D", "09", "0E" }
            };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    StringBuilder bin = new StringBuilder("00000000");
                    for (int k = 0; k < 4; k++)
                    {
                        string res = "00000000";
                        string elm2 = HexToBinary(matrix[k, j]);
                        if (state_matrix2[i, k] == "09")
                        {
                            string lastor = Multby09(elm2);
                            res = lastor;
                        }
                        else if (state_matrix2[i, k] == "0D")
                        {
                            string lastor = Multby0D(elm2);
                            res = lastor;
                        }
                        else if (state_matrix2[i, k] == "0E")
                        {
                            string lastor = Multby0E(elm2);
                            res = lastor;
                        }
                        else
                        {
                            string lastor = Multby0B(elm2);
                            res = lastor;
                        }

                        bin = new StringBuilder(Xor(bin.ToString(), res));
                    }
                    resultMatrix[i, j] = BinaryToHex(bin.ToString());
                }
            }
            return resultMatrix;
        }
        public string Multby09(string elm2)
        {

            string shiftedNumber = elm2.Substring(1) + "0";
            if (elm2[0] == '1') { shiftedNumber = Xor(shiftedNumber, "00011011"); }

            if (shiftedNumber[0] == '1') { shiftedNumber = Xor((shiftedNumber.Substring(1) + "0"), "00011011"); }
            else
            {
                shiftedNumber = shiftedNumber.Substring(1) + "0";
            }

            if (shiftedNumber[0] == '1') { shiftedNumber = Xor((shiftedNumber.Substring(1) + "0"), "00011011"); }
            else { shiftedNumber = shiftedNumber.Substring(1) + "0"; }

            string lastor = Xor(shiftedNumber, elm2);

            return lastor;

        }
        public string Multby0B(string elm2)
        {

            string shiftedNumber = Multby09(elm2);
            string sh02 = Multby02(elm2);
            string lastor = Xor(shiftedNumber, sh02);

            return lastor;

        }
        public string Multby0D(string elm2)
        {

            string shiftedNumber1 = Multby09(elm2);
            string shiftedNumber = elm2.Substring(1) + "0";
            if (elm2[0] == '1') { shiftedNumber = Xor(shiftedNumber, "00011011"); }

            if (shiftedNumber[0] == '1') { shiftedNumber = Xor((shiftedNumber.Substring(1) + "0"), "00011011"); }
            else
            {
                shiftedNumber = shiftedNumber.Substring(1) + "0";
            }
            string lastor = Xor(shiftedNumber1, shiftedNumber);
            return lastor;

        }
        public string Multby0E(string elm2)
        {

            string shiftedNumber1 = Multby02(elm2);
            string shiftedNumber = elm2.Substring(1) + "0";
            if (elm2[0] == '1') { shiftedNumber = Xor(shiftedNumber, "00011011"); }

            if (shiftedNumber[0] == '1') { shiftedNumber = Xor((shiftedNumber.Substring(1) + "0"), "00011011"); }
            else
            {
                shiftedNumber = shiftedNumber.Substring(1) + "0";
            }
            string firstor = Xor(shiftedNumber1, shiftedNumber);
            string lastor = Xor(firstor, shift04(elm2));
            return lastor;

        }
        public string Multby02(string elm2)
        {
            string shiftedNumber = elm2.Substring(1) + "0";
            if (elm2[0] == '1') { shiftedNumber = Xor(shiftedNumber, "00011011"); }
            return shiftedNumber;

        }
        public string shift04(string elm2)
        {
            string shiftedNumber = elm2.Substring(1) + "0";
            if (elm2[0] == '1') { shiftedNumber = Xor(shiftedNumber, "00011011"); }

            if (shiftedNumber[0] == '1') { shiftedNumber = Xor((shiftedNumber.Substring(1) + "0"), "00011011"); }
            else
            {
                shiftedNumber = shiftedNumber.Substring(1) + "0";
            }

            if (shiftedNumber[0] == '1') { shiftedNumber = Xor((shiftedNumber.Substring(1) + "0"), "00011011"); }
            else { shiftedNumber = shiftedNumber.Substring(1) + "0"; }
            return shiftedNumber;

        }
        public string[,] SubByte(string[,] m, int row = 4, int col = 4, bool useInverseSBox = false)
        {

            string[,] sBox = new string[16, 16]
{
    {"63", "7C", "77", "7B", "F2", "6B", "6F", "C5", "30", "01", "67", "2B", "FE", "D7", "AB", "76"},
    {"CA", "82", "C9", "7D", "FA", "59", "47", "F0", "AD", "D4", "A2", "AF", "9C", "A4", "72", "C0"},
    {"B7", "FD", "93", "26", "36", "3F", "F7", "CC", "34", "A5", "E5", "F1", "71", "D8", "31", "15"},
    {"04", "C7", "23", "C3", "18", "96", "05", "9A", "07", "12", "80", "E2", "EB", "27", "B2", "75"},
    {"09", "83", "2C", "1A", "1B", "6E", "5A", "A0", "52", "3B", "D6", "B3", "29", "E3", "2F", "84"},
    {"53", "D1", "00", "ED", "20", "FC", "B1", "5B", "6A", "CB", "BE", "39", "4A", "4C", "58", "CF"},
    {"D0", "EF", "AA", "FB", "43", "4D", "33", "85", "45", "F9", "02", "7F", "50", "3C", "9F", "A8"},
    {"51", "A3", "40", "8F", "92", "9D", "38", "F5", "BC", "B6", "DA", "21", "10", "FF", "F3", "D2"},
    {"CD", "0C", "13", "EC", "5F", "97", "44", "17", "C4", "A7", "7E", "3D", "64", "5D", "19", "73"},
    {"60", "81", "4F", "DC", "22", "2A", "90", "88", "46", "EE", "B8", "14", "DE", "5E", "0B", "DB"},
    {"E0", "32", "3A", "0A", "49", "06", "24", "5C", "C2", "D3", "AC", "62", "91", "95", "E4", "79"},
    {"E7", "C8", "37", "6D", "8D", "D5", "4E", "A9", "6C", "56", "F4", "EA", "65", "7A", "AE", "08"},
    {"BA", "78", "25", "2E", "1C", "A6", "B4", "C6", "E8", "DD", "74", "1F", "4B", "BD", "8B", "8A"},
    {"70", "3E", "B5", "66", "48", "03", "F6", "0E", "61", "35", "57", "B9", "86", "C1", "1D", "9E"},
    {"E1", "F8", "98", "11", "69", "D9", "8E", "94", "9B", "1E", "87", "E9", "CE", "55", "28", "DF"},
    {"8C", "A1", "89", "0D", "BF", "E6", "42", "68", "41", "99", "2D", "0F", "B0", "54", "BB", "16"}
};

            string[,] inverseSBox = new string[16, 16]
{
    {"52", "09", "6A", "D5", "30", "36", "A5", "38", "BF", "40", "A3", "9E", "81", "F3", "D7", "FB"},
    {"7C", "E3", "39", "82", "9B", "2F", "FF", "87", "34", "8E", "43", "44", "C4", "DE", "E9", "CB"},
    {"54", "7B", "94", "32", "A6", "C2", "23", "3D", "EE", "4C", "95", "0B", "42", "FA", "C3", "4E"},
    {"08", "2E", "A1", "66", "28", "D9", "24", "B2", "76", "5B", "A2", "49", "6D", "8B", "D1", "25"},
    {"72", "F8", "F6", "64", "86", "68", "98", "16", "D4", "A4", "5C", "CC", "5D", "65", "B6", "92"},
    {"6C", "70", "48", "50", "FD", "ED", "B9", "DA", "5E", "15", "46", "57", "A7", "8D", "9D", "84"},
    {"90", "D8", "AB", "00", "8C", "BC", "D3", "0A", "F7", "E4", "58", "05", "B8", "B3", "45", "06"},
    {"D0", "2C", "1E", "8F", "CA", "3F", "0F", "02", "C1", "AF", "BD", "03", "01", "13", "8A", "6B"},
    {"3A", "91", "11", "41", "4F", "67", "DC", "EA", "97", "F2", "CF", "CE", "F0", "B4", "E6", "73"},
    {"96", "AC", "74", "22", "E7", "AD", "35", "85", "E2", "F9", "37", "E8", "1C", "75", "DF", "6E"},
    {"47", "F1", "1A", "71", "1D", "29", "C5", "89", "6F", "B7", "62", "0E", "AA", "18", "BE", "1B"},
    {"FC", "56", "3E", "4B", "C6", "D2", "79", "20", "9A", "DB", "C0", "FE", "78", "CD", "5A", "F4"},
    {"1F", "DD", "A8", "33", "88", "07", "C7", "31", "B1", "12", "10", "59", "27", "80", "EC", "5F"},
    {"60", "51", "7F", "A9", "19", "B5", "4A", "0D", "2D", "E5", "7A", "9F", "93", "C9", "9C", "EF"},
    {"A0", "E0", "3B", "4D", "AE", "2A", "F5", "B0", "C8", "EB", "BB", "3C", "83", "53", "99", "61"},
    {"17", "2B", "04", "7E", "BA", "77", "D6", "26", "E1", "69", "14", "63", "55", "21", "0C", "7D"}
};
            string[,] selectedSBox;
            if (useInverseSBox == true)
            {
                selectedSBox = inverseSBox;

            }
            else
            {
                selectedSBox = sBox;

            }

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    int sBoxR = Convert.ToInt32(m[r, c].Substring(0, 1), 16);
                    int sBoxC = Convert.ToInt32(m[r, c].Substring(1, 1), 16);
                    m[r, c] = selectedSBox[sBoxR, sBoxC];
                }
            }


            return m;
        }

        public string[,] KeySchudel(string key, string[,] Rcon, bool useInverseSBox = false)
        {

            //key matrix
            string[,] OldKey = new string[4, 4];
            OldKey = ToMatrix(key);
            //shift 1st col
            string[,] fstcol = new string[4, 1];

            fstcol[0, 0] = OldKey[1, 3];
            fstcol[1, 0] = OldKey[2, 3];
            fstcol[2, 0] = OldKey[3, 3];
            fstcol[3, 0] = OldKey[0, 3];

            //sub byte
            fstcol = SubByte(fstcol, 4, 1, useInverseSBox);
            //XOR
            string[,] NewKey = new string[4, 4];
            string val1 = "";
            string val2 = "";
            string val3 = "";
            string hex = "";
            int res = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    if (i == 0)
                    {
                        val1 = HexToBinary(OldKey[j, i]);
                        val2 = HexToBinary(Rcon[j, 0]);
                        val3 = HexToBinary(fstcol[j, i]);
                        hex = "";

                        for (int n = 0; n < 8; n++)
                        {
                            res = int.Parse(val1[n].ToString()) +
                            int.Parse(val2[n].ToString()) +
                            int.Parse(val3[n].ToString());

                            if (res == 3 || res == 1)
                            {
                                hex += "1";
                            }
                            else
                            {
                                hex += "0";
                            }
                        }

                        hex = BinaryToHex(hex);

                        NewKey[j, i] = hex;

                    }
                    else
                    {
                        val1 = HexToBinary(OldKey[j, i]);
                        val2 = HexToBinary(NewKey[j, i - 1]);
                        hex = "";
                        for (int n = 0; n < 8; n++)
                        {
                            if (int.Parse(val1[n].ToString()) == int.Parse(val2[n].ToString()))
                            {
                                hex += "0";
                            }
                            else
                            {
                                hex += "1";
                            }
                        }

                        hex = BinaryToHex(hex);
                        NewKey[j, i] = hex;
                    }
                }

            }

            return NewKey;
        }
        public string[,] KeyGeneration(string key, bool useInverseSBox = false)

        {
            string KeyStr = key;
            string[,] Keys = new string[4, 40];
            string[,] KeyMat = new string[4, 4];
            int A = 0;
            string[,] Rcon = {
        { "01", "02", "04", "08", "10", "20", "40", "80", "1B", "36" },
        { "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" },
        { "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" },
        { "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" }
            };
            for (int X = 0; X < 10; X++)
            {

                //conncatination key
                if (X != 0)
                {

                    KeyStr = "0x";
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            KeyStr += KeyMat[j, i];
                        }
                    }

                }
                // divide rcon columns for loop over x

                string[,] Column = new string[4, 1];

                for (int i = 0; i < 4; i++)
                {
                    Column[i, 0] = Rcon[i, X];

                }

                KeyMat = KeySchudel(KeyStr, Column, useInverseSBox);

                Keys = CopyMatrixToLargerMatrix(KeyMat, Keys, A);

                A += 4;

            }
            return Keys;
        }


        public override string Decrypt(string cipherText, string key)
        {
            string PlainText = "0X";
            string[,] cipher = new string[4, 4];
            string[,] KeyG = new string[4, 4];
            string[,] Keys = new string[4, 40];
            Keys = KeyGeneration(key);
            cipher = ToMatrix(cipherText);



            int A = 36;
            for (int i = 0; i < 11; i++)
            {
                if (i != 10)
                {
                    KeyG = CopyMatrixToSmallMatrix(Keys, KeyG, A);
                }
                if (i == 0)
                {

                    cipher = AddRoundKey(cipher, KeyG);
                    cipher = ShiftL(cipher);
                    cipher = SubByte(cipher, 4, 4, true);
                }
                else if (i == 10)
                {

                    cipher = AddRoundKey(cipher, ToMatrix(key));
                }
                else
                {
                    cipher = AddRoundKey(cipher, KeyG);
                    cipher = INV_Mix_Columns(cipher);
                    cipher = ShiftL(cipher);
                    cipher = SubByte(cipher, 4, 4, true);
                }
                A = A - 4;

            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    PlainText += cipher[j, i];

                }

            }
            Console.Write(PlainText);
            Console.WriteLine();
            return PlainText;
        }

        public override string Encrypt(string plainText, string key)
        {

            string PlainText = "0X";
            string[,] cipher = new string[4, 4];
            string[,] KeyG = new string[4, 4];
            string[,] Keys = new string[4, 40];
            Keys = KeyGeneration(key);
            cipher = ToMatrix(plainText);
            string[,] State = {
                { "02", "03", "01", "01" },
                { "01", "02", "03", "01" },
                { "01", "01", "02", "03" },
                { "03", "01", "01", "02" }
            };


            int A = -4;
            for (int i = 0; i < 11; i++)
            {
                if (i != 0)
                {
                    KeyG = CopyMatrixToSmallMatrix(Keys, KeyG, A);
                }
                if (i == 0)
                {

                    cipher = AddRoundKey(cipher, ToMatrix(key));


                }
                else if (i == 10)
                {
                    cipher = SubByte(cipher, 4, 4);
                    cipher = ShiftR(cipher);
                    cipher = AddRoundKey(cipher, KeyG);

                }
                else
                {
                    cipher = SubByte(cipher, 4, 4);
                    cipher = ShiftR(cipher);
                    cipher = Mix_Columns(cipher, State);
                    cipher = AddRoundKey(cipher, KeyG);



                }
                A = A + 4;

            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    PlainText += cipher[j, i];

                }

            }
            Console.Write(PlainText);
            Console.WriteLine();
            return PlainText;
        }
    }
}
