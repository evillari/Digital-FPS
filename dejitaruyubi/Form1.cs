using System;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DPFP;
using DPFP.Capture;



namespace dejitaruyubi
{
    public partial class mainform : Form, DPFP.Capture.EventHandler
    {

        
        private DPFP.Capture.Capture Capturer;
        private DPFP.Processing.Enrollment Enroller;
        private DPFP.Verification.Verification Verifier;
        private DPFP.Processing.DataPurpose gpurpose;


        public mainform()
        {
            InitializeComponent();

        }


        private void Init()
        {
  

            Enroller = new DPFP.Processing.Enrollment();
            try
            {
                Capturer = new DPFP.Capture.Capture();
                if (null != Capturer)
                    Capturer.EventHandler = this;
                else
                    Dejitaru.SetControlProperty(this, lstatus, "Text", "Can't initiate capture operation.");


            }
            catch
            {
                Dejitaru.SetControlProperty(this, lstatus, "Text", "Can't initiate capture operation.");

            }
        }

        private void Start()
        {
            if (Capturer != null)
            {
                try
                {
                    Capturer.StartCapture();
                    Dejitaru.SetControlProperty(this, lstatus, "Text", "Place finger on the scanner to start.");

                }
                catch
                {
                    Dejitaru.SetControlProperty(this, lstatus, "Text", "Can't start capture.");

                }
            }
        }


        private void Stop()
        {
            if (Capturer != null)
            {
                try
                {
                    Capturer.StopCapture();

                }
                catch
                {
                    Dejitaru.SetControlProperty(this, lstatus, "Text", "Can't stop capture.");

                }
            }
        }


        private void Process(DPFP.Sample sample, DPFP.Processing.DataPurpose purpose)
        {
            if (purpose == DPFP.Processing.DataPurpose.Enrollment)
            {
                DPFP.FeatureSet features = ExtractFeatures(sample, purpose);
                if (features != null)
                {
                    try
                    {
                        Enroller.AddFeatures(features);

                    }
                    finally
                    {
                        switch (Enroller.TemplateStatus)
                        {
                            case DPFP.Processing.Enrollment.Status.Ready:

                                byte[] fpsample = new byte[0];
                                Enroller.Template.Serialize(ref fpsample);
                                Dejitaru.progress += 1;
                                Dejitaru.SetControlProperty(this, prbenroll, "Value", Dejitaru.progress);
                                Dejitaru.progress = 0;
                                Enroller.Clear();
                                gpurpose = DPFP.Processing.DataPurpose.Unknown;
                                Dejitaru.SerializetoDB(fpsample, lstatus, this);
                                Stop();
                                break;
                            case DPFP.Processing.Enrollment.Status.Failed:

                                Dejitaru.SetControlProperty(this, lstatus, "Text", "Enrollment failed.");

                                Start();
                                break;
                            case DPFP.Processing.Enrollment.Status.Insufficient:

                                Dejitaru.progress += 1;
                                Dejitaru.SetControlProperty(this, prbenroll, "Value", Dejitaru.progress);

                                Dejitaru.SetControlProperty(this, lstatus, "Text", "Needing more samples for enrollment.Please scan more.");

                                break;
                            case DPFP.Processing.Enrollment.Status.Unknown:
                                Dejitaru.SetControlProperty(this, lstatus, "Text", "Unknown");

                                break;

                        }
                    }
                }
            }
            if (purpose == DPFP.Processing.DataPurpose.Verification)
            {

                DPFP.FeatureSet features = ExtractFeatures(sample, purpose);
                string rhandler = "";
                if (features != null)
                {
                    //DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();

                    //Verifier.Verify(features, template, ref result);

                    string[] results = VerifyDB(features);

                    if (results[0].Equals("true"))
                    {
                        rhandler = "Match found. \n\n";
                        for (int i = 1; i < results.Length; i++)
                        {
                            rhandler = rhandler + " " + results[i] + "\n";
                        }
                    }
                    else
                    {
                        rhandler = "No match found.";
                    }
                    Dejitaru.SetControlProperty(this, lstatus, "Text", rhandler);

                }
            }

        }

        private DPFP.FeatureSet ExtractFeatures(DPFP.Sample sample, DPFP.Processing.DataPurpose purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(sample, purpose, ref feedback, ref features);
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
            {
                return features;
            }
            else
            {
                return null;

            }
        }


