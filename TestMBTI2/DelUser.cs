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
    public partial class DelUser : Form
    {
        private Dbdriver dbDriver = new Dbdriver();
        private SqlDataAdapter adapter;
        private DataTable table;
        public DelUser()
        {
            InitializeComponent();
            LoadUsers();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void LoadUsers()
        {
            string query = "SELECT * FROM Пользователь";
            try
            {
                dbDriver.openConnection();
                adapter = new SqlDataAdapter(query, dbDriver.getConnection());
                table = new DataTable();
                adapter.Fill(table);
                dataGridViewUsers.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке пользователей: " + ex.Message);
            }
        }
        private void DelUser_Load(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {

            int userId = Convert.ToInt32(dataGridViewUsers.SelectedCells[0].Value);
            string query = "DELETE FROM Пользователь WHERE iduser = @UserId";
            try
            {
                dbDriver.openConnection();
                SqlCommand command = new SqlCommand(query, dbDriver.getConnection());
                command.Parameters.AddWithValue("@UserId", userId);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Пользователь удален.");
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении пользователя.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении пользователя: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Adminpanel frm_panel3 = new Adminpanel();
            frm_panel3.Show();
            this.Close();
        }
    }
}
