using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using ServeSync.Model;


namespace ServeSync.View
{
    public partial class frmCategoryView : ServeSync.SampleView
    {
        // This constructor initializes the form and calls the GetData method to load data into the DataGridView.
        public frmCategoryView()
        {
            // InitializeComponent() is an auto-generated method that sets up the form's components (buttons, textboxes, etc.).
            InitializeComponent();
            // Call the GetData method to load data into the DataGridView when the form is loaded.
            GetData();
        }
        
        public void GetData()
        {
            // This method constructs a SQL query to select all categories where the category name matches the search text.
            string qry = "SELECT * FROM category WHERE category_name LIKE '%"+ txtSearch.Text +"%' ";
            // The following lines define the columns to be displayed in the DataGridView.
            ListBox lb = new ListBox();
            // dgvid is the ID of the category.
            lb.Items.Add(dgvid);
            // dgvName is the name of the category.
            lb.Items.Add(dgvName);
            // LoadData executes the query, gets a DataTable, and then sets each grid column’s DataPropertyName to the corresponding field name before binding.
            MainClass.LoadData(qry, guna2DataGridView1, lb);
        }

        private void frmCategoryView_Load(object sender, EventArgs e)
        {
            GetData();
        }
        // This method is called when the "Add" button is clicked. It opens the frmCategoryAdd form to add a new category.
        public override void btnAdd_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the frmCategoryAdd form.
            // frmCategoryAdd frm = new frmCategoryAdd();
            // Opens the Category Add dialog (frmCategoryAdd) modally.
            // When the user closes that dialog(after saving), calls GetData() again to refresh the list.
            // frm.ShowDialog();
            // Calls the GetData method to refresh the DataGridView with the latest data.
            MainClass.BlurBackground(new frmCategoryAdd());
            GetData();
        }
        // Any time the text changes in txtSearch, it re‑runs GetData(), giving you live filtering of the grid.
        // public override void txtSearch_TextChanged(object sender, EventArgs e)
        // {
        //    GetData();
        // }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        // This method is called when a cell in the DataGridView is clicked. It checks if the clicked cell is in the edit or delete column and performs the corresponding action.
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Checks if the clicked cell is in the edit column (dgvedit).
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                // Creates a new instance of the frmCategoryAdd form.
                frmCategoryAdd frm = new frmCategoryAdd();
                // Sets the id of the frmCategoryAdd form to the ID of the selected category in the DataGridView.
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                // Sets the text of the txtName textbox in the frmCategoryAdd form to the name of the selected category.
                frm.txtName.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                // Opens the Category Add dialog (frmCategoryAdd) modally.
                // frm.ShowDialog();
                MainClass.BlurBackground(frm);
                // Calls the GetData method to refresh the DataGridView with the latest data.
                GetData();
            }
            // Checks if the clicked cell is in the delete column (dgvdel).
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                if (guna2MessageDialog1.Show("Are you sure you want to delete?") == DialogResult.Yes)
                {
                    // Reads the category_id from the current row of the DataGridView.
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                    // Executes a SQL DELETE query to remove the selected category from the database.
                    string qry = "DELETE FROM category WHERE category_id = " + id + "";
                    Hashtable ht = new Hashtable(); // Creates a new Hashtable to store the parameters for the SQL query.
                    MainClass.SQL(qry, ht); // Executes the SQL query using the MainClass.SQL method.
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    // Shows a message box indicating that the category was deleted successfully.
                    guna2MessageDialog1.Show("Deleted Successfully");
                    // Calls the GetData method to refresh the DataGridView with the latest data.
                    GetData();
                }
            }
        }

        public void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            // This method constructs a SQL query to select all categories where the category name matches the search text.
            string qry = "SELECT * FROM category WHERE category_name LIKE '%" + txtSearch.Text + "%' ";
            // The following lines define the columns to be displayed in the DataGridView.
            ListBox lb = new ListBox();
            // dgvid is the ID of the category.
            lb.Items.Add(dgvid);
            // dgvName is the name of the category.
            lb.Items.Add(dgvName);
            // LoadData executes the query, gets a DataTable, and then sets each grid column’s DataPropertyName to the corresponding field name before binding.
            MainClass.LoadData(qry, guna2DataGridView1, lb);
        }
    }
}
