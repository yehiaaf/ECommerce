using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ECommerceApp
{
    public partial class PaymentForm : Form
    {
        private int _orderId;
        private decimal _totalAmount;
        
        public PaymentForm(int orderid, decimal total)
        {
            InitializeComponent();
            
            this._orderId = orderid;
            this._totalAmount = total;
            
            lblTotal.Text = $"Cart Total: {_totalAmount} EGP";
            this.Text = $"Payment for Order #{_orderId}";
        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            
            SqlParameter[] sqlparams = new SqlParameter[]
            {
                new SqlParameter("@oid", _orderId)
            };

            
            string query = @"
            SELECT p.name AS [Product], op.quantity AS [Qty], p.price AS [Unit Price], 
            (op.quantity * p.price) AS [Subtotal]
            FROM order_product op
            JOIN product p ON op.product_id = p.product_id
            WHERE op.order_id = @oid";

            DataTable dt = DBHelper.ExecuteQuery(query, sqlparams);
            dgvCart.DataSource = dt;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method before proceeding.", 
                    "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedMethod = comboBox1.SelectedItem.ToString();

            try
            {
                SqlParameter[] p = new SqlParameter[]
                {
                    new SqlParameter("@oid", _orderId),
                    new SqlParameter("@status", "Processing")
                };

                
                string sql = "UPDATE [Order] SET [Status] = @status WHERE Order_ID = @oid";

                
                DBHelper.ExecuteNonQuery(sql, p); 
                
                SqlParameter[] paymentParams = new SqlParameter[]
                {
                    new SqlParameter("@oid", _orderId),
                    new SqlParameter("@amount", _totalAmount),
                    new SqlParameter("@method", selectedMethod),
                    new SqlParameter("@status", "paid"),
                    new SqlParameter("@date", DateTime.Now)
                };
                
                string insertPaymentSql = @"
            INSERT INTO payment (order_id, payment_amount, payment_method, payment_status, payment_date) 
            VALUES (@oid, @amount, @method, @status, @date)";

                DBHelper.ExecuteQuery(insertPaymentSql, paymentParams);

                MessageBox.Show("Payment recorded and order is now processing!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the order: " + ex.Message, 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}