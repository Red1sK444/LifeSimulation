namespace OOP_LifeSimulation
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
            this.Start = new System.Windows.Forms.Button();
            this.DrawableMap = new System.Windows.Forms.PictureBox();
            this.Timer = new System.Timers.Timer();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.DrawableMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.Timer)).BeginInit();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(32, 21);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(158, 51);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start Simulation\r\n";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.button1_Click);
            // 
            // DrawableMap
            // 
            this.DrawableMap.Location = new System.Drawing.Point(219, 21);
            this.DrawableMap.Name = "DrawableMap";
            this.DrawableMap.Size = new System.Drawing.Size(10000, 10000);
            this.DrawableMap.TabIndex = 1;
            this.DrawableMap.TabStop = false;
            this.DrawableMap.Click += new System.EventHandler(this.DrawableMap_Click);
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.SynchronizingObject = this;
            this.Timer.Elapsed += new System.Timers.ElapsedEventHandler(this.Timer_Elapsed);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 126);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 51);
            this.button1.TabIndex = 2;
            this.button1.Text = "IncreaseScale";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(32, 226);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 51);
            this.button2.TabIndex = 3;
            this.button2.Text = "DecreaseScale";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(59, 339);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(3, 408);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 369);
            this.label1.TabIndex = 5;
            this.label1.Text = "Info:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1580, 1421);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DrawableMap);
            this.Controls.Add(this.Start);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize) (this.DrawableMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.Timer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.TextBox textBox1;

        private System.Windows.Forms.Button button2;

        private System.Windows.Forms.Button button1;

        private System.Timers.Timer Timer;

        private System.Windows.Forms.Button Start;

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox DrawableMap;
    }
}