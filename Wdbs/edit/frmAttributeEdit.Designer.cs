namespace Wdbs.edit
{
    partial class frmAttributeEdit
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvLayer = new System.Windows.Forms.TreeView();
            this.gridViewAttribute = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAttribute)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridViewAttribute);
            this.splitContainer1.Size = new System.Drawing.Size(574, 488);
            this.splitContainer1.SplitterDistance = 191;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tvLayer);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 488);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图层名称";
            // 
            // tvLayer
            // 
            this.tvLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvLayer.Location = new System.Drawing.Point(3, 17);
            this.tvLayer.Name = "tvLayer";
            this.tvLayer.Size = new System.Drawing.Size(185, 468);
            this.tvLayer.TabIndex = 0;
            this.tvLayer.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvLayer_NodeMouseClick);
            // 
            // gridViewAttribute
            // 
            this.gridViewAttribute.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridViewAttribute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewAttribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridViewAttribute.Location = new System.Drawing.Point(0, 0);
            this.gridViewAttribute.Name = "gridViewAttribute";
            this.gridViewAttribute.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.gridViewAttribute.RowTemplate.Height = 23;
            this.gridViewAttribute.Size = new System.Drawing.Size(379, 488);
            this.gridViewAttribute.TabIndex = 1;
            this.gridViewAttribute.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridViewAttribute_CellValueChanged);
            // 
            // frmAttributeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 488);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmAttributeEdit";
            this.Text = "frmAttributeEdit";
            this.Load += new System.EventHandler(this.frmAttributeEdit_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAttribute)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView tvLayer;
        private System.Windows.Forms.DataGridView gridViewAttribute;
    }
}