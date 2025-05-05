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
    public partial class frmStaffAdd : Form
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        public int id = 0;

        private void frmStaffAdd_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Declares a string variable to hold the SQL query.
            string qry = "";
            // If id is 0, it means a new category is being added. If not, an existing category is being updated.
            if (id == 0)
            {
                qry = "INSERT INTO staff(staff_name, staff_phone, staff_role) VALUES (@Name, @Phone, @Role)";
            }
            else
            {
                qry = "UPDATE staff SET staff_name = @Name, staff_phone = @Phone, staff_role = @Role WHERE staff_id = @id ";
            }
            // Creates a new Hashtable to store the parameters for the SQL query.
            Hashtable ht = new Hashtable();
            // @id → the form’s id
            ht.Add("@id", id);
            // @Name → the text in the textbox txtName (the category name input)
            ht.Add("@Name", txtName.Text);
            ht.Add("@Phone", txtPhone.Text);
            ht.Add("@Role", cbRole.Text);

            // Executes the SQL query using the MainClass.SQL method, passing in the query and the parameters.
            if (MainClass.SQL(qry, ht) > 0)
            {
                // If the SQL operation was successful (i.e., it affected one or more rows), it shows a message box indicating success.
                guna2MessageDialog1.Show("Saved Successfully");
                // Resets the id to 0 (prepares for a new insert).
                id = 0;
                // txtName.Text = "";
                txtName.Focus(); // Sets focus back to the textbox for the next input.
                this.Close(); // Closes the form.
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
