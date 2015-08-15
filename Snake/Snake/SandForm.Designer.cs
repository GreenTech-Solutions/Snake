namespace Snake
{
    partial class SandForm
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
            this.Map = new System.Windows.Forms.DataGridView();
            this.x1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x5 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x6 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x7 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x8 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x9 = new System.Windows.Forms.DataGridViewImageColumn();
            this.x10 = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Map)).BeginInit();
            this.SuspendLayout();
            // 
            // Map
            // 
            this.Map.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Map.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.x1,
            this.x2,
            this.x3,
            this.x4,
            this.x5,
            this.x6,
            this.x7,
            this.x8,
            this.x9,
            this.x10});
            this.Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Map.Location = new System.Drawing.Point(0, 0);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(1045, 337);
            this.Map.TabIndex = 0;
            // 
            // x1
            // 
            this.x1.HeaderText = "x1";
            this.x1.Image = global::Snake.Resources.TextureTestEmpty;
            this.x1.Name = "x1";
            // 
            // x2
            // 
            this.x2.HeaderText = "x2";
            this.x2.Image = global::Snake.Resources.TextureTestEmpty;
            this.x2.Name = "x2";
            // 
            // x3
            // 
            this.x3.HeaderText = "x3";
            this.x3.Image = global::Snake.Resources.TextureTestEmpty;
            this.x3.Name = "x3";
            // 
            // x4
            // 
            this.x4.HeaderText = "x4";
            this.x4.Name = "x4";
            // 
            // x5
            // 
            this.x5.HeaderText = "x5";
            this.x5.Name = "x5";
            // 
            // x6
            // 
            this.x6.HeaderText = "x6";
            this.x6.Name = "x6";
            // 
            // x7
            // 
            this.x7.HeaderText = "x7";
            this.x7.Name = "x7";
            // 
            // x8
            // 
            this.x8.HeaderText = "x8";
            this.x8.Name = "x8";
            // 
            // x9
            // 
            this.x9.HeaderText = "x9";
            this.x9.Name = "x9";
            // 
            // x10
            // 
            this.x10.HeaderText = "x10";
            this.x10.Name = "x10";
            // 
            // SandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 337);
            this.Controls.Add(this.Map);
            this.Name = "SandForm";
            this.Text = "SandForm";
            this.Load += new System.EventHandler(this.SandForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Map)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Map;
        private System.Windows.Forms.DataGridViewImageColumn x1;
        private System.Windows.Forms.DataGridViewImageColumn x2;
        private System.Windows.Forms.DataGridViewImageColumn x3;
        private System.Windows.Forms.DataGridViewImageColumn x4;
        private System.Windows.Forms.DataGridViewImageColumn x5;
        private System.Windows.Forms.DataGridViewImageColumn x6;
        private System.Windows.Forms.DataGridViewImageColumn x7;
        private System.Windows.Forms.DataGridViewImageColumn x8;
        private System.Windows.Forms.DataGridViewImageColumn x9;
        private System.Windows.Forms.DataGridViewImageColumn x10;
    }
}