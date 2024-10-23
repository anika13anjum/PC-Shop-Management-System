﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace PC_Shop_Management_System
{
    public partial class noticeAdmin : Form
    {
        public noticeAdmin()
        {
            InitializeComponent();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
            GC.Collect();
        }

        private void noticeAdmin_Load(object sender, EventArgs e)
        {
            bindNoticeTB();
        }
        private void bindNoticeTB()
        {
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select * from noticeTB";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sd);
            guna2DataGridView1.DataSource = dt;
            con.Close();
        }

        private void guna2DataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            guna2TextBox8.Text = guna2DataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox8.Text != "")
                {

                    SqlConnection con = new SqlConnection(Program.connectionString);
                    string query = "insert into noticeTB values(@time, @message)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@time ", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@message ", guna2TextBox8.Text);
                    con.Open();
                    int a = cmd.ExecuteNonQuery();
                    if (a > 0)
                    {
                        MessageBox.Show("Notice Published Successfully");
                    }
                    con.Close();
                    bindNoticeTB();
                    guna2TextBox8.Clear();


                }
                else
                {
                    MessageBox.Show("You have to Fill All the field first.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }

        }

        private void noticeUpdateBt_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "update noticeTB set message=@message where dateTime=@time";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@message ", guna2TextBox8.Text);
                cmd.Parameters.AddWithValue("@time", guna2DataGridView1.SelectedRows[0].Cells[0].Value);
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Notice Updated Successfully");
                }
                else
                {
                    MessageBox.Show("Data Not Updated.");
                }
                con.Close();
                bindNoticeTB();
                guna2TextBox8.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }


        }

        private void noticeDeleteBt_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(Program.connectionString);
                string query = "delete from noticeTB where dateTime=@time";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@time", guna2DataGridView1.SelectedRows[0].Cells[0].Value);
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Notice Deleted Successfully");
                }
                else
                {
                    MessageBox.Show("Notice Not Deleted.");
                }
                con.Close();
                bindNoticeTB();
                guna2TextBox8.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);

            }

        }
    }
}