using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.Data.DB2;
using System.Data;
using System.Xml;
using System.Diagnostics;
using TestUtilities.db;

namespace TestUtilities.Practice
{
    public class CoreGameDB2Handler : DB2Handler
    {
        public static string ad2ConnString = "Server=192.168.101.74:50000;Database=ESI_DB;UID=gtkinst1;PWD=gtkinst1;";

        public static string strLottoTicketSql;
        public static string strOpenLottoTicketSql;
        public static string strConn = "Server=192.168.101.87:50000;Database=ESI_DB;UID=gtkinst1;PWD=gtkinst1;";

        public int getLottoTicketNumber(string email)
        {
            strLottoTicketSql = @"SELECT 
                                NZDEV.ESI_TICKET_INFO.USER_ID, 
                                NZDEV.ESI_TICKET_INFO.GAME_ID, 
                                NZDEV.ESI_TICKET_INFO.TICKET_SERIAL_NO, 
                                NZDEV.ES_SECURITY.USER_NAME, 
                                NZDEV.ESI_TICKET_DETAILS.DRAW_DATE,
                                NZDEV.ESI_TICKET_DETAILS.ORDER_ID, 
                                NZDEV.ES_LINE_ITEMS.ITEM_FULFILL_ST_ID
	                            FROM NZDEV.ES_SECURITY
	                            LEFT JOIN NZDEV.ESI_TICKET_INFO
	                            ON NZDEV.ESI_TICKET_INFO.USER_ID = NZDEV.ES_SECURITY.USER_ID
	                            LEFT JOIN NZDEV.ESI_TICKET_DETAILS
	                            ON NZDEV.ESI_TICKET_INFO.TICKET_SERIAL_NO = NZDEV.ESI_TICKET_DETAILS.TICKET_SERIAL_NO
	                            LEFT JOIN NZDEV.ES_LINE_ITEMS
	                            ON NZDEV.ES_LINE_ITEMS.ORDER_ID = NZDEV.ESI_TICKET_DETAILS.ORDER_ID
	                            WHERE NZDEV.ES_SECURITY.USER_NAME = '"+email+"'  AND NZDEV.ESI_TICKET_INFO.GAME_ID IN  ( '20','12','2')";
            int lottoTicketNumber = getCount(strConn, strLottoTicketSql);
            return lottoTicketNumber;
        }

        public int getLottoOpenTicketNumber(string email)
        {
            strLottoTicketSql = @"SELECT 
                                NZDEV.ESI_TICKET_INFO.USER_ID, 
                                NZDEV.ESI_TICKET_INFO.GAME_ID, 
                                NZDEV.ESI_TICKET_INFO.TICKET_SERIAL_NO, 
                                NZDEV.ES_SECURITY.USER_NAME, 
                                NZDEV.ESI_TICKET_DETAILS.DRAW_DATE,
                                NZDEV.ESI_TICKET_DETAILS.ORDER_ID, 
                                NZDEV.ES_LINE_ITEMS.ITEM_FULFILL_ST_ID
	                            FROM NZDEV.ES_SECURITY
	                            LEFT JOIN NZDEV.ESI_TICKET_INFO
	                            ON NZDEV.ESI_TICKET_INFO.USER_ID = NZDEV.ES_SECURITY.USER_ID
	                            LEFT JOIN NZDEV.ESI_TICKET_DETAILS
	                            ON NZDEV.ESI_TICKET_INFO.TICKET_SERIAL_NO = NZDEV.ESI_TICKET_DETAILS.TICKET_SERIAL_NO
	                            LEFT JOIN NZDEV.ES_LINE_ITEMS
	                            ON NZDEV.ES_LINE_ITEMS.ORDER_ID = NZDEV.ESI_TICKET_DETAILS.ORDER_ID
	                            WHERE NZDEV.ES_SECURITY.USER_NAME = '" + email + "'  AND NZDEV.ESI_TICKET_INFO.GAME_ID IN  ( '20','12','2')";
            strOpenLottoTicketSql = strLottoTicketSql + "AND NZDEV.ES_LINE_ITEMS.ITEM_FULFILL_ST_ID = 'PURCH'";
            int lottoOpenTicketNumber = getCount(strConn, strOpenLottoTicketSql);
            return lottoOpenTicketNumber;
        }
        public int getLottoFavNumber(string email)
        {
            string strLottoFavNoSql = @"SELECT COUNT(*)
	                                FROM NZDEV.ES_SECURITY
	                                LEFT JOIN NZDEV.ESI_NZ_FAVOURITES
	                                ON NZDEV.ESI_NZ_FAVOURITES.USER_ID = NZDEV.ES_SECURITY.USER_ID
	                                WHERE NZDEV.ES_SECURITY.USER_NAME = '" + email + "' and NZDEV.ESI_NZ_FAVOURITES.GAME_NAME LIKE 'Lotto%'";
            int lottoFavNumber = getCount(strConn, strLottoFavNoSql);
            return lottoFavNumber;
        }

        public void getLottoUserNumber()
        {
            string strLottoUserSql = @"SELECT BANK_ACCT_NUMBER,BANK_ACCOUNT,MODIFIED_BY, CREATION_DT, USER_TYPE
                                        FROM NZDEV.ES_BANK_ACCOUNT
                                        inner join NZDEV.ES_SECURITY
                                        on NZDEV.ES_BANK_ACCOUNT.MODIFIED_BY = NZDEV.ES_SECURITY.USER_NAME";
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
                            Console.WriteLine("No. " + i + ":" + dt.Rows[i][0].ToString() + dt.Rows[i][1].ToString() + dt.Rows[i][2].ToString() );
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                conn.Close();
            }
            Console.Read();
        }
    }
}
