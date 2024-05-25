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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxUploadedImage = new System.Windows.Forms.PictureBox();
            this.timeExecutionLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.resultLabel = new System.Windows.Forms.Label();
            this.KMPLabel = new System.Windows.Forms.Label();
            this.BMLabel = new System.Windows.Forms.Label();
            this.customButton2 = new src.CustomControl.CustomButton();
            this.customButton1 = new src.CustomControl.CustomButton();
            this.toggleButton2 = new src.CustomControl.ToggleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUploadedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(814, 199);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(400, 650);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.GreenYellow;
            this.label1.Location = new System.Drawing.Point(572, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(976, 126);
            this.label1.TabIndex = 4;
            this.label1.Text = "FINGERPRINT MATCHER";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // pictureBoxUploadedImage
            // 
            this.pictureBoxUploadedImage.BackColor = System.Drawing.SystemColors.HighlightText;
            this.pictureBoxUploadedImage.Location = new System.Drawing.Point(130, 199);
            this.pictureBoxUploadedImage.Name = "pictureBoxUploadedImage";
            this.pictureBoxUploadedImage.Size = new System.Drawing.Size(400, 650);
            this.pictureBoxUploadedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxUploadedImage.TabIndex = 6;
            this.pictureBoxUploadedImage.TabStop = false;
            this.pictureBoxUploadedImage.Click += new System.EventHandler(this.pictureBoxUploadedImage_Click);
            // 
            // timeExecutionLabel
            // 
            this.timeExecutionLabel.AutoSize = true;
            this.timeExecutionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeExecutionLabel.ForeColor = System.Drawing.Color.GreenYellow;
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
            this.label2.ForeColor = System.Drawing.Color.GreenYellow;
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
            this.resultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultLabel.ForeColor = System.Drawing.Color.GreenYellow;
            this.resultLabel.Location = new System.Drawing.Point(1352, 199);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(85, 29);
            this.resultLabel.TabIndex = 14;
            this.resultLabel.Text = "HASIL";
            this.resultLabel.Click += new System.EventHandler(this.resultLabel_Click);
            // 
            // KMPLabel
            // 
            this.KMPLabel.AutoSize = true;
            this.KMPLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KMPLabel.ForeColor = System.Drawing.Color.GreenYellow;
            this.KMPLabel.Location = new System.Drawing.Point(523, 952);
            this.KMPLabel.Name = "KMPLabel";
            this.KMPLabel.Size = new System.Drawing.Size(85, 37);
            this.KMPLabel.TabIndex = 16;
            this.KMPLabel.Text = "KMP";
            this.KMPLabel.Click += new System.EventHandler(this.label3_Click_1);
            // 
            // BMLabel
            // 
            this.BMLabel.AutoSize = true;
            this.BMLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BMLabel.ForeColor = System.Drawing.Color.GreenYellow;
            this.BMLabel.Location = new System.Drawing.Point(770, 952);
            this.BMLabel.Name = "BMLabel";
            this.BMLabel.Size = new System.Drawing.Size(64, 37);
            this.BMLabel.TabIndex = 17;
            this.BMLabel.Text = "BM";
            this.BMLabel.Click += new System.EventHandler(this.label3_Click_2);
            // 
            // customButton2
            // 
            this.customButton2.BackColor = System.Drawing.Color.White;
            this.customButton2.BackgroundColor = System.Drawing.Color.White;
            this.customButton2.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.customButton2.BorderRadius = 20;
            this.customButton2.BorderSize = 0;
            this.customButton2.FlatAppearance.BorderSize = 0;
            this.customButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton2.ForeColor = System.Drawing.Color.YellowGreen;
            this.customButton2.Location = new System.Drawing.Point(909, 929);
            this.customButton2.Name = "customButton2";
            this.customButton2.Size = new System.Drawing.Size(209, 79);
            this.customButton2.TabIndex = 19;
            this.customButton2.Text = "SEARCH";
            this.customButton2.TextColor = System.Drawing.Color.YellowGreen;
            this.customButton2.UseVisualStyleBackColor = false;
            this.customButton2.Click += new System.EventHandler(this.customButton2_Click);
            // 
            // customButton1
            // 
            this.customButton1.BackColor = System.Drawing.Color.White;
            this.customButton1.BackgroundColor = System.Drawing.Color.White;
            this.customButton1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.customButton1.BorderRadius = 20;
            this.customButton1.BorderSize = 0;
            this.customButton1.FlatAppearance.BorderSize = 0;
            this.customButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customButton1.ForeColor = System.Drawing.Color.YellowGreen;
            this.customButton1.Location = new System.Drawing.Point(191, 929);
            this.customButton1.Name = "customButton1";
            this.customButton1.Size = new System.Drawing.Size(209, 79);
            this.customButton1.TabIndex = 18;
            this.customButton1.Text = "UPLOAD";
            this.customButton1.TextColor = System.Drawing.Color.YellowGreen;
            this.customButton1.UseVisualStyleBackColor = false;
            this.customButton1.Click += new System.EventHandler(this.customButton1_Click_1);
            // 
            // toggleButton2
            // 
            this.toggleButton2.AutoSize = true;
            this.toggleButton2.Location = new System.Drawing.Point(614, 941);
            this.toggleButton2.MinimumSize = new System.Drawing.Size(150, 50);
            this.toggleButton2.Name = "toggleButton2";
            this.toggleButton2.Size = new System.Drawing.Size(150, 50);
            this.toggleButton2.TabIndex = 15;
            this.toggleButton2.Text = "toggleButton2";
            this.toggleButton2.UseVisualStyleBackColor = true;
            this.toggleButton2.CheckedChanged += new System.EventHandler(this.toggleButton2_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.BlueViolet;
            this.ClientSize = new System.Drawing.Size(1978, 1144);
            this.Controls.Add(this.customButton2);
            this.Controls.Add(this.customButton1);
            this.Controls.Add(this.BMLabel);
            this.Controls.Add(this.KMPLabel);
            this.Controls.Add(this.toggleButton2);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timeExecutionLabel);
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
        private System.Windows.Forms.Label timeExecutionLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label resultLabel;
        private CustomControl.ToggleButton toggleButton2;
        private System.Windows.Forms.Label KMPLabel;
        private System.Windows.Forms.Label BMLabel;
        private CustomControl.CustomButton customButton1;
        private CustomControl.CustomButton customButton2;
    }
}

