using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace ServeSync.View
{
    public partial class frmKitchenView : Form
    {
        public frmKitchenView()
        {
            InitializeComponent();
        }

        private void frmKitchenView_Load(object sender, EventArgs e)
        {
            GetOrders();
        }

        private void GetOrders()
        {
            flowLayoutPanel1.Controls.Clear();
            string qry1 = @"SELECT * FROM table_main WHERE status = 'Pending'";
            SqlCommand cmd1 = new SqlCommand(qry1, MainClass.con);
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(dt1);

            FlowLayoutPanel p1;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                p1 = new FlowLayoutPanel();
                p1.AutoSize = true;
                p1.Width = 230;
                p1.Height = 350;
                p1.FlowDirection = FlowDirection.TopDown;
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Margin = new Padding(10,10,10,10);

                FlowLayoutPanel p2 = new FlowLayoutPanel(); 
                p2.BackColor = Color.FromArgb(255, 140, 0);
                p2.AutoSize = true;
                p2.Width = 230;
                p2.Height = 1250;
                p2.FlowDirection = FlowDirection.TopDown;
                p2.Margin = new Padding(0, 0, 0, 0);

                Label lbl1 = new Label();
                lbl1.ForeColor = Color.White;
                lbl1.Margin = new Padding(10, 10, 3, 0);
                lbl1.AutoSize = true;

                Label lbl2 = new Label();
                lbl2.ForeColor = Color.White;
                lbl2.Margin = new Padding(10, 5, 3, 0);
                lbl2.AutoSize = true;

                Label lbl3 = new Label();
                lbl3.ForeColor = Color.White;
                lbl3.Margin = new Padding(10, 5, 3, 0);
                lbl3.AutoSize = true;

                Label lbl4 = new Label();
                lbl4.ForeColor = Color.White;
                lbl4.Margin = new Padding(10, 5, 3, 10);
                lbl4.AutoSize = true;

                lbl1.Text = "Table : " + dt1.Rows[i]["table_name"].ToString();
                lbl2.Text = "Waiter Name : " + dt1.Rows[i]["waiter_name"].ToString();
                lbl3.Text = "Order Time : " + dt1.Rows[i]["a_time"].ToString();
                lbl4.Text = "Order Type: " + dt1.Rows[i]["order_type"].ToString();

                p2.Controls.Add(lbl1);
                p2.Controls.Add(lbl2);
                p2.Controls.Add(lbl3);
                p2.Controls.Add(lbl4);

                p1.Controls.Add(p2);

                // Now add products
                int mid = 0;
                mid = Convert.ToInt32(dt1.Rows[i]["main_id"].ToString());

                string qry2 = @"SELECT * FROM table_main m 
                                INNER JOIN table_details d ON m.main_id = d.main_id
                                INNER JOIN products p ON p.product_id = d.product_id
                                WHERE m.main_id = "+mid+"";
                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);

                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    Label lbl5 = new Label();
                    lbl5.ForeColor = Color.Black;
                    lbl5.Margin = new Padding(10, 5, 3, 0);
                    lbl5.AutoSize = true;

                    int no = j + 1;

                    lbl5.Text = "" + no + " " + dt2.Rows[j]["product_name"].ToString() + " " + dt2.Rows[j]["quantity"].ToString();

                    p1.Controls.Add(lbl5);
                    // lbl5.Controls.Add(lbl5);
                }
                // Add button to change the order status
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.Size = new Size(100, 35);
                b.FillColor = Color.FromArgb(255, 140, 0);
                b.Margin = new Padding(30,5,3,10);
                b.Text = "Complete";
                b.Tag = dt1.Rows[i]["main_id"].ToString();
                b.Click += new EventHandler(b_Click);
                p1.Controls.Add(b);

                flowLayoutPanel1.Controls.Add(p1);
            }

        }

        private void b_Click(object sender, EventArgs e)
        {
            int id=Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            if (guna2MessageDialog1.Show("Are you sure you want to complete this order?") == DialogResult.Yes)
            {
                string qry = "UPDATE table_main SET status = 'Completed' WHERE main_id = @ID";
                Hashtable ht = new Hashtable(); // Creates a new Hashtable to store the parameters for the SQL query.
                ht.Add("@ID", id);

                if (MainClass.SQL(qry,ht)>0)
                {
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Show("Saved Successfully");
                }
                GetOrders();
            }

        }
    }
}
