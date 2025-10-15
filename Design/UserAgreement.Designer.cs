namespace DCP
{
    partial class UserAgreement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserAgreement));
            this.radioButtonAccept = new System.Windows.Forms.RadioButton();
            this.radioButtonDontAccept = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox15 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonAccept
            // 
            this.radioButtonAccept.AutoSize = true;
            this.radioButtonAccept.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButtonAccept.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonAccept.Location = new System.Drawing.Point(306, 369);
            this.radioButtonAccept.Name = "radioButtonAccept";
            this.radioButtonAccept.Size = new System.Drawing.Size(203, 22);
            this.radioButtonAccept.TabIndex = 0;
            this.radioButtonAccept.TabStop = true;
            this.radioButtonAccept.Text = "I accept the agreement";
            this.radioButtonAccept.UseVisualStyleBackColor = true;
            // 
            // radioButtonDontAccept
            // 
            this.radioButtonDontAccept.AutoSize = true;
            this.radioButtonDontAccept.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButtonDontAccept.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonDontAccept.Location = new System.Drawing.Point(306, 414);
            this.radioButtonDontAccept.Name = "radioButtonDontAccept";
            this.radioButtonDontAccept.Size = new System.Drawing.Size(246, 22);
            this.radioButtonDontAccept.TabIndex = 1;
            this.radioButtonDontAccept.TabStop = true;
            this.radioButtonDontAccept.Text = "I don\'t accept the agreement";
            this.radioButtonDontAccept.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(301, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 28);
            this.label1.TabIndex = 3;
            this.label1.Text = "HCS User Agreement";
            // 
            // buttonNext
            // 
            this.buttonNext.BackColor = System.Drawing.Color.Crimson;
            this.buttonNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonNext.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonNext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Tomato;
            this.buttonNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNext.Font = new System.Drawing.Font("Verdana", 9F);
            this.buttonNext.Location = new System.Drawing.Point(478, 467);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(134, 44);
            this.buttonNext.TabIndex = 5;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = false;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.Crimson;
            this.buttonBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Tomato;
            this.buttonBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Font = new System.Drawing.Font("Verdana", 9F);
            this.buttonBack.Location = new System.Drawing.Point(319, 467);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(134, 44);
            this.buttonBack.TabIndex = 6;
            this.buttonBack.Text = "Back";
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(306, 177);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(329, 167);
            this.richTextBox1.TabIndex = 41;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::DCP.Properties.Resources.HCS_Logo_Crimson;
            this.pictureBox4.Location = new System.Drawing.Point(22, 50);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(232, 207);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 42;
            this.pictureBox4.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Crimson;
            this.panel1.Controls.Add(this.pictureBox4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 543);
            this.panel1.TabIndex = 43;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9F);
            this.label2.Location = new System.Drawing.Point(303, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(327, 108);
            this.label2.TabIndex = 44;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // pictureBox15
            // 
            this.pictureBox15.BackColor = System.Drawing.Color.Black;
            this.pictureBox15.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox15.Location = new System.Drawing.Point(311, 182);
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.Size = new System.Drawing.Size(329, 167);
            this.pictureBox15.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox15.TabIndex = 45;
            this.pictureBox15.TabStop = false;
            // 
            // UserAgreement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCoral;
            this.ClientSize = new System.Drawing.Size(682, 543);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonDontAccept);
            this.Controls.Add(this.radioButtonAccept);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox15);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserAgreement";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonAccept;
        private System.Windows.Forms.RadioButton radioButtonDontAccept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox15;
    }
}