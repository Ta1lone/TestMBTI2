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

namespace TestMBTI2
{
    public partial class sign_up : Form
    {
        private Dbdriver dbDriver = new Dbdriver();
        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Пользователь (login, password) VALUES (@Username, @Password)";

            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@Username", textBox1.Text);
                command.Parameters.AddWithValue("@Password", textBox2.Text);

                command.ExecuteNonQuery();

                MessageBox.Show("Регистрация прошла успешно!");
                log_in form = new log_in();
                this.Close();
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации: " + ex.Message);
            }
            finally
            {
                dbDriver.closeConnection();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            log_in form = new log_in();
            this.Close();
            form.Show();
        }
    }
}
