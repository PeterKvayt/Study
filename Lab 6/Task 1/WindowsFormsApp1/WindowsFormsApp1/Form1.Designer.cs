namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxSumm = new System.Windows.Forms.TextBox();
            this.comboBoxChooseOurMoney = new System.Windows.Forms.ComboBox();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxChooseConvertableMoney = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxSumm
            // 
            this.textBoxSumm.ForeColor = System.Drawing.Color.Gray;
            this.textBoxSumm.Location = new System.Drawing.Point(12, 12);
            this.textBoxSumm.Name = "textBoxSumm";
            this.textBoxSumm.Size = new System.Drawing.Size(171, 22);
            this.textBoxSumm.TabIndex = 0;
            this.textBoxSumm.Text = "Введите сумму";
            this.textBoxSumm.Click += new System.EventHandler(this.textBoxSumm_Click);
            this.textBoxSumm.TextChanged += new System.EventHandler(this.textBoxSumm_TextChanged);
            this.textBoxSumm.Leave += new System.EventHandler(this.textBoxSumm_Leave);
            // 
            // comboBoxChooseOurMoney
            // 
            this.comboBoxChooseOurMoney.FormattingEnabled = true;
            this.comboBoxChooseOurMoney.Items.AddRange(new object[] {
            "USD",
            "EUR",
            "BYN",
            "RUB"});
            this.comboBoxChooseOurMoney.Location = new System.Drawing.Point(190, 12);
            this.comboBoxChooseOurMoney.Name = "comboBoxChooseOurMoney";
            this.comboBoxChooseOurMoney.Size = new System.Drawing.Size(162, 24);
            this.comboBoxChooseOurMoney.TabIndex = 1;
            this.comboBoxChooseOurMoney.Text = "Выберите валюту";
            this.comboBoxChooseOurMoney.SelectedIndexChanged += new System.EventHandler(this.comboBoxChooseOurMoney_SelectedIndexChanged);
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(12, 87);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ReadOnly = true;
            this.textBoxResult.Size = new System.Drawing.Size(171, 22);
            this.textBoxResult.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Результат";
            // 
            // comboBoxChooseConvertableMoney
            // 
            this.comboBoxChooseConvertableMoney.Enabled = false;
            this.comboBoxChooseConvertableMoney.FormattingEnabled = true;
            this.comboBoxChooseConvertableMoney.Items.AddRange(new object[] {
            "USD",
            "EUR",
            "BYN",
            "RUB"});
            this.comboBoxChooseConvertableMoney.Location = new System.Drawing.Point(189, 87);
            this.comboBoxChooseConvertableMoney.Name = "comboBoxChooseConvertableMoney";
            this.comboBoxChooseConvertableMoney.Size = new System.Drawing.Size(162, 24);
            this.comboBoxChooseConvertableMoney.TabIndex = 4;
            this.comboBoxChooseConvertableMoney.Text = "Выберите валюту";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(339, 69);
            this.button1.TabIndex = 6;
            this.button1.Text = "Перевести";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 202);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxChooseConvertableMoney);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.comboBoxChooseOurMoney);
            this.Controls.Add(this.textBoxSumm);
            this.Name = "Form1";
            this.Text = "Калькулятор валют";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSumm;
        private System.Windows.Forms.ComboBox comboBoxChooseOurMoney;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxChooseConvertableMoney;
        private System.Windows.Forms.Button button1;
    }
}

