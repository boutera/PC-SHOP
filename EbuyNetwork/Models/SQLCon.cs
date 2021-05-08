using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EbuyNetwork.Models
{
    public class SQLCon
    {
        
        public static SqlConnection getConnection() {
            SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=ebuynetwork;Integrated Security=True;MultipleActiveResultSets=True ");

            con.Open();
            return con; 
        }     

    }
}