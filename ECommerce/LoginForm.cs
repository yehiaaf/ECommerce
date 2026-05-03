

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ECommerceApp
{
    public partial class LoginForm : Form
    {
        // Static "session" so any form can read who is logged in.
        public static int    LoggedInUserID = 0;
        public static string LoggedInRole   = "";
        public static string LoggedInName   = "";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Clear();
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Customer");
            cmbRole.SelectedIndex = -1;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // ---------- 1) GUI Validation ----------
            if (cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a role.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SqlParameter[] sqlparams = new SqlParameter[] {
                new SqlParameter("@u", txtUsername.Text.Trim()),
                new SqlParameter("@p", txtPassword.Text)};
            DataTable dataTable = DBHelper.ExecuteQuery("SELECT User_ID, [Role] FROM [User] WHERE Username = @u AND [Password] = @p", sqlparams);

            if(dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Invalid username or password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int userId = (int)dataTable.Rows[0]["User_ID"];
            string role = (string)dataTable.Rows[0]["Role"];

            if (role != cmbRole.Text)
            {
                MessageBox.Show("This account is not registered as a " + cmbRole.Text + ".",
                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sqlparams = new SqlParameter[] {
                new SqlParameter("@id", userId)};
            string fullName = (string)DBHelper.ExecuteScalar("SELECT dbo.GetUserFullName(@id)", sqlparams);

            LoggedInUserID = userId;
            LoggedInRole = role;
            LoggedInName = fullName;

            MessageBox.Show("Welcome, " + fullName + "!", "Login Successful",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Hide();
            if (role == "Admin")
            {
                AdminDashboard dash = new AdminDashboard();
                dash.ShowDialog();
            }
            else
            {
                ProductCatalog cat = new ProductCatalog();
                cat.ShowDialog();
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
