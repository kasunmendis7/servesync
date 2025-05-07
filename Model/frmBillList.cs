using Guna.UI2.WinForms;
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
    public partial class frmBillList : Form
    {
        public frmBillList()
        {
            InitializeComponent();
        }

        public int main_id = 0; 
        private void frmBillList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            // This method constructs a SQL query to select all categories where the category name matches the search text.
            string qry = @"SELECT main_id, table_name, waiter_name, order_type, status, total FROM table_main WHERE status != 'Pending' ";
            // The following lines define the columns to be displayed in the DataGridView.
            ListBox lb = new ListBox();
            // dgvid is the ID of the category.
            lb.Items.Add(dgvid);
            // LoadData executes the query, gets a DataTable, and then sets each grid column’s DataPropertyName to the corresponding field name before binding.
            lb.Items.Add(dgvtable);
            lb.Items.Add(dgvWaiter);
            lb.Items.Add(dgvType);
            lb.Items.Add(dgvStatus);
            lb.Items.Add(dgvTotal);

            MainClass.LoadData(qry, guna2DataGridView1, lb);
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // For serial no. 
            int count = 0;
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Checks if the clicked cell is in the edit column (dgvedit).
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                main_id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                this.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
