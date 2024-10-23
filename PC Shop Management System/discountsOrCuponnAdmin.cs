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
    public partial class discountsOrCuponnAdmin : Form
    {
        public discountsOrCuponnAdmin()
        {
            InitializeComponent();
        }

        private void discountsOrCuponnAdmin_Load(object sender, EventArgs e)
        {
            reset();
            bindDiscountsAndCouponsData();
        }
        private void reset()
        {
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox5.Clear();
            guna2TextBox6.Clear();
            guna2TextBox4.Clear();
            guna2TextBox3.Clear();
        }
        private void bindDiscountsAndCouponsData()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select * from discountsTB";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                guna2TextBox1.Text = sd["discounts"].ToString();
            }
            con.Close();
            
            SqlConnection con1 = new SqlConnection(Program.connectionString);
            string query1 = "select * from couponsTB";
            con1.Open();
            SqlCommand cmd1 = new SqlCommand(query1, con1);
            SqlDataReader sd1 = cmd1.ExecuteReader();
            if (sd1.Read())
            {
                guna2TextBox6.Text = sd1["couponDiscount"].ToString();
                guna2TextBox5.Text = sd1["coupon"].ToString();
            }
            con1.Close();
        }

        private void setDiscountBt_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "update discountsTB set discounts=@discount where sn=0";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@discount", Int32.Parse(guna2TextBox2.Text));
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Discount Added Successfully");
            }
            con.Close();
            reset();
            bindDiscountsAndCouponsData();
        }

        private void setCouponBt_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "update couponsTB set coupon=@coupon, couponDiscount=@couponDiscount where sn=0";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@coupon", guna2TextBox4.Text);
            cmd.Parameters.AddWithValue("@couponDiscount", Int32.Parse(guna2TextBox3.Text));
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Coupon Added Successfully");
            }
            con.Close();
            reset();
            bindDiscountsAndCouponsData();
        }

        private void removeDiscountBt_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "update discountsTB set discounts=0 where sn=0";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Discount Removed Successfully");
            }
            con.Close();
            reset();
            bindDiscountsAndCouponsData();
        }

        private void removeCouponsBt_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "update couponsTB set coupon='', couponDiscount=0 where sn=0";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Coupon Removed Successfully");
            }
            con.Close();
            reset();
            bindDiscountsAndCouponsData();
        }
    }
}
