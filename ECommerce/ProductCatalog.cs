

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
            dgvProducts.DataSource =
                DBHelper.ExecuteQuery("SELECT Product_ID, [Name] AS Product, Price, Stock_Quantity " +
                "FROM Product ORDER BY Product_ID");
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

            int newOrderId = 0;

            SqlParameter[] sqlparams = new SqlParameter[]
            {
                new SqlParameter("@User_ID",  LoginForm.LoggedInUserID),
                new SqlParameter("@Status",   "Pending")
            };

            try
            {
                newOrderId = (int)DBHelper.ExecuteScalarStoredProcedure("placeOrder", sqlparams);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Order creation failed:\n\n" + ex.Message,
                    "Order Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int linesAdded = 0;
            List<string> failures = new List<string>();

            foreach (CartLine line in cart)
            {

                sqlparams = new SqlParameter[]
                {
                    new SqlParameter("@Order_ID", newOrderId),
                    new SqlParameter("@Product_ID", line.ProductID),
                    new SqlParameter("@Quantity",   line.Quantity)
                };
                try
                {
                    DBHelper.ExecuteStoredProcedureNonQuery("AddProductToOrder", sqlparams);

                    linesAdded++;
                }
                catch (SqlException ex)
                {
                    failures.Add("- " + line.ProductName + ": " + ex.Message);
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
            Form f = new Form
                { Text = "My Orders", Width = 650, Height = 500, StartPosition = FormStartPosition.CenterParent };

            FlowLayoutPanel listPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10)
            };

            f.Controls.Add(listPanel);
            
            void RefreshOrders()
            {
                listPanel.Controls.Clear();

                SqlParameter[] sqlparams = new SqlParameter[] { new SqlParameter("@uid", LoginForm.LoggedInUserID) };

                string query = @"
            SELECT o.Order_ID, o.[Status], o.Order_Date, ISNULL(SUM(op.quantity * p.price), 0) AS Total_Amount
            FROM [Order] o
            LEFT JOIN order_product op ON o.Order_ID = op.order_id
            LEFT JOIN product p ON op.product_id = p.product_id
            WHERE o.User_ID = @uid
            GROUP BY o.Order_ID, o.[Status], o.Order_Date
            ORDER BY o.Order_Date DESC";

                DataTable tbl = DBHelper.ExecuteQuery(query, sqlparams);

                if (tbl.Rows.Count == 0)
                {
                    Label lblEmpty = new Label { Text = "No orders found.", AutoSize = true };
                    listPanel.Controls.Add(lblEmpty);
                    return;
                }

                foreach (DataRow row in tbl.Rows)
                {
                    int orderId = Convert.ToInt32(row["Order_ID"]);
                    decimal total = Convert.ToDecimal(row["Total_Amount"]);
                    string status = row["Status"].ToString();
                    string date = Convert.ToDateTime(row["Order_Date"]).ToShortDateString();

                    Panel rowPanel = new Panel
                    {
                        Width = 580, Height = 60, BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(0, 0, 0, 10)
                    };

                    Label lblInfo = new Label
                    {
                        Text = $"Order #{orderId} | Date: {date} | Status: {status} | Total: {total:C2}",
                        Location = new Point(10, 20), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    };

                    if (status == "Pending")
                    {
                        Button btnPay = new Button
                        {
                            Text = "Pay", Size = new Size(100, 30), Location = new Point(460, 15),
                            BackColor = Color.LightGreen
                        };
                        btnPay.Click += (s, args) =>
                        {
                            PaymentForm payForm = new PaymentForm(orderId, total);
                            payForm.ShowDialog();
                            RefreshOrders();
                        };
                        rowPanel.Controls.Add(btnPay);
                    }

                    rowPanel.Controls.Add(lblInfo);
                    listPanel.Controls.Add(rowPanel);
                }
            }
            RefreshOrders();

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
