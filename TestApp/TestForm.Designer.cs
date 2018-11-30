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
            this.EventLog = new System.Windows.Forms.ListBox();
            this.pagedControl1 = new Manina.Windows.Forms.PagedControl();
            this.page1 = new Manina.Windows.Forms.Page();
            this.SuspendLayout();
            // 
            // PrevButton
            // 
            this.PrevButton.Location = new System.Drawing.Point(71, 218);
            this.PrevButton.Name = "PrevButton";
            this.PrevButton.Size = new System.Drawing.Size(23, 23);
            this.PrevButton.TabIndex = 1;
            this.PrevButton.Text = "<";
            this.PrevButton.UseVisualStyleBackColor = true;
            this.PrevButton.Click += new System.EventHandler(this.PrevButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(100, 218);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(23, 23);
            this.NextButton.TabIndex = 1;
            this.NextButton.Text = ">";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // CurrentPageLabel
            // 
            this.CurrentPageLabel.AutoSize = true;
            this.CurrentPageLabel.Location = new System.Drawing.Point(129, 223);
            this.CurrentPageLabel.Name = "CurrentPageLabel";
            this.CurrentPageLabel.Size = new System.Drawing.Size(43, 13);
            this.CurrentPageLabel.TabIndex = 2;
            this.CurrentPageLabel.Text = "<none>";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(13, 218);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(23, 23);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "+";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(42, 218);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(23, 23);
            this.RemoveButton.TabIndex = 1;
            this.RemoveButton.Text = "-";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // EventLog
            // 
            this.EventLog.FormattingEnabled = true;
            this.EventLog.IntegralHeight = false;
            this.EventLog.Location = new System.Drawing.Point(13, 258);
            this.EventLog.Name = "EventLog";
            this.EventLog.Size = new System.Drawing.Size(775, 180);
            this.EventLog.TabIndex = 3;
            // 
            // pagedControl1
            // 
            this.pagedControl1.Location = new System.Drawing.Point(12, 12);
            this.pagedControl1.Name = "pagedControl1";
            this.pagedControl1.Size = new System.Drawing.Size(776, 200);
            this.pagedControl1.TabIndex = 0;
            this.pagedControl1.PageAdded += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageAdded);
            this.pagedControl1.PageRemoved += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageRemoved);
            this.pagedControl1.PageChanging += new Manina.Windows.Forms.PagedControl.PageChangingEventHandler(this.pagedControl1_PageChanging);
            this.pagedControl1.PageChanged += new Manina.Windows.Forms.PagedControl.PageChangedEventHandler(this.pagedControl1_PageChanged);
            this.pagedControl1.PageValidating += new Manina.Windows.Forms.PagedControl.PageValidatingEventHandler(this.pagedControl1_PageValidating);
            this.pagedControl1.PageValidated += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageValidated);
            this.pagedControl1.PageHidden += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageHidden);
            this.pagedControl1.PageShown += new Manina.Windows.Forms.PagedControl.PageEventHandler(this.pagedControl1_PageShown);
            // 
            // page1
            // 
            this.page1.Location = new System.Drawing.Point(1, 1);
            this.page1.Name = "page1";
            this.page1.Size = new System.Drawing.Size(0, 0);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EventLog);
            this.Controls.Add(this.CurrentPageLabel);
            this.Controls.Add(this.NextButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.PrevButton);
            this.Controls.Add(this.pagedControl1);
            this.Name = "TestForm";
            this.Text = "PagedControl Test Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Manina.Windows.Forms.PagedControl pagedControl1;
        private Manina.Windows.Forms.Page page1;
        private System.Windows.Forms.Button PrevButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Label CurrentPageLabel;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.ListBox EventLog;
    }
}

