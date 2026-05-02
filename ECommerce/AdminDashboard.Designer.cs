namespace ECommerceApp
{
    partial class AdminDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button btnRunReport;
        private System.Windows.Forms.Button btnManageTables;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.DataGridView dgvReport;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.btnRunReport = new System.Windows.Forms.Button();
            this.btnManageTables = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            //
            // lblWelcome
            //
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(20, 15);
            this.lblWelcome.Text = "Welcome, Admin";
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(220, 50);
            this.lblTitle.Text = "Sales Report  (sp_SalesReport)";
            //
            // lblStart
            //
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(30, 110);
            this.lblStart.Text = "Start Date:";
            //
            // dtpStart
            //
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(120, 105);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(180, 23);
            //
            // lblEnd
            //
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(330, 110);
            this.lblEnd.Text = "End Date:";
            //
            // dtpEnd
            //
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(420, 105);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(180, 23);
            //
            // btnRunReport
            //
            this.btnRunReport.Location = new System.Drawing.Point(620, 102);
            this.btnRunReport.Name = "btnRunReport";
            this.btnRunReport.Size = new System.Drawing.Size(120, 32);
            this.btnRunReport.Text = "Run Report";
            this.btnRunReport.UseVisualStyleBackColor = true;
            this.btnRunReport.Click += new System.EventHandler(this.btnRunReport_Click);
            //
            // dgvReport
            //
            this.dgvReport.Location = new System.Drawing.Point(20, 160);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.Size = new System.Drawing.Size(740, 320);
            this.dgvReport.ReadOnly = true;
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            //
            // btnManageTables
            //
            this.btnManageTables.Location = new System.Drawing.Point(20, 500);
            this.btnManageTables.Name = "btnManageTables";
            this.btnManageTables.Size = new System.Drawing.Size(180, 36);
            this.btnManageTables.Text = "Manage Tables (CRUD)";
            this.btnManageTables.UseVisualStyleBackColor = true;
            this.btnManageTables.Click += new System.EventHandler(this.btnManageTables_Click);
            //
            // btnLogout
            //
            this.btnLogout.Location = new System.Drawing.Point(640, 500);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(120, 36);
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            //
            // AdminDashboard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(220, 230, 245);
            this.ClientSize = new System.Drawing.Size(790, 560);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnManageTables);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.btnRunReport);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblWelcome);
            this.Name = "AdminDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Dashboard";
            this.Load += new System.EventHandler(this.AdminDashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
