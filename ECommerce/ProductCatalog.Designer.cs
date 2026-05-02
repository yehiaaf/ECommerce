namespace ECommerceApp
{
    partial class ProductCatalog
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblProductsHeader;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnAddToCart;
        private System.Windows.Forms.Label lblCartHeader;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.Button btnRemoveFromCart;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnPlaceOrder;
        private System.Windows.Forms.Button btnMyOrders;
        private System.Windows.Forms.Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblProductsHeader = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.lblQty = new System.Windows.Forms.Label();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.btnAddToCart = new System.Windows.Forms.Button();
            this.lblCartHeader = new System.Windows.Forms.Label();
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.btnRemoveFromCart = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnPlaceOrder = new System.Windows.Forms.Button();
            this.btnMyOrders = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).BeginInit();
            this.SuspendLayout();
            //
            // lblWelcome
            //
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(20, 12);
            this.lblWelcome.Text = "Welcome";
            //
            // lblProductsHeader
            //
            this.lblProductsHeader.AutoSize = true;
            this.lblProductsHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblProductsHeader.Location = new System.Drawing.Point(20, 50);
            this.lblProductsHeader.Text = "Products";
            //
            // dgvProducts
            //
            this.dgvProducts.Location = new System.Drawing.Point(20, 80);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.Size = new System.Drawing.Size(450, 360);
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            //
            // lblQty
            //
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new System.Drawing.Point(20, 460);
            this.lblQty.Text = "Quantity:";
            //
            // txtQty
            //
            this.txtQty.Location = new System.Drawing.Point(90, 457);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(80, 23);
            //
            // btnAddToCart
            //
            this.btnAddToCart.Location = new System.Drawing.Point(190, 454);
            this.btnAddToCart.Name = "btnAddToCart";
            this.btnAddToCart.Size = new System.Drawing.Size(140, 30);
            this.btnAddToCart.Text = "Add to Cart";
            this.btnAddToCart.UseVisualStyleBackColor = true;
            this.btnAddToCart.Click += new System.EventHandler(this.btnAddToCart_Click);
            //
            // lblCartHeader
            //
            this.lblCartHeader.AutoSize = true;
            this.lblCartHeader.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCartHeader.Location = new System.Drawing.Point(490, 50);
            this.lblCartHeader.Text = "My Cart";
            //
            // dgvCart
            //
            this.dgvCart.Location = new System.Drawing.Point(490, 80);
            this.dgvCart.Name = "dgvCart";
            this.dgvCart.Size = new System.Drawing.Size(440, 320);
            this.dgvCart.ReadOnly = true;
            this.dgvCart.AllowUserToAddRows = false;
            this.dgvCart.AllowUserToDeleteRows = false;
            this.dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            //
            // btnRemoveFromCart
            //
            this.btnRemoveFromCart.Location = new System.Drawing.Point(490, 410);
            this.btnRemoveFromCart.Name = "btnRemoveFromCart";
            this.btnRemoveFromCart.Size = new System.Drawing.Size(140, 30);
            this.btnRemoveFromCart.Text = "Remove Selected";
            this.btnRemoveFromCart.UseVisualStyleBackColor = true;
            this.btnRemoveFromCart.Click += new System.EventHandler(this.btnRemoveFromCart_Click);
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(700, 415);
            this.lblTotal.Text = "Cart Total:  0.00 EGP";
            //
            // btnPlaceOrder
            //
            this.btnPlaceOrder.Location = new System.Drawing.Point(720, 454);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new System.Drawing.Size(210, 36);
            this.btnPlaceOrder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPlaceOrder.Text = "Place Order";
            this.btnPlaceOrder.UseVisualStyleBackColor = true;
            this.btnPlaceOrder.Click += new System.EventHandler(this.btnPlaceOrder_Click);
            //
            // btnMyOrders
            //
            this.btnMyOrders.Location = new System.Drawing.Point(20, 510);
            this.btnMyOrders.Name = "btnMyOrders";
            this.btnMyOrders.Size = new System.Drawing.Size(140, 32);
            this.btnMyOrders.Text = "My Orders";
            this.btnMyOrders.UseVisualStyleBackColor = true;
            this.btnMyOrders.Click += new System.EventHandler(this.btnMyOrders_Click);
            //
            // btnLogout
            //
            this.btnLogout.Location = new System.Drawing.Point(810, 510);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(120, 32);
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            //
            // ProductCatalog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(220, 230, 245);
            this.ClientSize = new System.Drawing.Size(950, 565);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnMyOrders);
            this.Controls.Add(this.btnPlaceOrder);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnRemoveFromCart);
            this.Controls.Add(this.dgvCart);
            this.Controls.Add(this.lblCartHeader);
            this.Controls.Add(this.btnAddToCart);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.lblProductsHeader);
            this.Controls.Add(this.lblWelcome);
            this.Name = "ProductCatalog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Catalog";
            this.Load += new System.EventHandler(this.ProductCatalog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
