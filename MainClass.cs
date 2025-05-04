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
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

// Declares a namespace called ServeSync to logically group related classes
namespace ServeSync
{
    // Defines an internal class MainClass (accessible only within the same assembly)
    class MainClass
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

        // Method for CRUD operation
        /* 
         public static → callable without instantiating MainClass.
         int → returns the number of rows affected.
         Parameters:
            string qry → the SQL statement with parameter placeholders (e.g. @id, @Name).
            Hashtable ht → a map of parameter names to values.
         */
        public static int SQL(string qry, Hashtable ht) 
        {
            // Declare a local integer res, initialized to 0. This will hold the result of ExecuteNonQuery().
            int res = 0;
            try
            {
                // Instantiate a SqlCommand object using the provided SQL text (qry) and the shared SqlConnection (con).
                SqlCommand cmd = new SqlCommand(qry, con);
                // Specify that the command is a raw text query (as opposed to a stored procedure).
                cmd.CommandType = CommandType.Text;
                //Loop through each entry in the Hashtable:
                // item.Key is the parameter name(e.g. "@Name").
                // item.Value is its value (e.g. "Electronics").
                foreach (DictionaryEntry item in ht)
                {
                    // Call AddWithValue to bind each parameter to the command, preventing SQL injection.
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                // Check if the shared connection con is closed; if so, open it.
                if (con.State == ConnectionState.Closed){ con.Open(); }
                // Execute the command. For INSERT/UPDATE/DELETE, this returns the number of rows affected—store it in res.
                res = cmd.ExecuteNonQuery();
                // If the connection is still open after execution, close it to free resources.
                if (con.State == ConnectionState.Open) { con.Close(); }
            } catch (Exception ex)
            {
                // If an exception occurs, show a message box with the error details.
                MessageBox.Show(ex.ToString());
                // Ensure the connection is closed to avoid resource leaks.
                con.Close();
            }
            // Return the number of rows affected (or 0 if an error occurred before assignment).
            return res;
        }
        // For loading data from database
        /* 
            void → no return value.
            Parameters:
                string qry → a SELECT statement.
                DataGridView gv → the grid control to populate.
                ListBox lb → a list whose items are the column objects of the grid; used to map columns to data fields.
        */
        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {
            // Serial number in gridView
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
            try
            {
                // Create a SqlCommand for the SELECT query and mark it as text.
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                // SqlDataAdapter executes the command behind the scenes and fills a DataTable (dt) with the result rows.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // Create a DataTable to hold the data.
                DataTable dt = new DataTable();
                // Fill the DataTable with the results of the SQL query.
                da.Fill(dt);
                /*
                   Loop through each item in the ListBox (lb):
                    1)Cast the item back to a DataGridViewColumn.
                    2)Read its Name (the grid’s column name).
                    3)Set that column’s DataPropertyName to the corresponding data‑table column name—dt.Columns[i].ToString()—so the grid knows which field to show in each column. 
                 */
                for (int i=0; i < lb.Items.Count; i++)
                {
                    // Cast the item to a DataGridViewColumn.
                    string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    // Set the DataPropertyName of the grid column to the corresponding data table column name.
                    gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();
                }
                // Finally, bind the entire DataTable to the grid. The column mappings you set ensure each grid column displays the correct field.
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                // If an error occurs, show a message box with the error details.
                MessageBox.Show(ex.ToString());
                // Ensure the connection is closed to avoid resource leaks.
                con.Close();
            }
        }

        private static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;
            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        public static void BlurBackground(Form Model)
        {
            Form Background = new Form();
            using(Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = frmMain.Instance.Size;
                Background.Location = frmMain.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Background.Dispose();
            }
        }
    }
}
