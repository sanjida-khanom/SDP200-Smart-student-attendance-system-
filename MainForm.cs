using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp17
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm Lf = new LoginForm();
            Lf.ShowDialog();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegistrationForm Rf = new RegistrationForm();
            Rf.ShowDialog();
            Rf = null;
            this.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongDateString();
            label3.Text = DateTime.Now.ToLongTimeString();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm Lf = new LoginForm();
            Lf.ShowDialog();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            ScanForm Sf = new ScanForm();
            Sf.ShowDialog();
            Sf = null;
            this.Show();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.Hide();
            attendanceForm Attf = new attendanceForm();
            Attf.ShowDialog();
            Attf = null;
            this.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentInfo Stinfo = new StudentInfo();
            Stinfo.ShowDialog();
            Stinfo = null;
            this.Show();
        }
    }
}
