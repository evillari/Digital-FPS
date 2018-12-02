using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dejitaruyubi
{
    public partial class startup : Form
    {
        int x = 0;
        private const int count = 3;
        public startup()
        {
            InitializeComponent();
        }

        private void startup_Load(object sender, EventArgs e)
        {

            lblloadstatus.Text = "Initializing components...";



        }

        private void startup_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        
           x += 1;
           if(x > count)
            {
                timer1.Enabled = false;
                lblloadstatus.Text = "Checking data source...";
                Afterload();

            }
        
        }

        private void Afterload()
        {
            string[] keys = DejitaruInit.GetncheckDSNKey();
            if (keys.Length > 0)
            {
                lblloadstatus.Text = "Data source found.";
                
                if (Dejitaru.TestDB(keys[0], keys[1], keys[2]))
                {
                    DejitaruInit.IsDBReady = true;
                    this.Close();
                }
                else
                {
                    DejitaruInit.IsDBReady = false;
                    this.Close();
                }
            }
            else
            {
                lblloadstatus.Text = "No data source found.";
                DejitaruInit.IsDBReady = false;
                this.Close();
            }
        }

        private void startup_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;
           
        }

        private void lbltech_Click(object sender, EventArgs e)
        {

        }
    }
}
