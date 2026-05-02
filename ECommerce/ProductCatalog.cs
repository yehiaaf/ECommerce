

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ECommerceApp
{
    public partial class ProductCatalog : Form
    {
        // Simple cart line holder
        private class CartLine
        {
            public int     ProductID;
            public string  ProductName;
            public decimal Price;        
            public int     Quantity;
            public decimal Subtotal { get { return Price * Quantity; } }
        }

        private readonly List<CartLine> cart = new List<CartLine>();

        public ProductCatalog() { InitializeComponent(); }

        private void ProductCatalog_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome, " + LoginForm.LoggedInName + "    |    Role: Customer";
            LoadProducts();
            RefreshCartGrid();
        }

        
        private void LoadProducts()
        {
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(DBHelper.ConnectionString);

            // 2- Open the connection.
            con.Open();

            // 3- Instantiate a new command with a query and connection as parameters
            SqlCommand cmd = new SqlCommand(
                "SELECT Product_ID, [Name] AS Product, Price, Stock_Quantity " +
                "FROM Product ORDER BY Product_ID", con);
            cmd.CommandType = CommandType.Text;

            // 4- Call Execute reader to get query results
            SqlDataReader reader = cmd.ExecuteReader();

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

            dgvProducts.DataSource = tbl;
        }

        
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            { MessageBox.Show("Pick a product from the table first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            int qty;
            if (!int.TryParse(txtQty.Text, out qty) || qty <= 0)
            { MessageBox.Show("Quantity must be a positive whole number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            int productId     = Convert.ToInt32(dgvProducts.CurrentRow.Cells["Product_ID"].Value);
            string prodName   = dgvProducts.CurrentRow.Cells["Product"].Value.ToString();
            decimal price     = Convert.ToDecimal(dgvProducts.CurrentRow.Cells["Price"].Value);
            int stockNow      = Convert.ToInt32(dgvProducts.CurrentRow.Cells["Stock_Quantity"].Value);

            if (qty > stockNow)
            { MessageBox.Show("Only " + stockNow + " in stock. Lower the quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            CartLine existing = cart.Find(c => c.ProductID == productId);
            if (existing != null) existing.Quantity += qty;
            else cart.Add(new CartLine { ProductID = productId, ProductName = prodName, Price = price, Quantity = qty });

            RefreshCartGrid();
            txtQty.Text = "";
        }

        private void btnRemoveFromCart_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow == null)
            { MessageBox.Show("Pick a cart line to remove.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            int idx = dgvCart.CurrentRow.Index;
            if (idx >= 0 && idx < cart.Count) { cart.RemoveAt(idx); RefreshCartGrid(); }
        }

        private void RefreshCartGrid()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Product_ID", typeof(int));
            tbl.Columns.Add("Product",    typeof(string));
            tbl.Columns.Add("Price",      typeof(decimal));
            tbl.Columns.Add("Quantity",   typeof(int));
            tbl.Columns.Add("Subtotal",   typeof(decimal));

            decimal total = 0m;
            foreach (CartLine line in cart)
            {
                tbl.Rows.Add(line.ProductID, line.ProductName, line.Price, line.Quantity, line.Subtotal);
                total += line.Subtotal;
            }
            dgvCart.DataSource    = tbl;
            lblTotal.Text = "Cart Total:  " + total.ToString("F2") + " EGP";
        }

        
        
        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            { MessageBox.Show("Your cart is empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            
            int newOrderId;

           
            SqlConnection conId = new SqlConnection(DBHelper.ConnectionString);
            try
            {
                // 2- Open the connection.
                conId.Open();

                // 1. Instantiate a new command
                SqlCommand cmdId = new SqlCommand("SELECT ISNULL(MAX(Order_ID),0)+1 FROM [Order]", conId);

                // 2. Call ExecuteScalar to send command
                
                newOrderId = (int)cmdId.ExecuteScalar();
            }
            finally
            {
                // 3- Close Connection
                conId.Close();
            }

         

            // 1- Instantiate the SqlConnection
            SqlConnection con1 = new SqlConnection(DBHelper.ConnectionString);
            try
            {
                // 2- Open the connection.
                con1.Open();

                // 1. create a command object identifying the stored procedure
                SqlCommand cmd1 = new SqlCommand("sp_PlaceOrder", con1);

                // 2. set the command object so it knows to execute a stored procedure
                cmd1.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd1.Parameters.Add(new SqlParameter("@Order_ID", newOrderId));
                cmd1.Parameters.Add(new SqlParameter("@User_ID",  LoginForm.LoggedInUserID));
                cmd1.Parameters.Add(new SqlParameter("@Status",   "Pending"));

                // 4. then you can execute the sp
                cmd1.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // RAISERROR from the SP surfaces here as a SqlException
                MessageBox.Show("Order creation failed:\n\n" + ex.Message,
                    "Order Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                // 5- Close Connection
                con1.Close();
            }

           
            int linesAdded = 0;
            List<string> failures = new List<string>();

            foreach (CartLine line in cart)
            {
                // 1- Instantiate the SqlConnection
                SqlConnection con2 = new SqlConnection(DBHelper.ConnectionString);
                try
                {
                    // 2- Open the connection.
                    con2.Open();

                    // 1. create a command object identifying the stored procedure
                    SqlCommand cmd2 = new SqlCommand("sp_AddProductToOrder", con2);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd2.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd2.Parameters.Add(new SqlParameter("@Order_ID",   newOrderId));
                    cmd2.Parameters.Add(new SqlParameter("@Product_ID", line.ProductID));
                    cmd2.Parameters.Add(new SqlParameter("@Quantity",   line.Quantity));

                    // 4. then you can execute the sp
                    cmd2.ExecuteNonQuery();

                    linesAdded++;
                }
                catch (SqlException ex)
                {
                    // The SP RAISERRORs for: "Insufficient stock", "Product does
                    // not exist", "Quantity must be greater than zero".
                    failures.Add("- " + line.ProductName + ": " + ex.Message);
                }
                finally
                {
                    // 5- Close Connection
                    con2.Close();
                }
            }

            // --- Feedback ---
            if (failures.Count == 0)
            {
                MessageBox.Show(
                    "Order #" + newOrderId + " placed successfully!\n" +
                    linesAdded + " product line(s) added.\n\n" +
                    "Stock was validated, decremented, and the order total\n" +
                    "was recomputed atomically by the stored procedure.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Order #" + newOrderId + " created with issues:\n\n" +
                    string.Join("\n", failures) + "\n\nLines succeeded: " + linesAdded,
                    "Partial Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            cart.Clear();
            RefreshCartGrid();
            LoadProducts();   
        }

        
        private void btnMyOrders_Click(object sender, EventArgs e)
        {
            // 1- Instantiate the SqlConnection
            SqlConnection con = new SqlConnection(DBHelper.ConnectionString);

            // 2- Open the connection.
            con.Open();

            // 3- Instantiate a new command with a query and connection as parameters
            SqlCommand cmd = new SqlCommand(
                "SELECT Order_ID, [Status], Order_Date, Total_Amount " +
                "FROM [Order] WHERE User_ID = @uid ORDER BY Order_Date DESC", con);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@uid", LoginForm.LoggedInUserID));

            // 4- Call Execute reader to get query results
            SqlDataReader reader = cmd.ExecuteReader();

            // To use the values from reader, we create DataTable
            DataTable tbl = new DataTable();
            for (int i = 0; i < reader.FieldCount; i++)
                tbl.Columns.Add(reader.GetName(i), reader.GetFieldType(i));

            // To add a row to the table we use DataRow object
            DataRow row;
            try
            {
                while (reader.Read())
                {
                    row = tbl.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader[i];
                    tbl.Rows.Add(row);
                }
            }
            finally
            {
                // 5- Close the reader and the connection
                reader.Close();
                con.Close();
            }

            if (tbl.Rows.Count == 0)
            { MessageBox.Show("You haven't placed any orders yet.", "My Orders", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            Form f = new Form { Text = "My Orders", Width = 600, Height = 400, StartPosition = FormStartPosition.CenterParent };
            DataGridView g = new DataGridView
            {
                Dock = DockStyle.Fill, DataSource = tbl,
                ReadOnly = true, AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            f.Controls.Add(g);
            f.ShowDialog();
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
