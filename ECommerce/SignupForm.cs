

using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ECommerceApp
{
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, EventArgs e)
        {
            // ---------- 1) Validation (matches every CHECK constraint in SQL) ----------
            if (string.IsNullOrWhiteSpace(txtUserId.Text)
             || string.IsNullOrWhiteSpace(txtFirstName.Text)
             || string.IsNullOrWhiteSpace(txtLastName.Text)
             || string.IsNullOrWhiteSpace(txtUsername.Text)
             || string.IsNullOrWhiteSpace(txtPassword.Text)
             || string.IsNullOrWhiteSpace(txtEmail.Text)
             || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("All fields are required (Middle Name is optional).",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId;
            if (!int.TryParse(txtUserId.Text, out userId))
            {
                MessageBox.Show("User ID must be a number.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (!Regex.IsMatch(txtPhone.Text, @"^01[0125]\d{8}$"))
            {
                MessageBox.Show(
                    "Invalid Egyptian phone number.\n\n" +
                    "Must be 11 digits starting with:\n" +
                    "  010xxxxxxxx  (Vodafone)\n" +
                    "  011xxxxxxxx  (Etisalat)\n" +
                    "  012xxxxxxxx  (Orange)\n" +
                    "  015xxxxxxxx  (WE)",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            SqlConnection con = new SqlConnection(DBHelper.ConnectionString);
            try
            {
                // 2- Open the connection.
                con.Open();

                // 3- Prepare command string
                string insertString =
                    @"INSERT INTO [User]
                        (User_ID, First_Name, Middle_Name, Last_Name,
                         Username, [Password], Email, Phone_Number, [Role])
                      VALUES
                        (@id, @fn, @mn, @ln, @un, @pw, @em, @ph, 'Customer')";

                // 4- Instantiate a new command with a query and connection as parameters
                SqlCommand cmd = new SqlCommand(insertString, con);

                // 5- Set Parameters
                cmd.Parameters.Add(new SqlParameter("@id", userId));
                cmd.Parameters.Add(new SqlParameter("@fn", txtFirstName.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@mn",
                    string.IsNullOrWhiteSpace(txtMiddleName.Text) ? (object)DBNull.Value
                                                                  : txtMiddleName.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@ln", txtLastName.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@un", txtUsername.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@pw", txtPassword.Text));
                cmd.Parameters.Add(new SqlParameter("@em", txtEmail.Text.Trim()));
                cmd.Parameters.Add(new SqlParameter("@ph", txtPhone.Text.Trim()));

                // 6- Call ExecuteNonQuery to execute insert stmt at server.
                cmd.ExecuteNonQuery();

                MessageBox.Show("Sign-up successful! Your User ID is: " + userId,
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("This Username or User ID already exists. Please pick another.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                // 7- Close Connection
                con.Close();
            }
        }
    }
}