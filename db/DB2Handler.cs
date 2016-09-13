using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.Data.DB2;
using System.Data;
using System.Xml;
using System.Diagnostics;

namespace TestUtilities.db
{
    
    //1. Install ibm_data_server_runtime_client_win64_v11.1
    //2. Install ibm_data_server_driver_package_win64_v11.1
    //3. Install ibm_database_addins_for_visualstudio_v11.1.exe
    //4. Add IBM.Data.DB2 in reference

    //Code reference: https://www.ibm.com/support/knowledgecenter/SSEPGG_9.7.0/com.ibm.swg.im.dbclient.adonet.ref.doc/doc/DB2CommandClass.html

    public class DB2Handler
    {
        public static void readData(string strConn, string strSql)
        {
            using (DB2Connection conn = new DB2Connection(strConn))
            {
                DB2Command cmd = new DB2Command(strSql, conn);
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
                            Console.WriteLine("No. " + i + ":" + dt.Rows[i][0].ToString() + dt.Rows[i][1].ToString() + dt.Rows[i][2].ToString() + dt.Rows[i][3].ToString() + dt.Rows[i][4].ToString() + dt.Rows[i][5].ToString() + dt.Rows[i][6].ToString());
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

        public static int getCount(string strConn, string strSql)
        {
            int count=0;
            using (DB2Connection conn = new DB2Connection(strConn))
            {
                DB2Command cmd = new DB2Command(strSql, conn);
                try
                {
                    conn.Open();

                    DB2DataAdapter adp = new DB2DataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    DataTable dt = ds.Tables[0];

                    count =  dt.Rows.Count;
                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                conn.Close();
            }
            return count;
        }

        public void ReadMyData(string myConnString, string mySelectQuery)
        {

            DB2Connection myConnection = new DB2Connection(myConnString);
            DB2Command myCommand = new DB2Command(mySelectQuery, myConnection);
            myConnection.Open();
            DB2DataReader myReader = myCommand.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    Console.WriteLine(myReader.GetInt32(0) + ", " + myReader.GetString(1));
                }
            }
            finally
            {
                // always call Close when done reading.
                myReader.Close();
                // always call Close when done with connection.
                myConnection.Close();
            }
        }

        public void ReadMyData(string myConnString)
        {
            string mySelectQuery = "SELECT SALES, SALES_PERSON FROM SALES";
            DB2Connection myConnection = new DB2Connection(myConnString);
            DB2Command myCommand = new DB2Command(mySelectQuery, myConnection);
            myConnection.Open();
            DB2DataReader myReader;
            myReader = myCommand.ExecuteReader();
            // Always call Read before accessing data.
            while (myReader.Read())
            {
                Console.WriteLine(myReader.GetInt32(0) + ", " + myReader.GetString(1));
            }
            // always call Close when done reading.
            myReader.Close();
            // Close the connection when done with it.
            myConnection.Close();
        }

        //creates a DB2Command, then executes it by passing a string that is an SQL SELECT statement, and a string to use to connect to the database. CommandBehavior is then set to CloseConnection.
        public void CreateMyDB2DataReader(string mySelectQuery, string myConnectionString)
        {
            DB2Connection myConnection = new DB2Connection(myConnectionString);
            DB2Command myCommand = new DB2Command(mySelectQuery, myConnection);
            myCommand.Connection.Open();
            DB2DataReader myReader =
            myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            while (myReader.Read())
            {
                Console.WriteLine(myReader.GetString(0));
            }
            myReader.Close();
            myConnection.Close();
        }

        //ExecuteNonQuery() - the return value is the number of rows affected by all statements in the command.
        public void CreateMyDB2Command(string myExecuteQuery, string myConnectionString)
        {
            DB2Connection myConnection = new DB2Connection(myConnectionString);
            DB2Command myCommand = new DB2Command(myExecuteQuery, myConnection);
            myCommand.Connection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

        //Use the ExecuteScalar method to retrieve a single value (for example, an aggregate value) from a database. 
        //This requires less code than using the ExecuteReader method, and then performing the operations necessary to generate the single value from the data returned by a   DB2DataReader .
        public void CreateMyDB2Command(string myScalarQuery, DB2Connection myConnection)
        {
            DB2Command myCommand = new DB2Command(myScalarQuery, myConnection);
            myCommand.Connection.Open();
            object qryValue = myCommand.ExecuteScalar();

            int count = Convert.ToInt32(myCommand.ExecuteScalar());
            Debug.Print("Count: {0} ", count);

            myConnection.Close();
        }


        //demonstrates how to read XML data using an XmlReader.
        public static string getProdData(DB2Connection conn)
        {
            String xmlString = "";
            String cmdSQL = "SELECT description FROM product WHERE pid='100-101-01'";
            DB2Command cmd = new DB2Command(cmdSQL, conn);
            XmlReader reader = cmd.ExecuteXmlReader();

            while (reader.Read())
            {
                xmlString = reader.ReadOuterXml();
            }

            return xmlString;
        }


        internal void PerfDataQuery()
        {
            throw new NotImplementedException();
        }
    }
}
