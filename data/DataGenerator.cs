using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.data
{
    public class DataGenerator
    {
        public static string allChar = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
	    public static string letterChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string numberChar = "0123456789";

        public static int GetRanNumber(int min, int max)
        {
            Random random = new Random();
            int res = random.Next(min, max);
            return res;
        }
        public static string generateRandomString(int length, Random random)
        {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(allChar[random.Next(allChar.Length)]);
            }
            return result.ToString();
        }
        // Generate the string with the length required
        public static String generateString(int length)
        {
            StringBuilder sb = new StringBuilder();
            int r = 0;
            for (int i = 0; i < length; i++)
            {
                Random random = new Random();
                r = random.Next(0,allChar.Length);
                Debug.Print("NO: {0} is {1} ",i, r); 
                sb.Append(allChar[r]);
                sb.Append(r);
            }
            return sb.ToString();
        }

        // Generate random gmail email address
        public static String generateEmail()
        {
            Random rnd = new Random();
            int length = 11;
            String ccxl = generateRandomString(length, rnd);
            StringBuilder sb = new StringBuilder("FunctionTest." + ccxl);
            sb.Append("@test.com");
            return sb.ToString();
        }

    }
}
