using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp17
{
    public partial class RegistrationForm : Form
    {
        private MySqlConnection con = new MySqlConnection();
        public RegistrationForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
        }
        string Gender;
        private void picturebox_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(fd.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QRCoder.QRCodeGenerator QG = new QRCoder.QRCodeGenerator();
            var MyData = QG.CreateQrCode(ID_text.Text, QRCoder.QRCodeGenerator.ECCLevel.H);
            var code = new QRCoder.QRCode(MyData);
            pictureBox2.Image = code.GetGraphic(100);

            try
            {
                MemoryStream ms = new MemoryStream();
                //pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                //byte[] Photo = ms.ToArray();
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] Photo = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(Photo, 0, Photo.Length);
                con.Open();
                MySqlCommand coman = new MySqlCommand();
                coman.Connection = con;
                coman.CommandText = "insert into registration_tb (ID,Name,FatherName,EmailAddress,DateOfBirth,Class,PhoneNumber,Gender,Photo) values('" + ID_text.Text + " ', ' " + Name_text.Text + " ',' " + Fname_text.Text + " ',' " + Email_text.Text + " ','" + dateTime_text.Text + "','" + Class_text.Text + "','" + Phone_text.Text + "','" + Gender + "',@photo)";
                coman.Parameters.AddWithValue("@photo", Photo);
                coman.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data Save Successfull !");
                ID_text.Clear();
                Name_text.Clear();
                Fname_text.Clear();
                Email_text.Clear();
                Class_text.Clear();
                Phone_text.Clear();
                pictureBox1.Image = null;
                //loadbtn_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }

            string initialDIR = @"C:\Users\acer\Desktop\QRfiles";
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = initialDIR;
            dialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|GIF|*.gif";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image.Save(dialog.FileName);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Gender = "Male";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Gender = "Female";
        }

        

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
