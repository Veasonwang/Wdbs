namespace Wdbs
{
    partial class FormQueryBySpace
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxBufferDistance = new System.Windows.Forms.TextBox();
            this.checkBoxBuffer = new System.Windows.Forms.CheckBox();
            this.comboBoxMethods = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelSelectedCount = new System.Windows.Forms.Label();
            this.checkBoxUseSelected = new System.Windows.Forms.CheckBox();
            this.comboBoxSourceLayer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxSelectable = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBoxTargetLayers = new System.Windows.Forms.CheckedListBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(421, 600);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 31);
            this.buttonClose.TabIndex = 52;
            this.buttonClose.Text = "关闭";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(321, 600);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(4);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(80, 31);
            this.buttonApply.TabIndex = 51;
            this.buttonApply.Text = "应用";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(220, 600);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(80, 31);
            this.buttonOK.TabIndex = 50;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(27, 583);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(475, 4);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(337, 544);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 15);
            this.label5.TabIndex = 48;
            this.label5.Text = "(单位与地图单位相同)";
            // 
            // textBoxBufferDistance
            // 
            this.textBoxBufferDistance.Location = new System.Drawing.Point(373, 510);
            this.textBoxBufferDistance.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxBufferDistance.Name = "textBoxBufferDistance";
            this.textBoxBufferDistance.Size = new System.Drawing.Size(127, 25);
            this.textBoxBufferDistance.TabIndex = 47;
            // 
            // checkBoxBuffer
            // 
            this.checkBoxBuffer.AutoSize = true;
            this.checkBoxBuffer.Location = new System.Drawing.Point(27, 512);
            this.checkBoxBuffer.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxBuffer.Name = "checkBoxBuffer";
            this.checkBoxBuffer.Size = new System.Drawing.Size(329, 19);
            this.checkBoxBuffer.TabIndex = 46;
            this.checkBoxBuffer.Text = "对源图层使用缓冲区进行查询。缓冲区大小：";
            this.checkBoxBuffer.UseVisualStyleBackColor = true;
            // 
            // comboBoxMethods
            // 
            this.comboBoxMethods.FormattingEnabled = true;
            this.comboBoxMethods.Items.AddRange(new object[] {
            "目标图层的要素与源图层的要素相交 (intersect)",
            "目标图层的要素位于源图层要素的一定距离范围内 (within)",
            "目标图层的要素包含源图层的要素 (contain)",
            "目标图层的要素在源图层的要素内 (within)",
            "目标图层的要素与源图层要素的边界相接 (touch)",
            "目标图层的要素被源图层要素的轮廓穿过 (cross)"});
            this.comboBoxMethods.Location = new System.Drawing.Point(23, 458);
            this.comboBoxMethods.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxMethods.Name = "comboBoxMethods";
            this.comboBoxMethods.Size = new System.Drawing.Size(477, 23);
            this.comboBoxMethods.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 436);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 15);
            this.label4.TabIndex = 44;
            this.label4.Text = "空间选择方法：";
            // 
            // labelSelectedCount
            // 
            this.labelSelectedCount.AutoSize = true;
            this.labelSelectedCount.Location = new System.Drawing.Point(252, 402);
            this.labelSelectedCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSelectedCount.Name = "labelSelectedCount";
            this.labelSelectedCount.Size = new System.Drawing.Size(182, 15);
            this.labelSelectedCount.TabIndex = 43;
            this.labelSelectedCount.Text = "(当前有 0 个要素被选择)";
            // 
            // checkBoxUseSelected
            // 
            this.checkBoxUseSelected.AutoSize = true;
            this.checkBoxUseSelected.Enabled = false;
            this.checkBoxUseSelected.Location = new System.Drawing.Point(27, 397);
            this.checkBoxUseSelected.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxUseSelected.Name = "checkBoxUseSelected";
            this.checkBoxUseSelected.Size = new System.Drawing.Size(149, 19);
            this.checkBoxUseSelected.TabIndex = 42;
            this.checkBoxUseSelected.Text = "使用被选择的要素";
            this.checkBoxUseSelected.UseVisualStyleBackColor = true;
            // 
            // comboBoxSourceLayer
            // 
            this.comboBoxSourceLayer.FormattingEnabled = true;
            this.comboBoxSourceLayer.Location = new System.Drawing.Point(23, 364);
            this.comboBoxSourceLayer.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxSourceLayer.Name = "comboBoxSourceLayer";
            this.comboBoxSourceLayer.Size = new System.Drawing.Size(477, 23);
            this.comboBoxSourceLayer.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 342);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 40;
            this.label2.Text = "源图层：";
            // 
            // checkBoxSelectable
            // 
            this.checkBoxSelectable.AutoSize = true;
            this.checkBoxSelectable.Location = new System.Drawing.Point(27, 302);
            this.checkBoxSelectable.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxSelectable.Name = "checkBoxSelectable";
            this.checkBoxSelectable.Size = new System.Drawing.Size(134, 19);
            this.checkBoxSelectable.TabIndex = 39;
            this.checkBoxSelectable.Text = "只列出可选图层";
            this.checkBoxSelectable.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 38;
            this.label1.Text = "目标图层：";
            // 
            // checkedListBoxTargetLayers
            // 
            this.checkedListBoxTargetLayers.FormattingEnabled = true;
            this.checkedListBoxTargetLayers.Location = new System.Drawing.Point(23, 90);
            this.checkedListBoxTargetLayers.Margin = new System.Windows.Forms.Padding(4);
            this.checkedListBoxTargetLayers.Name = "checkedListBoxTargetLayers";
            this.checkedListBoxTargetLayers.Size = new System.Drawing.Size(477, 204);
            this.checkedListBoxTargetLayers.TabIndex = 37;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(20, 13);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(481, 42);
            this.labelDescription.TabIndex = 36;
            this.labelDescription.Text = "根据空间位置选择是使用源图层中的要素与目标图层的空间关系（如覆盖、相交等），在目标图层中选择要素的操作。";
            // 
            // FormQueryBySpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 645);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxBufferDistance);
            this.Controls.Add(this.checkBoxBuffer);
            this.Controls.Add(this.comboBoxMethods);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelSelectedCount);
            this.Controls.Add(this.checkBoxUseSelected);
            this.Controls.Add(this.comboBoxSourceLayer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBoxSelectable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBoxTargetLayers);
            this.Controls.Add(this.labelDescription);
            this.Name = "FormQueryBySpace";
            this.Text = "FormQueryBySpace";
            this.Load += new System.EventHandler(this.FormQueryBySpace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxBufferDistance;
        private System.Windows.Forms.CheckBox checkBoxBuffer;
        private System.Windows.Forms.ComboBox comboBoxMethods;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelSelectedCount;
        private System.Windows.Forms.CheckBox checkBoxUseSelected;
        private System.Windows.Forms.ComboBox comboBoxSourceLayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxSelectable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBoxTargetLayers;
        private System.Windows.Forms.Label labelDescription;

    }
}