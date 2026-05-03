namespace ECommerceApp
{
    partial class SignupForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblMiddleName;
        private System.Windows.Forms.TextBox txtMiddleName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Button btnSignup;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblMiddleName = new System.Windows.Forms.Label();
            this.txtMiddleName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.btnSignup = new System.Windows.Forms.Button();
            this.SuspendLayout();

            int leftLabel = 60, leftBox = 200, boxW = 220, y = 30, step = 38;

            this.lblFirstName.AutoSize = true; this.lblFirstName.Location = new System.Drawing.Point(leftLabel, y + 4); this.lblFirstName.Text = "First Name:";
            this.txtFirstName.Location = new System.Drawing.Point(leftBox, y); this.txtFirstName.Name = "txtFirstName"; this.txtFirstName.Size = new System.Drawing.Size(boxW, 23);
            y += step;

            this.lblMiddleName.AutoSize = true; this.lblMiddleName.Location = new System.Drawing.Point(leftLabel, y + 4); this.lblMiddleName.Text = "Middle Name (optional):";
            this.txtMiddleName.Location = new System.Drawing.Point(leftBox, y); this.txtMiddleName.Name = "txtMiddleName"; this.txtMiddleName.Size = new System.Drawing.Size(boxW, 23);
            y += step;

            this.lblLastName.AutoSize = true; this.lblLastName.Location = new System.Drawing.Point(leftLabel, y + 4); this.lblLastName.Text = "Last Name:";
            this.txtLastName.Location = new System.Drawing.Point(leftBox, y); this.txtLastName.Name = "txtLastName"; this.txtLastName.Size = new System.Drawing.Size(boxW, 23);
            y += step;

            this.lblUsername.AutoSize = true; this.lblUsername.Location = new System.Drawing.Point(leftLabel, y + 4); this.lblUsername.Text = "Username:";
            this.txtUsername.Location = new System.Drawing.Point(leftBox, y); this.txtUsername.Name = "txtUsername"; this.txtUsername.Size = new System.Drawing.Size(boxW, 23);
            y += step;

            this.lblPassword.AutoSize = true; this.lblPassword.Location = new System.Drawing.Point(leftLabel, y + 4); this.lblPassword.Text = "Password:";
            this.txtPassword.Location = new System.Drawing.Point(leftBox, y); this.txtPassword.Name = "txtPassword"; this.txtPassword.Size = new System.Drawing.Size(boxW, 23);
            this.txtPassword.PasswordChar = '*';
            y += step;

            this.lblEmail.AutoSize = true; this.lblEmail.Location = new System.Drawing.Point(leftLabel, y + 4); this.lblEmail.Text = "Email:";
            this.txtEmail.Location = new System.Drawing.Point(leftBox, y); this.txtEmail.Name = "txtEmail"; this.txtEmail.Size = new System.Drawing.Size(boxW, 23);
            y += step;

            this.lblPhone.AutoSize = true; this.lblPhone.Location = new System.Drawing.Point(leftLabel, y + 4); this.lblPhone.Text = "Phone (11 digits):";
            this.txtPhone.Location = new System.Drawing.Point(leftBox, y); this.txtPhone.Name = "txtPhone"; this.txtPhone.Size = new System.Drawing.Size(boxW, 23);
            this.txtPhone.MaxLength = 11;
            y += step + 15;

            //
            // btnSignup
            //
            this.btnSignup.Location = new System.Drawing.Point(200, y);
            this.btnSignup.Name = "btnSignup";
            this.btnSignup.Size = new System.Drawing.Size(110, 32);
            this.btnSignup.Text = "Signup";
            this.btnSignup.UseVisualStyleBackColor = true;
            this.btnSignup.Click += new System.EventHandler(this.btnSignup_Click);
            //
            // SignupForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(220, 230, 245);
            this.ClientSize = new System.Drawing.Size(520, y + 90);
            this.Controls.Add(this.lblFirstName);  this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lblMiddleName); this.Controls.Add(this.txtMiddleName);
            this.Controls.Add(this.lblLastName);   this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.lblUsername);   this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);   this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblEmail);      this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPhone);      this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.btnSignup);
            this.Name = "SignupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign Up";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
