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
    public partial class DashboardAdmin : Form
    {
        public DashboardAdmin()
        {
            InitializeComponent();
        }

        private void exitAdminBt_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Application.Exit();
        }

        private void noticeAdminBt_Click(object sender, EventArgs e)
        {
            noticeAdmin notAdmin = new noticeAdmin();
            notAdmin.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            accountSettingsAdmin sa = new accountSettingsAdmin();
            sa.ShowDialog();
        }

        private void DashboardAdmin_Load(object sender, EventArgs e)
        {
            welcomeText();
            bindTotalNumberOfProductSold();
            bindTotalSalesTB();
            bindTodaysTotalSalesTB();
        }
        private void welcomeText()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select userName from loginTB where userType='admin'";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                welcomeUserIdLabel.Text = sd["userName"].ToString();
            }
            con.Close();
        }
        private void bindTodaysTotalSalesTB()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select * from todaysTotalSalesTB";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                label8.Text = sd["todaysTotalSales"].ToString();
            }
            con.Close();
        }

        private void bindTotalNumberOfProductSold()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select * from totalNumberOfProductSoldTB";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                label4.Text = sd["totalSold"].ToString();
            }
            con.Close();
        }

        private void bindTotalSalesTB()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select * from totalSalesTB";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                label5.Text = sd["totalSales"].ToString();
            }
            con.Close();
        }
    }
}
