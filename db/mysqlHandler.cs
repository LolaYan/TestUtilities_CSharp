using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace TestUtilities.db
{
    public class mysqlHandler
    {
        public void InsertData(string myConnString, string mySelectQuery)
        {

            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();

            using (MySqlConnection conn = new MySqlConnection(conn_string.ToString()))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {    //watch out for this SQL injection vulnerability below
                    cmd.CommandText = string.Format("INSERT Test (lat, long) VALUES ({0},{1})");
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            
        }

        public void ReadData(string myConnString, string mySelectQuery)
        {

            string connStr = "server=192.168.101.104;user=p2p;database=p2p;password=D0ntN33dTh1s;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string sql = "SELECT * FROM opted_game WHERE status = 2";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using (MySqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    /* iterate once per row */
                    Console.WriteLine(rdr.GetString(0));
                }
                rdr.Close();
                conn.Close();
            }

        }

    }
}
