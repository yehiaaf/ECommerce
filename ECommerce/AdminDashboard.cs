

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ECommerceApp
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard() { InitializeComponent(); }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {
          
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter("@id", LoginForm.LoggedInUserID)
                };

                object result = DBHelper.ExecuteScalar("SELECT dbo.fn_GetUserFullName(@id)", sqlparams);
                string fullName = (result == null) ? "Admin" : (string)result;

                lblWelcome.Text = "Welcome, " + fullName + "    |    Role: Admin";
            }
            catch
            {
                lblWelcome.Text = "Welcome, Admin";
            }

            // Default date range: last 30 days
            dtpStart.Value = DateTime.Now.AddDays(-30);
            dtpEnd.Value   = DateTime.Now;
        }

        
        private void btnRunReport_Click(object sender, EventArgs e)
        {
            if (dtpStart.Value > dtpEnd.Value)
            {
                MessageBox.Show("Start date cannot be after end date.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlParameter[] sqlparams = new SqlParameter[]
            {
                new SqlParameter("@StartDate", dtpStart.Value),
                new SqlParameter("@EndDate",   dtpEnd.Value)
            };

            DataTable tbl = DBHelper.ExecuteStoredProcedure("SalesReport", sqlparams);

            dgvReport.DataSource = tbl;

            if (tbl.Rows.Count == 0)
                MessageBox.Show("No sales found in the selected date range.",
                    "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Sales report loaded: " + tbl.Rows.Count + " category row(s).",
                    "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnManageTables_Click(object sender, EventArgs e)
        {
            MainForm main = new MainForm();
            main.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm.LoggedInUserID = 0;
            LoginForm.LoggedInRole   = "";
            LoginForm.LoggedInName   = "";
            this.Close();
        }
    }
}
