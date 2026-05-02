namespace ECommerceApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblSelectTable;
        private System.Windows.Forms.ComboBox cmbTable;
        private System.Windows.Forms.Panel pnlFields;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnSalesReport;
        private System.Windows.Forms.Button btnPlaceOrder;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.DataGridView dgvData;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblSelectTable = new System.Windows.Forms.Label();
            this.cmbTable = new System.Windows.Forms.ComboBox();
            this.pnlFields = new System.Windows.Forms.Panel();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnSalesReport = new System.Windows.Forms.Button();
            this.btnPlaceOrder = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.dgvData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            //
            // lblSelectTable
            //
            this.lblSelectTable.AutoSize = true;
            this.lblSelectTable.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSelectTable.Location = new System.Drawing.Point(280, 18);
            this.lblSelectTable.Text = "Select Table";
            //
            // cmbTable
            //
            this.cmbTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTable.Location = new System.Drawing.Point(390, 14);
            this.cmbTable.Name = "cmbTable";
            this.cmbTable.Size = new System.Drawing.Size(200, 23);
            this.cmbTable.SelectedIndexChanged += new System.EventHandler(this.cmbTable_SelectedIndexChanged);
            //
            // pnlFields
            //
            this.pnlFields.Location = new System.Drawing.Point(20, 60);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(580, 320);
            this.pnlFields.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            //
            // btnInsert
            //
            this.btnInsert.Location = new System.Drawing.Point(640, 60);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(110, 32);
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            //
            // btnDelete
            //
            this.btnDelete.Location = new System.Drawing.Point(640, 105);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 32);
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            //
            // btnUpdate
            //
            this.btnUpdate.Location = new System.Drawing.Point(640, 150);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(110, 32);
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            //
            // btnView
            //
            this.btnView.Location = new System.Drawing.Point(640, 195);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(110, 32);
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            //
            // btnSalesReport
            //
            this.btnSalesReport.Location = new System.Drawing.Point(640, 250);
            this.btnSalesReport.Name = "btnSalesReport";
            this.btnSalesReport.Size = new System.Drawing.Size(110, 32);
            this.btnSalesReport.Text = "Sales Report";
            this.btnSalesReport.UseVisualStyleBackColor = true;
            this.btnSalesReport.Click += new System.EventHandler(this.btnSalesReport_Click);
            //
            // btnPlaceOrder
            //
            this.btnPlaceOrder.Location = new System.Drawing.Point(640, 295);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new System.Drawing.Size(110, 32);
            this.btnPlaceOrder.Text = "Place Order";
            this.btnPlaceOrder.UseVisualStyleBackColor = true;
            this.btnPlaceOrder.Click += new System.EventHandler(this.btnPlaceOrder_Click);
            //
            // btnLogout
            //
            this.btnLogout.Location = new System.Drawing.Point(640, 348);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(110, 32);
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            //
            // dgvData
            //
            this.dgvData.Location = new System.Drawing.Point(20, 400);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(740, 220);
            this.dgvData.ReadOnly = true;
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvData_CellClick);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(220, 230, 245);
            this.ClientSize = new System.Drawing.Size(790, 640);
            this.Controls.Add(this.dgvData);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnPlaceOrder);
            this.Controls.Add(this.btnSalesReport);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.pnlFields);
            this.Controls.Add(this.cmbTable);
            this.Controls.Add(this.lblSelectTable);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "E-Commerce GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
