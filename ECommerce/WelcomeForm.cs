using System;
using System.Windows.Forms;

namespace ECommerceApp
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            this.Hide();
            login.ShowDialog();
            this.Show();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            SignupForm signup = new SignupForm();
            this.Hide();
            signup.ShowDialog();
            this.Show();
        }
    }
}
