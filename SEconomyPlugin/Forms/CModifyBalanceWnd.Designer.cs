namespace Wolfje.Plugins.SEconomy.Forms {
    partial class CModifyBalanceWnd {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblCurrencyName2 = new System.Windows.Forms.Label();
			this.txtTradeCurrency = new System.Windows.Forms.TextBox();
			this.lblCurrencyName = new System.Windows.Forms.Label();
			this.ddlTradeAccount = new System.Windows.Forms.ComboBox();
			this.rbTradeWith = new System.Windows.Forms.RadioButton();
			this.txtSetCurrency = new System.Windows.Forms.TextBox();
			this.rbSetTo = new System.Windows.Forms.RadioButton();
			this.btnModify = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblAccountName = new System.Windows.Forms.Label();
			this.lblBalance = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lblCurrencyName2);
			this.groupBox1.Controls.Add(this.txtTradeCurrency);
			this.groupBox1.Controls.Add(this.lblCurrencyName);
			this.groupBox1.Controls.Add(this.ddlTradeAccount);
			this.groupBox1.Controls.Add(this.rbTradeWith);
			this.groupBox1.Controls.Add(this.txtSetCurrency);
			this.groupBox1.Controls.Add(this.rbSetTo);
			this.groupBox1.Location = new System.Drawing.Point(15, 36);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(578, 102);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "修改";
			// 
			// lblCurrencyName2
			// 
			this.lblCurrencyName2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCurrencyName2.AutoSize = true;
			this.lblCurrencyName2.Location = new System.Drawing.Point(472, 62);
			this.lblCurrencyName2.Name = "lblCurrencyName2";
			this.lblCurrencyName2.Size = new System.Drawing.Size(89, 12);
			this.lblCurrencyName2.TabIndex = 7;
			this.lblCurrencyName2.Text = "currency names";
			// 
			// txtTradeCurrency
			// 
			this.txtTradeCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTradeCurrency.Enabled = false;
			this.txtTradeCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTradeCurrency.Location = new System.Drawing.Point(331, 58);
			this.txtTradeCurrency.Name = "txtTradeCurrency";
			this.txtTradeCurrency.Size = new System.Drawing.Size(135, 20);
			this.txtTradeCurrency.TabIndex = 6;
			// 
			// lblCurrencyName
			// 
			this.lblCurrencyName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCurrencyName.AutoSize = true;
			this.lblCurrencyName.Location = new System.Drawing.Point(472, 32);
			this.lblCurrencyName.Name = "lblCurrencyName";
			this.lblCurrencyName.Size = new System.Drawing.Size(89, 12);
			this.lblCurrencyName.TabIndex = 3;
			this.lblCurrencyName.Text = "currency names";
			// 
			// ddlTradeAccount
			// 
			this.ddlTradeAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlTradeAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.ddlTradeAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.ddlTradeAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlTradeAccount.Enabled = false;
			this.ddlTradeAccount.FormattingEnabled = true;
			this.ddlTradeAccount.Location = new System.Drawing.Point(147, 57);
			this.ddlTradeAccount.Name = "ddlTradeAccount";
			this.ddlTradeAccount.Size = new System.Drawing.Size(178, 20);
			this.ddlTradeAccount.TabIndex = 5;
			// 
			// rbTradeWith
			// 
			this.rbTradeWith.AutoSize = true;
			this.rbTradeWith.Enabled = false;
			this.rbTradeWith.Location = new System.Drawing.Point(21, 58);
			this.rbTradeWith.Name = "rbTradeWith";
			this.rbTradeWith.Size = new System.Drawing.Size(71, 16);
			this.rbTradeWith.TabIndex = 4;
			this.rbTradeWith.TabStop = true;
			this.rbTradeWith.Text = "使其支付";
			this.rbTradeWith.UseVisualStyleBackColor = true;
			// 
			// txtSetCurrency
			// 
			this.txtSetCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSetCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSetCurrency.Location = new System.Drawing.Point(147, 28);
			this.txtSetCurrency.Name = "txtSetCurrency";
			this.txtSetCurrency.Size = new System.Drawing.Size(319, 20);
			this.txtSetCurrency.TabIndex = 1;
			// 
			// rbSetTo
			// 
			this.rbSetTo.AutoSize = true;
			this.rbSetTo.Checked = true;
			this.rbSetTo.Location = new System.Drawing.Point(21, 29);
			this.rbSetTo.Name = "rbSetTo";
			this.rbSetTo.Size = new System.Drawing.Size(95, 16);
			this.rbSetTo.TabIndex = 0;
			this.rbSetTo.TabStop = true;
			this.rbSetTo.Text = "将其账户设为";
			this.rbSetTo.UseVisualStyleBackColor = true;
			// 
			// btnModify
			// 
			this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnModify.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnModify.Location = new System.Drawing.Point(518, 157);
			this.btnModify.Name = "btnModify";
			this.btnModify.Size = new System.Drawing.Size(75, 21);
			this.btnModify.TabIndex = 1;
			this.btnModify.Text = "确认";
			this.btnModify.UseVisualStyleBackColor = true;
			this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.Location = new System.Drawing.Point(15, 157);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 21);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblAccountName
			// 
			this.lblAccountName.AutoSize = true;
			this.lblAccountName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAccountName.Location = new System.Drawing.Point(12, 13);
			this.lblAccountName.Name = "lblAccountName";
			this.lblAccountName.Size = new System.Drawing.Size(46, 13);
			this.lblAccountName.TabIndex = 0;
			this.lblAccountName.Text = "账户名";
			// 
			// lblBalance
			// 
			this.lblBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblBalance.Location = new System.Drawing.Point(253, 13);
			this.lblBalance.Name = "lblBalance";
			this.lblBalance.Size = new System.Drawing.Size(340, 12);
			this.lblBalance.TabIndex = 0;
			this.lblBalance.Text = "Balance";
			this.lblBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// CModifyBalanceWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(605, 189);
			this.Controls.Add(this.lblBalance);
			this.Controls.Add(this.lblAccountName);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnModify);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CModifyBalanceWnd";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CModifyBalanceWnd";
			this.Load += new System.EventHandler(this.CModifyBalanceWnd_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblAccountName;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblCurrencyName;
        private System.Windows.Forms.ComboBox ddlTradeAccount;
        private System.Windows.Forms.RadioButton rbTradeWith;
        private System.Windows.Forms.TextBox txtSetCurrency;
        private System.Windows.Forms.RadioButton rbSetTo;
        private System.Windows.Forms.Label lblCurrencyName2;
        private System.Windows.Forms.TextBox txtTradeCurrency;
    }
}