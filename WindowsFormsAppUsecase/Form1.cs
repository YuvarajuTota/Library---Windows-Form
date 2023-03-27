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
            UserRegistration registrationpage = new UserRegistration();
            this.Hide();
            registrationpage.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginPage loginPage1 = new LoginPage();
            this.Hide();
            loginPage1.Show();
        }

        
    }
}