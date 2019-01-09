namespace TestApp
{
    partial class TestForm
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
            this.pagedControl1 = new TestApp.PagedControlTest();
            this.page2 = new Manina.Windows.Forms.Page();
            this.page3 = new Manina.Windows.Forms.Page();
            this.page4 = new Manina.Windows.Forms.Page();
            this.page5 = new Manina.Windows.Forms.Page();
            this.pagedControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pagedControl1
            // 
            this.pagedControl1.Controls.Add(this.page2);
            this.pagedControl1.Controls.Add(this.page3);
            this.pagedControl1.Controls.Add(this.page4);
            this.pagedControl1.Controls.Add(this.page5);
            this.pagedControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagedControl1.Location = new System.Drawing.Point(0, 0);
            this.pagedControl1.Name = "pagedControl1";
            this.pagedControl1.Size = new System.Drawing.Size(800, 450);
            this.pagedControl1.TabIndex = 0;
            // 
            // page2
            // 
            this.page2.Location = new System.Drawing.Point(1, 1);
            this.page2.Name = "page2";
            this.page2.Size = new System.Drawing.Size(798, 401);
            // 
            // page3
            // 
            this.page3.Location = new System.Drawing.Point(1, 1);
            this.page3.Name = "page3";
            this.page3.Size = new System.Drawing.Size(774, 395);
            this.page3.Text = "Page 3";
            // 
            // page4
            // 
            this.page4.Location = new System.Drawing.Point(1, 1);
            this.page4.Name = "page4";
            this.page4.Size = new System.Drawing.Size(774, 395);
            this.page4.Text = "Page 4";
            // 
            // page5
            // 
            this.page5.Location = new System.Drawing.Point(1, 1);
            this.page5.Name = "page5";
            this.page5.Size = new System.Drawing.Size(774, 395);
            this.page5.Text = "Page 5";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pagedControl1);
            this.Name = "TestForm";
            this.Text = "PagedControl Test Form";
            this.pagedControl1.ResumeLayout(false);
            this.pagedControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PagedControlTest pagedControl1;
        private Manina.Windows.Forms.Page page2;
        private Manina.Windows.Forms.Page page3;
        private Manina.Windows.Forms.Page page4;
        private Manina.Windows.Forms.Page page5;
    }
}

