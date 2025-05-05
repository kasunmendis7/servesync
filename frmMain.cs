using ServeSync.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServeSync
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        // For accessing form main
        static frmMain _obj;
        public static frmMain Instance
        {
            get
            {
                if (_obj == null)
                {
                    _obj = new frmMain();
                }
                return _obj;
            }
        }

        // Method to add controls in Main Form
        // This method takes a Form object as a parameter and adds it to the ControlsPanel.
        public void AddControls(Form f)
        {
            ControlsPanel.Controls.Clear(); // Clear any existing controls in the panel
            f.Dock = DockStyle.Fill; // Set the form to fill the panel
            f.TopLevel = false; // Set the form to be a child of the panel
            ControlsPanel.Controls.Add(f); // Add the form to the panel
            f.Show(); // Show the form
        }
        // Event handler for the close button click
        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }
        // Event handler for the "Home" button click
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Call the AddControls method to load the home form
            AddControls(new frmHome());
        }
        // Event handler for the form load event
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Set the default form to be displayed in the panel
            lblUser.Text = MainClass.USER;
            _obj = this; // Set the static instance to the current form
        }
        // Event handler for the "Category" button click
        private void btnCategory_Click(object sender, EventArgs e)
        {
            // Call the AddControls method to load the category view form
            AddControls(new frmCategoryView());

        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            AddControls(new frmTableView());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            AddControls(new frmStaffView());
        }
    }
}
