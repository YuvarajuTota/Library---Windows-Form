using System;
using System.Windows.Forms;

namespace WindowsFormsAppUsecase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var registrationpage = new UserRegistration();
            Hide();
            registrationpage.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var loginPage1 = new LoginPage();
            Hide();
            loginPage1.Show();
        }
    }
}