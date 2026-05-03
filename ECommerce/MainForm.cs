

using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ECommerceApp
{
    public partial class MainForm : Form
    {
        // Per-table column definitions. First column = primary key.
        private static readonly System.Collections.Generic.Dictionary<string, string[]> TableColumns
            = new System.Collections.Generic.Dictionary<string, string[]>
        {
            { "User",          new[] { "User_ID","First_Name","Middle_Name","Last_Name","Username","Password","Email","Phone_Number","Role" } },
            { "Address",       new[] { "Address_ID","User_ID","House_Number","Street","City","Country","Postal_Code" } },
            { "Category",      new[] { "Category_ID","Name","Description" } },
            { "Product",       new[] { "Product_ID","Category_ID","Name","Description","Price","Stock_Quantity" } },
            { "Order",         new[] { "Order_ID","User_ID","Status","Order_Date","Total_Amount" } },
            { "Order_Product", new[] { "Order_ID","Product_ID","Quantity" } },
            { "Shipping",      new[] { "Tracking_Number","Order_ID","Shipment_Date","Delivery_Date","Shipping_Status" } },
            { "Payment",       new[] { "Payment_ID","Order_ID","Payment_Date","Payment_Method","Payment_Amount","Payment_Status" } }
        };

        private static readonly System.Collections.Generic.HashSet<string> NullableColumns
            = new System.Collections.Generic.HashSet<string>
        { "Middle_Name","House_Number","Street","Postal_Code","Description","Shipment_Date","Delivery_Date" };

        // Decimal / Int / DateTime columns for type-safe parameters 
        private enum ColType { String, Int, Decimal, DateTime }
        private static readonly System.Collections.Generic.Dictionary<string, ColType> ColumnTypes
            = new System.Collections.Generic.Dictionary<string, ColType>
        {
            { "Price", ColType.Decimal }, { "Total_Amount", ColType.Decimal }, { "Payment_Amount", ColType.Decimal },
            { "User_ID", ColType.Int }, { "Address_ID", ColType.Int }, { "Category_ID", ColType.Int },
            { "Product_ID", ColType.Int }, { "Order_ID", ColType.Int }, { "Tracking_Number", ColType.Int },
            { "Payment_ID", ColType.Int }, { "Stock_Quantity", ColType.Int }, { "Quantity", ColType.Int },
            { "Order_Date", ColType.DateTime }, { "Shipment_Date", ColType.DateTime },
            { "Delivery_Date", ColType.DateTime }, { "Payment_Date", ColType.DateTime }
        };

        private string currentTable;
        private string[]  currentColumns;
        private TextBox[] currentTextBoxes;

        public MainForm() { InitializeComponent(); }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbTable.Items.Clear();
            cmbTable.Items.AddRange(new string[] {
                "User","Address","Category","Product",
                "Order","Order_Product","Shipping","Payment"
            });
            btnSalesReport.Enabled = (LoginForm.LoggedInRole == "Admin");
            this.Text = "E-Commerce CRUD — Logged in as " + LoginForm.LoggedInRole
                      + " (" + LoginForm.LoggedInName + ")";
        }

        private void cmbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTable.SelectedIndex == -1) return;
            BuildFieldsFor(cmbTable.SelectedItem.ToString());
            btnView_Click(sender, e);
        }

        private void BuildFieldsFor(string table)
        {
            pnlFields.Controls.Clear();
            currentTable = table;
            currentColumns = TableColumns[table];
            currentTextBoxes = new TextBox[currentColumns.Length];
            int y = 10;
            for (int i = 0; i < currentColumns.Length; i++)
            {
                Label lbl = new Label  { Text = currentColumns[i] + ":", Top = y, Left = 10, Width = 130, AutoSize = false };
                TextBox tb = new TextBox { Top = y, Left = 150, Width = 280, Name = "tx_" + currentColumns[i] };
                if(i == 0 && table != "Order_Product")
                {
                    tb.Text = "0";
                    tb.ReadOnly = true;
                }
                pnlFields.Controls.Add(lbl);
                pnlFields.Controls.Add(tb);
                currentTextBoxes[i] = tb;
                y += 30;
            }
        }

        
        private void btnView_Click(object sender, EventArgs e)
        {
            if (cmbTable.SelectedIndex == -1) return;
            string table = cmbTable.SelectedItem.ToString();

            dgvData.DataSource = DBHelper.ExecuteQuery("SELECT * FROM [" + table + "]");
        }

        // Click a grid row -> fill the textboxes
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || currentColumns == null) return;
            DataGridViewRow row = dgvData.Rows[e.RowIndex];
            for (int i = 0; i < currentColumns.Length; i++)
            {
                if (!dgvData.Columns.Contains(currentColumns[i])) continue;
                object val = row.Cells[currentColumns[i]].Value;
                currentTextBoxes[i].Text = (val == null || val == DBNull.Value) ? "" : val.ToString();
            }
        }

        
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (cmbTable.SelectedIndex == -1)
            { MessageBox.Show("Please select a table first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            string table = cmbTable.SelectedItem.ToString();
            SqlParameter[] parameters = BuildAllParameters();
            if (parameters == null) return;

            string[] insertableColumns = currentColumns;
            if (table != "Order_Product") insertableColumns = insertableColumns.Skip(1).ToArray();

            // 3- Prepare command string
            string cols       = string.Join(",", insertableColumns.Select(c => "[" + c + "]"));
            string pars       = string.Join(",", insertableColumns.Select(c => "@" + c));
            string insertString = "INSERT INTO [" + table + "] (" + cols + ") VALUES (" + pars + ")";

            try
            {
                DBHelper.ExecuteNonQuery(insertString, parameters);

                MessageBox.Show("Saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnView_Click(sender, e);
            }
            catch (SqlException ex) { ShowSqlError("Insert", ex, table); }
        }

        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cmbTable.SelectedIndex == -1)
            { MessageBox.Show("Please select a table first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            string table = cmbTable.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(currentTextBoxes[0].Text))
            { MessageBox.Show("Select a row from the grid first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            SqlParameter[] parameters = BuildAllParameters();
            if (parameters == null) return;

            int pkCount = (table == "Order_Product") ? 2 : 1;
            string whereClause = (table == "Order_Product")
                ? "[Order_ID]=@Order_ID AND [Product_ID]=@Product_ID"
                : "[" + currentColumns[0] + "]=@" + currentColumns[0];
            string setClause = string.Join(",", currentColumns.Skip(pkCount).Select(c => "[" + c + "]=@" + c));

            if (string.IsNullOrEmpty(setClause))
            { MessageBox.Show("Nothing to update on this table.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            string updateString = "UPDATE [" + table + "] SET " + setClause + " WHERE " + whereClause;

            try
            {
                int rows = DBHelper.ExecuteNonQuery(updateString, parameters);

                if (rows > 0)
                { MessageBox.Show("Updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); btnView_Click(sender, e); }
                else
                  MessageBox.Show("No row matched that ID.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) { ShowSqlError("Update", ex, table); }
        }

       
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbTable.SelectedIndex == -1)
            { MessageBox.Show("Please select a table first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            string table = cmbTable.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(currentTextBoxes[0].Text))
            { MessageBox.Show("Select a row from the grid first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string deleteString;
            SqlParameter[] parameters;

            if (table == "Order_Product")
            {
                deleteString = "DELETE FROM [Order_Product] WHERE Order_ID=@oid AND Product_ID=@pid";
                parameters = new[]
                {
                    new SqlParameter("@oid", int.Parse(currentTextBoxes[0].Text)),
                    new SqlParameter("@pid", int.Parse(currentTextBoxes[1].Text))
                };
            }
            else
            {
                deleteString = "DELETE FROM [" + table + "] WHERE [" + currentColumns[0] + "]=@id";
                parameters = new[] { new SqlParameter("@id", currentTextBoxes[0].Text) };
            }
            
            try
            {
                int rows = DBHelper.ExecuteNonQuery(deleteString,parameters);

                if (rows > 0)
                { MessageBox.Show("Deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); btnView_Click(sender, e); }
                else
                  MessageBox.Show("No matching row was found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) { ShowSqlError("Delete", ex, table); }
        }

        
        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            if (LoginForm.LoggedInRole != "Admin")
            { MessageBox.Show("Access denied. Admins only.", "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            dgvData.DataSource = DBHelper.ExecuteStoredProcedure("SalesReport");
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            ProductCatalog cat = new ProductCatalog();
            cat.ShowDialog();
            if (cmbTable.SelectedIndex != -1) btnView_Click(sender, e);
        }

        private void btnLogout_Click(object sender, EventArgs e) { this.Close(); }

        // ===================================================================
        // Helpers
        // ===================================================================
        private SqlParameter BuildTypedParameter(string column, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                if (NullableColumns.Contains(column))
                    return new SqlParameter("@" + column, DBNull.Value);
                MessageBox.Show("Field '" + column + "' is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            ColType t;
            if (!ColumnTypes.TryGetValue(column, out t)) t = ColType.String;
            try
            {
                switch (t)
                {
                    case ColType.Int:
                        int iv; if (!int.TryParse(text, out iv)) throw new FormatException("must be a whole number");
                        return new SqlParameter("@" + column, iv);
                    case ColType.Decimal:
                        decimal dv;
                        if (!decimal.TryParse(text, System.Globalization.NumberStyles.Number,
                            System.Globalization.CultureInfo.InvariantCulture, out dv))
                            if (!decimal.TryParse(text, out dv)) throw new FormatException("must be a decimal number");
                        SqlParameter dp = new SqlParameter("@" + column, SqlDbType.Decimal);
                        dp.Precision = 18; dp.Scale = 2; dp.Value = decimal.Round(dv, 2);
                        return dp;
                    case ColType.DateTime:
                        DateTime dt; if (!DateTime.TryParse(text, out dt)) throw new FormatException("must be a valid date");
                        return new SqlParameter("@" + column, dt);
                    default:
                        return new SqlParameter("@" + column, text);
                }
            }
            catch (FormatException fx)
            {
                MessageBox.Show("Field '" + column + "' " + fx.Message + ".", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        private SqlParameter[] BuildAllParameters()
        {
            var parms = new SqlParameter[currentColumns.Length];
            for (int i = 0; i < currentColumns.Length; i++)
            {
                SqlParameter p = BuildTypedParameter(currentColumns[i], currentTextBoxes[i].Text);
                if (p == null) return null;
                parms[i] = p;
            }
            return parms;
        }

        private void ShowSqlError(string operation, SqlException ex, string table)
        {
            string msg;
            switch (ex.Number)
            {
                case 547:
                    msg = ex.Message.Contains("CHECK")
                        ? "A CHECK constraint was violated. Verify: price > 0, stock >= 0, valid status/email/phone."
                        : "Cannot complete " + operation + ": this record is referenced by other tables.\n" +
                          "Financial/audit records use ON DELETE NO ACTION. Delete the related rows first.";
                    break;
                case 2627: case 2601:
                    if (table == "Payment" && ex.Message.Contains("UQ_Payment_Order"))
                        msg = "This Order already has a Payment record (1:1 relationship enforced).";
                    else if (table == "Shipping" && ex.Message.Contains("UQ_Shipping_Order"))
                        msg = "This Order already has a Shipping record (1:1 relationship enforced).";
                    else
                        msg = "Duplicate value: a record with this primary key or unique field already exists.";
                    break;
                default: msg = operation + " failed:\n\n" + ex.Message; break;
            }
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
