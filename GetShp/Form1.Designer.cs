namespace GetShp
{
    partial class GetShp
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
            this.button1 = new System.Windows.Forms.Button();
            this.InPutPath = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.OutPutPath = new System.Windows.Forms.TextBox();
            this.OpenBT = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.runBT = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Tips = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(292, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "浏览";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InPutPath
            // 
            this.InPutPath.AllowDrop = true;
            this.InPutPath.Location = new System.Drawing.Point(12, 31);
            this.InPutPath.Name = "InPutPath";
            this.InPutPath.ReadOnly = true;
            this.InPutPath.Size = new System.Drawing.Size(271, 21);
            this.InPutPath.TabIndex = 1;
            this.InPutPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.InPutPath_DragDrop);
            this.InPutPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.InPutPath_DragEnter_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(292, 91);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "浏览";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OutPutPath
            // 
            this.OutPutPath.Location = new System.Drawing.Point(12, 93);
            this.OutPutPath.Name = "OutPutPath";
            this.OutPutPath.ReadOnly = true;
            this.OutPutPath.Size = new System.Drawing.Size(271, 21);
            this.OutPutPath.TabIndex = 1;
            // 
            // OpenBT
            // 
            this.OpenBT.Location = new System.Drawing.Point(373, 90);
            this.OpenBT.Name = "OpenBT";
            this.OpenBT.Size = new System.Drawing.Size(75, 23);
            this.OpenBT.TabIndex = 2;
            this.OpenBT.Text = "打开";
            this.OpenBT.UseVisualStyleBackColor = true;
            this.OpenBT.Click += new System.EventHandler(this.OpenBT_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "源文件";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "目标文件夹";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 195);
            this.label3.MaximumSize = new System.Drawing.Size(395, 84);
            this.label3.MinimumSize = new System.Drawing.Size(395, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(395, 84);
            this.label3.TabIndex = 4;
            this.label3.Text = "提示:1.支持拖拽源文件；\r\n\r\n     2.路径与文件名只支持中文，若文件名包含中文，请重命名为英文；\r\n\r\n     3.同一源文件不能多次解析到同一目标文" +
    "件夹，若要如此，请删除\r\n\r\n       目标文件夹下，由此文件解析得到的shp文件。";
            // 
            // runBT
            // 
            this.runBT.Location = new System.Drawing.Point(373, 123);
            this.runBT.Name = "runBT";
            this.runBT.Size = new System.Drawing.Size(75, 23);
            this.runBT.TabIndex = 6;
            this.runBT.Text = "确定";
            this.runBT.UseVisualStyleBackColor = true;
            this.runBT.Click += new System.EventHandler(this.runBT_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(373, 31);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "打开";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 156);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(435, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // Tips
            // 
            this.Tips.AutoSize = true;
            this.Tips.Location = new System.Drawing.Point(13, 133);
            this.Tips.Name = "Tips";
            this.Tips.Size = new System.Drawing.Size(161, 12);
            this.Tips.TabIndex = 8;
            this.Tips.Text = "正在解析文件，请稍候。。。";
            // 
            // GetShp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 299);
            this.Controls.Add(this.Tips);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.runBT);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OpenBT);
            this.Controls.Add(this.OutPutPath);
            this.Controls.Add(this.InPutPath);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(480, 337);
            this.MinimumSize = new System.Drawing.Size(480, 337);
            this.Name = "GetShp";
            this.Text = "GetShp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox InPutPath;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox OutPutPath;
        private System.Windows.Forms.Button OpenBT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button runBT;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label Tips;
    }
}

