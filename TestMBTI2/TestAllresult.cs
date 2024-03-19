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
    public partial class TestAllresult : Form
    {
        private int userId;
        private Dbdriver dbDriver = new Dbdriver();
        public TestAllresult(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Testresult_Load(object sender, EventArgs e)
        {
            
            ShowUserTestResults(userId);
        }
        private void ShowUserTestResults(int userId)
        {
            string query = "SELECT r.idresult, r.TypeMBTI, r.descriptionType, r.recomendation " +
                           "FROM Результаты r " +
                           "JOIN Тест_has_Результаты thr ON r.idresult = thr.idresult " +
                           "WHERE thr.Тест_Пользователь_iduser = @userId";

            try
            {
                dbDriver.openConnection();

                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@userId", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при получении результатов теста: " + ex.Message);
            }
            finally
            {
                dbDriver.closeConnection();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
