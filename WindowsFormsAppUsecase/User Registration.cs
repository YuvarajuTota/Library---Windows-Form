using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsAppUsecase
{
    public partial class UserRegistration : Form
    {
        private TextBox txtEmailAddress;

        public UserRegistration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) ||
                string.IsNullOrEmpty(txtEmailAddress.Text) || string.IsNullOrEmpty(txtPassword.Text) ||
                string.IsNullOrEmpty(txtCnf.Text))
            {
                if (txtPassword.Text != txtCnf.Text) MessageBox.Show(@"Password didn't matched! Please try again...");
                MessageBox.Show(@"Please fill in all fields to submit the form");
            }
            else
            {
                MessageBox.Show(@"Registration Successful...");
                var connectionString = "server=localhost;uid=root;pwd=Yuvi@12345;database=ado";
                var connection = new MySqlConnection(connectionString);
                try
                {
                    connection.Open();
                    var sql =
                        "INSERT INTO user (FirstName, LastName, EmailAddress, Password, ConfirmPassword) VALUES (@FirstName, @LastName, @EmailAddress, @Password, @ConfirmPassword)";
                    var cmd = new MySqlCommand(sql, connection);
                    // Set parameters
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@EmailAddress", txtEmailAddress.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@ConfirmPassword", txtCnf.Text);
                    var reader = cmd.ExecuteReader();
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtEmailAddress.Text = "";
                    txtPassword.Text = "";
                    txtCnf.Text = "";
                    var loginPage = new LoginPage();
                    Hide();
                    loginPage.Show();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            var loginPage = new LoginPage();
            Hide();
            loginPage.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var exitMessage = "Are you sure! You want to exit. Your data will be lost...";
            DialogResult userExit;
            userExit = MessageBox.Show(exitMessage, @"Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (userExit == DialogResult.Yes) Application.Exit();
        }

        private void txtEmailAddress_TextChanged(object sender, EventArgs e)
        {
            var pattern = @"^[a-zA-Z0-9.]+@gmail\.com$";
            if (Regex.IsMatch(txtEmailAddress.Text, pattern))
            {
                errEmail.ForeColor = Color.Green;
                errEmail.Text = @"Valid Email";
                errorProvider1.SetError(txtEmailAddress, "");
                errorProvider2.SetError(txtEmailAddress, "Valid Email");
            }
            else
            {
                errEmail.ForeColor = Color.Red;
                errEmail.Text = @"Invalid Email format! Email should ends with @gmail.com";
                errorProvider1.SetError(txtEmailAddress, "Please provide valid Email");
            }

            var query = "Select EmailAddress from user where EmailAddress = @EmailAddress";
            var connectionString = "server=localhost;uid=root;pwd=Yuvi@12345;database=ado";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@EmailAddress", txtEmailAddress.Text);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userEmail.ForeColor = Color.Red;
                            userEmail.Text = @"Email already registered";
                            errorProvider1.SetError(txtEmailAddress, "Please use new email address");
                            errorProvider2.SetError(txtEmailAddress, "");
                            errEmail.Text = "";
                        }
                        else
                        {
                            userEmail.Text = "";
                            errorProvider1.SetError(txtEmailAddress, "");
                        }
                    }
                }

                connection.Close();
            }
        }

        private void txtCnf_TextChanged(object sender, EventArgs e)
        {
            var password = txtPassword.Text;
            var cnf = txtCnf.Text;
            if (password == cnf)
            {
                errCnf.ForeColor = Color.Green;
                errCnf.Text = @"Password Matched";
                errorProvider1.SetError(txtCnf, "");
                errorProvider2.SetError(txtCnf, "Password Matched");
            }
            else
            {
                errCnf.ForeColor = Color.Red;
                errCnf.Text = @"Password not matched";
                errorProvider1.SetError(txtCnf, "Please enter the matching password");
                errorProvider2.SetError(txtCnf, "");
            }
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            var firstName = txtFirstName.Text;
            if (string.IsNullOrEmpty(firstName))
            {
                errFN.ForeColor = Color.Red;
                errFN.Text = @"First Name is required";
                errorProvider1.SetError(txtFirstName, "Please enter First Name");
            }
            else
            {
                errFN.Text = "";
                errorProvider1.SetError(txtFirstName, "");
            }
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            var lastName = txtLastName.Text;
            if (string.IsNullOrEmpty(lastName))
            {
                errLN.ForeColor = Color.Red;
                errLN.Text = @"Last Name is required";
                errorProvider1.SetError(txtLastName, "Please enter Last Name");
            }
            else
            {
                errLN.Text = "";
                errorProvider1.SetError(txtLastName, "");
            }
        }

        private void txtEmailAddress_Validating(object sender, CancelEventArgs e)
        {
            var email = txtEmailAddress.Text;
            if (string.IsNullOrEmpty(email))
            {
                errEmail.ForeColor = Color.Red;
                errEmail.Text = @"Email is required";
                errorProvider1.SetError(txtEmailAddress, "Please enter Email");
            }
            else
            {
                errEmail.Text = "";
                errorProvider1.SetError(txtEmailAddress, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            var password = txtPassword.Text;
            if (string.IsNullOrEmpty(password))
            {
                errPassword.ForeColor = Color.Red;
                errPassword.Text = @"Password is required";
                errorProvider1.SetError(txtPassword, "Please enter Password");
            }
            else
            {
                errPassword.Text = "";
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void txtCnf_Validating(object sender, CancelEventArgs e)
        {
            var cnf = txtCnf.Text;
            if (string.IsNullOrEmpty(cnf))
            {
                errCnf.ForeColor = Color.Red;
                errCnf.Text = @"Confirm Password is required";
                errorProvider1.SetError(txtCnf, "Please enter Confirm Password");
            }
            else
            {
                errCnf.Text = "";
                errorProvider1.SetError(txtCnf, "");
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            var password = txtPassword.Text;
            var passwordPattern = "^(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!@#$%^&*()_+])[A-Za-z0-9!@#$%^&*()_+]{8,}$";
            if (Regex.IsMatch(password, passwordPattern))
            {
                errPassword.ForeColor = Color.Green;
                errorProvider1.SetError(txtPassword, "");
                errorProvider2.SetError(txtPassword, "Valid Password");
                errPassword.Text = @"Valid Password";
            }
            else
            {
                errPassword.ForeColor = Color.Red;
                errPassword.Text =
                    @"Password should contain at least one uppercase letter, one special character, one number and minimun length is 8";
                errorProvider1.SetError(txtPassword, "Invalid password format");
            }
        }
    }
}