namespace LocalizationEditor
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.returnStatusBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DataView = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.LOAD_ORI = new System.Windows.Forms.Button();
            this.LOAD_TRA = new System.Windows.Forms.Button();
            this.SAVE = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FILE_ORI = new System.Windows.Forms.Label();
            this.LANG_ORI = new System.Windows.Forms.Label();
            this.FILE_TRA = new System.Windows.Forms.Label();
            this.LANG_TRA = new System.Windows.Forms.Label();
            this.MSG = new System.Windows.Forms.Label();
            this.Hint = new System.Windows.Forms.Label();
            this.Col_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Ori = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Tra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.returnStatusBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            this.SuspendLayout();
            // 
            // returnStatusBindingSource
            // 
            this.returnStatusBindingSource.DataSource = typeof(UnityStandardUtils.Web.HttpRequest.ReturnStatus);
            // 
            // DataView
            // 
            this.DataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_ID,
            this.Col_Ori,
            this.Col_Tra});
            this.DataView.Location = new System.Drawing.Point(12, 85);
            this.DataView.Name = "DataView";
            this.DataView.RowTemplate.Height = 23;
            this.DataView.Size = new System.Drawing.Size(893, 374);
            this.DataView.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // LOAD_ORI
            // 
            this.LOAD_ORI.Location = new System.Drawing.Point(333, 12);
            this.LOAD_ORI.Name = "LOAD_ORI";
            this.LOAD_ORI.Size = new System.Drawing.Size(282, 35);
            this.LOAD_ORI.TabIndex = 1;
            this.LOAD_ORI.Text = "加载原文";
            this.LOAD_ORI.UseVisualStyleBackColor = true;
            this.LOAD_ORI.Click += new System.EventHandler(this.button1_Click);
            // 
            // LOAD_TRA
            // 
            this.LOAD_TRA.Location = new System.Drawing.Point(621, 12);
            this.LOAD_TRA.Name = "LOAD_TRA";
            this.LOAD_TRA.Size = new System.Drawing.Size(284, 34);
            this.LOAD_TRA.TabIndex = 2;
            this.LOAD_TRA.Text = "加载译文";
            this.LOAD_TRA.UseVisualStyleBackColor = true;
            this.LOAD_TRA.Click += new System.EventHandler(this.LOAD_TRA_Click);
            // 
            // SAVE
            // 
            this.SAVE.Location = new System.Drawing.Point(333, 466);
            this.SAVE.Name = "SAVE";
            this.SAVE.Size = new System.Drawing.Size(572, 36);
            this.SAVE.TabIndex = 4;
            this.SAVE.Text = "保存修改";
            this.SAVE.UseVisualStyleBackColor = true;
            this.SAVE.Click += new System.EventHandler(this.SAVE_ORI_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 50);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(69, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "当前文件：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 66);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2);
            this.label2.Size = new System.Drawing.Size(57, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "  语言：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // FILE_ORI
            // 
            this.FILE_ORI.AutoSize = true;
            this.FILE_ORI.Location = new System.Drawing.Point(448, 50);
            this.FILE_ORI.Name = "FILE_ORI";
            this.FILE_ORI.Padding = new System.Windows.Forms.Padding(2);
            this.FILE_ORI.Size = new System.Drawing.Size(45, 16);
            this.FILE_ORI.TabIndex = 7;
            this.FILE_ORI.Text = "未加载";
            // 
            // LANG_ORI
            // 
            this.LANG_ORI.AutoSize = true;
            this.LANG_ORI.Location = new System.Drawing.Point(448, 66);
            this.LANG_ORI.Name = "LANG_ORI";
            this.LANG_ORI.Padding = new System.Windows.Forms.Padding(2);
            this.LANG_ORI.Size = new System.Drawing.Size(45, 16);
            this.LANG_ORI.TabIndex = 8;
            this.LANG_ORI.Text = "未加载";
            // 
            // FILE_TRA
            // 
            this.FILE_TRA.AutoSize = true;
            this.FILE_TRA.Location = new System.Drawing.Point(737, 49);
            this.FILE_TRA.Name = "FILE_TRA";
            this.FILE_TRA.Padding = new System.Windows.Forms.Padding(2);
            this.FILE_TRA.Size = new System.Drawing.Size(45, 16);
            this.FILE_TRA.TabIndex = 9;
            this.FILE_TRA.Text = "未加载";
            // 
            // LANG_TRA
            // 
            this.LANG_TRA.AutoSize = true;
            this.LANG_TRA.Location = new System.Drawing.Point(737, 66);
            this.LANG_TRA.Name = "LANG_TRA";
            this.LANG_TRA.Padding = new System.Windows.Forms.Padding(2);
            this.LANG_TRA.Size = new System.Drawing.Size(45, 16);
            this.LANG_TRA.TabIndex = 10;
            this.LANG_TRA.Text = "未加载";
            // 
            // MSG
            // 
            this.MSG.AutoSize = true;
            this.MSG.Location = new System.Drawing.Point(12, 466);
            this.MSG.Name = "MSG";
            this.MSG.Size = new System.Drawing.Size(0, 12);
            this.MSG.TabIndex = 11;
            // 
            // Hint
            // 
            this.Hint.AutoSize = true;
            this.Hint.Location = new System.Drawing.Point(12, 477);
            this.Hint.Name = "Hint";
            this.Hint.Size = new System.Drawing.Size(77, 12);
            this.Hint.TabIndex = 12;
            this.Hint.Text = "程序已启动！";
            // 
            // Col_ID
            // 
            this.Col_ID.HeaderText = "ID(不可留空)";
            this.Col_ID.Name = "Col_ID";
            // 
            // Col_Ori
            // 
            this.Col_Ori.HeaderText = "原文";
            this.Col_Ori.Name = "Col_Ori";
            // 
            // Col_Tra
            // 
            this.Col_Tra.HeaderText = "译文";
            this.Col_Tra.Name = "Col_Tra";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 510);
            this.Controls.Add(this.Hint);
            this.Controls.Add(this.MSG);
            this.Controls.Add(this.LANG_TRA);
            this.Controls.Add(this.FILE_TRA);
            this.Controls.Add(this.LANG_ORI);
            this.Controls.Add(this.FILE_ORI);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SAVE);
            this.Controls.Add(this.LOAD_TRA);
            this.Controls.Add(this.LOAD_ORI);
            this.Controls.Add(this.DataView);
            this.Name = "Form1";
            this.Text = "本地化编辑器";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.returnStatusBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource returnStatusBindingSource;
        private System.Windows.Forms.DataGridView DataView;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button LOAD_ORI;
        private System.Windows.Forms.Button LOAD_TRA;
        private System.Windows.Forms.Button SAVE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label FILE_ORI;
        private System.Windows.Forms.Label LANG_ORI;
        private System.Windows.Forms.Label FILE_TRA;
        private System.Windows.Forms.Label LANG_TRA;
        private System.Windows.Forms.Label MSG;
        private System.Windows.Forms.Label Hint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Ori;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Tra;
    }
}

