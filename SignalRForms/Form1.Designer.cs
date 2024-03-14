namespace SignalRForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblUser = new Label();
            lblMessage = new Label();
            txtUser = new TextBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            lbMessages = new ListBox();
            SuspendLayout();
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(42, 38);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(38, 20);
            lblUser.TabIndex = 0;
            lblUser.Text = "User";
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(42, 71);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(67, 20);
            lblMessage.TabIndex = 1;
            lblMessage.Text = "Message";
            // 
            // txtUser
            // 
            txtUser.Location = new Point(143, 35);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(280, 27);
            txtUser.TabIndex = 2;
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(143, 68);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(280, 27);
            txtMessage.TabIndex = 3;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSend.Location = new Point(523, 68);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 5;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // lbMessages
            // 
            lbMessages.FormattingEnabled = true;
            lbMessages.Location = new Point(42, 114);
            lbMessages.Name = "lbMessages";
            lbMessages.Size = new Size(575, 324);
            lbMessages.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(677, 463);
            Controls.Add(lbMessages);
            Controls.Add(btnSend);
            Controls.Add(txtMessage);
            Controls.Add(txtUser);
            Controls.Add(lblMessage);
            Controls.Add(lblUser);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUser;
        private Label lblMessage;
        private TextBox txtUser;
        private TextBox txtMessage;
        private Button btnSend;
        private ListBox lbMessages;
    }
}
