namespace AppsAgainstHumanity.Server
{
    partial class expansionPackForm
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
            this.saveSelectionBtn = new System.Windows.Forms.Button();
            this.reloadDecksBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.expansionPackListBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // saveSelectionBtn
            // 
            this.saveSelectionBtn.Location = new System.Drawing.Point(329, 12);
            this.saveSelectionBtn.Name = "saveSelectionBtn";
            this.saveSelectionBtn.Size = new System.Drawing.Size(122, 23);
            this.saveSelectionBtn.TabIndex = 0;
            this.saveSelectionBtn.Text = "Save Selections";
            this.saveSelectionBtn.UseVisualStyleBackColor = true;
            this.saveSelectionBtn.Click += new System.EventHandler(this.saveSelectionBtn_Click);
            // 
            // reloadDecksBtn
            // 
            this.reloadDecksBtn.Location = new System.Drawing.Point(329, 41);
            this.reloadDecksBtn.Name = "reloadDecksBtn";
            this.reloadDecksBtn.Size = new System.Drawing.Size(122, 23);
            this.reloadDecksBtn.TabIndex = 1;
            this.reloadDecksBtn.Text = "Reload Packs";
            this.reloadDecksBtn.UseVisualStyleBackColor = true;
            this.reloadDecksBtn.Click += new System.EventHandler(this.reloadDecksBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(329, 244);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(122, 23);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // expansionPackListBox
            // 
            this.expansionPackListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.expansionPackListBox.FormattingEnabled = true;
            this.expansionPackListBox.Location = new System.Drawing.Point(12, 12);
            this.expansionPackListBox.Name = "expansionPackListBox";
            this.expansionPackListBox.ScrollAlwaysVisible = true;
            this.expansionPackListBox.Size = new System.Drawing.Size(303, 255);
            this.expansionPackListBox.TabIndex = 4;
            // 
            // expansionPackForm
            // 
            this.AcceptButton = this.saveSelectionBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(463, 279);
            this.ControlBox = false;
            this.Controls.Add(this.expansionPackListBox);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.reloadDecksBtn);
            this.Controls.Add(this.saveSelectionBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "expansionPackForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Expansion Packs";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveSelectionBtn;
        private System.Windows.Forms.Button reloadDecksBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.CheckedListBox expansionPackListBox;
    }
}