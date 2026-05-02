

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
          
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(DBHelper.ConnectionString);
            try
            {
                // 2- Open the connection.
                con.Open();

                // 1. Instantiate a new command
                SqlCommand cmd = new SqlCommand("SELECT dbo.fn_GetUserFullName(@id)", con);
                cmd.Parameters.Add(new SqlParameter("@id", LoginForm.LoggedInUserID));

                // 2. Call ExecuteScalar to send command
                // (return type is object, so we cast — Lab 9 page 10)
                object result = cmd.ExecuteScalar();
                string fullName = (result == null) ? "Admin" : (string)result;

                lblWelcome.Text = "Welcome, " + fullName + "    |    Role: Admin";
            }
            catch
            {
                lblWelcome.Text = "Welcome, Admin";
            }
            finally
            {
                // 3- Close Connection
                con.Close();
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

            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(DBHelper.ConnectionString);

            // 2- Open the connection.
            con.Open();

            // 1. create a command object identifying the stored procedure
            SqlCommand cmd = new SqlCommand("sp_SalesReport", con);

            // 2. set the command object so it knows to execute a stored procedure
            cmd.CommandType = CommandType.StoredProcedure;

            // 3. add parameter to command, which will be passed to the stored procedure
            cmd.Parameters.Add(new SqlParameter("@StartDate", dtpStart.Value));
            cmd.Parameters.Add(new SqlParameter("@EndDate",   dtpEnd.Value));

            // 4. then you can execute the sp
            SqlDataReader reader = cmd.ExecuteReader();

            // Build DataTable — Lab 8 GetPatients() pattern (page 18)
            // To use the values from reader, we create DataTable
            DataTable tbl = new DataTable();

            // Add columns to the table, according to the columns in the reader
            for (int i = 0; i < reader.FieldCount; i++)
                tbl.Columns.Add(reader.GetName(i), reader.GetFieldType(i));

            // To add a row to the table we use DataRow object
            DataRow row;
            try
            {
                while (reader.Read())
                {
                    // To ensure that the row has the same columns in the table, we use NewRow()
                    row = tbl.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader[i];
                    // Finally we add the row to the table
                    tbl.Rows.Add(row);
                }
            }
            finally
            {
                // 5- Close the reader and the connection
                reader.Close();
                con.Close();
            }

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
