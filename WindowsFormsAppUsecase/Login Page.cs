﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsAppUsecase
{
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show(@"Please fill in all details to proceed...");
            }
            else
            {
                var conn = new MySqlConnection("server=localhost;uid=root;pwd=Yuvi@12345;database=ado");
                var cmd = new MySqlCommand(
                    "Select count(*) from user where emailaddress=@EmailAddress and password=@Password",
                    conn);
                cmd.Parameters.AddWithValue("@EmailAddress", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Password", txtPass.Text);
                try
                {
                    conn.Open();
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 1)
                    {
                        MessageBox.Show(@"Login Successful...");
                        conn.Close();
                        txtEmail.Clear();
                        txtPass.Clear();
                        Hide();
                        var libraryHomeScreen = new LibraryHomeScreen();
                        libraryHomeScreen.Show();
                    }
                    else
                    {
                        MessageBox.Show(@"Invalid Login details! Please try again");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var exitMessage = "Are you sure! You want to exit!";
            DialogResult userExit;
            userExit = MessageBox.Show(exitMessage, @"Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (userExit == DialogResult.Yes) Application.Exit();
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            var mail = txtEmail.Text;
            if (string.IsNullOrEmpty(mail))
            {
                errEmail.ForeColor = Color.Red;
                errEmail.Text = @"Email is required";
                errorProvider1.SetError(txtEmail, "Please enter Email");
            }
            else
            {
                errEmail.Text = "";
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void txtPass_Validating(object sender, CancelEventArgs e)
        {
            var password = txtPass.Text;
            if (string.IsNullOrEmpty(password))
            {
                errPass.ForeColor = Color.Red;
                errPass.Text = @"Password is required";
                errorProvider1.SetError(txtPass, "Please enter password");
            }
            else
            {
                errPass.Text = "";
                errorProvider1.SetError(txtPass, "");
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
                    errUserCheck.ForeColor = Color.Red;
                    errUserCheck.Text = @"Email not registered";
                    errEmail.Text = "";
                    errorProvider1.SetError(txtEmail, "Email not registered");
                    errorProvider2.SetError(txtEmail, "");
                }
                else
                {
                    errEmail.Text = "";
                    errorProvider2.SetError(txtEmail, "");
                    errUserCheck.Text = "";
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

        private void label4_Click(object sender, EventArgs e)
        {
            var forgotPassword = new Forgot_Password();
            Hide();
            forgotPassword.Show();
        }
    }
}