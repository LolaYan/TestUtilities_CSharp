using IBM.Data.DB2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtilities.db;
using TestUtilities.Excel;
using TestUtilities.NZBankAccountUtils;
namespace TestUtilities.Practice
{

    public class LottoPerformTestData
    {
        public static CoreGameDB2Handler CoreGameDB2Handler;
        public static string strConn = "Server=192.168.101.87:50000;Database=ESI_DB;UID=gtkinst1;PWD=gtkinst1;";
        public static string strLottoUserSql = @"SELECT BANK_ACCT_NUMBER, BANK_CODE, BANK_ACCOUNT,MODIFIED_BY, CREATION_DT, USER_TYPE, USER_ID, LOGIN_STATUS, WAGER_STATUS , WALLET_STATUS, BANK_ACCT_TYPE
                                        FROM NZDEV.ES_SECURITY c
                                        inner join 
                                        (
                                                SELECT * FROM NZDEV.ES_BANK_ACCOUNT WHERE CREATION_DT IN 
                                                (
                                                SELECT MAX(CREATION_DT)
                                                FROM NZDEV.ES_BANK_ACCOUNT 
                                                GROUP BY MODIFIED_BY 
                                                HAVING COUNT(MODIFIED_BY) > 1 OR COUNT(MODIFIED_BY) = 1
                                                )
                                        ) m
                                        on c.USER_NAME = m.MODIFIED_BY
                                        WHere USER_TYPE =5";

        public static void PerfData_LottoTickets_Generator()
        {
            string email;
            int lottoTicketNumber = 0;
            int lottoOpenTicketNumber = 0;
            int lottoCompleteTicketNumber = 0;

            CoreGameDB2Handler = new CoreGameDB2Handler();
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader("cat1_20000_users.csv"))
            {
                using (CsvFileWriter writer = new CsvFileWriter("cat1_20000_users_lottoTickets.csv"))
                {
                    CsvRow row = new CsvRow();
                    //Loop the data of cat1_20000_users.csv
                    while (reader.ReadRow(row))
                    {
                        email = row.ToArray()[0];
                        lottoTicketNumber = CoreGameDB2Handler.getLottoTicketNumber(email);
                        lottoOpenTicketNumber = CoreGameDB2Handler.getLottoOpenTicketNumber(email);
                        lottoCompleteTicketNumber = lottoTicketNumber - lottoOpenTicketNumber;
                        Console.WriteLine(email);
                        Console.WriteLine(lottoTicketNumber);
                        Console.WriteLine(lottoOpenTicketNumber);
                        Console.WriteLine(lottoCompleteTicketNumber);
                        if (lottoTicketNumber > 0)
                        {
                            CsvRow writeRow = new CsvRow();
                            writeRow.Add(email);
                            writeRow.Add(lottoTicketNumber.ToString());
                            writeRow.Add(lottoOpenTicketNumber.ToString());
                            writeRow.Add(lottoCompleteTicketNumber.ToString());
                            writer.WriteRow(writeRow);
                        }
                    }
                }
            }

        }

