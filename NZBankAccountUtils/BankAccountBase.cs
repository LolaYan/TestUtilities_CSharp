using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.NZBankAccountUtils
{
    public class BankAccountBase
    {
        public static string BankIDStr;
        public static string BankBranchStr;
        public static string AlgorithmType;
        public static string AlgorithmMod;
        public static string checkDigit;
        public static string BankAccountBaseStr;
        public static string suffixStr;
        public static string BankAccountStr;
        public static int[] algorithmWeight;

        public static string accountBaseLengthSetting;
        public static string suffixSetting = "000";

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string PaddingLeftWithZero(int num, int length)
        {
            string number = num.ToString().PadLeft(length, '0');
            return number;
        }

        //getBankAccount
        public static string getBankAccount()
        {
            //1. get BankID
            BankIDStr = getBankID();
            //2. get BankBranch
            //Based on BankID, filter the AlgorithmType of D, E, E, G, X, F
            BankBranchStr = getBankBranch(BankIDStr);
            doMod();
            Console.WriteLine("BankIDStr: " + BankIDStr);
            Console.WriteLine("BankBranchStr: " + BankBranchStr);
            Console.WriteLine("BankAccountBaseStr: " + BankAccountBaseStr);
            Console.WriteLine("suffixStr: " + suffixStr);
            Console.WriteLine("AlgorithmType: " + AlgorithmType);
            Console.WriteLine("algorithmWeight: " + string.Join(",", algorithmWeight));
            Console.WriteLine("AlgorithmMod: " + AlgorithmMod);
            Console.WriteLine("checkDigit: " + checkDigit);
            Console.WriteLine("BankAccountStr: " + BankIDStr + "-" + BankBranchStr + "-" + BankAccountBaseStr + "-" + suffixStr);
            Console.WriteLine("BankAccountStr: " + BankAccountStr);
            Console.WriteLine("BankAccountStr length: " + BankAccountStr.Length);
            return BankAccountStr;
        }



        public static string doMod()
        {
            do
            {
                getBankAccountBaseStr();

                String tempBankAccountStr = BankIDStr + BankBranchStr + BankAccountBaseStr;
                //validate the length of BankAccountStr is 2+4+7 = 13
                // get AlgorithmWeight
                algorithmWeight = GetAlgorithmWeight(AlgorithmType);
                if (AlgorithmType.Equals("A") || AlgorithmType.Equals("B") || AlgorithmType.Equals("C") || AlgorithmType.Equals("D"))
                {

                    checkDigit = GetCheckDigit_Mod11(AlgorithmType, tempBankAccountStr, algorithmWeight);
                }
                else if (AlgorithmType.Equals("E"))
                {
                    tempBankAccountStr = tempBankAccountStr + suffixStr;
                    checkDigit = GetCheckDigit_Mod11(AlgorithmType, tempBankAccountStr, algorithmWeight);
                }
                else if (AlgorithmType.Equals("F"))
                {
                    checkDigit = GetCheckDigit_Mod10(AlgorithmType, tempBankAccountStr, algorithmWeight);
                }
                else if (AlgorithmType.Equals("G"))
                {
                    tempBankAccountStr = tempBankAccountStr + suffixStr;
                    checkDigit = GetCheckDigit_Mod10(AlgorithmType, tempBankAccountStr, algorithmWeight);
                }
                else if (AlgorithmType.Equals("X"))
                {
                    AlgorithmMod = "Mod 1";
                    checkDigit = RandomNumber(0, 9).ToString();
                }
            } while (checkDigit.Equals("N/A"));


            if (AlgorithmType.Equals("E") || AlgorithmType.Equals("G"))
            {

                suffixStr = suffixStr + checkDigit;
            }
            else
            {
                BankAccountBaseStr = BankAccountBaseStr + checkDigit;
                checkSuffixSetting();
            }

            checkAccountBaseLengthSetting();
            BankAccountStr = BankIDStr + BankBranchStr + BankAccountBaseStr + suffixStr;

            return BankAccountStr;
        }


        //If AlgorithmType is E or G, return the account base and first 3 digit of suffix.
        //If AlgorithmType is others, return first 7 digit of the account base and first 3 digit of suffix.
        public static void getBankAccountBaseStr()
        {
            int ranNoBankAccountBase;
            int ranNoSuffix;

            if (AlgorithmType.Equals("E") || AlgorithmType.Equals("G"))
            {
                ranNoBankAccountBase = RandomNumber(0, 09999999);
                BankAccountBaseStr = PaddingLeftWithZero(ranNoBankAccountBase, 8);

                ranNoSuffix = RandomNumber(0, 099);
                suffixStr = PaddingLeftWithZero(ranNoSuffix, 3);

            }
            else
            {
                ranNoBankAccountBase = RandomNumber(0, 0999999);
                BankAccountBaseStr = PaddingLeftWithZero(ranNoBankAccountBase, 7);

                ranNoSuffix = RandomNumber(0, 9999);
                suffixStr = PaddingLeftWithZero(ranNoSuffix, 4);
            }

            setAlgorithmBasedonAccountBase(ranNoBankAccountBase);
            //After setAlgorithmBasedonAccountBase(), the AlgorithmType should be set up.



        }

        public static void setAlgorithmBasedonAccountBase(int accountBase)
        {
            if (AlgorithmType.Equals("Unkown"))
            {
                if (accountBase < 99000)
                {
                    AlgorithmType = "A";

                }
                else
                {
                    AlgorithmType = "B";
                }
            }
        }

        public static string getBankID()
        {
            string[] BankIDArr = { "01", "02", "03", "06", "08", "09", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "33", "35", "38" };
            Random random = new Random();
            int index = random.Next(0, BankIDArr.Count() - 1);
            BankIDStr = BankIDArr[index];
            return BankIDStr;
        }

        public static string getBankID(string type)
        {
            switch (type)
            {
                case "A":
                    BankIDStr = getBankID_AB();
                    break;
                case "B":
                    BankIDStr = getBankID_AB();
                    break;
                case "D":
                    BankIDStr = getBankID_D();
                    break;
                case "E":
                    BankIDStr = getBankID_E();
                    break;
                case "F":
                    BankIDStr = getBankID_F();
                    break;
                case "G":
                    BankIDStr = getBankID_G();
                    break;
                case "X":
                    BankIDStr = getBankID_X();
                    break;
                default:
                    BankIDStr = getBankID_AB();
                    break;
            }
            return BankIDStr;
        }


        public static string getBankID_AB()
        {
            string[] BankIDArr = { "01", "02", "03", "06", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "27", "30", "35", "38" };
            Random random = new Random();
            int index = random.Next(0, BankIDArr.Count() - 1);
            BankIDStr = BankIDArr[index];
            return BankIDStr;
        }

        public static string getBankID_D()
        {
            BankIDStr = "08";
            return BankIDStr;
        }

        public static string getBankID_E()
        {
            BankIDStr = "09";
            return BankIDStr;
        }

        public static string getBankID_F()
        {
            string[] BankIDArr = { "25", "33" };
            Random random = new Random();
            int index = random.Next(0, BankIDArr.Count() - 1);
            BankIDStr = BankIDArr[index];
            return BankIDStr;
        }


        public static string getBankID_G()
        {
            string[] BankIDArr = { "26", "28", "29" };
            Random random = new Random();
            int index = random.Next(0, BankIDArr.Count() - 1);
            BankIDStr = BankIDArr[index];
            return BankIDStr;
        }

        public static string getBankID_X()
        {
            BankIDStr = "31";
            return BankIDStr;
        }

        public static string getBankBranch(string BankIDStr)
        {
            int BankBranch;
            string BankBranchStr;
            //string BankIDStr = PaddingLeftWithZero(BankID,2);
            int i;
            AlgorithmType = "Unkown";
            switch (BankIDStr)
            {
                case "01":
                    i = RandomNumber(1, 3);
                    if (i == 1)
                    {
                        BankBranch = RandomNumber(1, 999);
                    }
                    else if (i == 2)
                    {
                        BankBranch = RandomNumber(1100, 1199);
                    }
                    else
                    {
                        BankBranch = RandomNumber(1800, 1899);
                    }
                    break;
                case "02":
                    i = RandomNumber(1, 2);
                    if (i == 1)
                    {
                        BankBranch = RandomNumber(1, 999);
                    }
                    else
                    {
                        BankBranch = RandomNumber(1200, 1299);
                    }
                    break;
                case "03":
                    i = RandomNumber(1, 5);
                    if (i == 1)
                    {
                        BankBranch = RandomNumber(1, 999);
                    }
                    else if (i == 2)
                    {
                        BankBranch = RandomNumber(1300, 1399);
                    }
                    else if (i == 3)
                    {
                        BankBranch = RandomNumber(1500, 1599);
                    }
                    else if (i == 4)
                    {
                        BankBranch = RandomNumber(1700, 1799);
                    }
                    else
                    {
                        BankBranch = RandomNumber(1900, 1999);
                    }
                    break;
                case "06":
                    i = RandomNumber(1, 2);
                    if (i == 1)
                    {
                        BankBranch = RandomNumber(1, 999);
                    }
                    else
                    {
                        BankBranch = RandomNumber(1400, 1499);
                    }
                    break;
                case "08":
                    BankBranch = RandomNumber(6500, 6599);
                    AlgorithmType = "D";
                    break;
                case "09":
                    BankBranch = 0;
                    AlgorithmType = "E";
                    break;
                case "11":
                    i = RandomNumber(1, 2);
                    if (i == 1)
                    {
                        BankBranch = RandomNumber(5000, 6499);
                    }
                    else
                    {
                        BankBranch = RandomNumber(6600, 8999);
                    }
                    break;
                case "12":
                    i = RandomNumber(1, 3);
                    if (i == 1)
                    {
                        BankBranch = RandomNumber(3000, 3299);
                    }
                    else if (i == 2)
                    {
                        BankBranch = RandomNumber(3400, 3499);
                    }
                    else
                    {
                        BankBranch = RandomNumber(3600, 3699);
                    }
                    break;
                case "13":
                    BankBranch = RandomNumber(4900, 4999);
                    break;
                case "14":
                    BankBranch = RandomNumber(4700, 4799);
                    break;
                case "15":
                    BankBranch = RandomNumber(3900, 3999);
                    break;
                case "16":
                    BankBranch = RandomNumber(4400, 4499);
                    break;
                case "17":
                    BankBranch = RandomNumber(3300, 3399);
                    break;
                case "18":
                    BankBranch = RandomNumber(3500, 3599);
                    break;
                case "19":
                    BankBranch = RandomNumber(4600, 4649);
                    break;
                case "20":
                    BankBranch = RandomNumber(4100, 4199);
                    break;
                case "21":
                    BankBranch = RandomNumber(4800, 4899);
                    break;
                case "22":
                    BankBranch = RandomNumber(4000, 4049);
                    break;
                case "23":
                    BankBranch = RandomNumber(3700, 3799);
                    break;
                case "24":
                    BankBranch = RandomNumber(4300, 4399);
                    break;
                case "25":
                    BankBranch = RandomNumber(2500, 2599);
                    AlgorithmType = "F";
                    break;
                case "26":
                    BankBranch = RandomNumber(2600, 2699);
                    AlgorithmType = "G";
                    break;
                case "27":
                    BankBranch = RandomNumber(3800, 3849);
                    break;
                case "28":
                    BankBranch = RandomNumber(2100, 2149);
                    AlgorithmType = "G";
                    break;
                case "29":
                    BankBranch = RandomNumber(2150, 2299);
                    AlgorithmType = "G";
                    break;
                case "30":
                    BankBranch = RandomNumber(2900, 2949);
                    break;
                case "31":
                    BankBranch = RandomNumber(2800, 2849);
                    AlgorithmType = "X";
                    break;
                case "33":
                    BankBranch = RandomNumber(6700, 6799);
                    AlgorithmType = "F";
                    break;
                case "35":
                    BankBranch = RandomNumber(2400, 2499);
                    break;
                case "38":
                    BankBranch = RandomNumber(9000, 9499);
                    break;
                default:
                    BankIDStr = "01";
                    BankBranch = RandomNumber(1, 999);
                    break;

            }

            BankBranchStr = PaddingLeftWithZero(BankBranch, 4);
            return BankBranchStr;
        }

        public static int getBankAccountBasePartially()
        {
            int BankAccountBase1 = RandomNumber(0, 9999999);
            int BankAccountBase2 = RandomNumber(0, int.Parse("99999999999")); // only for Algorithm E and G
            return BankAccountBase1;
        }


        public static int[] GetAlgorithmWeight(string AlgorithmType)
        {

            int[] WeightNumberArr = new int[18] { 0, 0, 6, 3, 7, 9, 0, 0, 10, 5, 8, 4, 2, 1, 0, 0, 0, 0 };
            switch (AlgorithmType)
            {
                case "A":
                    int[] WeightNumberArrA = new int[18] { 0, 0, 6, 3, 7, 9, 0, 0, 10, 5, 8, 4, 2, 1, 0, 0, 0, 0 };
                    WeightNumberArr = WeightNumberArrA;
                    break;
                case "B":
                    int[] WeightNumberArrB = new int[18] { 0, 0, 0, 0, 0, 0, 0, 0, 10, 5, 8, 4, 2, 1, 0, 0, 0, 0 };
                    WeightNumberArr = WeightNumberArrB;
                    break;
                case "C":
                    int[] WeightNumberArrC = new int[18] { 3, 7, 0, 0, 0, 0, 9, 1, 10, 5, 3, 4, 2, 1, 0, 0, 0, 0 };
                    WeightNumberArr = WeightNumberArrC;
                    break;
                case "D":
                    int[] WeightNumberArrD = new int[18] { 0, 0, 0, 0, 0, 0, 0, 7, 6, 5, 4, 3, 2, 1, 0, 0, 0, 0 };
                    WeightNumberArr = WeightNumberArrD;
                    break;
                case "E":
                    int[] WeightNumberArrE = new int[18] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 4, 3, 2, 0, 0, 0, 1 };
                    WeightNumberArr = WeightNumberArrE;
                    break;
                case "F":
                    int[] WeightNumberArrF = new int[18] { 0, 0, 0, 0, 0, 0, 0, 1, 7, 3, 1, 7, 3, 1, 0, 0, 0, 0 };
                    WeightNumberArr = WeightNumberArrF;
                    break;
                case "G":
                    int[] WeightNumberArrG = new int[18] { 0, 0, 0, 0, 0, 0, 0, 1, 3, 7, 1, 3, 7, 1, 0, 3, 7, 1 };
                    WeightNumberArr = WeightNumberArrG;
                    break;
                case "X":
                    int[] WeightNumberArrX = new int[18] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    WeightNumberArr = WeightNumberArrX;
                    break;
                default:
                    break;

            }
            return WeightNumberArr;
        }
        public static string GetCheckDigit_Mod11(string AlgorithmType, string number, int[] WeightNumberArr)
        {

            //Only AlgorithmType A, B, C, D, E are accepted here
            int sum = 0;
            int digitSum = 0;
            int digitValue = 0;
            int weightValue = 0;
            int accountLength;

            AlgorithmMod = "Mod11";

            if (AlgorithmType.Equals("E"))
            {
                accountLength = 17;
            }
            else
            {
                accountLength = 13;
            }

            for (int i = 0; i < accountLength; i++)
            {
                digitValue = (int)char.GetNumericValue(number[i]);
                weightValue = WeightNumberArr[i];
                digitSum = digitValue * weightValue;
                sum += digitSum;
                //Console.WriteLine("No: " + i + "digitValue: " + digitValue + " * weightValue: " + weightValue + " = digitSum: " + digitSum);
            }
            int modNo = 11;
            int mod = (sum % modNo);
            if (mod == 0)
            {
                return "0";
            }
            else if (mod == 1)
            {
                return "N/A";
            }
            else
            {

                return (modNo - mod).ToString();
            }
        }
        public static string GetCheckDigit_Mod10(string AlgorithmType, string number, int[] WeightNumberArr)
        {
            //Only AlgorithmType F, G are accepted here
            int sum = 0;
            int digitSum = 0;
            int digitValue = 0;
            int weightValue = 0;
            int accountLength;

            AlgorithmMod = "Mod10";

            if (AlgorithmType.Equals("G"))
            {
                accountLength = 17;
            }
            else
            {
                accountLength = 13;
            }

            for (int i = 0; i < accountLength; i++)
            {
                digitValue = (int)char.GetNumericValue(number[i]);
                weightValue = WeightNumberArr[i];
                digitSum = digitValue * weightValue;
                sum += digitSum;
                //Console.WriteLine("No: " + i + "digitValue: " + digitValue + " * weightValue: " + weightValue + " = digitSum: " + digitSum);
            }
            int modNo = 10;
            int mod = (sum % modNo);
            if (mod == 0)
            {
                return "0";
            }
            else
            {

                return (modNo - mod).ToString();
            }
        }

        public static void printInfo()
        {
            Console.WriteLine("BankIDStr: " + BankIDStr);
            Console.WriteLine("BankBranchStr: " + BankBranchStr);
            Console.WriteLine("BankAccountBaseStr: " + BankAccountBaseStr);
            Console.WriteLine("suffixStr: " + suffixStr);
            Console.WriteLine("AlgorithmType: " + AlgorithmType);
            Console.WriteLine("algorithmWeight: " + string.Join(",", algorithmWeight));
            Console.WriteLine("AlgorithmMod: " + AlgorithmMod);
            Console.WriteLine("checkDigit: " + checkDigit);
            Console.WriteLine("BankAccountStr: " + BankIDStr + "-" + BankBranchStr + "-" + BankAccountBaseStr + "-" + suffixStr);
            Console.WriteLine("BankAccountStr: " + BankAccountStr);
            Console.WriteLine("BankAccountStr length: " + BankAccountStr.Length);

        }

        public static void checkAccountBaseLengthSetting()
        {
            if (accountBaseLengthSetting.Equals("7"))
            {
                BankAccountBaseStr = BankAccountBaseStr.Remove(0, 1);
            }
        }

        public static void checkSuffixSetting()
        {
            //Not applied to Algorithm E and G
            if (suffixSetting.Equals("R4"))
            {
            }
            else if (suffixSetting.Equals("R3"))
            {
                suffixStr = suffixStr.Remove(0, 1);
            }
            else if (suffixSetting.Equals("0000"))
            {
                suffixStr = "0000";
            }
            else
            {
                suffixStr = "000";
            }
        }

    }
}