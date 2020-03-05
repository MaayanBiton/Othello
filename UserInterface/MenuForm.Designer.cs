namespace UserInterface
{
    partial class MenuForm
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
            this.buttonBoardSize = new System.Windows.Forms.Button();
            this.buttonComputer = new System.Windows.Forms.Button();
            this.buttonHuman = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBoardSize
            // 
            this.buttonBoardSize.Location = new System.Drawing.Point(16, 24);
            this.buttonBoardSize.Name = "buttonBoardSize";
            this.buttonBoardSize.Size = new System.Drawing.Size(347, 46);
            this.buttonBoardSize.TabIndex = 0;
            this.buttonBoardSize.Text = "Board Size: 6x6 (click to increase)";
            this.buttonBoardSize.UseVisualStyleBackColor = true;
            this.buttonBoardSize.Click += new System.EventHandler(this.buttonBoardSize_Click);
            // 
            // buttonComputer
            // 
            this.buttonComputer.Location = new System.Drawing.Point(16, 87);
            this.buttonComputer.Name = "buttonComputer";
            this.buttonComputer.Size = new System.Drawing.Size(171, 63);
            this.buttonComputer.TabIndex = 1;
            this.buttonComputer.Text = "Play against the computer";
            this.buttonComputer.UseVisualStyleBackColor = true;
            this.buttonComputer.Click += new System.EventHandler(this.buttonComputer_Click);
            // 
            // buttonHuman
            // 
            this.buttonHuman.Location = new System.Drawing.Point(193, 87);
            this.buttonHuman.Name = "buttonHuman";
            this.buttonHuman.Size = new System.Drawing.Size(170, 63);
            this.buttonHuman.TabIndex = 2;
            this.buttonHuman.Text = "Play against your friend";
            this.buttonHuman.UseVisualStyleBackColor = true;
            this.buttonHuman.Click += new System.EventHandler(this.buttonHuman_Click);
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 162);
            this.Controls.Add(this.buttonHuman);
            this.Controls.Add(this.buttonComputer);
            this.Controls.Add(this.buttonBoardSize);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonBoardSize;
        private System.Windows.Forms.Button buttonComputer;
        private System.Windows.Forms.Button buttonHuman;
    }
}