        private string[] VerifyDB(DPFP.FeatureSet features)
        {
            string[] arr = new string[5];
            OdbcConnection cn;
            OdbcCommand cmd;
            byte[] temp = new byte[0];
            string query = "Select * from fpbasic";
            string dsnname = "fpbasic";
            string uid = "Administrator";
            string password = "2wsx3edc";
            cn = new OdbcConnection("dsn=" + dsnname + "; UID=" + uid + "; PWD=" + password + ";");
            cn.Open();
            cmd = new OdbcCommand(query, cn);
            OdbcDataReader reader = cmd.ExecuteReader();

            DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();
            Verifier = new DPFP.Verification.Verification();
            while (reader.Read())
            {
                for (int index = 0; index < reader.FieldCount; index++)
                {

                    Stream stream = new MemoryStream((byte[])reader.GetValue(5));
                    DPFP.Template obj = new DPFP.Template(stream);
                    Verifier.Verify(features, obj, ref result);
                    if (result.Verified)
                    {

                        arr[0] = "true";
                        arr[1] = reader.GetString(1);
                        arr[2] = reader.GetString(2);
                        arr[3] = reader.GetString(3);
                        arr[4] = reader.GetInt32(4).ToString();
                        return arr;
                    }

                }
            }

            cn.Close();
            arr[0] = "false";
            return arr;

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            Point pt1 = new Point(100, 100);
            Point pt2 = new Point(300, 300);
            Brush brush = new SolidBrush(Color.Red);
            Pen pen = new Pen(brush);

            g.DrawLine(pen, pt1, pt2);

        }

        private void btnverify_Click(object sender, EventArgs e)
        {
            gpurpose = DPFP.Processing.DataPurpose.Verification;
            SetControl();
        }

