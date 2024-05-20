namespace src
{
    partial class Form1
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxUploadedImage = new System.Windows.Forms.PictureBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.uploadButton = new System.Windows.Forms.Button();
            this.timeExecutionLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.toggleButton1 = new src.CustomControl.ToggleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUploadedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.pictureBox2.Location = new System.Drawing.Point(811, 199);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(400, 650);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(674, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(705, 82);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tugas Besar 3 Stima";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // pictureBoxUploadedImage
            // 
            this.pictureBoxUploadedImage.BackColor = System.Drawing.SystemColors.HighlightText;
            this.pictureBoxUploadedImage.Location = new System.Drawing.Point(142, 199);
            this.pictureBoxUploadedImage.Name = "pictureBoxUploadedImage";
            this.pictureBoxUploadedImage.Size = new System.Drawing.Size(400, 650);
            this.pictureBoxUploadedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxUploadedImage.TabIndex = 6;
            this.pictureBoxUploadedImage.TabStop = false;
            this.pictureBoxUploadedImage.Click += new System.EventHandler(this.pictureBoxUploadedImage_Click);
            // 
            // searchButton
            // 
            this.searchButton.ForeColor = System.Drawing.Color.Black;
            this.searchButton.Location = new System.Drawing.Point(875, 929);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(240, 79);
            this.searchButton.TabIndex = 10;
            this.searchButton.Text = "SEARCH";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // uploadButton
            // 
            this.uploadButton.ForeColor = System.Drawing.Color.Black;
            this.uploadButton.Location = new System.Drawing.Point(188, 929);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(275, 72);
            this.uploadButton.TabIndex = 11;
            this.uploadButton.Text = "UPLOAD";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // timeExecutionLabel
            // 
            this.timeExecutionLabel.AutoSize = true;
            this.timeExecutionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeExecutionLabel.Location = new System.Drawing.Point(1303, 929);
            this.timeExecutionLabel.Name = "timeExecutionLabel";
            this.timeExecutionLabel.Size = new System.Drawing.Size(286, 37);
            this.timeExecutionLabel.TabIndex = 12;
            this.timeExecutionLabel.Text = "Waktu pencarian : ";
            this.timeExecutionLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1303, 971);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 37);
            this.label2.TabIndex = 13;
            this.label2.Text = "Persentase Kecocokan : ";
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Location = new System.Drawing.Point(1398, 210);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(0, 20);
            this.resultLabel.TabIndex = 14;
            // 
            // toggleButton1
            // 
            this.toggleButton1.AutoSize = true;
            this.toggleButton1.Location = new System.Drawing.Point(608, 954);
            this.toggleButton1.MinimumSize = new System.Drawing.Size(45, 22);
            this.toggleButton1.Name = "toggleButton1";
            this.toggleButton1.Size = new System.Drawing.Size(136, 24);
            this.toggleButton1.TabIndex = 9;
            this.toggleButton1.Text = "toggleButton1";
            this.toggleButton1.UseVisualStyleBackColor = true;
            this.toggleButton1.CheckedChanged += new System.EventHandler(this.toggleButton1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1978, 1144);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeExecutionLabel);
            this.Controls.Add(this.uploadButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.toggleButton1);
            this.Controls.Add(this.pictureBoxUploadedImage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUploadedImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxUploadedImage;
        private CustomControl.ToggleButton toggleButton1;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.Label timeExecutionLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label resultLabel;
    }
}

