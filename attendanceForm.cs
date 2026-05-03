using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp17
{
    public partial class attendanceForm : Form
    {
        private MySqlConnection con = new MySqlConnection();
        
        public attendanceForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
        }

        private void attendanceForm_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                MySqlCommand coman = new MySqlCommand();
                coman.Connection = con;
                string query = "select * from registration_tb ";
                coman.CommandText = query;
                MySqlDataAdapter da = new MySqlDataAdapter(coman);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                ID_text.Text = row.Cells["ID"].Value.ToString();
                Name_text.Text = row.Cells["Name"].Value.ToString();
                Fname_text.Text = row.Cells["FatherName"].Value.ToString();
                Email_text.Text = row.Cells["EmailAddress"].Value.ToString();
                Dateofbirth_text.Text = row.Cells["DateOfBirth"].Value.ToString();
                Class_text.Text = row.Cells["Class"].Value.ToString();
                Phone_text.Text = row.Cells["PhoneNumber"].Value.ToString();
                gender_text.Text = row.Cells["Gender"].Value.ToString();
                
                //image display
                byte[] bytes = (byte[])dataGridView1.CurrentRow.Cells["Photo"].Value;
                MemoryStream ms = new MemoryStream(bytes);
                pictureBox1.Image = Image.FromStream(ms);
                


            }
        }

        
    }
}
