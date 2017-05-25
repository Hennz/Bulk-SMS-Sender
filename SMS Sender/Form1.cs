using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Web;
using System.Net;
using System.Timers;

namespace SMS_Sender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string keyName = @"HKEY_CURRENT_USER\SOFTWARE\SMS Sender";
            string valueName = "registered";

            if ((string)Registry.GetValue(keyName, valueName, "0") == "0" || Registry.GetValue(keyName, valueName, "0") == null)
            {
                //code if key Not Exist
                Form5 dlg = new Form5();
                dlg.ShowDialog();
            }

            //int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader("senders.txt");
            while ((line = file.ReadLine()) != null)
            {
                comboBox1.Items.Add(line);
                //counter++;
            }

            comboBox1.SelectedIndex = 0;

            file.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "TXT files|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = dlg.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                            using (var reader = new StreamReader(myStream))
                            {
                                string line;
                                listBox1.Items.Clear();
                                while ((line = reader.ReadLine()) != null)
                                {
                                    listBox1.Items.Add(line);
                                }
                                reader.Close();
                            }
                            myStream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "TXT files|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.FileName != "")
                {
                    // Saves the Image via a FileStream created by the OpenFile method.  
                    System.IO.FileStream fs = (System.IO.FileStream)dlg.OpenFile();
                    // Saves the Image in the appropriate ImageFormat based upon the  
                    // File type selected in the dialog box.  
                    // NOTE that the FilterIndex property is one-based.  
                    using (var writer = new StreamWriter(fs))
                    {
                        for (int i = 0; i < listBox1.Items.Count; i++)
                        {
                            string line = listBox1.Items[i].ToString();
                            writer.WriteLine(line);
                        }
                    }
                    fs.Close();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2();
            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                string phone_number = dlg.phone_number;
                //Do something here with these values
                listBox1.Items.Add(phone_number);
            }
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            string phone_number = listBox1.GetItemText(listBox1.SelectedItem);
            Form3 dlg = new Form3(phone_number);

            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                phone_number = dlg.phone_number;

                //Do something here with these values
                listBox1.Items[index] = phone_number;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 dlg = new Form4();
            int index = listBox1.SelectedIndex;

            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                bool delete = dlg.delete;

                //Do something here with these values
                if (delete == true)
                {
                    listBox1.Items.RemoveAt(index);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2();
            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                string phone_number = dlg.phone_number;
                //Do something here with these values
                listBox1.Items.Add(phone_number);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            string phone_number = listBox1.GetItemText(listBox1.SelectedItem);
            Form3 dlg = new Form3(phone_number);

            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                phone_number = dlg.phone_number;

                //Do something here with these values
                listBox1.Items[index] = phone_number;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 dlg = new Form4();
            int index = listBox1.SelectedIndex;

            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                bool delete = dlg.delete;

                //Do something here with these values
                if ( delete == true )
                {
                    listBox1.Items.RemoveAt(index);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            count = 0;
            Tick();
        }

        private int count;

        void Tick()
        {
            if (count == listBox1.Items.Count)
            {
                MessageBox.Show("Sent All");
                return;
            }
            count++;
            WorkThreadFunction(listBox1.Items[count - 1].ToString());
        }

        public void WorkThreadFunction(string phone_number)
        {
            try
            {
                string mocean_api_key = "dadcf426";
                string mocean_api_secret = "d712f7f4";
                string senderid = comboBox1.GetItemText(comboBox1.SelectedItem);
                string message = textBox1.Text;
                string path = "https://rest-api.moceansms.com/rest/1/sms?mocean-api-key=" + mocean_api_key + "&mocean-api-secret=" + mocean_api_secret +
    "&mocean-to=" + phone_number + "&mocean-from=" + WebUtility.UrlEncode(senderid) + "&mocean-text=" + WebUtility.UrlEncode(message);

                /* Call the Path */
                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(path);

                WebResponse response = wrGETURL.GetResponse();
                response.Close();
                // MessageBox.Show(phone_number + " sent.");
                Tick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 dlg = new Form6();
            dlg.ShowDialog();
        }
    }
}
