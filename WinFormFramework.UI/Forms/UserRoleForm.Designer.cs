namespace WinFormFramework.UI.Forms
{
    partial class UserRoleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAvailable = new System.Windows.Forms.Label();
            this.lblAssigned = new System.Windows.Forms.Label();
            this.lstAvailable = new System.Windows.Forms.ListBox();
            this.lstAssigned = new System.Windows.Forms.ListBox();
            this.btnAssign = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Location = new System.Drawing.Point(30, 20);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(100, 17);
            this.lblAvailable.Text = "可用角色：";
            // 
            // lblAssigned
            // 
            this.lblAssigned.AutoSize = true;
            this.lblAssigned.Location = new System.Drawing.Point(320, 20);
            this.lblAssigned.Name = "lblAssigned";
            this.lblAssigned.Size = new System.Drawing.Size(100, 17);
            this.lblAssigned.Text = "已分配角色：";
            // 
            // lstAvailable
            // 
            this.lstAvailable.Location = new System.Drawing.Point(30, 50);
            this.lstAvailable.Name = "lstAvailable";
            this.lstAvailable.Size = new System.Drawing.Size(200, 250);
            this.lstAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAvailable.TabIndex = 0;
            // 
            // lstAssigned
            // 
            this.lstAssigned.Location = new System.Drawing.Point(320, 50);
            this.lstAssigned.Name = "lstAssigned";
            this.lstAssigned.Size = new System.Drawing.Size(200, 250);
            this.lstAssigned.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAssigned.TabIndex = 1;
            // 
            // btnAssign
            // 
            this.btnAssign.Location = new System.Drawing.Point(250, 120);
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.Size = new System.Drawing.Size(50, 25);
            this.btnAssign.TabIndex = 2;
            this.btnAssign.Text = ">>";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(250, 170);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(50, 25);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "<<";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(320, 320);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 25);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(440, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            // 
            // UserRoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.lblAssigned);
            this.Controls.Add(this.lstAvailable);
            this.Controls.Add(this.lstAssigned);
            this.Controls.Add(this.btnAssign);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserRoleForm";
            this.Text = "角色分配";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.Label lblAssigned;
        private System.Windows.Forms.ListBox lstAvailable;
        private System.Windows.Forms.ListBox lstAssigned;
        private System.Windows.Forms.Button btnAssign;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
} 