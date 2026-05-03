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
            // Fetch only the products that belong to THIS order
            SqlParameter[] sqlparams = new SqlParameter[]
            {
                new SqlParameter("@oid", _orderId)
            };

            // Simplified query to get product name, quantity, and subtotal
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
            throw new System.NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}