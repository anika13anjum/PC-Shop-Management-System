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

namespace PC_Shop_Management_System
{
    public partial class accountSettingsSalesman : Form
    {
        public accountSettingsSalesman()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "update LoginTB set userName=@userName, password=@password where userType=@userType";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@username", userNameTb.Text);
            cmd.Parameters.AddWithValue("@password", passTb.Text);
            cmd.Parameters.AddWithValue("@userType", "salesman");
            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Account Settings Saved Successfully");
            }
            else
            {
                MessageBox.Show("Account Settings Not Saved");
            }
            con.Close();
            this.Close();
            GC.Collect();
        }
    }
}
