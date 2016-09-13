using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CsvHelper;
using System;

using TestUtilities.db;
using IBM.Data.DB2;
using TestUtilities.WebServiceRequest;
using TestUtilities.Practice;
using TestUtilities.data;

namespace TestUtilities
{
 
    class Program
    {
        static void Main(string[] args)
        {

            /*
            CoreGameDB2Handler CoreGameDB2Handler = new CoreGameDB2Handler();
            CoreGameDB2Handler.getLottoUserNumber();
            Console.WriteLine("");
            */
            
            /*
            DataGenerator DataGenerator = new DataGenerator();
            Debug.Print("Email: {0} ", DataGenerator.generateEmail()); 
             * */

            /*
            LottoPerformTestData LottoPerformTestData = new LottoPerformTestData();
            //LottoPerformTestData.PerfData_LottoTickets_Generator();
            //LottoPerformTestData.PerfData_LottoFavNumber_Generator();
            //LottoPerformTestData.LottoBankAccountNumber_Validator_Simple();
            //LottoPerformTestData.Invalid_LottoBankAccountFile_Writer();
            LottoPerformTestData.Invalid_LottoBankAccountFile_Writer_Simple();
            */

            /*
            SauceLabAutomationIOSDebug SauceLabAutomationIOSDebug = new SauceLabAutomationIOSDebug();
            SauceLabAutomationIOSDebug.verifyIOSAppTest();
            */

            /*
            SauceLabAutomationAndroidDebug SauceLabAutomationAndroidDebug = new SauceLabAutomationAndroidDebug();
            SauceLabAutomationAndroidDebug.verifyAndroidAppTest();
            */
            /*
            DB2Handler DB2Handler = new DB2Handler();
            DB2Handler.PerfDataQuery();
            */

            /*
            mysqlHandler mysqlHandler = new mysqlHandler();
            mysqlHandler.ReadData("", "");
            */
            /*
            SoapServiceHandler SoapSericeHandler = new SoapServiceHandler();
            SoapServiceHandler.Login();
            */

            RestServiceHandler RestServiceHandler = new RestServiceHandler();
            RestServiceHandler.DPSCall();
        }
     }

}

