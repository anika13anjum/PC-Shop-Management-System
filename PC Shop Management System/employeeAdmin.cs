using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_Shop_Management_System
{
    public partial class employeeAdmin : Form
    {
        public employeeAdmin()
        {
            InitializeComponent();
        }

        private void employeeAdmin_Load(object sender, EventArgs e)
        {
            try
            {
                ResetControl();
                BindGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }
        void BindGridView()
        {
            DataGridViewImageColumn dvg = new DataGridViewImageColumn();
            dvg = (DataGridViewImageColumn)guna2DataGridView1.Columns[7];
            dvg.ImageLayout = DataGridViewImageCellLayout.Stretch;
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.RowTemplate.Height = 100;

            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select empNId,empName,post,salary,empAddress,phoneNumber,email,picture from EmployeeTB";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader sd = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sd);
            guna2DataGridView1.DataSource = dt;
            con.Close();
            ResetControl();
        }
        void ResetControl()
        {
            guna2TextBox11.Clear();
            guna2TextBox1.Clear();
            guna2TextBox10.Clear();
            guna2TextBox9.Clear();
            guna2TextBox8.Clear();
            guna2TextBox7.Clear();
            guna2TextBox4.Clear();
            guna2PictureBox1.Image = Properties.Resources.user_white1;
        }

        private void insertBt_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2TextBox8.Text != "" && guna2TextBox7.Text != "" && guna2TextBox9.Text != "" && guna2TextBox10.Text != "" && guna2TextBox11.Text != "" && guna2TextBox1.Text != "")
                {
                    SqlConnection con = new SqlConnection(Program.connectionString);
                    string query = "insert into EmployeeTB values(@empNId, @empName, @post, @salary, @receivableSalary, @workingDuration, @empAddress, @phoneNumber, @email, @picture)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@empNId", guna2TextBox7.Text);
                    cmd.Parameters.AddWithValue("@empName", guna2TextBox8.Text);
                    cmd.Parameters.AddWithValue("@post", guna2ComboBox1.Text);
                    cmd.Parameters.AddWithValue("@salary", guna2TextBox1.Text);
                    cmd.Parameters.AddWithValue("@receivableSalary", getReceivableSalary(guna2TextBox7.Text));
                    cmd.Parameters.AddWithValue("@workingDuration", getWorkingDuration(guna2TextBox7.Text));
                    cmd.Parameters.AddWithValue("@empAddress", guna2TextBox9.Text);
                    cmd.Parameters.AddWithValue("@phoneNumber", guna2TextBox10.Text);
                    cmd.Parameters.AddWithValue("@email", guna2TextBox11.Text);
                    cmd.Parameters.AddWithValue("@picture", SavePhoto());

                    int a = cmd.ExecuteNonQuery();
                    if (a > 0)
                    {
                        MessageBox.Show("Data Inserted Successfully");
                        BindGridView();
                        ResetControl();
                    }
                    else
                    {
                        MessageBox.Show("Data not inserted !");
                    }
                    con.Close();
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
        private byte[] SavePhoto()
        {
            MemoryStream ms = new MemoryStream();
            guna2PictureBox1.Image.Save(ms, guna2PictureBox1.Image.RawFormat);
            return ms.GetBuffer();
        }
        private Image GetPhoto(byte[] photo)
        {
            MemoryStream ms = new MemoryStream(photo);
            return Image.FromStream(ms);
        }
        private int getReceivableSalary(string nid)
        {
            int Salary = 0;
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select receivableSalary from EmployeeTB where empNId=@nid";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nid", nid);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.HasRows)
            {
                if (sd.Read())
                {
                    Salary = (int)sd["receivableSalary"];
                }
            }
            con.Close();
            return Salary;
        }
        private int getWorkingDuration(string nid)
        {
            int duration = 0;
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select workingDuration from EmployeeTB where empNId=@nid";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@nid", nid);
            SqlDataReader sd = cmd.ExecuteReader();
            if (sd.HasRows)
            {
                if (sd.Read())
                {
                    duration = (int)sd["workingDuration"];
                }
            }
            con.Close();
            return duration;
        }

        private void guna2DataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                guna2TextBox7.Text = guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                guna2TextBox8.Text = guna2DataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                guna2ComboBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                guna2TextBox1.Text = guna2DataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                guna2TextBox9.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                guna2TextBox10.Text = guna2DataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                guna2TextBox11.Text = guna2DataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                guna2PictureBox1.Image = GetPhoto((byte[])guna2DataGridView1.SelectedRows[0].Cells[7].Value);
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
                string query = "update EmployeeTB set empName=@empName, post=@post, salary=@salary, empAddress=@empAddress, phoneNumber=@phoneNumber, email=@email, picture=@picture where empNId=@empNId";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@empNId", guna2TextBox7.Text);
                cmd.Parameters.AddWithValue("@empName", guna2TextBox8.Text);
                cmd.Parameters.AddWithValue("@post", guna2ComboBox1.Text);
                cmd.Parameters.AddWithValue("@salary", Int32.Parse(guna2TextBox1.Text));
                cmd.Parameters.AddWithValue("@phoneNumber", guna2TextBox10.Text);
                cmd.Parameters.AddWithValue("@email", guna2TextBox11.Text);
                cmd.Parameters.AddWithValue("@empAddress", guna2TextBox9.Text);
                cmd.Parameters.AddWithValue("@picture", SavePhoto());
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Data updated Successfully");
                    ResetControl();
                    BindGridView();
                }
                else
                {
                    MessageBox.Show("Update unsuccesful !");
                }
                con.Close();
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
                string query = "delete from EmployeeTB where empNId=@empNId";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@empNId", guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Data Deleted succesfully!");
                    BindGridView();
                }
                else
                {
                    MessageBox.Show("Data not Deleted!");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception name: " + ex.Message);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Please select image.";
                ofd.Filter = "Image File (*.png, *.jpg, *.jpeg;) | *.png;*.jpg;*.jpeg;";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    guna2PictureBox1.Image = new Bitmap(ofd.FileName);
                }

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
                string query = "select empNId,empName,post,salary,empAddress,phoneNumber,email,picture from EmployeeTB where empName=@searchedName";
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
            SqlConnection con = new SqlConnection(Program.connectionString);
            string query = "select empNId,empName,post,salary,empAddress,phoneNumber,email,picture from EmployeeTB empNId=@searchedNId";
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@searchedNId", guna2TextBox4.Text);
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

        private void refreshBt_Click(object sender, EventArgs e)
        {
            ResetControl();
            BindGridView();
        }
    }
}
