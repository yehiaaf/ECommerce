

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

            // ---------- 2) Connected Mode authentication ----------
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(DBHelper.ConnectionString);
            try
            {
                // 2- Open the connection.
                con.Open();

                // 3- Instantiate a new command with a query and connection as parameters
                SqlCommand cmd = new SqlCommand(
                    "SELECT User_ID, [Role] FROM [User] " +
                    "WHERE Username = @u AND [Password] = @p", con);
                cmd.Parameters.Add(new SqlParameter("@u", txtUsername.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@p", txtPassword.Text));

                // 4- Call Execute reader to get query results
                SqlDataReader rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    rdr.Close();
                    MessageBox.Show("Invalid username or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int userId  = (int)rdr["User_ID"];
                string role = (string)rdr["Role"];
                rdr.Close();

                if (role != cmbRole.Text)
                {
                    MessageBox.Show("This account is not registered as a " + cmbRole.Text + ".",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                SqlCommand fnCmd = new SqlCommand(
                    "SELECT dbo.fn_GetUserFullName(@id)", con);
                fnCmd.Parameters.Add(new SqlParameter("@id", userId));
                // 2. Call ExecuteScalar to send command (cast to string because
                //    the return type of ExecuteScalar is type object )
                string fullName = (string)fnCmd.ExecuteScalar();

                LoggedInUserID = userId;
                LoggedInRole   = role;
                LoggedInName   = fullName;

                MessageBox.Show("Welcome, " + fullName + "!", "Login Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                this.Hide();
                if (role == "Admin")
                {
                    AdminDashboard dash = new AdminDashboard();
                    dash.ShowDialog();
                }
                else // Customer
                {
                    ProductCatalog cat = new ProductCatalog();
                    cat.ShowDialog();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 5- Close the connection
                con.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
