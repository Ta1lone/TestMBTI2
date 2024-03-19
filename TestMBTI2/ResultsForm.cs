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
    public partial class ResultsForm : Form
    {
        private Dbdriver dbDriver = new Dbdriver();
        private SqlDataReader reader;
        private int userId;
        private int idTest;
        private int idresult;
        private string personalityType;
        private string description;
        private string recommendations;
        public ResultsForm(string personalityType, string description, string recommendations, int userId, int idTest, int idresult)
        {
            InitializeComponent();
            this.personalityType = personalityType;
            this.description = description;
            this.recommendations = recommendations;
            this.userId = userId;
            this.idTest = idTest;
            this.idresult = idresult;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void ResultsForm_Load(object sender, EventArgs e)
        {
            labelPersonalityType.Text = personalityType;
            labelDescription.Text = description;
            labelRecommendations.Text = recommendations;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTestIdRes();
            Form1 Form2 = new Form1(userId);
            Form2.Show();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddTestIdRes();
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddTestIdRes();
            TestAllresult tess = new TestAllresult(userId);
            tess.Show();
            this.Hide();
        }
        public void AddTestIdRes()
        {
            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }
            string query = "INSERT INTO Тест_has_Результаты (Тест_idTest, Тест_Пользователь_iduser, idresult) VALUES (@idTest, @userId, @idresult)";

            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@idTest", idTest);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@idresult", idresult);

                command.ExecuteNonQuery();

                dbDriver.closeConnection();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных в таблицу: " + ex.Message);
            }
        }
    }
}
