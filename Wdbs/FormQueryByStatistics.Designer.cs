namespace Wdbs
{
    partial class FormQueryByStatistics
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelStatisticsResult = new System.Windows.Forms.Label();
            this.comboBoxFields = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxLayers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelSelection = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelStatisticsResult);
            this.groupBox1.Location = new System.Drawing.Point(0, 147);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(403, 228);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "统计结果";
            // 
            // labelStatisticsResult
            // 
            this.labelStatisticsResult.Location = new System.Drawing.Point(13, 32);
            this.labelStatisticsResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatisticsResult.Name = "labelStatisticsResult";
            this.labelStatisticsResult.Size = new System.Drawing.Size(367, 179);
            this.labelStatisticsResult.TabIndex = 0;
            // 
            // comboBoxFields
            // 
            this.comboBoxFields.FormattingEnabled = true;
            this.comboBoxFields.Location = new System.Drawing.Point(63, 100);
            this.comboBoxFields.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxFields.Name = "comboBoxFields";
            this.comboBoxFields.Size = new System.Drawing.Size(339, 23);
            this.comboBoxFields.TabIndex = 10;
            this.comboBoxFields.SelectedIndexChanged += new System.EventHandler(this.comboBoxFields_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 103);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "字段：";
            // 
            // comboBoxLayers
            // 
            this.comboBoxLayers.FormattingEnabled = true;
            this.comboBoxLayers.Location = new System.Drawing.Point(63, 56);
            this.comboBoxLayers.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxLayers.Name = "comboBoxLayers";
            this.comboBoxLayers.Size = new System.Drawing.Size(339, 23);
            this.comboBoxLayers.TabIndex = 8;
            this.comboBoxLayers.SelectedIndexChanged += new System.EventHandler(this.comboBoxLayers_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "图层：";
            // 
            // labelSelection
            // 
            this.labelSelection.AutoSize = true;
            this.labelSelection.Location = new System.Drawing.Point(0, 13);
            this.labelSelection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSelection.Name = "labelSelection";
            this.labelSelection.Size = new System.Drawing.Size(355, 15);
            this.labelSelection.TabIndex = 6;
            this.labelSelection.Text = "当前地图选择集共有   个图层的   个要素被选中。";
            // 
            // FormQueryByStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 389);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxFields);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxLayers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelSelection);
            this.Name = "FormQueryByStatistics";
            this.Text = "FormQueryByStatistics";
            this.Load += new System.EventHandler(this.FormQueryByStatistics_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelStatisticsResult;
        private System.Windows.Forms.ComboBox comboBoxFields;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxLayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSelection;
    }
}