        public static void PerfData_LottoFavNumber_Generator()
        {
            string email;
            int lottoFavNumber = 0;

            CoreGameDB2Handler = new CoreGameDB2Handler();
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader("cat1_20000_users.csv"))
            {
                using (CsvFileWriter writer = new CsvFileWriter("cat1_20000_users_lottoFav.csv"))
                {
                    CsvRow row = new CsvRow();
                    //Loop the data of cat1_20000_users.csv
                    while (reader.ReadRow(row))
                    {
                        email = row.ToArray()[0];
                        lottoFavNumber = CoreGameDB2Handler.getLottoFavNumber(email);
                        Console.WriteLine(email);
                        Console.WriteLine(lottoFavNumber);
                        if (lottoFavNumber > 0)
                        {
                            CsvRow writeRow = new CsvRow();
                            writeRow.Add(email);
                            writeRow.Add(lottoFavNumber.ToString());
                            writer.WriteRow(writeRow);
                        }
                    }
                }
            }

        }

        public static void LottoBankAccountNumber_Validator()
        {
            string BANK_ACCT_NUMBER;
            string BANK_CODE;
            string BANK_ACCOUNT;
            string MODIFIED_BY;
            string CREATION_DT;
            string USER_TYPE;
            string USER_ID;
            string LOGIN_STATUS;
            string WAGER_STATUS;
            string WALLET_STATUS;
            string BANK_ACCT_TYPE;

            string BankID;
            string BankBranch;
            string BankAccount;
            string BankSuffix;

            string result;
            string errorMsg;

            CoreGameDB2Handler = new CoreGameDB2Handler();
            // Read sample data from CSV file
            using (CsvFileWriter writer = new CsvFileWriter("CAT1_UserBankAccountValidation_result.csv"))
            {
                using (DB2Connection conn = new DB2Connection(strConn))
                {
                    DB2Command cmd = new DB2Command(strLottoUserSql, conn);
                    try
                    {
                        conn.Open();

                        DB2DataAdapter adp = new DB2DataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        DataTable dt = ds.Tables[0];

                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                BANK_ACCT_NUMBER = dt.Rows[i][0].ToString();
                                BANK_CODE = dt.Rows[i][1].ToString();
                                BANK_ACCOUNT = dt.Rows[i][2].ToString();
                                MODIFIED_BY = dt.Rows[i][3].ToString();
                                CREATION_DT = dt.Rows[i][4].ToString();
                                USER_TYPE = dt.Rows[i][5].ToString();
                                USER_ID = dt.Rows[i][6].ToString();
                                LOGIN_STATUS = dt.Rows[i][7].ToString();
                                WAGER_STATUS = dt.Rows[i][8].ToString();
                                WALLET_STATUS = dt.Rows[i][9].ToString();
                                BANK_ACCT_TYPE = dt.Rows[i][10].ToString();

                                Console.WriteLine("No. " + i + " :" + BANK_ACCT_NUMBER + " - " + BANK_CODE + ", " + BANK_ACCOUNT + ", " + MODIFIED_BY);

                                if (BANK_ACCT_NUMBER == "" || BANK_CODE == "" || BANK_ACCOUNT == "" || IsDigitsOnly(BANK_ACCT_NUMBER) == false || BANK_CODE.Length != 6 || BANK_ACCT_NUMBER.Length <= 13)
                                {
                                    result = "false";
                                    errorMsg = "Invalid Bank Value or length";
                                    CsvRow writeRow = new CsvRow();
                                    writeRow.Add(BANK_ACCT_NUMBER);
                                    writeRow.Add(BANK_CODE);
                                    writeRow.Add(BANK_ACCOUNT);
                                    writeRow.Add(MODIFIED_BY);
                                    writeRow.Add(USER_ID);
                                    writeRow.Add(CREATION_DT);
                                    writeRow.Add(USER_TYPE);
                                    writeRow.Add(LOGIN_STATUS);
                                    writeRow.Add(WAGER_STATUS);
                                    writeRow.Add(WALLET_STATUS);
                                    writeRow.Add(BANK_ACCT_TYPE);
                                    writeRow.Add(result);
                                    writeRow.Add(errorMsg);
                                    writer.WriteRow(writeRow);
                                }
                                else
                                {
                                    //Validate Bank Account
                                    BankID = BANK_CODE.Substring(0, 2);
                                    BankBranch = BANK_CODE.Substring(2, 4);
                                    //BankAccount = BANK_ACCT_NUMBER.Substring(6, 7);
                                    BankAccount = BANK_ACCOUNT;
                                    int tlength = BANK_CODE.Length + BANK_ACCOUNT.Length;
                                    BankSuffix = BANK_ACCT_NUMBER.Substring(tlength, BANK_ACCT_NUMBER.Length - tlength);

                                    bool resBankID = BankAccountValidator.validateBankID(BankID);
                                    bool resBankBranch = BankAccountValidator.validateBankBranch(BankID, BankBranch);
                                    bool resBankAccount = BankAccountValidator.validateBankAccountBase(BankID, BankBranch, BankAccount, BankSuffix);
                                    string alg = BankAccountValidator.AlgorithmType;
                                    string actualCheckDigit = BankAccountValidator.ActualCheckDigit;
                                    string expectCheckDigit = BankAccountValidator.ExpectCheckDigit;
                                    if (resBankID == false)
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank ID";
                                    }
                                    else if (resBankBranch == false)
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank Branch";
                                    }
                                    else if (resBankID == true && resBankBranch == true && resBankAccount == true)
                                    {

                                        result = "true";
                                        errorMsg = "Valid Bank Account";
                                        string msg = "Valid Bank Account! \r\n " +
                                            "AlgorithmType: " + alg + " \r\n " +
                                            "actualCheckDigit: " + actualCheckDigit + " \r\n " +
                                            "expectCheckDigit: " + expectCheckDigit + " \r\n ";
                                    }
                                    else
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank Account";
                                        string msg = "Invalid Bank Account! \r\n " +
                                            "AlgorithmType: " + alg + " \r\n " +
                                            "actualCheckDigit: " + actualCheckDigit + " \r\n " +
                                            "expectCheckDigit: " + expectCheckDigit + " \r\n ";

                                    }

                                    CsvRow writeRow = new CsvRow();
                                    writeRow.Add(BANK_ACCT_NUMBER);
                                    writeRow.Add(BANK_CODE);
                                    writeRow.Add(BANK_ACCOUNT);
                                    writeRow.Add(MODIFIED_BY);
                                    writeRow.Add(USER_ID);
                                    writeRow.Add(CREATION_DT);
                                    writeRow.Add(USER_TYPE);
                                    writeRow.Add(LOGIN_STATUS);
                                    writeRow.Add(WAGER_STATUS);
                                    writeRow.Add(WALLET_STATUS);
                                    writeRow.Add(BANK_ACCT_TYPE);
                                    writeRow.Add(result);
                                    writeRow.Add(errorMsg);
                                    writeRow.Add(BankID);
                                    writeRow.Add(BankBranch);
                                    writeRow.Add(BankAccount);
                                    writeRow.Add(BankSuffix);
                                    writeRow.Add(alg);
                                    writeRow.Add(actualCheckDigit);
                                    writeRow.Add(expectCheckDigit);
                                    writer.WriteRow(writeRow);
                                }
                                
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    conn.Close();
                }
                //Console.Read();
                    
                            
                       
                    
             }
        }

        public static void LottoBankAccountNumber_Validator_Simple()
        {
            string BANK_ACCT_NUMBER;
            string BANK_CODE;
            string BANK_ACCOUNT;
            string MODIFIED_BY;
            string CREATION_DT;
            string USER_TYPE;
            string USER_ID;
            string LOGIN_STATUS;
            string WAGER_STATUS;
            string WALLET_STATUS;
            string BANK_ACCT_TYPE;

            string BankID;
            string BankBranch;
            string BankAccount;
            string BankSuffix;

            string result;
            string errorMsg;

            CoreGameDB2Handler = new CoreGameDB2Handler();
            // Read sample data from CSV file
            using (CsvFileWriter writer = new CsvFileWriter("CAT1_UserBankAccountValidation_Simple_Result.csv"))
            {
                using (DB2Connection conn = new DB2Connection(strConn))
                {
                    DB2Command cmd = new DB2Command(strLottoUserSql, conn);
                    try
                    {
                        conn.Open();

                        DB2DataAdapter adp = new DB2DataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        DataTable dt = ds.Tables[0];

                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                BANK_ACCT_NUMBER = dt.Rows[i][0].ToString();
                                BANK_CODE = dt.Rows[i][1].ToString();
                                BANK_ACCOUNT = dt.Rows[i][2].ToString();
                                MODIFIED_BY = dt.Rows[i][3].ToString();
                                CREATION_DT = dt.Rows[i][4].ToString();
                                USER_TYPE = dt.Rows[i][5].ToString();
                                USER_ID = dt.Rows[i][6].ToString();
                                LOGIN_STATUS = dt.Rows[i][7].ToString();
                                WAGER_STATUS = dt.Rows[i][8].ToString();
                                WALLET_STATUS = dt.Rows[i][9].ToString();
                                BANK_ACCT_TYPE = dt.Rows[i][10].ToString();

                                Console.WriteLine("No. " + i + " :" + BANK_ACCT_NUMBER + " - " + BANK_CODE + ", " + BANK_ACCOUNT + ", " + MODIFIED_BY);

                                if (BANK_ACCT_NUMBER == "" || BANK_CODE == "" || BANK_ACCOUNT == "" || IsDigitsOnly(BANK_ACCT_NUMBER) == false || BANK_CODE.Length != 6 || BANK_ACCT_NUMBER.Length <= 13)
                                {
                                    result = "false";
                                    errorMsg = "Invalid Bank Value or length";
                                }
                                else
                                {
                                    //Validate Bank Account
                                    BankID = BANK_CODE.Substring(0, 2);
                                    BankBranch = BANK_CODE.Substring(2, 4);
                                    //BankAccount = BANK_ACCT_NUMBER.Substring(6, 7);
                                    BankAccount = BANK_ACCOUNT;
                                    int tlength = BANK_CODE.Length + BANK_ACCOUNT.Length;
                                    BankSuffix = BANK_ACCT_NUMBER.Substring(tlength, BANK_ACCT_NUMBER.Length - tlength);

                                    bool resBankID = BankAccountValidator.validateBankID(BankID);
                                    bool resBankBranch = BankAccountValidator.validateBankBranch(BankID, BankBranch);
                                    bool resBankAccount = BankAccountValidator.validateBankAccountBase(BankID, BankBranch, BankAccount, BankSuffix);
                                    string alg = BankAccountValidator.AlgorithmType;
                                    string actualCheckDigit = BankAccountValidator.ActualCheckDigit;
                                    string expectCheckDigit = BankAccountValidator.ExpectCheckDigit;
                                    if (resBankID == false)
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank ID";
                                    }
                                    else if (resBankBranch == false)
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank Branch";
                                    }
                                    else if (resBankID == true && resBankBranch == true && resBankAccount == true)
                                    {

                                        result = "true";
                                        errorMsg = "Valid Bank Account";
                                        string msg = "Valid Bank Account! \r\n " +
                                            "AlgorithmType: " + alg + " \r\n " +
                                            "actualCheckDigit: " + actualCheckDigit + " \r\n " +
                                            "expectCheckDigit: " + expectCheckDigit + " \r\n ";
                                    }
                                    else
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank Account";
                                        string msg = "Invalid Bank Account! \r\n " +
                                            "AlgorithmType: " + alg + " \r\n " +
                                            "actualCheckDigit: " + actualCheckDigit + " \r\n " +
                                            "expectCheckDigit: " + expectCheckDigit + " \r\n ";

                                    }

                                    
                                }

                                CsvRow writeRow = new CsvRow();
                                writeRow.Add(BANK_ACCT_NUMBER);
                                writeRow.Add(BANK_CODE);
                                writeRow.Add(BANK_ACCOUNT);
                                writeRow.Add(MODIFIED_BY);
                                writeRow.Add(USER_ID);
                                writeRow.Add(USER_TYPE);
                                writeRow.Add(result);
                                writeRow.Add(errorMsg);
                                writer.WriteRow(writeRow);

                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    conn.Close();
                }
                //Console.Read();




            }
        }

        public static void Invalid_LottoBankAccountFile_Writer()
        {
            string BANK_ACCT_NUMBER;
            string BANK_CODE;
            string BANK_ACCOUNT;
            string MODIFIED_BY;
            string CREATION_DT;
            string USER_TYPE;
            string USER_ID;
            string LOGIN_STATUS;
            string WAGER_STATUS;
            string WALLET_STATUS;
            string BANK_ACCT_TYPE;

            string BankID;
            string BankBranch;
            string BankAccount;
            string BankSuffix;

            string result;
            string errorMsg;

            CoreGameDB2Handler = new CoreGameDB2Handler();
            // Read sample data from CSV file
            using (CsvFileWriter writer = new CsvFileWriter("CAT1_InvalidUserBankAccount_Result.csv"))
            {
                using (DB2Connection conn = new DB2Connection(strConn))
                {
                    DB2Command cmd = new DB2Command(strLottoUserSql, conn);
                    try
                    {
                        conn.Open();

                        DB2DataAdapter adp = new DB2DataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        DataTable dt = ds.Tables[0];

                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                BANK_ACCT_NUMBER = dt.Rows[i][0].ToString();
                                BANK_CODE = dt.Rows[i][1].ToString();
                                BANK_ACCOUNT = dt.Rows[i][2].ToString();
                                MODIFIED_BY = dt.Rows[i][3].ToString();
                                CREATION_DT = dt.Rows[i][4].ToString();
                                USER_TYPE = dt.Rows[i][5].ToString();
                                USER_ID = dt.Rows[i][6].ToString();
                                LOGIN_STATUS = dt.Rows[i][7].ToString();
                                WAGER_STATUS = dt.Rows[i][8].ToString();
                                WALLET_STATUS = dt.Rows[i][9].ToString();
                                BANK_ACCT_TYPE = dt.Rows[i][10].ToString();

                                Console.WriteLine("No. " + i + " :" + BANK_ACCT_NUMBER + " - " + BANK_CODE + ", " + BANK_ACCOUNT + ", " + MODIFIED_BY);

                                if (BANK_ACCT_NUMBER == "" || BANK_CODE == "" || BANK_ACCOUNT == "" || IsDigitsOnly(BANK_ACCT_NUMBER) == false || BANK_CODE.Length != 6 || BANK_ACCT_NUMBER.Length <= 13)
                                {
                                    result = "false";
                                    errorMsg = "Invalid Bank Value or length";
                                    CsvRow writeRow = new CsvRow();
                                    writeRow.Add(BANK_ACCT_NUMBER);
                                    writeRow.Add(BANK_CODE);
                                    writeRow.Add(BANK_ACCOUNT);
                                    writeRow.Add(MODIFIED_BY);
                                    writeRow.Add(USER_ID);
                                    writeRow.Add(CREATION_DT);
                                    writeRow.Add(USER_TYPE);
                                    writeRow.Add(LOGIN_STATUS);
                                    writeRow.Add(WAGER_STATUS);
                                    writeRow.Add(WALLET_STATUS);
                                    writeRow.Add(BANK_ACCT_TYPE);
                                    writeRow.Add(result);
                                    writeRow.Add(errorMsg);
                                    writer.WriteRow(writeRow);
                                }
                                else
                                {
                                    //Validate Bank Account
                                    BankID = BANK_CODE.Substring(0, 2);
                                    BankBranch = BANK_CODE.Substring(2, 4);
                                    //BankAccount = BANK_ACCT_NUMBER.Substring(6, 7);
                                    BankAccount = BANK_ACCOUNT;
                                    int tlength = BANK_CODE.Length + BANK_ACCOUNT.Length;
                                    BankSuffix = BANK_ACCT_NUMBER.Substring(tlength, BANK_ACCT_NUMBER.Length - tlength);

                                    bool resBankID = BankAccountValidator.validateBankID(BankID);
                                    bool resBankBranch = BankAccountValidator.validateBankBranch(BankID, BankBranch);
                                    bool resBankAccount = BankAccountValidator.validateBankAccountBase(BankID, BankBranch, BankAccount, BankSuffix);
                                    string alg = BankAccountValidator.AlgorithmType;
                                    string actualCheckDigit = BankAccountValidator.ActualCheckDigit;
                                    string expectCheckDigit = BankAccountValidator.ExpectCheckDigit;
                                    if (resBankID == false)
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank ID";
                                    }
                                    else if (resBankBranch == false)
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank Branch";
                                    }
                                    else if (resBankID == true && resBankBranch == true && resBankAccount == true)
                                    {

                                        result = "true";
                                        errorMsg = "Valid Bank Account";
                                        string msg = "Valid Bank Account! \r\n " +
                                            "AlgorithmType: " + alg + " \r\n " +
                                            "actualCheckDigit: " + actualCheckDigit + " \r\n " +
                                            "expectCheckDigit: " + expectCheckDigit + " \r\n ";
                                    }
                                    else
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank Account";
                                        string msg = "Invalid Bank Account! \r\n " +
                                            "AlgorithmType: " + alg + " \r\n " +
                                            "actualCheckDigit: " + actualCheckDigit + " \r\n " +
                                            "expectCheckDigit: " + expectCheckDigit + " \r\n ";

                                    }
                                    if(result == "false")
                                    {
                                        CsvRow writeRow = new CsvRow();
                                        writeRow.Add(BANK_ACCT_NUMBER);
                                        writeRow.Add(BANK_CODE);
                                        writeRow.Add(BANK_ACCOUNT);
                                        writeRow.Add(MODIFIED_BY);
                                        writeRow.Add(USER_ID);
                                        writeRow.Add(CREATION_DT);
                                        writeRow.Add(USER_TYPE);
                                        writeRow.Add(LOGIN_STATUS);
                                        writeRow.Add(WAGER_STATUS);
                                        writeRow.Add(WALLET_STATUS);
                                        writeRow.Add(BANK_ACCT_TYPE);
                                        writeRow.Add(result);
                                        writeRow.Add(errorMsg);
                                        writeRow.Add(BankID);
                                        writeRow.Add(BankBranch);
                                        writeRow.Add(BankAccount);
                                        writeRow.Add(BankSuffix);
                                        writeRow.Add(alg);
                                        writeRow.Add(actualCheckDigit);
                                        writeRow.Add(expectCheckDigit);
                                        writer.WriteRow(writeRow);
                                    }

                                    
                                }

                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    conn.Close();
                }
                //Console.Read();




            }
        }

        public static void Invalid_LottoBankAccountFile_Writer_Simple()
        {
            string BANK_ACCT_NUMBER;
            string BANK_CODE;
            string BANK_ACCOUNT;
            string MODIFIED_BY;
            string CREATION_DT;
            string USER_TYPE;
            string USER_ID;
            string LOGIN_STATUS;
            string WAGER_STATUS;
            string WALLET_STATUS;
            string BANK_ACCT_TYPE;

            string BankID;
            string BankBranch;
            string BankAccount;
            string BankSuffix;

            string result;
            string errorMsg;

            CoreGameDB2Handler = new CoreGameDB2Handler();
            // Read sample data from CSV file
            using (CsvFileWriter writer = new CsvFileWriter("CAT1_InvalidUserBankAccount_Result_Simple.csv"))
            {
                using (DB2Connection conn = new DB2Connection(strConn))
                {
                    DB2Command cmd = new DB2Command(strLottoUserSql, conn);
                    try
                    {
                        conn.Open();

                        DB2DataAdapter adp = new DB2DataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        DataTable dt = ds.Tables[0];

                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                BANK_ACCT_NUMBER = dt.Rows[i][0].ToString();
                                BANK_CODE = dt.Rows[i][1].ToString();
                                BANK_ACCOUNT = dt.Rows[i][2].ToString();
                                MODIFIED_BY = dt.Rows[i][3].ToString();
                                CREATION_DT = dt.Rows[i][4].ToString();
                                USER_TYPE = dt.Rows[i][5].ToString();
                                USER_ID = dt.Rows[i][6].ToString();
                                LOGIN_STATUS = dt.Rows[i][7].ToString();
                                WAGER_STATUS = dt.Rows[i][8].ToString();
                                WALLET_STATUS = dt.Rows[i][9].ToString();
                                BANK_ACCT_TYPE = dt.Rows[i][10].ToString();

                                Console.WriteLine("No. " + i + " :" + BANK_ACCT_NUMBER + " - " + BANK_CODE + ", " + BANK_ACCOUNT + ", " + MODIFIED_BY);

                                if (BANK_ACCT_NUMBER == "" || BANK_CODE == "" || BANK_ACCOUNT == "" || IsDigitsOnly(BANK_ACCT_NUMBER) == false || BANK_CODE.Length != 6 || BANK_ACCT_NUMBER.Length <= 13)
                                {
                                    result = "false";
                                    errorMsg = "Invalid Bank Value or length";
                                }
                                else
                                {
                                    //Validate Bank Account
                                    BankID = BANK_CODE.Substring(0, 2);
                                    BankBranch = BANK_CODE.Substring(2, 4);
                                    //BankAccount = BANK_ACCT_NUMBER.Substring(6, 7);
                                    BankAccount = BANK_ACCOUNT;
                                    int tlength = BANK_CODE.Length + BANK_ACCOUNT.Length;
                                    BankSuffix = BANK_ACCT_NUMBER.Substring(tlength, BANK_ACCT_NUMBER.Length - tlength);

                                    bool resBankID = BankAccountValidator.validateBankID(BankID);
                                    bool resBankBranch = BankAccountValidator.validateBankBranch(BankID, BankBranch);
                                    bool resBankAccount = BankAccountValidator.validateBankAccountBase(BankID, BankBranch, BankAccount, BankSuffix);
                                    string alg = BankAccountValidator.AlgorithmType;
                                    string actualCheckDigit = BankAccountValidator.ActualCheckDigit;
                                    string expectCheckDigit = BankAccountValidator.ExpectCheckDigit;
                                    if (resBankID == false)
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank ID";
                                    }
                                    else if (resBankBranch == false)
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank Branch";
                                    }
                                    else if (resBankID == true && resBankBranch == true && resBankAccount == true)
                                    {

                                        result = "true";
                                        errorMsg = "Valid Bank Account";
                                        string msg = "Valid Bank Account! \r\n " +
                                            "AlgorithmType: " + alg + " \r\n " +
                                            "actualCheckDigit: " + actualCheckDigit + " \r\n " +
                                            "expectCheckDigit: " + expectCheckDigit + " \r\n ";
                                    }
                                    else
                                    {
                                        result = "false";
                                        errorMsg = "Invalid Bank Account";
                                        string msg = "Invalid Bank Account! \r\n " +
                                            "AlgorithmType: " + alg + " \r\n " +
                                            "actualCheckDigit: " + actualCheckDigit + " \r\n " +
                                            "expectCheckDigit: " + expectCheckDigit + " \r\n ";

                                    }


                                }
                                if (result == "false")
                                {

                                    CsvRow writeRow = new CsvRow();
                                    writeRow.Add(BANK_ACCT_NUMBER);
                                    writeRow.Add(BANK_CODE);
                                    writeRow.Add(BANK_ACCOUNT);
                                    writeRow.Add(MODIFIED_BY);
                                    writeRow.Add(USER_ID);
                                    writeRow.Add(USER_TYPE);
                                    writeRow.Add(result);
                                    writeRow.Add(errorMsg);
                                    writer.WriteRow(writeRow);
                                }

                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    conn.Close();
                }
                //Console.Read();




            }
        }

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

    }
}
