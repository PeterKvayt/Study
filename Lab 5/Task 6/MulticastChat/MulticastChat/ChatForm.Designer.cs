namespace MulticastChat
{
    partial class ChatForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.textMessages = new System.Windows.Forms.TextBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(63, 9);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(206, 22);
            this.textName.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(731, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(812, 12);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Enabled = false;
            this.buttonSend.Location = new System.Drawing.Point(275, 9);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(254, 23);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textMessage
            // 
            this.textMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessage.Location = new System.Drawing.Point(15, 41);
            this.textMessage.Multiline = true;
            this.textMessage.Name = "textMessage";
            this.textMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMessage.Size = new System.Drawing.Size(254, 355);
            this.textMessage.TabIndex = 5;
            // 
            // textMessages
            // 
            this.textMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessages.Location = new System.Drawing.Point(275, 41);
            this.textMessages.Multiline = true;
            this.textMessages.Name = "textMessages";
            this.textMessages.ReadOnly = true;
            this.textMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMessages.Size = new System.Drawing.Size(612, 384);
            this.textMessages.TabIndex = 6;
            // 
            // statusBar
            // 
            this.statusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusBar.Location = new System.Drawing.Point(0, 428);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(899, 22);
            this.statusBar.TabIndex = 7;
            this.statusBar.Text = "statusStrip1";
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 450);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.textMessages);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textName);
            this.Controls.Add(this.label1);
            this.Name = "ChatForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.TextBox textMessages;
        private System.Windows.Forms.StatusStrip statusBar;
    }
}

