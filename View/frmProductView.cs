using ServeSync.Model;
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

namespace ServeSync.View
{
    public partial class frmProductView : SampleView
    {
        public frmProductView()
        {
            InitializeComponent();
        }

        private void frmProductView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        public void GetData()
        {
            // This method constructs a SQL query to select all categories where the category name matches the search text.
            string qry = "SELECT product_id, product_name, product_price, c.category_id, c.category_name FROM products p inner join category c on p.category_id = c.category_id WHERE product_name LIKE '%" + txtSearch.Text + "%' ";
            // The following lines define the columns to be displayed in the DataGridView.
            ListBox lb = new ListBox();
            // dgvid is the ID of the category.
            lb.Items.Add(dgvid);
            // dgvName is the name of the category.
            lb.Items.Add(dgvName);
            // LoadData executes the query, gets a DataTable, and then sets each grid column’s DataPropertyName to the corresponding field name before binding.
            lb.Items.Add(dgvPrice);
            lb.Items.Add(dgvCategoryID);
            lb.Items.Add(dgvCategory);
            MainClass.LoadData(qry, guna2DataGridView1, lb);
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            // Creates a new instance of the frmCategoryAdd form.
            frmProductAdd frm = new frmProductAdd();
            // Opens the Category Add dialog (frmCategoryAdd) modally.
            // When the user closes that dialog(after saving), calls GetData() again to refresh the list.
            MainClass.BlurBackground(frm);
            // Calls the GetData method to refresh the DataGridView with the latest data.
            GetData();
        }

        // Any time the text changes in txtSearch, it re‑runs GetData(), giving you live filtering of the grid.
        //public override void txtSearch_TextChanged(object sender, EventArgs e)
        //{
        //   GetData();
        //}

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Checks if the clicked cell is in the edit column (dgvedit).
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                // Creates a new instance of the frmCategoryAdd form.
                frmProductAdd frm = new frmProductAdd();
                // Sets the id of the frmCategoryAdd form to the ID of the selected category in the DataGridView.
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                frm.category_id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvCategoryID"].Value);
                // Sets the text of the txtName textbox in the frmCategoryAdd form to the name of the selected category.
                frm.txtName.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                frm.txtPrice.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvPrice"].Value);
                frm.cbCategory.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvCategory"].Value);
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
                    string qry = "DELETE FROM products WHERE product_id = " + id + "";
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
            string qry = "SELECT product_id, product_name, product_price, c.category_id, c.category_name FROM products p inner join category c on p.category_id = c.category_id WHERE product_name LIKE '%" + txtSearch.Text + "%' ";
            // The following lines define the columns to be displayed in the DataGridView.
            ListBox lb = new ListBox();
            // dgvid is the ID of the category.
            lb.Items.Add(dgvid);
            // dgvName is the name of the category.
            lb.Items.Add(dgvName);
            // LoadData executes the query, gets a DataTable, and then sets each grid column’s DataPropertyName to the corresponding field name before binding.
            lb.Items.Add(dgvPrice);
            lb.Items.Add(dgvCategoryID);
            lb.Items.Add(dgvCategory);
            MainClass.LoadData(qry, guna2DataGridView1, lb);
        }
    }
}
