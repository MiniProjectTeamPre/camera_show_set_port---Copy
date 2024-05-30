namespace camera_show_set_port {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.OneByOne = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.onebyone_ = new System.Windows.Forms.ToolStripMenuItem();
            this.set_form_cmd_to_arduino = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.set_form_cmd_to_arduino_rgbset = new System.Windows.Forms.ToolStripMenuItem();
            this.set_form_cmd_to_arduino_23017 = new System.Windows.Forms.ToolStripMenuItem();
            this.form_set_process_color = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.form_set_process_red = new System.Windows.Forms.ToolStripMenuItem();
            this.form_set_process_green = new System.Windows.Forms.ToolStripMenuItem();
            this.form_set_process_black = new System.Windows.Forms.ToolStripMenuItem();
            this.form_set_process_white = new System.Windows.Forms.ToolStripMenuItem();
            this.button3 = new System.Windows.Forms.Button();
            this.OneByOne.SuspendLayout();
            this.set_form_cmd_to_arduino.SuspendLayout();
            this.form_set_process_color.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(177, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "set";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.ContextMenuStrip = this.OneByOne;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.button2.Location = new System.Drawing.Point(195, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(185, 52);
            this.button2.TabIndex = 1;
            this.button2.Text = "run";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OneByOne
            // 
            this.OneByOne.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onebyone_});
            this.OneByOne.Name = "OneByOne";
            this.OneByOne.Size = new System.Drawing.Size(161, 26);
            // 
            // onebyone_
            // 
            this.onebyone_.Checked = true;
            this.onebyone_.CheckOnClick = true;
            this.onebyone_.CheckState = System.Windows.Forms.CheckState.Checked;
            this.onebyone_.Name = "onebyone_";
            this.onebyone_.Size = new System.Drawing.Size(160, 22);
            this.onebyone_.Text = "1 head 1 camera";
            this.onebyone_.Click += new System.EventHandler(this.onebyone__Click);
            // 
            // set_form_cmd_to_arduino
            // 
            this.set_form_cmd_to_arduino.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.set_form_cmd_to_arduino_rgbset,
            this.set_form_cmd_to_arduino_23017});
            this.set_form_cmd_to_arduino.Name = "set_form_cmd_to_arduino";
            this.set_form_cmd_to_arduino.Size = new System.Drawing.Size(108, 48);
            // 
            // set_form_cmd_to_arduino_rgbset
            // 
            this.set_form_cmd_to_arduino_rgbset.Name = "set_form_cmd_to_arduino_rgbset";
            this.set_form_cmd_to_arduino_rgbset.Size = new System.Drawing.Size(107, 22);
            this.set_form_cmd_to_arduino_rgbset.Text = "rgbset";
            this.set_form_cmd_to_arduino_rgbset.Click += new System.EventHandler(this.set_form_cmd_to_arduino_rgbset_Click);
            // 
            // set_form_cmd_to_arduino_23017
            // 
            this.set_form_cmd_to_arduino_23017.Name = "set_form_cmd_to_arduino_23017";
            this.set_form_cmd_to_arduino_23017.Size = new System.Drawing.Size(107, 22);
            this.set_form_cmd_to_arduino_23017.Text = "23017";
            this.set_form_cmd_to_arduino_23017.Click += new System.EventHandler(this.set_form_cmd_to_arduino_23017_Click);
            // 
            // form_set_process_color
            // 
            this.form_set_process_color.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.form_set_process_red,
            this.form_set_process_green,
            this.form_set_process_black,
            this.form_set_process_white});
            this.form_set_process_color.Name = "form_set_process_color";
            this.form_set_process_color.Size = new System.Drawing.Size(105, 92);
            // 
            // form_set_process_red
            // 
            this.form_set_process_red.Name = "form_set_process_red";
            this.form_set_process_red.Size = new System.Drawing.Size(104, 22);
            this.form_set_process_red.Text = "red";
            this.form_set_process_red.Click += new System.EventHandler(this.form_set_process_red_Click);
            // 
            // form_set_process_green
            // 
            this.form_set_process_green.Name = "form_set_process_green";
            this.form_set_process_green.Size = new System.Drawing.Size(104, 22);
            this.form_set_process_green.Text = "green";
            this.form_set_process_green.Click += new System.EventHandler(this.form_set_process_green_Click);
            // 
            // form_set_process_black
            // 
            this.form_set_process_black.Name = "form_set_process_black";
            this.form_set_process_black.Size = new System.Drawing.Size(104, 22);
            this.form_set_process_black.Text = "black";
            this.form_set_process_black.Click += new System.EventHandler(this.form_set_process_black_Click);
            // 
            // form_set_process_white
            // 
            this.form_set_process_white.Name = "form_set_process_white";
            this.form_set_process_white.Size = new System.Drawing.Size(104, 22);
            this.form_set_process_white.Text = "white";
            this.form_set_process_white.Click += new System.EventHandler(this.form_set_process_white_Click);
            // 
            // button3
            // 
            this.button3.ContextMenuStrip = this.OneByOne;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.button3.Location = new System.Drawing.Point(101, 128);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(185, 52);
            this.button3.TabIndex = 3;
            this.button3.Text = "manual";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 78);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.OneByOne.ResumeLayout(false);
            this.set_form_cmd_to_arduino.ResumeLayout(false);
            this.form_set_process_color.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip set_form_cmd_to_arduino;
        private System.Windows.Forms.ToolStripMenuItem set_form_cmd_to_arduino_rgbset;
        private System.Windows.Forms.ToolStripMenuItem set_form_cmd_to_arduino_23017;
        private System.Windows.Forms.ContextMenuStrip form_set_process_color;
        private System.Windows.Forms.ToolStripMenuItem form_set_process_red;
        private System.Windows.Forms.ToolStripMenuItem form_set_process_green;
        private System.Windows.Forms.ToolStripMenuItem form_set_process_black;
        private System.Windows.Forms.ToolStripMenuItem form_set_process_white;
        private System.Windows.Forms.ContextMenuStrip OneByOne;
        private System.Windows.Forms.ToolStripMenuItem onebyone_;
        private System.Windows.Forms.Button button3;
    }
}

