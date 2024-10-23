using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Shop_Management_System
{
    public partial class SalesMan : Form
    {
        public bool salesmanFrmClosed = false;
        public SalesMan()
        {
            InitializeComponent();
        }



        private void exitButton_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Application.Exit();
        }

        private void SalesMan_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!salesmanFrmClosed)
            {
                Program.loginForm.Close();
            }
            GC.Collect();
        }

        private void logoutSalesmanBt_Click(object sender, EventArgs e)
        {
            salesmanFrmClosed = true;
            this.Close();
            Program.loginForm.Show();
            GC.Collect();
        }

        private void noticeSMBt_Click(object sender, EventArgs e)
        {
            noticeSalesMan ns = new noticeSalesMan();
            ns.ShowDialog();
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            accountSettingsSalesman ss = new accountSettingsSalesman();
            ss.ShowDialog();
        }
        DateTime start;
        DateTime end;
        private void SalesMan_Load(object sender, EventArgs e)
        {
            welcomeText();
            bindProductData();
            guna2DataGridView1.Rows[0].Selected = true;
            guna2TextBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            stopwatch = new Stopwatch();
            stopwatch.Start();
            start = DateTime.Now;
        }
        public void bindProductData()
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
        private void welcomeText()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select userName from loginTB where userType='salesman'";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                welcomeUserIdLabel.Text = sd["userName"].ToString();
            }
            con.Close();
        }
        Stopwatch stopwatch;
        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = String.Format("{0:hh\\:mm\\:ss}", stopwatch.Elapsed);
        }

        private void SalesMan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!clickedStatus)
            {
                stopwatch.Stop();
                end = DateTime.Now;
                saveWorkingDuration();
            }
            //watchSaveWDData();
        }
        //private void watchSaveWDData()
        //{
        //    int duration = 0;
        //    SqlConnection con = new SqlConnection(Program.connectionString);
        //    string query = "select * from RegularWorkingDurationTB where post=@post";
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Parameters.AddWithValue("@post", "Salesman");
        //    SqlDataReader sd = cmd.ExecuteReader();
        //    if (sd.Read())
        //    {
        //        duration = (int)sd["duration"];
        //    }
        //    con.Close();
        //    MessageBox.Show(duration.ToString());
        //}
        private void saveWorkingDuration()
        {
            int duration = 0;
            TimeSpan ts = end - start;

            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select * from RegularWorkingDurationTB where post=@post";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@post", "Salesman");
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                duration = (int)sd["duration"];
            }
            con.Close();
            query = "update RegularWorkingDurationTB set duration=@duration where post=@post";
            con.Open();
            cmd = new SqlCommand(query, con);
            duration += Convert.ToInt32(ts.TotalSeconds);
            cmd.Parameters.AddWithValue("@duration", duration);
            cmd.Parameters.AddWithValue("@post", "Salesman");
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private bool clickedStatus = false;

        private void breakBt_Click(object sender, EventArgs e)
        {
            if (!clickedStatus)
            {
                guna2Panel2.FillColor = Color.FromArgb(255, 82, 96);
                clickedStatus = true;
                stopwatch.Stop();
                end = DateTime.Now;
                saveWorkingDuration();
            }
            else
            {
                guna2Panel2.FillColor = Color.FromArgb(14, 180, 138);
                clickedStatus = false;
                stopwatch.Start();
                start = DateTime.Now;
            }
        }
        void ResetControl()
        {
            guna2TextBox4.Clear();
            guna2TextBox5.Clear();
            guna2TextBox2.Clear();
            guna2TextBox1.Clear();
            discountBt.Enabled = true;
            applyCouponBt.Enabled = true;
            guna2DataGridView1.Rows[0].Selected = true;
            guna2TextBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }
        private void discountBt_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                guna2TextBox1.Text = (Int32.Parse(guna2TextBox1.Text) - getDiscount()).ToString();
                discountBt.Enabled = false;
            }
            else
            {
                MessageBox.Show("You have to select a row first");
            }
            //insertIdIntoAppliedDiscountList(guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            //if (!checkDiscountAppliedOrNot(guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString()))
            //{
            //}
            //else
            //{
            //    MessageBox.Show("Discount already applied in this product");
            //}
        }
        //private int getProductPrice(string pdId)
        //{
        //    int price = -1;
        //    SqlConnection con = new SqlConnection(Program.connectionString);
        //    string query = "select price from ProductTB where pdId=@pdId";
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Parameters.AddWithValue("@pdId", pdId);
        //    SqlDataReader sd = cmd.ExecuteReader();
        //    if (sd.HasRows)
        //    {
        //        if (sd.Read())
        //        {
        //            price = (int)sd["price"];
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Product ID Not found");
        //    }
        //    con.Close();
        //    return price;
        //}

        //private bool checkDiscountAppliedOrNot(string pdId)
        //{
        //    bool flag = false;
        //    SqlConnection con = new SqlConnection(Program.connectionString);
        //    string query = "select * from appliedDiscountsPdListTB where pdId=@pdId";
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Parameters.AddWithValue("@pdId", pdId);
        //    SqlDataReader sd = cmd.ExecuteReader();
        //    if (sd.HasRows)
        //    {
        //        flag = true;
        //    }
        //    con.Close();
        //    return flag;
        //}

        //private void insertIdIntoAppliedDiscountList(string pdId)
        //{
        //    SqlConnection con = new SqlConnection(Program.connectionString);
        //    string query = "insert into appliedDiscountsPdListTB values(@pdId)";
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Parameters.AddWithValue("@pdId", pdId);
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //}

        private int getDiscount()
        {
            int discount = 0;
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select * from discountsTB";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                discount = (int)sd["discounts"];
            }
            con.Close();
            return discount;
        }
        //private void resetAppliedDiscountList()
        //{
        //    SqlConnection con = new SqlConnection(Program.connectionString);
        //    string query = "delete from appliedDiscountsPdListTB";
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //}
        private void refreshBt_Click(object sender, EventArgs e)
        {
            ResetControl();
            bindProductData();
            //resetAppliedDiscountList();
        }

        private void guna2DataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            guna2TextBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }
        private void applyCouponBt_Click(object sender, EventArgs e)
        {
            if (checkValidCoupon(guna2TextBox5.Text))
            {
                guna2TextBox1.Text = (Int32.Parse(guna2TextBox1.Text) - getCouponDiscount(guna2TextBox5.Text)).ToString();
                applyCouponBt.Enabled = false;
            }
            else
            {
                MessageBox.Show("Invalid Coupon");
            }
        }

        private bool checkValidCoupon(string coupon)
        {
            bool flag = false;
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select coupon from couponsTB where coupon=@coupon";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@coupon", coupon);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.HasRows)
            {
                flag = true;
            }
            con.Close();
            return flag;
        }
        private int getCouponDiscount(string coupon)
        {
            int discount = 0;
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select couponDiscount from couponsTB where coupon=@coupon";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@coupon", coupon);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.HasRows)
            {
                if (sd.Read())
                {
                    discount = (int)sd["couponDiscount"];
                }
            }
            else
            {
                MessageBox.Show("Coupon Not Found");
            }
            con.Close();
            return discount;
        }
        private void guna2DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            discountBt.Enabled = true;
            applyCouponBt.Enabled = true;
        }

        private void soldBt_Click(object sender, EventArgs e)
        {
            try
            {
                if (Int32.Parse(guna2TextBox2.Text) > 0)
                {
                    if (Int32.Parse(guna2TextBox2.Text) <= Int32.Parse(guna2DataGridView1.SelectedRows[0].Cells[5].Value.ToString()))
                    {
                        SqlConnection con = new SqlConnection(Program.connectionString);
                        string query = "update ProductTB set availableQuantity=@availableQuantity where pdId=@pdId";
                        con.Open();
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@pdId", guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                        cmd.Parameters.AddWithValue("@availableQuantity", Int32.Parse(guna2DataGridView1.SelectedRows[0].Cells[5].Value.ToString()) - Int32.Parse(guna2TextBox2.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();

                        saveToTotalNumberOfProductSoldTB();
                        saveToTotalSalesTB();
                        saveToTodaysTotalSalesTB();
                        MessageBox.Show("Product has been Sold Successfully");

                        /*MessageBox.Show("saveToTotalNumberOfProductSoldTB: " + getCurrentTotalNumberofProductSold() + "\n" + "saveToTotalSalesTB: " + getCurrentTotalSales() + "\n" + "saveToTodaysTotalSalesTB: " + getCurrentTodaysTotalTotalSales());*/

                        bindProductData();
                    }
                    else
                    {
                        MessageBox.Show("Sold Quantity Must be less than Available Quantity");
                    }
                }
                else
                {
                    MessageBox.Show("Sold Quantity Must be greater than 0");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message + "\n" + "Please try again");
            }


        }

        private void saveToTodaysTotalSalesTB()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "update todaysTotalSalesTB set todaysTotalSales=@todaysTotalSales where sn=0";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@todaysTotalSales", Int32.Parse(guna2TextBox1.Text) * Int32.Parse(guna2TextBox2.Text) + getCurrentTodaysTotalTotalSales());
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void saveToTotalSalesTB()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "update totalSalesTB set totalSales=@totalSales where sn=0";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@totalSales", Int32.Parse(guna2TextBox1.Text) * Int32.Parse(guna2TextBox2.Text) + getCurrentTotalSales());
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void saveToTotalNumberOfProductSoldTB()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "update totalNumberOfProductSoldTB set totalSold=@totalSold where sn=0";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@totalSold", Int32.Parse(guna2TextBox2.Text) + getCurrentTotalNumberofProductSold());
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private int getCurrentTodaysTotalTotalSales()
        {
            int data = 0;
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select todaysTotalSales from todaysTotalSalesTB where sn=0";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                data = (int)sd["todaysTotalSales"];
            }
            con.Close();
            return data;
        }

        private int getCurrentTotalSales()
        {
            int data = 0;
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select totalSales from totalSalesTB where sn=0";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                data = (int)sd["totalSales"];
            }
            con.Close();
            return data;
        }

        private int getCurrentTotalNumberofProductSold()
        {
            int data = 0;
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select totalSold from totalNumberOfProductSoldTB where sn=0";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.Read())
            {
                data = (int)sd["totalSold"];
            }
            con.Close();
            return data;
        }

        private void searchNameBt_Click(object sender, EventArgs e)
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

        private void searchIdBt_Click(object sender, EventArgs e)
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
    }
}
