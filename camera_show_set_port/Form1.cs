using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace camera_show_set_port
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private VideoCapture.API captureApi = VideoCapture.API.Any;
        private void Form1_Load(object sender, EventArgs e)
        {
            bool start_program = true;
            try { File.ReadAllText("../config/camera_show_set_port_start_program.txt"); } catch { start_program = false; }
            //if (!start_program) button2.Visible = false;

            ComputerInfo computerInfo = new ComputerInfo();
            if (!computerInfo.OSFullName.Contains("Windows 7")) captureApi = VideoCapture.API.DShow;
            try { onebyone_.Checked = Convert.ToBoolean(File.ReadAllText("../config/camera_show_onebyone.txt")); } catch { }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            form_set_show();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string gg = "";
            gg += "นำชิ้นงานออกจากเครื่องเทสแล้วใช่หรือไม่\r\n";
            MessageBox.Show(gg);
            this.Hide();
            form_run_main();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            form_manual_show();
        }

        #region form_set
        private string path = "../config/camera_show_set_port_";
        Form form_set_form = new Form();
        NumericUpDown form_set_numer_numhead = new NumericUpDown();
        NumericUpDown form_set_numer_head = new NumericUpDown();
        NumericUpDown form_set_numer_port = new NumericUpDown();
        Button form_set_button_open_camera = new Button();
        TextBox form_set_textbox_light = new TextBox();
        TextBox form_set_textbox_mark = new TextBox();
        private SerialPort form_set_serial;
        TextBox form_set_textbox_process = new TextBox();
        TrackBar form_set_trackbar_process = new TrackBar();
        CheckBox form_set_check_read2d = new CheckBox();
        HScrollBar form_set_scrolbar_contrast = new HScrollBar();
        Label form_set_label_contrast = new Label();
        HScrollBar form_set_scrolbar_brightness = new HScrollBar();
        Label form_set_label_brightness = new Label();
        HScrollBar form_set_scrolbar_focus = new HScrollBar();
        Label form_set_label_focus = new Label();
        HScrollBar form_set_scrolbar_exposure = new HScrollBar();
        Label form_set_label_exposure = new Label();
        HScrollBar form_set_scrolbar_saturation = new HScrollBar();
        Label form_set_label_saturation = new Label();
        HScrollBar form_set_scrolbar_sharpness = new HScrollBar();
        Label form_set_label_sharpness = new Label();
        HScrollBar form_set_scrolbar_gain = new HScrollBar();
        Label form_set_label_gain = new Label();
        HScrollBar form_set_scrolbar_gamma = new HScrollBar();
        Label form_set_label_gamma = new Label();
        private void form_set_show()
        {
            Label form_set_label_numhead = new Label();
            form_set_label_numhead.Text = "num head";
            form_set_label_numhead.Location = new Point(10, 10);
            form_set_label_numhead.Size = new Size(70, 25);
            form_set_numer_numhead = new NumericUpDown();
            form_set_numer_numhead.ReadOnly = true;
            form_set_numer_numhead.Location = new Point(80, 8);
            form_set_numer_numhead.Minimum = 1;
            form_set_numer_numhead.Size = new Size(40, 20);
            try { form_set_numer_numhead.Value = Convert.ToInt32(File.ReadAllText("../config/num_head.txt")); } catch { }

            Label form_set_label_head = new Label();
            form_set_label_head.Text = "head";
            form_set_label_head.Location = new Point(10, 50);
            form_set_label_head.Size = new Size(70, 25);
            form_set_numer_head = new NumericUpDown();
            form_set_numer_head.ValueChanged += Form_set_numer_head_ValueChanged;
            form_set_numer_head.ReadOnly = true;
            form_set_numer_head.Location = new Point(80, 48);
            form_set_numer_head.Minimum = 1;
            form_set_numer_head.Size = new Size(40, 20);

            Label form_set_label_port = new Label();
            form_set_label_port.Text = "port";
            form_set_label_port.Location = new Point(10, 90);
            form_set_label_port.Size = new Size(70, 25);
            form_set_numer_port = new NumericUpDown();
            form_set_numer_port.ReadOnly = true;
            form_set_numer_port.Location = new Point(80, 88);
            form_set_numer_port.Size = new Size(40, 20);
            try { form_set_numer_port.Value = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_port_read2d.txt")); } catch { }

            form_set_button_open_camera = new Button();
            form_set_button_open_camera.Click += Form_set_button_open_camera_Click;
            form_set_button_open_camera.Text = "open camera";
            form_set_button_open_camera.Location = new Point(150, 7);
            form_set_button_open_camera.Size = new Size(170, 70);

            Button form_set_button_save = new Button();
            form_set_button_save.Click += Form_set_button_save_Click;
            form_set_button_save.Text = "save";
            form_set_button_save.Location = new Point(150, 85);
            form_set_button_save.Size = new Size(110, 30);

            Button form_set_button_explain = new Button();
            form_set_button_explain.Click += Form_set_button_explain_Click;
            form_set_button_explain.Text = "Explain";
            form_set_button_explain.Location = new Point(270, 85);
            form_set_button_explain.Size = new Size(50, 30);

            form_set_textbox_light = new TextBox();
            form_set_textbox_light.ContextMenuStrip = set_form_cmd_to_arduino;
            form_set_textbox_light.KeyDown += Form_set_textbox_light_KeyDown;
            form_set_textbox_light.Location = new Point(10, 130);
            form_set_textbox_light.Size = new Size(120, form_set_textbox_light.Size.Height);
            try { form_set_textbox_light.Text = File.ReadAllText(path + form_set_numer_head.Value + "_light_read2d.txt"); } catch { form_set_textbox_light.Text = "rgbset,1,0,255,0,0"; }
            form_set_textbox_light.Enabled = false;
            ManagementObjectSearcher objOSDetails2 = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'");
            ManagementObjectCollection osDetailsCollection2 = objOSDetails2.Get();
            foreach (ManagementObject usblist in osDetailsCollection2)
            {
                if (usblist["Description"].ToString() != "USB-SERIAL CH340" && usblist["Description"].ToString() != "USB Serial Port") continue;
                string[] arrport = usblist.GetPropertyValue("NAME").ToString().Split('(', ')');
                form_set_serial = new SerialPort(arrport[1]);
                form_set_serial.BaudRate = 9600;
                form_set_serial.DataBits = 8;
                Stopwatch s = new Stopwatch();
                s.Restart();
                while (s.ElapsedMilliseconds < 2500)
                {
                    try { form_set_serial.Close(); } catch { }
                    try { form_set_serial.Open(); } catch { Thread.Sleep(50); continue; }
                    s.Stop();
                    break;
                }
                if (!s.IsRunning) form_set_textbox_light.Enabled = true;
                break;
            }
            Button form_set_button_light_on = new Button();
            form_set_button_light_on.Click += Form_set_button_light_on_Click;
            if (form_set_textbox_light.Enabled) form_set_button_light_on.Enabled = true;
            else form_set_button_light_on.Enabled = false;
            form_set_button_light_on.Text = "on";
            form_set_button_light_on.Location = new Point(135, 128);
            form_set_button_light_on.Size = new Size(40, 23);
            Button form_set_button_light_off = new Button();
            form_set_button_light_off.Click += Form_set_button_light_off_Click;
            if (form_set_textbox_light.Enabled) form_set_button_light_off.Enabled = true;
            else form_set_button_light_off.Enabled = false;
            form_set_button_light_off.Text = "off";
            form_set_button_light_off.Location = new Point(180, 128);
            form_set_button_light_off.Size = new Size(40, 23);

            form_set_textbox_mark = new TextBox();
            form_set_textbox_mark.Location = new Point(230, 130);
            form_set_textbox_mark.Size = new Size(88, form_set_textbox_mark.Size.Height);
            form_set_textbox_mark.TextAlign = HorizontalAlignment.Right;
            try { form_set_textbox_mark.Text = File.ReadAllText(path + form_set_numer_head.Value + "_mark_read2d.txt"); } catch { form_set_textbox_mark.Text = "0"; }

            form_set_textbox_process = new TextBox();
            form_set_textbox_process.KeyDown += Form_set_textbox_process_KeyDown;
            form_set_textbox_process.ContextMenuStrip = form_set_process_color;
            form_set_textbox_process.Location = new Point(10, 170);
            form_set_textbox_process.Size = new Size(140, form_set_textbox_process.Size.Height);
            try { form_set_textbox_process.Text = File.ReadAllText(path + form_set_numer_head.Value + "_process_read2d.txt"); } catch { form_set_textbox_process.Text = "0,60,0,255,150,255"; }

            Label form_set_label_rgb = new Label();
            form_set_label_rgb.Text = "rgb";
            form_set_label_rgb.Location = new Point(form_set_textbox_process.Location.X + 150, 171);
            form_set_label_rgb.Size = new Size(22, 25);

            form_set_trackbar_process = new TrackBar();
            form_set_trackbar_process.ValueChanged += Form_set_trackbar_process_ValueChanged;
            form_set_trackbar_process.Location = new Point(form_set_label_rgb.Location.X + 15, 166);
            form_set_trackbar_process.Size = new Size(50, form_set_trackbar_process.Size.Height);
            form_set_trackbar_process.Maximum = 1;
            try { form_set_trackbar_process.Value = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_process_rgb_read2d.txt")); } catch { form_set_trackbar_process.Value = 1; }

            Label form_set_label_hsv = new Label();
            form_set_label_hsv.Text = "hsv";
            form_set_label_hsv.Location = new Point(form_set_trackbar_process.Location.X + 47, 171);
            form_set_label_hsv.Size = new Size(24, 25);

            form_set_check_read2d = new CheckBox();
            form_set_check_read2d.CheckedChanged += Form_set_check_read2d_CheckedChanged;
            form_set_check_read2d.Location = new Point(263, 172);
            form_set_check_read2d.Size = new Size(14, 14);
            form_set_check_read2d.Checked = true;
            if (onebyone_.Checked) form_set_check_read2d.Enabled = false;

            Label form_set_label_read2d = new Label();
            form_set_label_read2d.Text = "read2d";
            form_set_label_read2d.Location = new Point(280, 171);
            form_set_label_read2d.Size = new Size(50, 25);

            form_set_scrolbar_contrast = new HScrollBar();
            form_set_scrolbar_contrast.Scroll += Form_set_scrolbar_contrast_Scroll;
            form_set_scrolbar_contrast.Location = new Point(10, 220);
            form_set_scrolbar_contrast.Size = new Size(250, form_set_scrolbar_contrast.Size.Height);
            form_set_scrolbar_contrast.LargeChange = 1;
            try { form_set_scrolbar_contrast.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_contrast_min_read2d.txt")); } catch { form_set_scrolbar_contrast.Minimum = -999; }
            try { form_set_scrolbar_contrast.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_contrast_max_read2d.txt")); } catch { form_set_scrolbar_contrast.Maximum = 999; }
            try { form_set_scrolbar_contrast.Value = Convert.ToInt32(File.ReadAllText(path + "1_contrast_read2d.txt")); } catch { form_set_scrolbar_contrast.Value = form_set_scrolbar_contrast.Minimum; }
            Label form_set_label_contrast_ = new Label();
            form_set_label_contrast_.Text = "contrast";
            form_set_label_contrast_.Location = new Point(10, form_set_scrolbar_contrast.Location.Y - 15);
            form_set_label_contrast = new Label();
            form_set_label_contrast.Text = form_set_scrolbar_contrast.Value.ToString();
            form_set_label_contrast.Location = new Point(290, form_set_scrolbar_contrast.Location.Y + 3);

            form_set_scrolbar_brightness = new HScrollBar();
            form_set_scrolbar_brightness.Scroll += Form_set_scrolbar_brightness_Scroll;
            form_set_scrolbar_brightness.Location = new Point(10, form_set_scrolbar_contrast.Location.Y + 50);
            form_set_scrolbar_brightness.Size = new Size(250, form_set_scrolbar_brightness.Size.Height);
            form_set_scrolbar_brightness.LargeChange = 1;
            try { form_set_scrolbar_brightness.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_brightness_min_read2d.txt")); } catch { form_set_scrolbar_brightness.Minimum = -999; }
            try { form_set_scrolbar_brightness.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_brightness_max_read2d.txt")); } catch { form_set_scrolbar_brightness.Maximum = 999; }
            try { form_set_scrolbar_brightness.Value = Convert.ToInt32(File.ReadAllText(path + "1_brightness_read2d.txt")); } catch { form_set_scrolbar_brightness.Value = form_set_scrolbar_brightness.Minimum; }
            Label form_set_label_brightness_ = new Label();
            form_set_label_brightness_.Text = "brightness";
            form_set_label_brightness_.Location = new Point(10, form_set_scrolbar_brightness.Location.Y - 15);
            form_set_label_brightness = new Label();
            form_set_label_brightness.Text = form_set_scrolbar_brightness.Value.ToString();
            form_set_label_brightness.Location = new Point(290, form_set_scrolbar_brightness.Location.Y + 3);

            form_set_scrolbar_focus = new HScrollBar();
            form_set_scrolbar_focus.Scroll += Form_set_scrolbar_focus_Scroll;
            form_set_scrolbar_focus.Location = new Point(10, form_set_scrolbar_brightness.Location.Y + 50);
            form_set_scrolbar_focus.Size = new Size(250, form_set_scrolbar_focus.Size.Height);
            form_set_scrolbar_focus.LargeChange = 1;
            try { form_set_scrolbar_focus.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_focus_min_read2d.txt")); } catch { form_set_scrolbar_focus.Minimum = -999; }
            try { form_set_scrolbar_focus.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_focus_max_read2d.txt")); } catch { form_set_scrolbar_focus.Maximum = 999; }
            try { form_set_scrolbar_focus.Value = Convert.ToInt32(File.ReadAllText(path + "1_focus_read2d.txt")); } catch { form_set_scrolbar_focus.Value = form_set_scrolbar_focus.Minimum; }
            Label form_set_label_focus_ = new Label();
            form_set_label_focus_.Text = "focus";
            form_set_label_focus_.Location = new Point(10, form_set_scrolbar_focus.Location.Y - 15);
            form_set_label_focus = new Label();
            form_set_label_focus.Text = form_set_scrolbar_focus.Value.ToString();
            form_set_label_focus.Location = new Point(290, form_set_scrolbar_focus.Location.Y + 3);

            form_set_scrolbar_exposure = new HScrollBar();
            form_set_scrolbar_exposure.Scroll += Form_set_scrolbar_exposure_Scroll;
            form_set_scrolbar_exposure.Location = new Point(10, form_set_scrolbar_focus.Location.Y + 50);
            form_set_scrolbar_exposure.Size = new Size(250, form_set_scrolbar_exposure.Size.Height);
            form_set_scrolbar_exposure.LargeChange = 1;
            try { form_set_scrolbar_exposure.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_exposure_min_read2d.txt")); } catch { form_set_scrolbar_exposure.Minimum = -999; }
            try { form_set_scrolbar_exposure.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_exposure_max_read2d.txt")); } catch { form_set_scrolbar_exposure.Maximum = 999; }
            try { form_set_scrolbar_exposure.Value = Convert.ToInt32(File.ReadAllText(path + "1_exposure_read2d.txt")); } catch { form_set_scrolbar_exposure.Value = form_set_scrolbar_exposure.Minimum; }
            Label form_set_label_exposure_ = new Label();
            form_set_label_exposure_.Text = "exposure";
            form_set_label_exposure_.Location = new Point(10, form_set_scrolbar_exposure.Location.Y - 15);
            form_set_label_exposure = new Label();
            form_set_label_exposure.Text = form_set_scrolbar_exposure.Value.ToString();
            form_set_label_exposure.Location = new Point(290, form_set_scrolbar_exposure.Location.Y + 3);

            form_set_scrolbar_saturation = new HScrollBar();
            form_set_scrolbar_saturation.Scroll += Form_set_scrolbar_saturation_Scroll;
            form_set_scrolbar_saturation.Location = new Point(10, form_set_scrolbar_exposure.Location.Y + 50);
            form_set_scrolbar_saturation.Size = new Size(250, form_set_scrolbar_saturation.Size.Height);
            form_set_scrolbar_saturation.LargeChange = 1;
            try { form_set_scrolbar_saturation.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_saturation_min_read2d.txt")); } catch { form_set_scrolbar_saturation.Minimum = -999; }
            try { form_set_scrolbar_saturation.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_saturation_max_read2d.txt")); } catch { form_set_scrolbar_saturation.Maximum = 999; }
            try { form_set_scrolbar_saturation.Value = Convert.ToInt32(File.ReadAllText(path + "1_saturation_read2d.txt")); } catch { form_set_scrolbar_saturation.Value = form_set_scrolbar_saturation.Minimum; }
            Label form_set_label_saturation_ = new Label();
            form_set_label_saturation_.Text = "saturation";
            form_set_label_saturation_.Location = new Point(10, form_set_scrolbar_saturation.Location.Y - 15);
            form_set_label_saturation = new Label();
            form_set_label_saturation.Text = form_set_scrolbar_saturation.Value.ToString();
            form_set_label_saturation.Location = new Point(290, form_set_scrolbar_saturation.Location.Y + 3);

            form_set_scrolbar_sharpness = new HScrollBar();
            form_set_scrolbar_sharpness.Scroll += Form_set_scrolbar_sharpness_Scroll;
            form_set_scrolbar_sharpness.Location = new Point(10, form_set_scrolbar_saturation.Location.Y + 50);
            form_set_scrolbar_sharpness.Size = new Size(250, form_set_scrolbar_sharpness.Size.Height);
            form_set_scrolbar_sharpness.LargeChange = 1;
            try { form_set_scrolbar_sharpness.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_sharpness_min_read2d.txt")); } catch { form_set_scrolbar_sharpness.Minimum = -999; }
            try { form_set_scrolbar_sharpness.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_sharpness_max_read2d.txt")); } catch { form_set_scrolbar_sharpness.Maximum = 999; }
            try { form_set_scrolbar_sharpness.Value = Convert.ToInt32(File.ReadAllText(path + "1_sharpness_read2d.txt")); } catch { form_set_scrolbar_sharpness.Value = form_set_scrolbar_sharpness.Minimum; }
            Label form_set_label_sharpness_ = new Label();
            form_set_label_sharpness_.Text = "sharpness";
            form_set_label_sharpness_.Location = new Point(10, form_set_scrolbar_sharpness.Location.Y - 15);
            form_set_label_sharpness = new Label();
            form_set_label_sharpness.Text = form_set_scrolbar_sharpness.Value.ToString();
            form_set_label_sharpness.Location = new Point(290, form_set_scrolbar_sharpness.Location.Y + 3);

            form_set_scrolbar_gain = new HScrollBar();
            form_set_scrolbar_gain.Scroll += Form_set_scrolbar_gain_Scroll;
            form_set_scrolbar_gain.Location = new Point(10, form_set_scrolbar_sharpness.Location.Y + 50);
            form_set_scrolbar_gain.Size = new Size(250, form_set_scrolbar_gain.Size.Height);
            form_set_scrolbar_gain.LargeChange = 1;
            try { form_set_scrolbar_gain.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_gain_min_read2d.txt")); } catch { form_set_scrolbar_gain.Minimum = -999; }
            try { form_set_scrolbar_gain.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_gain_max_read2d.txt")); } catch { form_set_scrolbar_gain.Maximum = 999; }
            try { form_set_scrolbar_gain.Value = Convert.ToInt32(File.ReadAllText(path + "1_gain_read2d.txt")); } catch { form_set_scrolbar_gain.Value = form_set_scrolbar_gain.Minimum; }
            Label form_set_label_gain_ = new Label();
            form_set_label_gain_.Text = "gain";
            form_set_label_gain_.Location = new Point(10, form_set_scrolbar_gain.Location.Y - 15);
            form_set_label_gain = new Label();
            form_set_label_gain.Text = form_set_scrolbar_gain.Value.ToString();
            form_set_label_gain.Location = new Point(290, form_set_scrolbar_gain.Location.Y + 3);

            form_set_scrolbar_gamma = new HScrollBar();
            form_set_scrolbar_gamma.Scroll += Form_set_scrolbar_gamma_Scroll;
            form_set_scrolbar_gamma.Location = new Point(10, form_set_scrolbar_gain.Location.Y + 50);
            form_set_scrolbar_gamma.Size = new Size(250, form_set_scrolbar_gamma.Size.Height);
            form_set_scrolbar_gamma.LargeChange = 1;
            try { form_set_scrolbar_gamma.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_gamma_min_read2d.txt")); } catch { form_set_scrolbar_gamma.Minimum = -999; }
            try { form_set_scrolbar_gamma.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_1_cam_gamma_max_read2d.txt")); } catch { form_set_scrolbar_gamma.Maximum = 999; }
            try { form_set_scrolbar_gamma.Value = Convert.ToInt32(File.ReadAllText(path + "1_gamma_read2d.txt")); } catch { form_set_scrolbar_gamma.Value = form_set_scrolbar_gamma.Minimum; }
            Label form_set_label_gamma_ = new Label();
            form_set_label_gamma_.Text = "gamma";
            form_set_label_gamma_.Location = new Point(10, form_set_scrolbar_gamma.Location.Y - 15);
            form_set_label_gamma = new Label();
            form_set_label_gamma.Text = form_set_scrolbar_gamma.Value.ToString();
            form_set_label_gamma.Location = new Point(290, form_set_scrolbar_gamma.Location.Y + 3);

            form_set_form = new Form();
            form_set_form.FormClosing += Form_set_form_FormClosing;
            form_set_form.Size = new Size(350, 650);
            form_set_form.Controls.Add(form_set_label_numhead);
            form_set_form.Controls.Add(form_set_numer_numhead);
            form_set_form.Controls.Add(form_set_label_head);
            form_set_form.Controls.Add(form_set_numer_head);
            form_set_form.Controls.Add(form_set_label_port);
            form_set_form.Controls.Add(form_set_numer_port);
            form_set_form.Controls.Add(form_set_button_open_camera);
            form_set_form.Controls.Add(form_set_button_save);
            form_set_form.Controls.Add(form_set_button_explain);
            form_set_form.Controls.Add(form_set_textbox_light);
            form_set_form.Controls.Add(form_set_button_light_on);
            form_set_form.Controls.Add(form_set_button_light_off);
            form_set_form.Controls.Add(form_set_textbox_mark);
            form_set_form.Controls.Add(form_set_textbox_process);
            form_set_form.Controls.Add(form_set_label_rgb);
            form_set_form.Controls.Add(form_set_trackbar_process);
            form_set_form.Controls.Add(form_set_label_hsv);
            form_set_form.Controls.Add(form_set_check_read2d);
            form_set_form.Controls.Add(form_set_label_read2d);
            form_set_form.Controls.Add(form_set_scrolbar_contrast);
            form_set_form.Controls.Add(form_set_label_contrast_);
            form_set_form.Controls.Add(form_set_label_contrast);
            form_set_form.Controls.Add(form_set_scrolbar_brightness);
            form_set_form.Controls.Add(form_set_label_brightness_);
            form_set_form.Controls.Add(form_set_label_brightness);
            form_set_form.Controls.Add(form_set_scrolbar_focus);
            form_set_form.Controls.Add(form_set_label_focus_);
            form_set_form.Controls.Add(form_set_label_focus);
            form_set_form.Controls.Add(form_set_scrolbar_exposure);
            form_set_form.Controls.Add(form_set_label_exposure_);
            form_set_form.Controls.Add(form_set_label_exposure);
            form_set_form.Controls.Add(form_set_scrolbar_saturation);
            form_set_form.Controls.Add(form_set_label_saturation_);
            form_set_form.Controls.Add(form_set_label_saturation);
            form_set_form.Controls.Add(form_set_scrolbar_sharpness);
            form_set_form.Controls.Add(form_set_label_sharpness_);
            form_set_form.Controls.Add(form_set_label_sharpness);
            form_set_form.Controls.Add(form_set_scrolbar_gain);
            form_set_form.Controls.Add(form_set_label_gain_);
            form_set_form.Controls.Add(form_set_label_gain);
            form_set_form.Controls.Add(form_set_scrolbar_gamma);
            form_set_form.Controls.Add(form_set_label_gamma_);
            form_set_form.Controls.Add(form_set_label_gamma);
            form_set_form.Show();
            form_set_form.Location = new Point(30, 30);
        }

        private void Form_set_check_read2d_CheckedChanged(object sender, EventArgs e)
        {
            form_set_numer_head_valuechanged_sup();
        }
        private void Form_set_trackbar_process_ValueChanged(object sender, EventArgs e)
        {
            form_camera_set_rgb();
        }
        private void Form_set_button_save_Click(object sender, EventArgs e)
        {
            string f = "";
            if (form_set_check_read2d.Checked) f = "_read2d";
            if (onebyone_.Checked)
            {
                File.WriteAllText("../config/test_head_" + form_set_numer_head.Value + "_port.txt", form_set_numer_port.Value.ToString());
                File.WriteAllText("../config/test_head_" + form_set_numer_head.Value + "_port_read2d.txt", form_set_numer_port.Value.ToString());
            }
            File.WriteAllText("../config/num_head.txt", form_set_numer_numhead.Value.ToString());
            File.WriteAllText("../config/test_head_" + form_set_numer_head.Value + "_port" + f + ".txt", form_set_numer_port.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_light" + f + ".txt", form_set_textbox_light.Text);
            File.WriteAllText(path + form_set_numer_head.Value + "_contrast" + f + ".txt", form_set_scrolbar_contrast.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_brightness" + f + ".txt", form_set_scrolbar_brightness.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_focus" + f + ".txt", form_set_scrolbar_focus.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_exposure" + f + ".txt", form_set_scrolbar_exposure.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_saturation" + f + ".txt", form_set_scrolbar_saturation.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_sharpness" + f + ".txt", form_set_scrolbar_sharpness.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_gain" + f + ".txt", form_set_scrolbar_gain.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_gamma" + f + ".txt", form_set_scrolbar_gamma.Value.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_mark" + f + ".txt", form_set_textbox_mark.Text);
            File.WriteAllText(path + form_set_numer_head.Value + "_process" + f + ".txt", form_set_textbox_process.Text);
            if (Convert.ToInt32(form_set_trackbar_process.Value) == 0)
                File.WriteAllText(path + form_set_numer_head.Value + "_process_rgb" + f + ".txt", "0");
            else File.WriteAllText(path + form_set_numer_head.Value + "_process_rgb" + f + ".txt", "1");
        }
        private void Form_set_button_explain_Click(object sender, EventArgs e) {
            string gg = "";
            gg += "ขั้นตอนการตั้งค่า\r\n";
            gg += "1.ระบุจำนวน num head ของเครื่องเทส\r\n";
            gg += "2.เลือก head ที่ต้องการจะเปิดกล้อง\r\n";
            gg += "3.เลือก port แล้วกด open จนกว่าจะเจอ port ที่ตรงกับ head นั้น\r\n";
            gg += "4.หลังจากเปิดกล้องแล้ว จึงจะสามารถปรับจูนค่ากล้องได้\r\n";
            gg += "5.หากยังไม่เปิดกล้องห้ามปรับจูนค่ากล้อง\r\n";
            gg += "6.ทุกครั้งที่ตั้งค่า head ใดๆเสร็จ ควรกดปุ่ม save เพื่อบันทึก\r\n";
            gg += "7.ปุ่ม save จะไม่ทำการบันทึกการปรับจูนค่ากล้องทั้งหมด\r\n";
            gg += "8.ควรจะจูนกล้อง ขณะที่เครื่องเทสไม่ได้กดชิ้นงาน\r\n";
            MessageBox.Show(gg);
        }
        private void Form_set_scrolbar_gamma_Scroll(object sender, ScrollEventArgs e)
        {
            form_set_label_gamma.Text = form_set_scrolbar_gamma.Value.ToString();
            if (capture == null) return;
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Gamma, form_set_scrolbar_gamma.Value); } catch { }
        }
        private void Form_set_scrolbar_gain_Scroll(object sender, ScrollEventArgs e)
        {
            form_set_label_gain.Text = form_set_scrolbar_gain.Value.ToString();
            if (capture == null) return;
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Gain, form_set_scrolbar_gain.Value); } catch { }
        }
        private void Form_set_scrolbar_sharpness_Scroll(object sender, ScrollEventArgs e)
        {
            form_set_label_sharpness.Text = form_set_scrolbar_sharpness.Value.ToString();
            if (capture == null) return;
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Sharpness, form_set_scrolbar_sharpness.Value); } catch { }
        }
        private void Form_set_scrolbar_saturation_Scroll(object sender, ScrollEventArgs e)
        {
            form_set_label_saturation.Text = form_set_scrolbar_saturation.Value.ToString();
            if (capture == null) return;
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Saturation, form_set_scrolbar_saturation.Value); } catch { }
        }
        private void Form_set_scrolbar_exposure_Scroll(object sender, ScrollEventArgs e)
        {
            form_set_label_exposure.Text = form_set_scrolbar_exposure.Value.ToString();
            if (capture == null) return;
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, form_set_scrolbar_exposure.Value); } catch { }
        }
        private void Form_set_scrolbar_focus_Scroll(object sender, ScrollEventArgs e)
        {
            form_set_label_focus.Text = form_set_scrolbar_focus.Value.ToString();
            if (capture == null) return;
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Focus, form_set_scrolbar_focus.Value); } catch { }
        }
        private void Form_set_scrolbar_brightness_Scroll(object sender, ScrollEventArgs e)
        {
            form_set_label_brightness.Text = form_set_scrolbar_brightness.Value.ToString();
            if (capture == null) return;
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Brightness, form_set_scrolbar_brightness.Value); } catch { }
        }
        private void Form_set_scrolbar_contrast_Scroll(object sender, ScrollEventArgs e)
        {
            form_set_label_contrast.Text = form_set_scrolbar_contrast.Value.ToString();
            if (capture == null) return;
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Contrast, form_set_scrolbar_contrast.Value); } catch { }
        }
        private void Form_set_button_light_off_Click(object sender, EventArgs e)
        {
            if (form_set_textbox_light.Text.Contains("rgbset")) form_set_light_send("rgbset,0,0,0,0,0");
            else form_set_light_send(form_set_textbox_light.Text.Substring(0, form_set_textbox_light.Text.Length - 1) + "0");
        }
        private void Form_set_button_light_on_Click(object sender, EventArgs e)
        {
            if (form_set_textbox_light.Text.Contains("rgbset")) form_set_light_send(form_set_textbox_light.Text);
            else form_set_light_send(form_set_textbox_light.Text.Substring(0, form_set_textbox_light.Text.Length - 1) + "1");
        }
        private void form_set_light_send(string cmd = "")
        {
            try
            {
                form_set_serial.DiscardInBuffer();
                form_set_serial.DiscardOutBuffer();
                form_set_serial.Write(cmd + "\n");
            }
            catch { }
            string rx = "";
            Stopwatch t = new Stopwatch();
            t.Restart();
            while (t.ElapsedMilliseconds < 2500)
            {
                try { rx = form_set_serial.ReadExisting(); } catch { }
                if (rx != "") { t.Stop(); break; }
                Thread.Sleep(50);
            }
            if (t.IsRunning) MessageBox.Show("arduino err");
            string sup_rx = "";
            t.Restart();
            while (t.ElapsedMilliseconds < 100)
            {
                try { sup_rx = form_set_serial.ReadExisting(); } catch { }
                if (sup_rx != "") { rx += sup_rx; t.Restart(); continue; }
                Thread.Sleep(50);
            }
        }
        private void Form_set_textbox_process_KeyDown(object sender, KeyEventArgs e)
        {
            form_camera_set_rgb();
        }
        private void Form_set_textbox_light_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            form_set_light_send(form_set_textbox_light.Text);
        }
        private void Form_set_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (form_camera_flag_run) { e.Cancel = true; return; }
            try { form_set_serial.Close(); } catch { }
            this.Show();
        }
        private void Form_set_button_open_camera_Click(object sender, EventArgs e)
        {
            form_set_button_open_camera.Enabled = false;
            form_camera_show();
        }
        private void Form_set_numer_head_ValueChanged(object sender, EventArgs e)
        {
            form_set_numer_head_valuechanged_sup();
        }
        private void form_set_numer_head_valuechanged_sup()
        {
            try { bool gg = form_set_form.Enabled; } catch { return; }
            string fedqq = "";
            if (form_set_check_read2d.Checked) fedqq = "_read2d";
            if (form_set_numer_head.Value > form_set_numer_numhead.Value) form_set_numer_head.Value = form_set_numer_numhead.Value;
            try
            {
                form_set_numer_port.Value = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_port" + fedqq + ".txt"));
            }
            catch { form_set_numer_port.Value = 0; }
            try
            {
                form_set_textbox_light.Text = File.ReadAllText(path + form_set_numer_head.Value + "_light" + fedqq + ".txt");
            }
            catch { form_set_textbox_light.Text = "rgbset,1,0,200,0,0"; }
            try { form_set_scrolbar_contrast.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_cam_contrast_min" + fedqq + ".txt")); } catch { }
            try { form_set_scrolbar_contrast.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_cam_contrast_max" + fedqq + ".txt")); } catch { }
            try { form_set_scrolbar_brightness.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_cam_brightness_min" + fedqq + ".txt")); } catch { }
            try { form_set_scrolbar_brightness.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_cam_brightness_max" + fedqq + ".txt")); } catch { }
            try { form_set_scrolbar_focus.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_cam_focus_min" + fedqq + ".txt")); } catch { }
            try { form_set_scrolbar_focus.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_cam_focus_max" + fedqq + ".txt")); } catch { }
            try { form_set_scrolbar_exposure.Minimum = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_cam_exposure_min" + fedqq + ".txt")); } catch { }
            try { form_set_scrolbar_exposure.Maximum = Convert.ToInt32(File.ReadAllText("../config/test_head_" + form_set_numer_head.Value + "_cam_exposure_max" + fedqq + ".txt")); } catch { }

            try
            {
                form_set_scrolbar_contrast.Value = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_contrast" + fedqq + ".txt"));
            }
            catch { form_set_scrolbar_contrast.Value = form_set_scrolbar_contrast.Minimum; }
            form_set_label_contrast.Text = form_set_scrolbar_contrast.Value.ToString();
            try
            {
                form_set_scrolbar_brightness.Value = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_brightness" + fedqq + ".txt"));
            }
            catch { form_set_scrolbar_brightness.Value = form_set_scrolbar_brightness.Minimum; }
            form_set_label_brightness.Text = form_set_scrolbar_brightness.Value.ToString();
            try
            {
                form_set_scrolbar_focus.Value = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_focus" + fedqq + ".txt"));
            }
            catch { form_set_scrolbar_focus.Value = form_set_scrolbar_focus.Minimum; }
            form_set_label_focus.Text = form_set_scrolbar_focus.Value.ToString();
            try
            {
                form_set_scrolbar_exposure.Value = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_exposure" + fedqq + ".txt"));
            }
            catch { form_set_scrolbar_exposure.Value = form_set_scrolbar_exposure.Minimum; }
            form_set_label_exposure.Text = form_set_scrolbar_exposure.Value.ToString();
            try
            {
                form_set_textbox_mark.Text = File.ReadAllText(path + form_set_numer_head.Value + "_mark" + fedqq + ".txt");
            }
            catch { form_set_textbox_mark.Text = "0"; }
            try
            {
                form_set_textbox_process.Text = File.ReadAllText(path + form_set_numer_head.Value + "_process" + fedqq + ".txt");
            }
            catch { form_set_textbox_process.Text = "0,60,0,255,150,255"; }
            try
            {
                form_set_trackbar_process.Value = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_process_rgb" + fedqq + ".txt"));
            }
            catch { form_set_trackbar_process.Value = 1; }
        }
        #endregion

        #region form_camera
        Form form_camera_form;
        private bool form_camera_flag_run = false;
        PictureBox form_camera_picture;
        private VideoCapture capture = null;
        private int frame_height = 600;
        private int frame_width = 600;
        private Image<Bgr, Byte> img;
        private Image<Hsv, Byte> img_hsv;
        private static Rectangle rect;
        private Bgr bgr_low = new Bgr(0, 100, 0);
        private Bgr bgr_high = new Bgr(100, 255, 50);
        private Hsv hsv_low = new Hsv(0, 0, 150);
        private Hsv hsv_high = new Hsv(60, 255, 255);
        private bool flag_hsv = true;
        bool IsMouseDown = false;
        Point StartLocation;
        Point EndLcation;
        private void form_camera_show()
        {
            string f = "";
            if (form_set_check_read2d.Checked) f = "_read2d";
            capture = new VideoCapture(Convert.ToInt32(form_set_numer_port.Value), captureApi);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, Convert.ToInt32(frame_height));
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, Convert.ToInt32(frame_width));
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Contrast, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_contrast" + f + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Brightness, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_brightness" + f + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Focus, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_focus" + f + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_exposure" + f + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Saturation, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_saturation" + f + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Sharpness, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_sharpness" + f + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Gain, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_gain" + f + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Gamma, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_gamma" + f + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, Convert.ToDouble(File.ReadAllText(path + form_set_numer_head.Value + "_exposure" + f + ".txt"))); } catch { }

            form_camera_picture = new PictureBox();
            form_camera_picture.MouseDown += Form_camera_picture_MouseDown;
            form_camera_picture.MouseMove += Form_camera_picture_MouseMove;
            form_camera_picture.MouseUp += Form_camera_picture_MouseUp;
            form_camera_picture.Paint += Form_camera_picture_Paint;
            form_camera_picture.Size = new Size(capture.Width, capture.Height);
            form_camera_picture.SizeMode = PictureBoxSizeMode.AutoSize;

            try { rect.X = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_rect_x" + f + ".txt")); } catch { rect.X = 0; }
            try { rect.Y = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_rect_y" + f + ".txt")); } catch { rect.Y = 0; }
            try { rect.Width = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_rect_width" + f + ".txt")); } catch { rect.Width = 0; }
            try { rect.Height = Convert.ToInt32(File.ReadAllText(path + form_set_numer_head.Value + "_rect_height" + f + ".txt")); } catch { rect.Height = 0; }
            form_camera_set_rgb();

            form_camera_form = new Form();
            form_camera_form.FormClosed += Form_camera_form_FormClosed;
            form_camera_form.Size = new Size(capture.Width, capture.Height);
            form_camera_form.StartPosition = FormStartPosition.CenterScreen;
            form_camera_form.Controls.Add(form_camera_picture);
            form_camera_form.Show();
            form_camera_flag_run = true;
            Application.Idle += form_camera_run;
        }
        private void form_camera_set_rgb()
        {
            if (form_set_trackbar_process.Value == 0)
            {
                flag_hsv = false;
                string[] bgr = form_set_textbox_process.Text.Split(',');
                bgr_low = new Bgr(Convert.ToInt32(bgr[4]), Convert.ToInt32(bgr[2]), Convert.ToInt32(bgr[0]));
                bgr_high = new Bgr(Convert.ToInt32(bgr[5]), Convert.ToInt32(bgr[3]), Convert.ToInt32(bgr[1]));
            }
            else
            {
                flag_hsv = true;
                string[] hsv = form_set_textbox_process.Text.Split(',');
                hsv_low = new Hsv(Convert.ToInt32(hsv[0]), Convert.ToInt32(hsv[2]), Convert.ToInt32(hsv[4]));
                hsv_high = new Hsv(Convert.ToInt32(hsv[1]), Convert.ToInt32(hsv[3]), Convert.ToInt32(hsv[5]));
            }
        }
        private Rectangle GetRectangle()
        {
            rect.X = Math.Min(StartLocation.X, EndLcation.X);
            rect.Y = Math.Min(StartLocation.Y, EndLcation.Y);
            rect.Width = Math.Abs(StartLocation.X - EndLcation.X);
            rect.Height = Math.Abs(StartLocation.Y - EndLcation.Y);
            return rect;
        }
        private void Form_camera_picture_Paint(object sender, PaintEventArgs e)
        {
            if (rect != null && IsMouseDown == true)
            {
                e.Graphics.DrawRectangle(Pens.Red, GetRectangle());
            }
        }
        private void Form_camera_picture_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsMouseDown != true) return;
            if (rect.Size.Width < 30 || rect.Size.Height < 30) return;
            Image<Bgr, byte> imgInput;
            EndLcation = e.Location;
            IsMouseDown = false;
            if (rect != null)
            {
                imgInput = img.Copy();
                imgInput.ROI = rect;
                Image<Bgr, byte> temp = imgInput.Copy();
            }
            string f = "";
            if (form_set_check_read2d.Checked) f = "_read2d";
            File.WriteAllText(path + form_set_numer_head.Value + "_rect_x" + f + ".txt", rect.X.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_rect_y" + f + ".txt", rect.Y.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_rect_width" + f + ".txt", rect.Width.ToString());
            File.WriteAllText(path + form_set_numer_head.Value + "_rect_height" + f + ".txt", rect.Height.ToString());
        }
        private void Form_camera_picture_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown == true)
            {
                EndLcation = e.Location;
                form_camera_picture.Invalidate();
            }
        }
        private void Form_camera_picture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            IsMouseDown = true;
            StartLocation = e.Location;
        }
        private void Form_camera_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            capture.Dispose();
            form_set_button_open_camera.Enabled = true;
            form_camera_flag_run = false;
        }
        private void form_camera_run(object sender, EventArgs e)
        {
            if (!form_camera_flag_run) return;
            if (capture == null || capture.Ptr == IntPtr.Zero || capture.Width == 0)
            {
                Thread.Sleep(50);
                return;
            }
            Mat frame;
            try
            {
                frame = capture.QueryFrame();
                img = frame.ToImage<Bgr, Byte>();
                img_hsv = frame.ToImage<Hsv, Byte>();
            }
            catch
            {
                MessageBox.Show("ไม่สามารถเปิดกล้องได้");
                return;
            }
            Graphics g = Graphics.FromImage(img.Bitmap);
            g.DrawRectangle(Pens.Lime, rect);
            Image<Bgr, byte> img_cut = null;
            Image<Hsv, byte> img_cut2 = null;
            Image<Bgr, byte> img1 = null;
            Image<Hsv, byte> img2 = null;
            int redpixels = 0;
            if (flag_hsv == false)
            {
                img_cut = img.Copy();
                img_cut.ROI = rect;
                img1 = img_cut.Copy();
                try { redpixels = img1.InRange(bgr_low, bgr_high).CountNonzero()[0]; } catch { }
            }
            else
            {
                img_cut2 = img_hsv.Copy();
                try { img_cut2.ROI = rect; } catch { }
                img2 = img_cut2.Copy();
                try { redpixels = img2.InRange(hsv_low, hsv_high).CountNonzero()[0]; } catch { }
            }
            CvInvoke.PutText(img, redpixels.ToString(), new Point(20, 30), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 255, 0), 2);
            if (redpixels > Convert.ToInt32(form_set_textbox_mark.Text))
                CvInvoke.PutText(img, "True", new Point(20, 60), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 255, 0), 2);
            else CvInvoke.PutText(img, "False", new Point(20, 60), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 255, 0), 2);
            form_camera_picture.Image = img.Bitmap;
        }
        #endregion

        #region form run
        Form form_run_form;
        PictureBox form_run_picture;
        private int hsv_mask = 0;
        private int hsv_timeout = 0;
        private Stopwatch form_run_timeout = new Stopwatch();
        private Stopwatch stopwatch_hsv_timeout = new Stopwatch();
        private bool form_run_flag_return = false;
        private int time_out = 1000;
        private void form_run_main()
        {
            try { run_num_head = Convert.ToInt32(File.ReadAllText("../config/num_head.txt")); } catch { }
            if (run_num_head == 0) { MessageBox.Show("num head = 0"); return; }
            run_head_no.Add("1+_read2d");
            run_head_no.Add("2+_read2d");
            run_head_no.Add("3+_read2d");
            run_head_no.Add("4+_read2d");
            int loop_xbcv = 2;
            if (onebyone_.Checked) loop_xbcv = 1;
            else
            {
                run_head_no.Add("1+");
                run_head_no.Add("2+");
                run_head_no.Add("3+");
                run_head_no.Add("4+");
            }
            int ffff = 0;
            string ssss = "";
            string f = "_read2d";
            for (int j = 0; j < loop_xbcv; j++)
            {
                if (j == 1) f = "";
                for (int i = 1; i <= run_num_head; i++)
                {
                    try { ffff = Convert.ToInt32(File.ReadAllText("../config/test_head_" + i + "_port" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + i + "_port" + f + ".txt"); return;
                    }
                    try { ssss = File.ReadAllText(path + i + "_light" + f + ".txt"); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_light" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_contrast" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_contrast" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_brightness" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_brightness" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_focus" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_focus" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_exposure" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_exposure" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_mark" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_mark" + f + ".txt"); return;
                    }
                    try { ssss = File.ReadAllText(path + i + "_process" + f + ".txt"); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_process" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_process_rgb" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_process_rgb" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_rect_x" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_rect_x" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_rect_y" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_rect_y" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_rect_width" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_rect_width" + f + ".txt"); return;
                    }
                    try { ffff = Convert.ToInt32(File.ReadAllText(path + i + "_rect_height" + f + ".txt")); }
                    catch
                    {
                        MessageBox.Show("Not find: " + path + i + "_rect_height" + f + ".txt"); return;
                    }
                }
            }
            Label lll = new Label();
            lll.Text = "Find prot arduino...";
            lll.Size = new Size(350, 75);
            FontFamily fontFamily = new FontFamily("Arial");
            lll.Font = new Font(fontFamily, 30, FontStyle.Bold, GraphicsUnit.Pixel);
            Form fff = new Form();
            fff.Size = new Size(400,100);
            fff.ControlBox = false;
            fff.StartPosition = FormStartPosition.CenterScreen;
            fff.Controls.Add(lll);
            fff.Show();
            bool flag_arduino = false;
            ManagementObjectSearcher objOSDetails2 = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'");
            ManagementObjectCollection osDetailsCollection2 = objOSDetails2.Get();
            foreach (ManagementObject usblist in osDetailsCollection2)
            {
                if (usblist["Description"].ToString() != "USB-SERIAL CH340" && usblist["Description"].ToString() != "USB Serial Port") continue;
                string[] arrport = usblist.GetPropertyValue("NAME").ToString().Split('(', ')');
                form_set_serial = new SerialPort(arrport[1]);
                form_set_serial.BaudRate = 9600;
                form_set_serial.DataBits = 8;
                Stopwatch s = new Stopwatch();
                s.Restart();
                while (s.ElapsedMilliseconds < 2500)
                {
                    try { form_set_serial.Close(); } catch { }
                    try { form_set_serial.Open(); } catch { Thread.Sleep(50); continue; }
                    s.Stop();
                    break;
                }
                if (s.IsRunning) { 
                    try { form_set_serial.Close(); } catch { } continue; 
                }
                try
                {
                    form_set_serial.DiscardInBuffer();
                    form_set_serial.DiscardOutBuffer();
                    form_set_serial.Write("23017,CHECKPORT\n");
                }
                catch { }
                string rx = "";
                s.Restart();
                while (s.ElapsedMilliseconds < 2500)
                {
                    Thread.Sleep(1000);
                    try { rx = form_set_serial.ReadExisting(); } catch { }
                    if (rx != "") { s.Stop(); break; }
                }
                if (s.IsRunning) { 
                    form_set_serial.Close(); continue; 
                }
                if (rx.Contains("RELAY_PORT_BY_DESIGN"))
                {
                    flag_arduino = true;
                    s.Stop();
                    break;
                }
                else { 
                    try { form_set_serial.Close(); } catch { } continue; 
                }
            }
            if (!flag_arduino) { MessageBox.Show("arduino err"); this.Show(); return; }
            fff.Close();
            capture = new VideoCapture(run_camera_no, captureApi);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, Convert.ToInt32(frame_height));
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, Convert.ToInt32(frame_width));
            setup();

            form_run_picture = new PictureBox();
            form_run_picture.Size = new Size(capture.Width, capture.Height);
            form_run_picture.SizeMode = PictureBoxSizeMode.AutoSize;

            form_run_form = new Form();
            form_run_form.FormClosed += Form_run_form_FormClosed;
            form_run_form.Size = new Size(capture.Width, capture.Height);
            form_run_form.StartPosition = FormStartPosition.CenterScreen;
            form_run_form.Controls.Add(form_run_picture);
            form_run_form.Show();
            stopwatch_hsv_timeout.Restart();
            Application.Idle += form_run_run;
        }
        private void setup_next_camera()
        {
            run_camera_no++;
            capture = new VideoCapture(run_camera_no, captureApi);
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, Convert.ToInt32(frame_height));
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, Convert.ToInt32(frame_width));
        }
        private int run_num_head = 0;
        private int run_camera_no = 0;
        private List<string> run_head_no = new List<string>();
        private int run_in_head_no = 0;
        private void setup()
        {
            form_set_light_send("rgbset,0,0,0,0,0");
            string[] bv = run_head_no[run_in_head_no].Split('+');
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Contrast, Convert.ToDouble(File.ReadAllText(path + bv[0] + "_contrast" + bv[1] + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Brightness, Convert.ToDouble(File.ReadAllText(path + bv[0] + "_brightness" + bv[1] + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Focus, Convert.ToDouble(File.ReadAllText(path + bv[0] + "_focus" + bv[1] + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Exposure, Convert.ToDouble(File.ReadAllText(path + bv[0] + "_exposure" + bv[1] + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Saturation, Convert.ToDouble(File.ReadAllText(path + bv[0] + "_saturation" + bv[1] + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Sharpness, Convert.ToDouble(File.ReadAllText(path + bv[0] + "_sharpness" + bv[1] + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Gain, Convert.ToDouble(File.ReadAllText(path + bv[0] + "_gain" + bv[1] + ".txt"))); } catch { }
            try { capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Gamma, Convert.ToDouble(File.ReadAllText(path + bv[0] + "_gamma" + bv[1] + ".txt"))); } catch { }
            hsv_mask = Convert.ToInt32(File.ReadAllText(path + bv[0] + "_mark" + bv[1] + ".txt"));
            try { rect.X = Convert.ToInt32(File.ReadAllText(path + bv[0] + "_rect_x" + bv[1] + ".txt")); } catch { }
            try { rect.Y = Convert.ToInt32(File.ReadAllText(path + bv[0] + "_rect_y" + bv[1] + ".txt")); } catch { }
            try { rect.Width = Convert.ToInt32(File.ReadAllText(path + bv[0] + "_rect_width" + bv[1] + ".txt")); } catch { }
            try { rect.Height = Convert.ToInt32(File.ReadAllText(path + bv[0] + "_rect_height" + bv[1] + ".txt")); } catch { }
            if (Convert.ToInt32(File.ReadAllText(path + bv[0] + "_process_rgb" + bv[1] + ".txt")) == 0)
            {
                flag_hsv = false;
                string[] bgr = File.ReadAllText(path + bv[0] + "_process" + bv[1] + ".txt").Split(',');
                bgr_low = new Bgr(Convert.ToInt32(bgr[4]), Convert.ToInt32(bgr[2]), Convert.ToInt32(bgr[0]));
                bgr_high = new Bgr(Convert.ToInt32(bgr[5]), Convert.ToInt32(bgr[3]), Convert.ToInt32(bgr[1]));
            }
            else
            {
                flag_hsv = true;
                string[] hsv = File.ReadAllText(path + bv[0] + "_process" + bv[1] + ".txt").Split(',');
                hsv_low = new Hsv(Convert.ToInt32(hsv[0]), Convert.ToInt32(hsv[2]), Convert.ToInt32(hsv[4]));
                hsv_high = new Hsv(Convert.ToInt32(hsv[1]), Convert.ToInt32(hsv[3]), Convert.ToInt32(hsv[5]));
            }
            form_set_light_send(File.ReadAllText(path + bv[0] + "_light" + bv[1] + ".txt"));
            form_run_timeout.Restart();
        }

        private void Form_run_form_FormClosed(object sender, FormClosedEventArgs e)
        {
            form_set_serial.Close();
            capture.Dispose();
        }
        private List<int> fr_port = new List<int>();
        private void form_run_run(object sender, EventArgs e)
        {
            if (form_run_flag_return) { form_run_flag_return = false; form_run_timeout.Restart(); }
            if (form_run_timeout.ElapsedMilliseconds >= time_out)
            {
                run_in_head_no++;
                if (run_in_head_no == run_head_no.Count) { MessageBox.Show("Set camera again."); form_set_light_send("rgbset,0,0,0,0,0"); Application.Exit(); return; }
                form_run_form.WindowState = FormWindowState.Minimized;
                form_run_form.WindowState = FormWindowState.Normal;
                setup();
                form_run_flag_return = true;
                return;
            }
            if (IsMouseDown == true) return;
            if (capture == null || capture.Ptr == IntPtr.Zero || capture.Width == 0)
            {
                Thread.Sleep(50);
                form_run_flag_return = true;
                return;
            }
            Mat frame;
            try
            {
                frame = capture.QueryFrame();
                img = frame.ToImage<Bgr, Byte>();
                img_hsv = frame.ToImage<Hsv, Byte>();
            }
            catch
            {
                MessageBox.Show("ไม่สามารถเปิดกล้องได้");
                return;
            }
            Graphics g = Graphics.FromImage(img.Bitmap);
            g.DrawRectangle(Pens.Red, rect);
            Image<Bgr, byte> img_cut = null;
            Image<Hsv, byte> img_cut2 = null;
            Image<Bgr, byte> img1 = null;
            Image<Hsv, byte> img2 = null;
            int redpixels = 0;
            if (flag_hsv == false)
            {
                img_cut = img.Copy();
                img_cut.ROI = rect;
                img1 = img_cut.Copy();
                try { redpixels = img1.InRange(bgr_low, bgr_high).CountNonzero()[0]; } catch { }
            }
            else
            {
                img_cut2 = img_hsv.Copy();
                try { img_cut2.ROI = rect; } catch { }
                img2 = img_cut2.Copy();
                try { redpixels = img2.InRange(hsv_low, hsv_high).CountNonzero()[0]; } catch { }
            }
            bool mask = false;
            if (redpixels >= hsv_mask)
            {
                form_run_timeout.Restart();
                if (stopwatch_hsv_timeout.ElapsedMilliseconds >= hsv_timeout) mask = true;
            }
            else
            {
                stopwatch_hsv_timeout.Restart();
                mask = false;
            }
            CvInvoke.PutText(img, redpixels.ToString(), new Point(20, 30), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            CvInvoke.PutText(img, mask.ToString(), new Point(20, 60), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            CvInvoke.PutText(img, stopwatch_hsv_timeout.ElapsedMilliseconds.ToString(), new Point(form_run_picture.Size.Width - 100, 30), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.5, new MCvScalar(0, 0, 255), 2);
            form_run_picture.Image = img.Bitmap;
            if (mask == true)
            {
                string[] bv = run_head_no[run_in_head_no].Split('+');
                if (onebyone_.Checked) {
                    File.WriteAllText("../config/test_head_" + bv[0] + "_port.txt", run_camera_no.ToString());
                    File.WriteAllText("../config/test_head_" + bv[0] + "_port_read2d.txt", run_camera_no.ToString());
                }
                else
                    File.WriteAllText("../config/test_head_" + bv[0] + "_port" + bv[1] + ".txt", run_camera_no.ToString());

                capture.Dispose();
                if (run_head_no.Count == 1) { form_set_light_send("rgbset,0,0,0,0,0"); MessageBox.Show("Set port camera ok."); Application.Exit(); return; }
                setup_next_camera();
                run_head_no.RemoveAt(run_in_head_no);
                run_in_head_no = 0;
                setup();
            }
        }
        #endregion

        #region form manual
        Form2_manual form_manual;
        Form form_manual_true;
        PictureBox ptb_form_manual_top1;
        PictureBox ptb_form_manual_top2;
        PictureBox ptb_form_manual_top3;
        PictureBox ptb_form_manual_top4;
        PictureBox ptb_form_manual_top5;
        PictureBox ptb_form_manual_buttom1;
        PictureBox ptb_form_manual_buttom2;
        PictureBox ptb_form_manual_buttom3;
        PictureBox ptb_form_manual_buttom4;
        PictureBox ptb_form_manual_buttom5;
        private void form_manual_show()
        {


            form_manual_true = new Form();
            form_manual_true.Size = new Size(1234, 609);


            form_manual_true.ShowDialog();
            this.Show();
            return;
            int num_camera = 0;
            for (int i = 0; i < 11; i++)
            {
                capture = new VideoCapture(i, captureApi);
                if (capture.Width != 0) num_camera++;
                capture.Dispose();
            }
            int g = 0;
            form_manual = new Form2_manual();
            form_manual.Size = new Size(10, 10);
            form_manual.num_camera = num_camera;
            int num = Convert.ToInt32(File.ReadAllText("../config/num_head.txt"));
            if (g < num) { form_manual.groupBox1.Enabled = true; form_manual.groupBox1.Visible = true; g++; }
            if (g < num) { form_manual.groupBox2.Enabled = true; form_manual.groupBox2.Visible = true; g++; }
            if (g < num) { form_manual.groupBox3.Enabled = true; form_manual.groupBox3.Visible = true; g++; }
            if (g < num) { form_manual.groupBox4.Enabled = true; form_manual.groupBox4.Visible = true; g++; }
            if (g < num) { form_manual.groupBox5.Enabled = true; form_manual.groupBox5.Visible = true; g++; }
            g = 0;
            if (g < num && !onebyone_.Checked) { form_manual.groupBox6.Enabled = true; form_manual.groupBox6.Visible = true; g++; }
            if (g < num && !onebyone_.Checked) { form_manual.groupBox7.Enabled = true; form_manual.groupBox7.Visible = true; g++; }
            if (g < num && !onebyone_.Checked) { form_manual.groupBox8.Enabled = true; form_manual.groupBox8.Visible = true; g++; }
            if (g < num && !onebyone_.Checked) { form_manual.groupBox9.Enabled = true; form_manual.groupBox9.Visible = true; g++; }
            if (g < num && !onebyone_.Checked) { form_manual.groupBox10.Enabled = true; form_manual.groupBox10.Visible = true; g++; }
            form_manual.ShowDialog();
            this.Show();
        }
        #endregion

        #region ContextMenuStrip
        private void set_form_cmd_to_arduino_rgbset_Click(object sender, EventArgs e)
        {
            form_set_textbox_light.Text = "rgbset,1,0,255,0,0";
        }
        private void set_form_cmd_to_arduino_23017_Click(object sender, EventArgs e)
        {
            form_set_textbox_light.Text = "23017,1,16,1";
        }
        private void form_set_process_red_Click(object sender, EventArgs e)
        {
            form_set_textbox_process.Text = "0,60,0,255,150,255";
            form_set_trackbar_process.Value = 1;
        }
        private void form_set_process_green_Click(object sender, EventArgs e)
        {
            form_set_textbox_process.Text = "0,100,100,255,0,50";
            form_set_trackbar_process.Value = 0;
        }
        private void form_set_process_black_Click(object sender, EventArgs e)
        {
            form_set_textbox_process.Text = "0,50,0,50,0,50";
            form_set_trackbar_process.Value = 0;
        }
        private void form_set_process_white_Click(object sender, EventArgs e)
        {
            form_set_textbox_process.Text = "200,255,200,255,200,255";
            form_set_trackbar_process.Value = 0;
        }
        private void onebyone__Click(object sender, EventArgs e)
        {
            File.WriteAllText("../config/camera_show_onebyone.txt", onebyone_.Checked.ToString());
        }
        #endregion
    }
}
