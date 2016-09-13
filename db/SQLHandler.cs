using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.db
{
    public class SQLHandler
    {

        public void ReadMyData()
        {
            SqlConnection objConnection = new SqlConnection("Data Source= SERVER\\SQLEXPRESS;Initial Catalog=SSS;Persist Security Info=True;User ID=user;Password=pass");
            SqlCommand objcommand = new SqlCommand();
            string strSQL;
            objcommand.Connection = objConnection;
            strSQL = "select * from company where companyid = @companyID ";
            try
            {
                objConnection.Open();
                SqlDataReader Query = objcommand.ExecuteReader();
                while (Query.Read())
                {
                    Debug.Print(Convert.ToString(Query["clientRef"]));

                }
                objConnection.Close();
            }
            catch (Exception ex)
            {
                Debug.Print("Error Retreiving info: " + ex.ToString());
                objConnection.Close();
            }
        }
    }
}
