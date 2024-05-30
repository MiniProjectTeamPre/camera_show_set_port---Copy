using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace camera_show_set_port {
    public partial class Form2_manual : Form {
        public Form2_manual() {
            InitializeComponent();
        }

        private VideoCapture capture = null;
        private VideoCapture.API captureApi = VideoCapture.API.Any;
        private Image<Bgr, Byte> img;
        public int num_camera = 0;
        private void Form2_manual_Load(object sender, EventArgs e) {
            for (int i = 0; i < num_camera; i++) {
                comboBox1.Items.Add(i);
                comboBox2.Items.Add(i);
                comboBox3.Items.Add(i);
                comboBox4.Items.Add(i);
                comboBox5.Items.Add(i);
                comboBox6.Items.Add(i);
                comboBox7.Items.Add(i);
                comboBox8.Items.Add(i);
                comboBox9.Items.Add(i);
                comboBox10.Items.Add(i);
            }
            try { comboBox1.Text = File.ReadAllText("../config/test_head_1_port.txt"); } catch { comboBox1.SelectedIndex = 0; }
            try { comboBox2.Text = File.ReadAllText("../config/test_head_2_port.txt"); } catch { comboBox2.SelectedIndex = 0; }
            try { comboBox3.Text = File.ReadAllText("../config/test_head_3_port.txt"); } catch { comboBox3.SelectedIndex = 0; }
            try { comboBox4.Text = File.ReadAllText("../config/test_head_4_port.txt"); } catch { comboBox4.SelectedIndex = 0; }
            try { comboBox5.Text = File.ReadAllText("../config/test_head_5_port.txt"); } catch { comboBox5.SelectedIndex = 0; }
            try { comboBox6.Text = File.ReadAllText("../config/test_head_1_port_read2d.txt"); } catch { comboBox6.SelectedIndex = 0; }
            try { comboBox7.Text = File.ReadAllText("../config/test_head_2_port_read2d.txt"); } catch { comboBox7.SelectedIndex = 0; }
            try { comboBox8.Text = File.ReadAllText("../config/test_head_3_port_read2d.txt"); } catch { comboBox8.SelectedIndex = 0; }
            try { comboBox9.Text = File.ReadAllText("../config/test_head_4_port_read2d.txt"); } catch { comboBox9.SelectedIndex = 0; }
            try { comboBox10.Text = File.ReadAllText("../config/test_head_5_port_read2d.txt"); } catch { comboBox10.SelectedIndex = 0; }

            Application.Idle += run;
        }

        private int flag_run = 1;
        private bool flag_click = false;
        private void run(object sender, EventArgs e) {
            if (!this.Visible) return;
            if (flag_click) {
                capture.Dispose();
                flag_click = false;
                Thread.Sleep(150);
            }
            ComboBox c = new ComboBox();
            PictureBox p = new PictureBox();
            switch (flag_run) {
                case 1: c = comboBox1; p = pictureBox1; break;
                case 2: c = comboBox2; p = pictureBox2; break;
                case 3: c = comboBox3; p = pictureBox3; break;
                case 4: c = comboBox4; p = pictureBox4; break;
                case 5: c = comboBox5; p = pictureBox5; break;
                case 6: c = comboBox6; p = pictureBox6; break;
                case 7: c = comboBox7; p = pictureBox7; break;
                case 8: c = comboBox8; p = pictureBox8; break;
                case 9: c = comboBox9; p = pictureBox9; break;
                case 10: c = comboBox10; p = pictureBox10; break;
            }
            if (capture == null || capture.Ptr == IntPtr.Zero || capture.Width == 0) {
                try { capture = new VideoCapture(Convert.ToInt32(c.Text), captureApi); } catch { }
                Thread.Sleep(50);
                return;
            }
            Mat frame;
            try {
                frame = capture.QueryFrame();
                img = frame.ToImage<Bgr, Byte>();
            } catch {
                MessageBox.Show("ไม่สามารถเปิดกล้องได้");
                return;
            }
            p.Image = img.Bitmap;
            this.Text = flag_run + "  " + c.Text;
            Thread.Sleep(10);
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 1;
        }
        private void pictureBox2_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 2;
        }
        private void pictureBox3_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 3;
        }
        private void pictureBox4_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 4;
        }
        private void pictureBox5_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 5;
        }
        private void pictureBox6_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 6;
        }
        private void pictureBox7_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 7;
        }
        private void pictureBox8_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 8;
        }
        private void pictureBox9_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 9;
        }
        private void pictureBox10_Click(object sender, EventArgs e) {
            flag_click = true;
            flag_run = 10;
        }

        private void save_port(int j) {
            GroupBox g = new GroupBox();
            ComboBox c = new ComboBox();
            switch (j) {
                case 1: g = groupBox1; c = comboBox1; break;
                case 2: g = groupBox2; c = comboBox2; break;
                case 3: g = groupBox3; c = comboBox3; break;
                case 4: g = groupBox4; c = comboBox4; break;
                case 5: g = groupBox5; c = comboBox5; break;
                case 6: g = groupBox6; c = comboBox6; break;
                case 7: g = groupBox7; c = comboBox7; break;
                case 8: g = groupBox8; c = comboBox8; break;
                case 9: g = groupBox9; c = comboBox9; break;
                case 10: g = groupBox10; c = comboBox10; break;
            }
            if (!g.Enabled) return;
            if (groupBox6.Enabled) {
                string f = "";
                if (j > 5) { f = "_read2d"; j -= 5; }
                File.WriteAllText("../config/test_head_" + j + "_port" + f + ".txt", c.Text);
            } else {
                File.WriteAllText("../config/test_head_" + j + "_port.txt", c.Text);
                File.WriteAllText("../config/test_head_" + j + "_port_read2d.txt", c.Text);
            }
        }

        private void Form2_manual_FormClosed(object sender, FormClosedEventArgs e) {
            capture.Dispose();
            for (int i = 1; i <= 10; i++) {
                save_port(i);
            }
        }
    }
}
