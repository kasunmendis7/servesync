using Guna.UI2.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServeSync.Model
{
    public partial class frmCheckout : Form
    {
        public frmCheckout()
        {
            InitializeComponent();
        }

        public double amt;
        public int main_id = 0;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtRecieved_TextChanged(object sender, EventArgs e)
        {
            double amt = 0;
            double receipt = 0;
            double change = 0;

            double.TryParse(txtBillAmount.Text, out amt);
            double.TryParse(txtRecieved.Text, out receipt);
            
            change = receipt - amt;

            txtChange.Text = change.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            string qry = @"UPDATE table_main SET total=@total, received = @received, [change] = @change, status='Paid' WHERE main_id = @main_id";

            Hashtable ht = new Hashtable();
            ht.Add("@total", txtBillAmount.Text);
            ht.Add("@received", txtRecieved.Text);
            ht.Add("@change", txtChange.Text);

            ht.Add("@main_id", main_id);

            if (MainClass.SQL(qry,ht)>0)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Show("Saved Successfully");
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            txtBillAmount.Text = amt.ToString();
        }
    }
}
