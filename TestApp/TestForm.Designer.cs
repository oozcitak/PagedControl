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
            this.PrevButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.CurrentPageLabel = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.pagedControl1 = new Manina.Windows.Forms.PagedControl();
            this.page1 = new Manina.Windows.Forms.Page();
            this.page2 = new Manina.Windows.Forms.Page();
            this.ClearButton = new System.Windows.Forms.Button();
            this.InsertButton = new System.Windows.Forms.Button();
            this.pagedControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PrevButton
            // 
            this.PrevButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PrevButton.Location = new System.Drawing.Point(194, 415);
            this.PrevButton.Name = "PrevButton";
            this.PrevButton.Size = new System.Drawing.Size(23, 23);
            this.PrevButton.TabIndex = 1;
            this.PrevButton.Text = "<";
            this.PrevButton.UseVisualStyleBackColor = true;
            this.PrevButton.Click += new System.EventHandler(this.PrevButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NextButton.Location = new System.Drawing.Point(216, 415);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(23, 23);
            this.NextButton.TabIndex = 1;
            this.NextButton.Text = ">";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // CurrentPageLabel
            // 
            this.CurrentPageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CurrentPageLabel.AutoSize = true;
            this.CurrentPageLabel.Location = new System.Drawing.Point(245, 420);
            this.CurrentPageLabel.Name = "CurrentPageLabel";
            this.CurrentPageLabel.Size = new System.Drawing.Size(43, 13);
            this.CurrentPageLabel.TabIndex = 2;
            this.CurrentPageLabel.Text = "<none>";
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddButton.Location = new System.Drawing.Point(13, 415);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(23, 23);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "+";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveButton.Location = new System.Drawing.Point(99, 415);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(23, 23);
            this.RemoveButton.TabIndex = 1;
            this.RemoveButton.Text = "-";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // pagedControl1
            // 
            this.pagedControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pagedControl1.Controls.Add(this.page1);
            this.pagedControl1.Controls.Add(this.page2);
            this.pagedControl1.Location = new System.Drawing.Point(12, 12);
            this.pagedControl1.Name = "pagedControl1";
            this.pagedControl1.Size = new System.Drawing.Size(776, 397);
            this.pagedControl1.TabIndex = 0;
            this.pagedControl1.PageAdded += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageAdded);
            this.pagedControl1.PageRemoved += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageRemoved);
            this.pagedControl1.PageChanging += new Manina.Windows.Forms.PagedControl.PageChangingEventHandler(this.pagedControl1_PageChanging);
            this.pagedControl1.PageChanged += new Manina.Windows.Forms.PagedControl.PageChangedEventHandler(this.pagedControl1_PageChanged);
            this.pagedControl1.PageValidating += new Manina.Windows.Forms.PagedControl.PageValidatingEventHandler(this.pagedControl1_PageValidating);
            this.pagedControl1.PageValidated += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageValidated);
            this.pagedControl1.PageHidden += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageHidden);
            this.pagedControl1.PageShown += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageShown);
            this.pagedControl1.PagePaint += new Manina.Windows.Forms.PagedControl.PagePaintEventHandler(this.pagedControl1_PagePaint);
            // 
            // page1
            // 
            this.page1.Location = new System.Drawing.Point(1, 1);
            this.page1.Name = "page1";
            this.page1.Size = new System.Drawing.Size(774, 395);
            // 
            // page2
            // 
            this.page2.Location = new System.Drawing.Point(1, 1);
            this.page2.Name = "page2";
            this.page2.Size = new System.Drawing.Size(0, 0);
            // 
            // ClearButton
            // 
            this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ClearButton.Location = new System.Drawing.Point(121, 415);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(65, 23);
            this.ClearButton.TabIndex = 1;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // InsertButton
            // 
            this.InsertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InsertButton.Location = new System.Drawing.Point(35, 415);
            this.InsertButton.Name = "InsertButton";
            this.InsertButton.Size = new System.Drawing.Size(65, 23);
            this.InsertButton.TabIndex = 1;
            this.InsertButton.Text = "Insert @ 0";
            this.InsertButton.UseVisualStyleBackColor = true;
            this.InsertButton.Click += new System.EventHandler(this.InsertButton_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CurrentPageLabel);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.InsertButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.PrevButton);
            this.Controls.Add(this.pagedControl1);
            this.Name = "TestForm";
            this.Text = "PagedControl Test Form";
            this.pagedControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Manina.Windows.Forms.PagedControl pagedControl1;
        private System.Windows.Forms.Button PrevButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Label CurrentPageLabel;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private Manina.Windows.Forms.Page page1;
        private Manina.Windows.Forms.Page page2;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button InsertButton;
    }
}

