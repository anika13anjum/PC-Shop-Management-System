using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Shop_Management_System
{
    public partial class productsAdmin : Form
    {
        public productsAdmin()
        {
            InitializeComponent();
        }

        private void insertBt_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox8.Text != "" && guna2TextBox1.Text != "" && guna2TextBox9.Text != "" && guna2TextBox10.Text != "")
                {
                    insertPrductInfoToDatabase();
                    bindAllProductData();
                    resetProductForm();
                }
                else
                {
                    MessageBox.Show("You have to All both the field first.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }

        }
        public void insertPrductInfoToDatabase()
        {
            try
            {
                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "insert into ProductTB values(@pdId, @pdName, @companyName, @modelName, @price, @availableQuantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("pdId", productIdGeneraotor());
                cmd.Parameters.AddWithValue("@pdName", guna2ComboBox1.Text);
                cmd.Parameters.AddWithValue("@companyName", guna2TextBox8.Text);
                cmd.Parameters.AddWithValue("@modelName", guna2TextBox1.Text);
                cmd.Parameters.AddWithValue("@price", guna2TextBox9.Text);
                cmd.Parameters.AddWithValue("@availableQuantity ", guna2TextBox10.Text);
                con.Open();
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Data Inserted Successfully");
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }

        }
        public string productIdGeneraotor()
        {
            int sn = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);
            string query = "select * from productSNcounterTB";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                sn = (int)sd["serial"];
            }
            con.Close();
            query = "update productSNcounterTB set serial=@serial where sn=0";
            con.Open();
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@serial", sn + 1);
            cmd.ExecuteNonQuery();
            con.Close();
            return "PD-" + sn;
        }
        public void bindAllProductData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "select * from ProductTB";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader sd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sd);
                guna2DataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }

            //SqlDataAdapter sda = new SqlDataAdapter(query, con);
            //DataTable data = new DataTable();
            //sda.Fill(data);
            //guna2DataGridView1.DataSource = data;
        }
        public void resetProductForm()
        {
            try
            {
                guna2TextBox8.Clear();
                guna2TextBox1.Clear();
                guna2TextBox9.Clear();
                guna2TextBox10.Clear();
                guna2TextBox4.Clear();
                guna2ComboBox2.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }

        }

        private void updateBt_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "update ProductTB set pdName=@pdName, companyName=@companyName, modelName=@modelName, price=@price, availableQuantity=@availableQuantity where pdId=@pdId";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pdId", guna2DataGridView1.SelectedRows[0].Cells[0].Value);
                cmd.Parameters.AddWithValue("@pdName", guna2ComboBox1.Text);
                cmd.Parameters.AddWithValue("@companyName", guna2TextBox8.Text);
                cmd.Parameters.AddWithValue("@modelName", guna2TextBox1.Text);
                cmd.Parameters.AddWithValue("@price", guna2TextBox9.Text);
                cmd.Parameters.AddWithValue("@availableQuantity", Int32.Parse(guna2TextBox10.Text) + (int)guna2DataGridView1.SelectedRows[0].Cells[5].Value);
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Data Updated Successfully");
                }
                else
                {
                    MessageBox.Show("Data Not Updated.");
                }
                con.Close();
                bindAllProductData();
                resetProductForm();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        private void deleteBt_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "delete from ProductTB where pdId = @pdId";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pdId", guna2DataGridView1.SelectedRows[0].Cells[0].Value);
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Data Deleted Successfully");
                }
                else
                {
                    MessageBox.Show("Data Not Deleted.");
                }
                con.Close();
                bindAllProductData();
                resetProductForm();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        private void refreshBt_Click(object sender, EventArgs e)
        {
            try
            {
                bindAllProductData();
                resetProductForm();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        private void searchNameBt_Click(object sender, EventArgs e)
        {
            try
            {

                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "select * from ProductTB where pdName=@searchedName or companyName=@searchedName or modelName=@searchedName";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@searchedName", guna2TextBox4.Text);
                SqlDataReader sd = cmd.ExecuteReader();
                if (sd.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(sd);
                    guna2DataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Not Found");
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }

        }

        private void searchIDbt_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "select * from ProductTB where pdId=@pdId";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pdId", guna2TextBox4.Text);
                SqlDataReader sd = cmd.ExecuteReader();
                if (sd.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(sd);
                    guna2DataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Not Found");
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        private void guna2DataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                guna2ComboBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                guna2TextBox8.Text = guna2DataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                guna2TextBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                guna2TextBox9.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                guna2TextBox10.Text = guna2DataGridView1.SelectedRows[0].Cells[5].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        private void productsAdmin_Load(object sender, EventArgs e)
        {
            try
            {

                bindAllProductData();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (guna2ComboBox2.SelectedIndex == 1)
                {
                    bindAllInStockProductData();
                }
                else if (guna2ComboBox2.SelectedIndex == 2)
                {
                    bindAllOutOfStockProductData();
                }
                else
                {
                    bindAllProductData();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        private void bindAllOutOfStockProductData()
        {
            try
            {

                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "select * from ProductTB where availableQuantity <= 0";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader sd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sd);
                guna2DataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        public void bindAllInStockProductData()
        {
            try
            {
                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "select * from ProductTB where availableQuantity > 0";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader sd = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sd);
                guna2DataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }
    }
}
