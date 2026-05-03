using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp17
{
    public partial class LoginForm : Form
    {
        private MySqlConnection con = new MySqlConnection();
        public LoginForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;
            MySqlDataReader dr;
            try
            {

                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM login_tb WHERE UserName='" + textBox1.Text + "' AND Password='" + textBox2.Text + "' ";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Success");
                    this.Hide();
                    MainForm mf = new MainForm();
                    mf.ShowDialog();
                }
                else
                {
                    MessageBox.Show("If You are Admin,Please Enter the correct username and password");
                }
                

            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }



        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //con.Open();
            //if (con.State == ConnectionState.Open)
            //{
            //    MessageBox.Show("Sucessfull DB Connection ");
            //    con.Close();
            //}
        }
    }
}
