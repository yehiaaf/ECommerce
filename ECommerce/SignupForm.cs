

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
            if (string.IsNullOrWhiteSpace(txtFirstName.Text)
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

            string insertString =
                @"INSERT INTO [User]
                    (First_Name, Middle_Name, Last_Name,
                        Username, [Password], Email, Phone_Number, [Role])
                    VALUES
                    (@fn, @mn, @ln, @un, @pw, @em, @ph, 'Customer')";
            SqlParameter[] sqlparams = new SqlParameter[]
            {
                new SqlParameter("@fn", txtFirstName.Text.Trim()),
                new SqlParameter("@mn", string.IsNullOrWhiteSpace(txtMiddleName.Text) ? (object)DBNull.Value
                                                                  : txtMiddleName.Text.Trim()),
                new SqlParameter("@ln", txtLastName.Text.Trim()),
                new SqlParameter("@un", txtUsername.Text.Trim()),
                new SqlParameter("@pw", txtPassword.Text),
                new SqlParameter("@em", txtEmail.Text.Trim()),
                new SqlParameter("@ph", txtPhone.Text.Trim())
            };
            try
            {
                DBHelper.ExecuteNonQuery(insertString, sqlparams);

                MessageBox.Show("Sign-up successful!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("This Username already exists. Please pick another.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}