        private void mainform_Load(object sender, EventArgs e)
        {

            btnapply.Enabled = false;
            prbenroll.Visible = false;
            lblnote.Visible = false;
            if (DejitaruInit.IsDBReady)
            {
                pmatchfound.Visible = false;
                pverified.Location = new Point(210, 3);
                gpurpose = DPFP.Processing.DataPurpose.Verification;
                //lblnote.Text = "Note: Requires four fingerprint samples for Enrollment.";
                Init();
                Start();
                

            }
            else
            {
                

            }


            //timer1.Enabled = true;
            //lblnote.Visible = false;

            
       


        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void posverify()
        {


        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            pverified.Location = new Point(pverified.Left - 10, 3);
            if (pverified.Left <= 5)
            {
                pverified.Left = 3;
                pmatchfound.Visible = true;
                timer1.Enabled = false;
            }

        }


        private void btnenroll_Click(object sender, EventArgs e)
        {
            btnenroll.BackColor = Color.FromArgb(255, 39, 147, 232);
            lbltask.Text = "Enrollment";
            pmatchfound.Visible = false;
            pverified.Location = new Point(210, 3);
            Enroller.Clear();
            gpurpose = DPFP.Processing.DataPurpose.Enrollment;
            SetControl();

        }

        public void OnComplete(object Capture, string ReaderSerialNumber, Sample sample)
        {

            Dejitaru.ShowFP(sample, picfinger, this);

            if (gpurpose == DPFP.Processing.DataPurpose.Enrollment)
            {
                Process(sample, gpurpose);
            }
            else if (gpurpose == DPFP.Processing.DataPurpose.Verification)
            {
                // ImageCC();
                // usample = sample;

            }
            else
            {

                //SetText("Please specify task.", labelResult);
                //Enroller.Clear();
            }

        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {

        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {

        }

        private void SetControl()
        {
            if (gpurpose == DPFP.Processing.DataPurpose.Enrollment)
            {

                Dejitaru.SetControlProperty(this, prbenroll, "Visible", true);
                Dejitaru.SetControlProperty(this, lblnote, "Location", new Point(53, 470));
                Dejitaru.SetControlProperty(this, lblnote, "Visible", true);
                Dejitaru.SetControlProperty(this, lblnote, "Text", "Note: Requires four fingerprint samples for Enrollment.");
            }
            else if (gpurpose == DPFP.Processing.DataPurpose.Verification)
            {
                Dejitaru.SetControlProperty(this, prbenroll, "Visible", false);
                Dejitaru.SetControlProperty(this, lblnote, "Location", new Point(53, 440));
                Dejitaru.SetControlProperty(this, lblnote, "Visible", true);
                Dejitaru.SetControlProperty(this, lblnote, "Text", "Place your index finger on the scanner to verify your identity.");
            }
            else
            {
                Dejitaru.SetControlProperty(this, prbenroll, "Visible", false);
                Dejitaru.SetControlProperty(this, lblnote, "Visible", false);
                Dejitaru.SetControlProperty(this, lblnote, "Text", "");
            }
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            
            Dejitaru.SetControlProperty(this, lstatus, "Text", "Connected");
            Dejitaru.SetControlProperty(this, lstatus, "ForeColor", Color.LightGreen);
            Dejitaru.SetControlProperty(this, btnverify, "Visible", true);
            Dejitaru.SetControlProperty(this, btnenroll, "Visible", true);
            SetControl();

        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {

            
            Dejitaru.SetControlProperty(this, lstatus, "Text", "Disconnected");
            Dejitaru.SetControlProperty(this, lstatus, "ForeColor", Color.Red);
            Dejitaru.SetControlProperty(this, btnverify, "Visible", false);
            Dejitaru.SetControlProperty(this, btnenroll, "Visible", false);
            Dejitaru.SetControlProperty(this, prbenroll, "Visible", false);
            Dejitaru.SetControlProperty(this, lblnote, "Visible", false);
            Dejitaru.SetControlProperty(this, lblnote, "Text", "");


        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, CaptureFeedback CaptureFeedback)
        {

        }

      


        private void button1_Click(object sender, EventArgs e)
        {
            string[] keys = DejitaruInit.GetncheckDSNKey();



            if (keys != null)
            {
                foreach (string key in keys)
                {
                    MessageBox.Show(key);
                }

                MessageBox.Show(Dejitaru.TestDB(keys[0], keys[1], keys[2]).ToString());

            }


        }

        private void tabSystem_Click(object sender, EventArgs e)
        {

        }

        private void pverification_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnsettings_Click(object sender, EventArgs e)
        {

            string[] datasource = Dejitaru.getDSNLists();
            string[] sysinfo = new string[5];
            sysinfo = Dejitaru.SystemInfo();
            

            lblmachname_container.Text = sysinfo[0];
            lblosv_container.Text = sysinfo[1];
            lblproccount_container.Text = sysinfo[2];
            lblclrv_container.Text = sysinfo[3];
            if (sysinfo[4] == "True")
            {
                lblsystype_container.Text = "64-Bit Operating System";
            }
            else
            {
                lblsystype_container.Text = "32-Bit Operating System";
            }

 
            cbdsn.Items.Clear();
            foreach (string dsn in datasource)
            {
                cbdsn.Items.Add(dsn);
            }
            
        }

        private void cbdsn_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] values = new string[4];
            values = Dejitaru.getODBCValue(cbdsn.SelectedItem.ToString());

            if (values != null || values.Length <= 0)
            {
                tbusername.Text = values[2];
                tbpassword.Text = "";

                Dejitaru.lastuser = values[2];
                
            }
            
        }

        private void btntestconn_Click(object sender, EventArgs e)
        {
            if ((tbusername.TextLength<=0 || tbusername.Text == null ) || (tbpassword.TextLength<=0||tbpassword.Text == null))
            {
                MessageBox.Show("Can't inititate test connection. Missing Credentials.", "Misssing Credentials.",MessageBoxButtons.OK);
            }
            else
            {
                string dsn = cbdsn.SelectedItem.ToString();
                if(dsn == null || dsn.Length <= 0) 
                {
                    MessageBox.Show("Can't inititate test connection. Please double check data source name.", "Data Source Missing.", MessageBoxButtons.OK);
                }
                else
                {
                    if (Dejitaru.TestDB(dsn, tbusername.Text, tbpassword.Text))
                    {
                        MessageBox.Show("Test connection successful.", "Test Connection", MessageBoxButtons.OK);
                        btnapply.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Test connection failed.", "Test Connection", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void btnapply_Click(object sender, EventArgs e)
        {
            if ((tbusername.TextLength <= 0 || tbusername.Text == null) || (tbpassword.TextLength <= 0 || tbpassword.Text == null))
            {
                MessageBox.Show("Missing Credentials.", "Misssing Credentials.", MessageBoxButtons.OK);
                btnapply.Enabled = false;
            }
            else
            {
                string dsn = cbdsn.SelectedItem.ToString();
                if (dsn == null || dsn.Length <= 0)
                {
                    MessageBox.Show("Please double check data source name.", "Data Source Missing.", MessageBoxButtons.OK);
                    btnapply.Enabled = false;
                }
                else
                {
                    if (Dejitaru.TestDB(dsn, tbusername.Text, tbpassword.Text))
                    {
                        MessageBox.Show("Connection saved.", "Data Source.", MessageBoxButtons.OK);
                        Dejitaru.CreateODBCkey(dsn);
                        Dejitaru.SControl(tbpassword.Text, dsn);
                        btnapply.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("DB Connection failed.", "DB Connection", MessageBoxButtons.OK);
                        btnapply.Enabled = false;
                    }
                }
            }
        }

        private void mainform_FormClosing(object sender, FormClosingEventArgs e)
        {

           
        }

    }
}
