using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServeSync.Model
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        public int MainID = 0;
        public string OrderType = "";
        public int driverID = 0;
        public string customerName = "";
        public string customerPhone = "";
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {

            lblTable.Visible = false;
            lblWaiter.Visible = false;
            lblDriverName.Visible = false;
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            loadProducts();
        }

        private void AddCategory()
        {
            string qry = "SELECT * FROM category";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryPanel.Controls.Clear();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(255, 192, 128);
                    b.Size = new Size(147, 45);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["category_name"].ToString();

                    // Even for click
                    b.Click += new EventHandler(b_Click);

                    CategoryPanel.Controls.Add(b);
                }

            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            if (b.Text == "All Categories")
            {
                txtSearch.Text = "1";
                txtSearch.Text = "";
                return;
            }
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        private void AddItems(string id, string proID, string name, string cat, string price, Image pimage)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(proID)
            };

            ProductPanel.Controls.Add(w);
            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;
                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    // This line checks if the product ID already exists and adds 1 to the quantity.
                    if (Convert.ToInt32(item.Cells["dgvproID"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) * 
                                                         double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        GetTotal();
                        return;
                    }
                }
                // This line adds a new product to the DataGridView with the product details. (first for Sr# and 2nd from id)
                guna2DataGridView1.Rows.Add(new object[] { 0, 0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                GetTotal();
            };
        }

        // Getting product from database
        private void loadProducts()
        {
            string qry = "SELECT * FROM products p inner join category c on p.category_id = c.category_id";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["product_image"];
                byte[] imagebytearray = imagearray;
                AddItems("0", item["product_id"].ToString(), item["product_name"].ToString(), item["category_name"].ToString(),
                                       item["product_price"].ToString(), (Image)(new ImageConverter().ConvertFrom(imagebytearray)));
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
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

        private void GetTotal()
        {
            double tot = 0;
            label3.Text = "";
            foreach (DataGridViewRow item in guna2DataGridView1.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }
            label3.Text = tot.ToString("N2");
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            guna2DataGridView1.Rows.Clear();
            MainID = 0;
            label3.Text = "0.00";
            btnDelivery.Checked = false;
            btnTake.Checked = false;
            lblDriverName.Visible = false;
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Delivery";
            btnDelivery.Checked = true;
            btnTake.Checked = false;

            frmAddCustomer frm = new frmAddCustomer();
            frm.mainID = MainID;
            frm.orderType = OrderType;
            MainClass.BlurBackground(frm);

            if (frm.txtName.Text != "") 
            {
                driverID = frm.driverID;
                lblDriverName.Text = "Customer Name: " + frm.txtName.Text + " Phone: " + frm.txtPhone.Text + " Driver: " + frm.cbDriver.Text;
                lblDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            OrderType = "Take Away";
            btnTake.Checked = true;
            btnDelivery.Checked = false;

            frmAddCustomer frm = new frmAddCustomer();
            frm.mainID = MainID;
            frm.orderType = OrderType;
            MainClass.BlurBackground(frm);

            if(frm.txtName.Text != "") // As take away did not have driver
            {
                driverID = frm.driverID;
                lblDriverName.Text = "Customer Name: " + frm.txtName.Text + " Phone: " + frm.txtPhone.Text;
                lblDriverName.Visible = true;
                customerName = frm.txtName.Text;
                customerPhone = frm.txtPhone.Text;
            }
        }

        private void btnDine_Click(object sender, EventArgs e)
        {
            OrderType = "Dine In";
            lblDriverName.Visible = false;

            // Create a form for table selection and waiter selection
            frmTableSelect frm = new frmTableSelect();
            MainClass.BlurBackground(frm);
            if (frm.TableName != "")
            {
                lblTable.Text = frm.TableName;
                lblTable.Visible = true;
            }
            else
            {
                lblTable.Text = "";
                lblTable.Visible = false;
            }

            frmWaiterSelect frm2 = new frmWaiterSelect();
            MainClass.BlurBackground(frm2);
            if (frm2.waiterName != "")
            {
                lblWaiter.Text = frm2.waiterName;
                lblWaiter.Visible = true;
            }
            else
            {
                lblWaiter.Text = "";
                lblWaiter.Visible = false;
            }
        }

        private void btnKot_Click(object sender, EventArgs e)
        {
            // Save the data to the database
            string qry1 = ""; // table_main
            string qry2 = ""; //table_details
            
            int detail_id = 0;
            if (MainID == 0) // Insert
            {
                qry1 = @"INSERT INTO table_main VALUES (@a_date, @a_time, @table_name, @waiter_name, @status, @order_type, @total, @received, @change, @driver_id, @customer_name, @customer_phone);
                          SELECT SCOPE_IDENTITY()";
                // This line retrieves the last inserted ID from the table_main table.
            }
            else // Update
            {
                qry1 = @"UPDATE table_main SET status=@status, total=@total, received=@received, change=@change WHERE main_id = @id";

            }

            SqlCommand cmd = new SqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@id", MainID);
            cmd.Parameters.AddWithValue("@a_date", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@a_time", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@table_name", lblTable.Text);
            cmd.Parameters.AddWithValue("@waiter_name", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Pending");
            cmd.Parameters.AddWithValue("@order_type", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(label3.Text)); // As we only save data for kitchen value will update when payment recieved
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@driver_id", driverID);
            cmd.Parameters.AddWithValue("@customer_name", customerName);
            cmd.Parameters.AddWithValue("@customer_phone", customerPhone);

            if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
            if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if (MainClass.con.State == ConnectionState.Open ) { MainClass.con.Close(); }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detail_id = Convert.ToInt32(row.Cells["dgvid"].Value);
                if (detail_id == 0) // Insert
                {
                    qry2 = @"INSERT INTO table_details VALUES (@main_id, @product_id, @quantity, @price, @amount)";
                }
                else // Update
                {
                    qry2 = @"UPDATE table_details SET quantity=@quantity, price=@price, amount=@amount WHERE detail_id = @detail_id";
                }
                SqlCommand cmd2= new SqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@id", detail_id);
                cmd2.Parameters.AddWithValue("@main_id", MainID);
                cmd2.Parameters.AddWithValue("@product_id", row.Cells["dgvproID"].Value);
                cmd2.Parameters.AddWithValue("@quantity", row.Cells["dgvQty"].Value);
                cmd2.Parameters.AddWithValue("@price", row.Cells["dgvPrice"].Value);
                cmd2.Parameters.AddWithValue("@amount", row.Cells["dgvAmount"].Value);
                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
                cmd2.ExecuteNonQuery(); 
                if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }

                
            }
            guna2MessageDialog1.Show("Saved successfully");
            MainID = 0;
            detail_id = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            label3.Text = "0.00";
            lblDriverName.Text = "";
            lblDriverName.Visible = false;
            btnTake.Checked = false;
            btnDelivery.Checked = false;
        }
        public int id = 0;
        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBillList frm = new frmBillList();
            MainClass.BlurBackground(frm);

            if (frm.main_id > 0)
            {
                id = frm.main_id;
                MainID = frm.main_id;
                LoadEntries();
            }
        }

        private void LoadEntries()
        {
            string qry = @"SELECT * FROM table_main m 
                                INNER JOIN table_details d ON m.main_id = d.main_id
                                INNER JOIN products p ON p.product_id = d.product_id
                                WHERE m.main_id = " + id + "";
            SqlCommand cmd2 = new SqlCommand(qry, MainClass.con);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            if (dt2.Rows[0]["order_type"].ToString() == "Delivery")
            {
                btnDelivery.Checked = true;
                lblWaiter.Visible = false;
                lblTable.Visible = false;
            }
            else if (dt2.Rows[0]["order_type"].ToString() == "Take Away")
            {
                btnTake.Checked = true;
                lblWaiter.Visible = false;
                lblTable.Visible = false;
            }else
            {
                btnDine.Checked = true;
                lblWaiter.Visible = true;
                lblTable.Visible = true;
            }

            guna2DataGridView1.Rows.Clear();

            foreach (DataRow item in dt2.Rows)
            {
                lblTable.Text = dt2.Rows[0]["table_name"].ToString();
                lblWaiter.Text = dt2.Rows[0]["waiter_name"].ToString();

                string detailID = item["detail_id"].ToString();
                string proID = item["product_id"].ToString();
                string proName = item["product_name"].ToString();
                string qty = item["quantity"].ToString();
                string price = item["price"].ToString();
                string amount = item["amount"].ToString();

                object[] obj = { 0, detailID, proID, proName, qty, price, amount };
                guna2DataGridView1.Rows.Add(obj);
            }
            GetTotal();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            frmCheckout frm = new frmCheckout();
            frm.main_id = id;
            frm.amt = Convert.ToDouble(label3.Text);
            MainClass.BlurBackground(frm);
            btnTake.Checked = false;
            btnDelivery.Checked = false;
            btnDine.Checked = false;

            MainID = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            label3.Text = "0.00";
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            btnTake.Checked = false;
            btnDelivery.Checked = false;
            btnDine.Checked = false;
            // Save the data to the database
            string qry1 = ""; // table_main
            string qry2 = ""; //table_details

            int detail_id = 0;

            if (OrderType=="")
            {
                guna2MessageDialog1.Show("Please select order type");
                return;
            }
            if (MainID == 0) // Insert
            {
                qry1 = @"INSERT INTO table_main VALUES (@a_date, @a_time, @table_name, @waiter_name, @status, @order_type, @total, @received, @change, @driver_id, @customer_name, @customer_phone);
                          SELECT SCOPE_IDENTITY()";
                // This line retrieves the last inserted ID from the table_main table.
            }
            else // Update
            {
                qry1 = @"UPDATE table_main SET status=@status, total=@total, received=@received, change=@change WHERE main_id = @id";

            }

            SqlCommand cmd = new SqlCommand(qry1, MainClass.con);
            cmd.Parameters.AddWithValue("@id", MainID);
            cmd.Parameters.AddWithValue("@a_date", Convert.ToDateTime(DateTime.Now.Date));
            cmd.Parameters.AddWithValue("@a_time", DateTime.Now.ToShortTimeString());
            cmd.Parameters.AddWithValue("@table_name", lblTable.Text);
            cmd.Parameters.AddWithValue("@waiter_name", lblWaiter.Text);
            cmd.Parameters.AddWithValue("@status", "Hold");
            cmd.Parameters.AddWithValue("@order_type", OrderType);
            cmd.Parameters.AddWithValue("@total", Convert.ToDouble(label3.Text)); // As we only save data for kitchen value will update when payment recieved
            cmd.Parameters.AddWithValue("@received", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@change", Convert.ToDouble(0));
            cmd.Parameters.AddWithValue("@driver_id", driverID);
            cmd.Parameters.AddWithValue("@customer_name", customerName);
            cmd.Parameters.AddWithValue("@customer_phone", customerPhone);

            if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
            if (MainID == 0) { MainID = Convert.ToInt32(cmd.ExecuteScalar()); } else { cmd.ExecuteNonQuery(); }
            if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                detail_id = Convert.ToInt32(row.Cells["dgvid"].Value);
                if (detail_id == 0) // Insert
                {
                    qry2 = @"INSERT INTO table_details VALUES (@main_id, @product_id, @quantity, @price, @amount)";
                }
                else // Update
                {
                    qry2 = @"UPDATE table_details SET quantity=@quantity, price=@price, amount=@amount WHERE detail_id = @detail_id";
                }
                SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
                cmd2.Parameters.AddWithValue("@id", detail_id);
                cmd2.Parameters.AddWithValue("@main_id", MainID);
                cmd2.Parameters.AddWithValue("@product_id", row.Cells["dgvproID"].Value);
                cmd2.Parameters.AddWithValue("@quantity", row.Cells["dgvQty"].Value);
                cmd2.Parameters.AddWithValue("@price", row.Cells["dgvPrice"].Value);
                cmd2.Parameters.AddWithValue("@amount", row.Cells["dgvAmount"].Value);
                if (MainClass.con.State == ConnectionState.Closed) { MainClass.con.Open(); }
                cmd2.ExecuteNonQuery();
                if (MainClass.con.State == ConnectionState.Open) { MainClass.con.Close(); }


            }
            guna2MessageDialog1.Show("Saved successfully");
            MainID = 0;
            detail_id = 0;
            guna2DataGridView1.Rows.Clear();
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblTable.Visible = false;
            lblWaiter.Visible = false;
            label3.Text = "0.00";
            lblDriverName.Text = "";
            lblDriverName.Visible = false;
        }
    }
}
