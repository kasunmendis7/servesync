using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServeSync.Model
{
    public partial class frmCategoryAdd : Form
    {
        // The constructor for the form. It initializes the form's components (buttons, textboxes, etc.) via InitializeComponent(), which is auto-generated.
        public frmCategoryAdd()
        {
            InitializeComponent();
        }
        // This variable is used to store the ID of the category being edited or added. If it's 0, a new category is being added; otherwise, an existing category is being edited.
        public int id = 0;

        // Event handler stub for when label1 is clicked.
        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Event handler for the "Save" button click.
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Declares a string variable to hold the SQL query.
            string qry = "";
            // If id is 0, it means a new category is being added. If not, an existing category is being updated.
            if (id==0)
            {
                qry = "INSERT INTO category VALUES (@Name)";
            }else
            {
                qry = "UPDATE category SET category_name = @Name WHERE category_id = @id ";
            }
            // Creates a new Hashtable to store the parameters for the SQL query.
            Hashtable ht = new Hashtable();
            // @id → the form’s id
            ht.Add("@id", id);
            // @Name → the text in the textbox txtName (the category name input)
            ht.Add("@Name", txtName.Text);

            // Executes the SQL query using the MainClass.SQL method, passing in the query and the parameters.
            if (MainClass.SQL(qry, ht)>0)
            {
                // If the SQL operation was successful (i.e., it affected one or more rows), it shows a message box indicating success.
                MessageBox.Show("Saved Successfully");
                // Resets the id to 0 (prepares for a new insert).
                id = 0;
                // txtName.Text = "";
                txtName.Focus(); // Sets focus back to the textbox for the next input.
                this.Close(); // Closes the form.
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        // Closes the form when the "Close" button is clicked.
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
