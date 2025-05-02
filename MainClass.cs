// Imports core .NET types like String, Boolean, etc
using System;
// Imports support for generic collections like List<T>, Dictionary<K,V>, etc
using System.Collections.Generic;
// Imports classes to interact with Microsoft SQL Server, e.g., SqlConnection, SqlCommand
using System.Data.SqlClient;
// Imports LINQ (Language Integrated Query) functionality
using System.Linq;
// Provides access to network information
using System.Net.NetworkInformation;
// Provides encoding and string manipulation classes
using System.Text;
// Enables asynchronous programming with tasks
using System.Threading.Tasks;
// Provides basic classes for data access, like DataTable, DataSet, etc
using System.Data;

// Declares a namespace called ServeSync to logically group related classes
namespace ServeSync
{
    // Defines an internal class MainClass (accessible only within the same assembly)
    internal class MainClass
    {
        // Defines a static, read-only connection string used to connect to the SQL Server instance SQLEXPRESS on the machine KASUNMENDIS, using the sa (System Admin) login
        public static readonly string con_string = "Data Source=KASUNMENDIS\\SQLEXPRESS; Initial Catalog=ServeSync; Persist Security Info=True; User ID=sa; Password=123;";
        // Creates a SqlConnection object with the above connection string
        public static readonly SqlConnection con = new SqlConnection(con_string);

        // Method to check user validation that returns true if user credentials are found in the database
        public static bool IsValidUser(string user, string pass)
        {
            // Initializes a boolean flag to false
            bool isValid = false;
            // Checks if a user exists with the given username and password
            string qry = @"SELECT * FROM users where username = '" + user + "' AND user_password = '" + pass + "'";
            // Creates a SQL command with the query and connection
            SqlCommand cmd = new SqlCommand(qry, con);
            // Creates an empty DataTable to hold the query results
            DataTable dt = new DataTable();
            // A SqlDataAdapter is created to execute the command and fill the data table
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // Fills the DataTable with the results of the SQL query
            da.Fill(dt);
            // 
            if (dt.Rows.Count > 0)
            {
                isValid = true;
                USER = dt.Rows[0]["user_name"].ToString();
            }
            
            return isValid;
        }

        // Create property for username
        public static string user;

        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }

    }
}
