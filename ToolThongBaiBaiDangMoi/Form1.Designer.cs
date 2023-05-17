
namespace ToolThongBaoBaiDangMoi
{
    partial class Form1
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
            this.richTextBoxLogChoTot = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTimeSleep = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnDeleteLogChoTot = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxLogChoTot
            // 
            this.richTextBoxLogChoTot.Location = new System.Drawing.Point(6, 19);
            this.richTextBoxLogChoTot.Name = "richTextBoxLogChoTot";
            this.richTextBoxLogChoTot.ReadOnly = true;
            this.richTextBoxLogChoTot.Size = new System.Drawing.Size(552, 370);
            this.richTextBoxLogChoTot.TabIndex = 2;
            this.richTextBoxLogChoTot.Text = "";
            this.richTextBoxLogChoTot.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxLogChoTot_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxLogChoTot);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(566, 398);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log Chợ Tốt";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(503, 416);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 72);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "CHẠY";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 420);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Thời gian nghỉ mỗi lần check:";
            // 
            // txtTimeSleep
            // 
            this.txtTimeSleep.Location = new System.Drawing.Point(161, 416);
            this.txtTimeSleep.Name = "txtTimeSleep";
            this.txtTimeSleep.Size = new System.Drawing.Size(54, 20);
            this.txtTimeSleep.TabIndex = 7;
            this.txtTimeSleep.Text = "15";
            this.txtTimeSleep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTimeSleep.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTimeSleep_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 420);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "(giây)";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(422, 417);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 71);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "DỪNG";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnDeleteLogChoTot
            // 
            this.btnDeleteLogChoTot.Location = new System.Drawing.Point(341, 420);
            this.btnDeleteLogChoTot.Name = "btnDeleteLogChoTot";
            this.btnDeleteLogChoTot.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteLogChoTot.TabIndex = 9;
            this.btnDeleteLogChoTot.Text = "Xóa log";
            this.btnDeleteLogChoTot.UseVisualStyleBackColor = true;
            this.btnDeleteLogChoTot.Click += new System.EventHandler(this.btnDeleteLogChoTot_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(12, 469);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(104, 23);
            this.btnCheck.TabIndex = 10;
            this.btnCheck.Text = "Mở để kiểm tra";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 504);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnDeleteLogChoTot);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTimeSleep);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông báo bài đăng mới";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBoxLogChoTot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTimeSleep;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnDeleteLogChoTot;
        private System.Windows.Forms.Button btnCheck;
    }
}

