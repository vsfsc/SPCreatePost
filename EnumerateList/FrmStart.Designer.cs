namespace EnumerateList
{
    partial class frmStart
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtiteUrl = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtWebUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtiteUrl
            // 
            this.txtiteUrl.Location = new System.Drawing.Point(108, 21);
            this.txtiteUrl.Name = "txtiteUrl";
            this.txtiteUrl.Size = new System.Drawing.Size(274, 21);
            this.txtiteUrl.TabIndex = 0;
            this.txtiteUrl.Text = "http://localhost";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(753, 22);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "导入";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(24, 142);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(804, 418);
            this.txtResult.TabIndex = 2;
            // 
            // txtWebUrl
            // 
            this.txtWebUrl.Location = new System.Drawing.Point(453, 24);
            this.txtWebUrl.Name = "txtWebUrl";
            this.txtWebUrl.Size = new System.Drawing.Size(274, 21);
            this.txtWebUrl.TabIndex = 0;
            this.txtWebUrl.Text = "blog";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(388, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "subWebUrl";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "列表所在的网站";
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 573);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtWebUrl);
            this.Controls.Add(this.txtiteUrl);
            this.Name = "frmStart";
            this.Text = "遍历当前网站的所有列表及模板";
            this.Load += new System.EventHandler(this.frmStart_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtiteUrl;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtWebUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

