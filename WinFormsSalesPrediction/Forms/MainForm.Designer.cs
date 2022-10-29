namespace WinFormsSalesPrediction
{
    partial class MainForm
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
            this.lblPredictionator = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblDay = new System.Windows.Forms.Label();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.txtMonth = new System.Windows.Forms.TextBox();
            this.txtDay = new System.Windows.Forms.TextBox();
            this.btnPredict = new System.Windows.Forms.Button();
            this.lblPrediction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPredictionator
            // 
            this.lblPredictionator.AutoSize = true;
            this.lblPredictionator.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPredictionator.Location = new System.Drawing.Point(36, 26);
            this.lblPredictionator.Name = "lblPredictionator";
            this.lblPredictionator.Size = new System.Drawing.Size(229, 37);
            this.lblPredictionator.TabIndex = 0;
            this.lblPredictionator.Text = "Predictionator";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.Location = new System.Drawing.Point(39, 78);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(47, 20);
            this.lblYear.TabIndex = 1;
            this.lblYear.Text = "Year:";
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.Location = new System.Drawing.Point(39, 104);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(58, 20);
            this.lblMonth.TabIndex = 2;
            this.lblMonth.Text = "Month:";
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = true;
            this.lblDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDay.Location = new System.Drawing.Point(39, 130);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(41, 20);
            this.lblDay.TabIndex = 3;
            this.lblDay.Text = "Day:";
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(125, 80);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(126, 20);
            this.txtYear.TabIndex = 4;
            this.txtYear.Text = "2022";
            // 
            // txtMonth
            // 
            this.txtMonth.Location = new System.Drawing.Point(125, 106);
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(126, 20);
            this.txtMonth.TabIndex = 5;
            this.txtMonth.Text = "10";
            // 
            // txtDay
            // 
            this.txtDay.Location = new System.Drawing.Point(125, 132);
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(126, 20);
            this.txtDay.TabIndex = 6;
            this.txtDay.Text = "28";
            // 
            // btnPredict
            // 
            this.btnPredict.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPredict.Location = new System.Drawing.Point(125, 159);
            this.btnPredict.Name = "btnPredict";
            this.btnPredict.Size = new System.Drawing.Size(88, 35);
            this.btnPredict.TabIndex = 7;
            this.btnPredict.Text = "Predict";
            this.btnPredict.UseVisualStyleBackColor = true;
            this.btnPredict.Click += new System.EventHandler(this.btnPredict_Click);
            // 
            // lblPrediction
            // 
            this.lblPrediction.AutoSize = true;
            this.lblPrediction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrediction.Location = new System.Drawing.Point(39, 210);
            this.lblPrediction.Name = "lblPrediction";
            this.lblPrediction.Size = new System.Drawing.Size(87, 20);
            this.lblPrediction.TabIndex = 8;
            this.lblPrediction.Text = "Prediction: ";
            this.lblPrediction.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 262);
            this.Controls.Add(this.lblPrediction);
            this.Controls.Add(this.btnPredict);
            this.Controls.Add(this.txtDay);
            this.Controls.Add(this.txtMonth);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.lblDay);
            this.Controls.Add(this.lblMonth);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.lblPredictionator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Prediction";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPredictionator;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.TextBox txtMonth;
        private System.Windows.Forms.TextBox txtDay;
        private System.Windows.Forms.Button btnPredict;
        private System.Windows.Forms.Label lblPrediction;
    }
}

