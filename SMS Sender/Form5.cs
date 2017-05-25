using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SMS_Sender
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string message;
            string caption = "Serial Key";
            string serial_number = textBox1.Text;
            if ( serial_number == "XAQR1-1AR45-SGWS2-32DGS-3FGQT")
            {
                string keyName = @"HKEY_CURRENT_USER\SOFTWARE\SMS Sender";
                string valueName = "registered";
                Registry.SetValue(keyName, valueName, "1");
                this.Close();
            }
            else
            {
                message = "Serial Key is incorrect. Please try again.";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.OK,
                                             MessageBoxIcon.Question);
            }
        }
    }
}
