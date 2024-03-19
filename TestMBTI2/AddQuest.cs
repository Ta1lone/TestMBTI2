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
    public partial class AddQuest : Form
    {
        private Dbdriver dbDriver = new Dbdriver();
        public AddQuest()
        {
            InitializeComponent();
            textBox1.MaxLength = 200;
            textBox2.MaxLength = 255;
            textBox3.MaxLength = 255;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string questionText = textBox1.Text;
            string answer1Text = textBox2.Text;
            string answer2Text = textBox3.Text;

            if (string.IsNullOrEmpty(questionText) || string.IsNullOrEmpty(answer1Text) || string.IsNullOrEmpty(answer2Text))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            try
            {
                dbDriver.openConnection();

                int idquest = GetLastQuestionId()+1;
                string query = "INSERT INTO Вопросы (TextQuest,idQuest) VALUES (@TextQuest,@idque)";
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@TextQuest", questionText);
                command.Parameters.AddWithValue("@idque", idquest);
                command.ExecuteNonQuery();

                query = "INSERT INTO Ответы1 (idQuest, ansver, ansver2) VALUES (@idQuest, @TextAnswer, @TextAnswer2)";
                command = new SqlCommand(query, dbDriver.getConnection());

                command.Parameters.AddWithValue("@idQuest", idquest);
                command.Parameters.AddWithValue("@TextAnswer", answer1Text);
                command.Parameters.AddWithValue("@TextAnswer2", answer2Text);
                command.ExecuteNonQuery();



                MessageBox.Show("Вопрос и ответы успешно добавлены.");
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении вопроса и ответов: " + ex.Message);
            }
            finally
            {
                dbDriver.closeConnection();
            }
        }
        private int GetLastQuestionId()
        {
            string query = "SELECT MAX(idQuest) FROM Вопросы";
            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении последнего вопроса: " + ex.Message);
                return 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Adminpanel frm_panel1 = new Adminpanel();
            frm_panel1.Show();
            this.Close();
        }
    }
}
