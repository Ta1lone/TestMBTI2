using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace TestMBTI2
{
   
        public partial class log_in : Form
    {
        private Dbdriver dbDriver = new Dbdriver();
        private object checkBoxShowPassword;
        private object textBoxPassword;
        

        public log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void log_in_Load(object sender, EventArgs e)
        {

            textBox2.MaxLength = 45;
            textBox1.MaxLength = 45;
            textBox2.UseSystemPasswordChar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Пользователь WHERE login = @Username AND password = @Password";

            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@Username", textBox1.Text);
                command.Parameters.AddWithValue("@Password", textBox2.Text);

                SqlDataReader reader = command.ExecuteReader();
                
                if (reader.HasRows)
                {
                    reader.Read(); 

                    int userId = reader.GetInt32(0);
                    MessageBox.Show("Успешная авторизация!");

                    
                    if (textBox1.Text == "admin" && textBox2.Text == "admin" )
                    {
                        Adminpanel frm_panel = new Adminpanel();
                        frm_panel.Show();
                        this.Hide();
                    }
                    else
                    {
                        Form1 frm_test = new Form1(userId);
                        frm_test.Show();
                        this.Hide();
                    }
                   
                }

                else
                {
                    MessageBox.Show("Неверное имя пользователя или пароль!");
                }

                reader.Close();
                dbDriver.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sign_up frm_sign = new sign_up();
            frm_sign.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
    }
}

    



