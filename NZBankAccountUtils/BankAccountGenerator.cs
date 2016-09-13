using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.NZBankAccountUtils
{
    public class BankAccountGenerator : BankAccountBase
    {


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

        public static string getBankAccountA()
        {
            //1. get BankID
            BankIDStr = getBankID_AB();
            //2. get BankBranch
            //Based on BankID, filter the AlgorithmType of D, E, E, G, X, F
            BankBranchStr = getBankBranch(BankIDStr);

            suffixStr = PaddingLeftWithZero(RandomNumber(0, 9999), 4);
            AlgorithmType = "A";
            do
            {
                BankAccountBaseStr = PaddingLeftWithZero(RandomNumber(0, 0098999), 7);
                String tempBankAccountStr = BankIDStr + BankBranchStr + BankAccountBaseStr;
                algorithmWeight = GetAlgorithmWeight(AlgorithmType);
                checkDigit = GetCheckDigit_Mod11(AlgorithmType, tempBankAccountStr, algorithmWeight);
            } while (checkDigit.Equals("N/A"));
            BankAccountBaseStr = BankAccountBaseStr + checkDigit;
            checkSuffixSetting();
            checkAccountBaseLengthSetting();
            BankAccountStr = BankIDStr + BankBranchStr + BankAccountBaseStr + suffixStr;
            printInfo();
            return BankAccountStr;
        }

        public static string getBankAccountB()
        {
            //1. get BankID
            BankIDStr = getBankID_AB();
            //2. get BankBranch
            //Based on BankID, filter the AlgorithmType of D, E, E, G, X, F
            BankBranchStr = getBankBranch(BankIDStr);

            suffixStr = PaddingLeftWithZero(RandomNumber(0, 9999), 4);
            AlgorithmType = "B";
            do
            {
                BankAccountBaseStr = PaddingLeftWithZero(RandomNumber(0099000, 0999999), 7);
                String tempBankAccountStr = BankIDStr + BankBranchStr + BankAccountBaseStr;
                algorithmWeight = GetAlgorithmWeight(AlgorithmType);
                checkDigit = GetCheckDigit_Mod11(AlgorithmType, tempBankAccountStr, algorithmWeight);
            } while (checkDigit.Equals("N/A"));
            BankAccountBaseStr = BankAccountBaseStr + checkDigit;
            checkSuffixSetting();
            checkAccountBaseLengthSetting();
            BankAccountStr = BankIDStr + BankBranchStr + BankAccountBaseStr + suffixStr;
            printInfo();
            return BankAccountStr;
        }

        public static string getBankAccount(string type)
        {
            switch (type)
            {
                case "A":
                    BankAccountStr = getBankAccountA();
                    break;
                case "B":
                    BankAccountStr = getBankAccountB();
                    break;
                default:
                    BankIDStr = getBankID(type);
                    BankBranchStr = getBankBranch(BankIDStr);
                    doMod();
                    break;
            }
            printInfo();
            return BankAccountStr;
        }
    }
}