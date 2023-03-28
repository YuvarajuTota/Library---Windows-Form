using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsAppUsecase
{
    public partial class LibraryHomeScreen : Form
    {
        private TextBox txtBkAt;
        private TextBox txtBkCode;
        public LibraryHomeScreen()
        {
            InitializeComponent();
            Load();
        }
        MySqlConnection connection = new MySqlConnection("server=localhost; uid=root; pwd=Yuvi@12345; database=ado");
        MySqlCommand cmd;
        MySqlDataReader read;
        string code;
        bool mode = true;
        string sql;
        public void Load()
        {
            try
            {
                sql = "Select * from library";
                cmd = new MySqlCommand(sql, connection);
                connection.Open();
                read = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void getCode(string code)
        {
            sql = "select * from library where BookCode = '" + code + "' ";
            cmd = new MySqlCommand(sql, connection);
            connection.Open();
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                txtBkCode.Text = read[0].ToString();
                txtBkAt.Text = read[1].ToString();
                txtBkTl.Text = read[2].ToString();
                txtBkCg.Text = read[3].ToString();
            }
            connection.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            var code = txtBkCode.Text;
            var title = txtBkTl.Text;
            var author = txtBkAt.Text;
            var category = txtBkCg.Text;
            if (mode == true)
            {
                sql =
                    "INSERT INTO LIBRARY (BookCode, BookAuthor, BookTitle, BookCategory) values (@BookCode, @BookAuthor, @BookTitle, @BookCategory)";
                connection.Open();
                cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@BookCode", txtBkCode.Text);
                cmd.Parameters.AddWithValue("@BookAuthor", txtBkAt.Text);
                cmd.Parameters.AddWithValue("@BookTitle", txtBkTl.Text);
                cmd.Parameters.AddWithValue("@BookCategory", txtBkCg.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show(@"Book Added Successfully!");
                txtBkCode.Clear();
                txtBkAt.Clear();
                txtBkTl.Clear();
                txtBkCg.Text = "";
                txtBkCode.Focus();
            }
            else
            {
                code = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql =
                    "Update library set BookAuthor = @BookAuthor, BookTitle = @BookTitle, BookCategory = @BookCategory where BookCode = @BookCode";
                    connection.Open();
                cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@BookCode", txtBkCode.Text);
                cmd.Parameters.AddWithValue("@BookAuthor", txtBkAt.Text);
                cmd.Parameters.AddWithValue("@BookTitle", txtBkTl.Text);
                cmd.Parameters.AddWithValue("@BookCategory", txtBkCg.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show(@"Book Updated Successfully!");
                txtBkCode.Clear();
                txtBkAt.Clear();
                txtBkTl.Clear();
                txtBkCg.Text = "";
                btnSave.Text = @"SAVE";
                mode = true;
            }
            connection.Close();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                mode = false;
                code = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtBkCode.ReadOnly = true;
                getCode(code);
                btnSave.Text = "EDIT";
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                
                mode = false;
                code = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "Delete from library where BookCode = @BookCode";
                connection.Open();
                cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@BookCode", code);
                cmd.ExecuteNonQuery();
                MessageBox.Show(@"Book Deleted Successfully");   
                connection.Close();
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Load();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBkCode.Clear();
            txtBkAt.Clear();
            txtBkTl.Clear();
            txtBkCg.Text = "";
            btnSave.Text = @"SAVE";
            mode = true;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            string exitMessage = "Are you sure! You want to exit!";
            DialogResult userExit;
            userExit = MessageBox.Show(exitMessage, @"Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (userExit == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}