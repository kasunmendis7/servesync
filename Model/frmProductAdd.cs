using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ServeSync.Model
{
    public partial class frmProductAdd : Form
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }

        public int id = 0;
        public int category_id = 0;

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            // For cb fill
            string qry = "SELECT category_id 'id', category_name 'name' FROM category";
            MainClass.CBFill(qry, cbCategory);
            if (category_id > 0) // For update
            {
                cbCategory.SelectedValue = category_id;
            }

            if (id > 0)
            {
                ForUpdateLoadData();
            }
        }

        string filePath;
        Byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files( .jpg; .png;)|* .png; *.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                txtImage.Image = new Bitmap(filePath);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Declares a string variable to hold the SQL query.
            string qry = "";
            // If id is 0, it means a new category is being added. If not, an existing category is being updated.
            if (id == 0)
            {
                qry = "INSERT INTO products VALUES (@Name, @Price, @Category, @Image)";
            }
            else
            {
                qry = "UPDATE products SET product_name = @Name, product_price = @Price, category_id = @Category, product_image = @Image  WHERE product_id = @id ";
            }
            // For image
            Image temp = new Bitmap(txtImage.Image);
            MemoryStream ms = new MemoryStream();
            temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            imageByteArray = ms.ToArray();

            // Creates a new Hashtable to store the parameters for the SQL query.
            Hashtable ht = new Hashtable();
            // @id → the form’s id
            ht.Add("@id", id);
            // @Name → the text in the textbox txtName (the category name input)
            ht.Add("@Name", txtName.Text);
            ht.Add("@Price", txtPrice.Text);
            ht.Add("@Category", Convert.ToInt32(cbCategory.SelectedValue));
            ht.Add("@Image", imageByteArray);

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

        private void ForUpdateLoadData()
        {
            // For cb fill
            string qry = "SELECT * from products where product_id = "+id+"";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["product_name"].ToString();
                txtPrice.Text = dt.Rows[0]["product_price"].ToString();
                Byte[] imageArray = (Byte[])(dt.Rows[0]["product_image"]);
                byte[] imageByteArray = imageArray;
                txtImage.Image = Image.FromStream(new MemoryStream(imageArray));
            }
        }
    }
}
