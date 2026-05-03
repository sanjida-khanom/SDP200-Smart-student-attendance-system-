using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace WindowsFormsApp17
{
    public partial class ScanForm : Form
    {
        private MySqlConnection con = new MySqlConnection();
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        public ScanForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_info;userid=root;password=;";
        }
        string Gender;
        private void ScanForm_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice)
                comboBox1.Items.Add(Device.Name);

            comboBox1.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();
            //Date and Time
            label2.Text = DateTime.Now.ToLongDateString();
            time_text.Text = DateTime.Now.ToLongTimeString();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
        }
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void ScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalFrame.IsRunning == true)
                FinalFrame.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BarcodeReader reader = new BarcodeReader();
            Result result = reader.Decode((Bitmap)pictureBox2.Image);
            try
            {
                string decoded = result.ToString().Trim();
                ID_text.Text = decoded;
                if (decoded != null)
                {
                    con.Open();
                    MySqlCommand coman = new MySqlCommand();
                    coman.Connection = con;
                    coman.CommandText = "select * from  registration_tb  where ID Like'%" + ID_text.Text + "%'";
                    MySqlDataReader dr = coman.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Name_text.Text = dr["Name"].ToString();
                        Fname_text.Text = dr["FatherName"].ToString();
                        Email_text.Text = dr["EmailAddress"].ToString();
                        Dateofbirth_text.Text = dr["DateOfBirth"].ToString();
                        Class_text.Text = dr["Class"].ToString();
                        Phone_text.Text = dr["PhoneNumber"].ToString();
                        gender_text.Text = dr["Gender"].ToString();
                        byte[] img = ((byte[])dr["Photo"]);
                        MemoryStream ms = new MemoryStream(img);
                        pictureBox1.Image = Image.FromStream(ms);


                    }

                    con.Close();
                    timer2.Start();
                    //MessageBox.Show("Data Save Successfull !");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error " + ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        

        private void Name_text_TextChanged(object sender, EventArgs e)
        {
            //try


            //{
            //    if (Name_text.Text.Length > 0)

            //    {
            //       // MessageBox.Show("OK");
            //        MemoryStream ms = new MemoryStream();
            //        pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        byte[] Photo = new byte[ms.Length];
            //        ms.Position = 0;
            //        ms.Read(Photo, 0, Photo.Length);
                                       
            //        con.Open();
            //        MySqlCommand coman = new MySqlCommand();
            //        coman.Connection = con;
            //        coman.CommandText = "insert into attendance_tbl (ID,Name,FatherName,EmailAddress,DateOfBirth,Class,PhoneNumber,Gender,InTime,Photo) values('" + ID_text.Text + " ', ' " + Name_text.Text + " ',' " + Fname_text.Text + " ',' " + Email_text.Text + " ','" + Dateofbirth_text.Text + "','" + Class_text.Text + "','" + Phone_text.Text + "','" + gender_text + "','"+time_text.Text+"',@photo)";
            //        coman.Parameters.AddWithValue("@photo", Photo);
            //        coman.ExecuteNonQuery();
            //        con.Close();
            //        MessageBox.Show("Data Save Successfull !");
                                   
                    
                    
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error " + ex);
            //}

        }

        private void gender_text_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("OK");
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] Photo = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(Photo, 0, Photo.Length);

                con.Open();
                MySqlCommand coman = new MySqlCommand();
                coman.Connection = con;
                coman.CommandText = "insert into attendance_tbl (ID,Name,FatherName,EmailAddress,DateOfBirth,Class,PhoneNumber,Gender,InTime,Photo) values('" + ID_text.Text + " ', ' " + Name_text.Text + " ',' " + Fname_text.Text + " ',' " + Email_text.Text + " ','" + Dateofbirth_text.Text + "','" + Class_text.Text + "','" + Phone_text.Text + "','" + gender_text.Text + "','" + time_text.Text + "',@photo)";
                coman.Parameters.AddWithValue("@photo", Photo);
                coman.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data Save Successfull !");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
