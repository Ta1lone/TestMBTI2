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
namespace TestMBTI2
{
    public partial class Adminpanel : Form
    {
        public Adminpanel()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Adminpanel_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DelUser frm_Del = new DelUser();
            frm_Del.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddQuest frm_add = new AddQuest();
            frm_add.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            log_in log_In = new log_in();
            log_In.Show();
            this.Close();
        }
    }
}
