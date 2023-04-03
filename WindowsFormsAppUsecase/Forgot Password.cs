using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsAppUsecase
{
    public partial class Forgot_Password : Form
    {
        public Forgot_Password()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            var message = "Are you sure! You want to Exit";
            DialogResult userExit;
            userExit = MessageBox.Show(message, @"Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (userExit == DialogResult.Yes) Application.Exit();
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            var mail = txtEmail.Text;
            if (string.IsNullOrEmpty(mail))
            {
                errEmail.ForeColor = Color.Red;
                errEmail.Text = @"Email is required";
                errorProvider1.SetError(txtEmail, "Email is required");
            }
            else
            {
                errEmail.Text = "";
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            var password = txtPass.Text;
            var passwordPattern = "^(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!@#$%^&*()_+])[A-Za-z0-9!@#$%^&*()_+]{8,}$";
            if (Regex.IsMatch(password, passwordPattern))
            {
                errPass.ForeColor = Color.Green;
                errorProvider1.SetError(txtPass, "");
                errorProvider2.SetError(txtPass, "Valid Password");
                errPass.Text = @"Valid Password";
            }
            else
            {
                errPass.ForeColor = Color.Red;
                errPass.Text =
                    @"Password should contain at least one uppercase letter, one special character, one number and minimun length is 8";
                errorProvider1.SetError(txtPass, "Invalid password format");
            }
        }

        private void txtCnf_TextChanged(object sender, EventArgs e)
        {
            var password = txtPass.Text;
            var cnf = txtCnf.Text;
            if (password != cnf)
            {
                errCnf.ForeColor = Color.Red;
                errCnf.Text = @"Password not matched";
                errorProvider1.SetError(txtCnf, "Password not matched");
                errorProvider2.SetError(txtCnf, "");
            }
            else
            {
                errCnf.ForeColor = Color.Green;
                errCnf.Text = @"Password matched";
                errorProvider1.SetError(txtCnf, "");
                errorProvider2.SetError(txtCnf, "Password matched");
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            UserCheck();
            var pattern = @"^[a-zA-Z0-9.]+@gmail\.com$";
            if (Regex.IsMatch(txtEmail.Text, pattern))
            {
                errEmail.ForeColor = Color.Green;
                //errEmail.Text = @"Valid Email";
                errorProvider1.SetError(txtEmail, "");
                //errorProvider2.SetError(txtEmail, "Valid Email");
            }
            else
            {
                errEmail.ForeColor = Color.Red;
                errEmail.Text = @"Invalid Email format! Email should ends with @gmail.com";
                errorProvider1.SetError(txtEmail, "Please provide valid Email");
            }
        }

        private void txtPass_Validating(object sender, CancelEventArgs e)
        {
            var password = txtPass.Text;
            if (string.IsNullOrEmpty(password))
            {
                errPass.ForeColor = Color.Red;
                errPass.Text = @"Password is required";
                errorProvider1.SetError(txtPass, "Password is required");
            }
            else
            {
                errPass.Text = "";
                errorProvider1.SetError(txtPass, "");
            }
        }

        private void txtCnf_Validating(object sender, CancelEventArgs e)
        {
            var cnf = txtCnf.Text;
            if (string.IsNullOrEmpty(cnf))
            {
                errCnf.ForeColor = Color.Red;
                errCnf.Text = @"Confirm Password is required";
                errorProvider1.SetError(txtCnf, "Confirm Password is required");
            }
            else
            {
                errCnf.Text = "";
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void UserCheck()
        {
            var connectionstring = "server = localhost;uid=root;pwd=Yuvi@12345;database=ado";
            var connection = new MySqlConnection(connectionstring);
            try
            {
                connection.Open();
                var query = "Select count(*) from user where EmailAddress = @EmailAddress";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text);
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                {
                    emailCheck.ForeColor = Color.Red;
                    emailCheck.Text = @"Email not registered";
                    errEmail.Text = "";
                    errorProvider1.SetError(txtEmail, "Email not registered");
                    errorProvider2.SetError(txtEmail, "");
                }
                else
                {
                    errEmail.Text = "";
                    errorProvider2.SetError(txtEmail, "");
                    emailCheck.Text = "";
                    errorProvider1.SetError(txtEmail, "");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPass.Text) ||
                string.IsNullOrEmpty(txtCnf.Text))
            {
                if (txtPass.Text != txtCnf.Text) MessageBox.Show(@"Password not matched! Please try again");

                MessageBox.Show(@"Please fill in all fields to proceed further");
            }
            else
            {
                var connectionstring = "server=localhost;uid=root;pwd=Yuvi@12345;database=ado";
                var connection = new MySqlConnection(connectionstring);
                try
                {
                    connection.Open();
                    var query =
                        "update user set Password=@Password, ConfirmPassword=@ConfirmPassword where EmailAddress = @EmailAddress";
                    var cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPass.Text);
                    cmd.Parameters.AddWithValue("@ConfirmPassword", txtCnf.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(@"Password updated successfully...");
                    txtEmail.Text = "";
                    txtPass.Text = "";
                    txtCnf.Text = "";

                    Hide();
                    var loginPage = new LoginPage();
                    loginPage.Show();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}