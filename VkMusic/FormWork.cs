using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using WMPLib;
using System.Threading;

namespace VkMusic
{
    public partial class FormWork : Form
    {
        public FormWork()
        {
            InitializeComponent();
        }

        WindowsMediaPlayer wmp = new WindowsMediaPlayer();
        List<string> AudioIds = new List<string>();
        List<string> Urls = new List<string>();
        List<string> Texts = new List<string>();

        private void FormWork_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FormWork_Load(object sender, EventArgs e)
        {            
            LoadAudios();
        }
        
        private void LoadAudios()
        {
            XmlDocument doc = Program.Api.GetAudiosById(Program.Api.UserId, 20);
            doc.Save("audios.xml");
            doc.Load("audios.xml");
            XmlNode node = doc["response"];
            //MessageBox.Show(node.InnerXml);
            XmlNodeList playlist = node.SelectNodes("audio");
            for (int i = 0; i < playlist.Count; i++)
            {
                listBox1.Items.Add(playlist[i]["artist"].InnerText + " - " + playlist[i]["title"].InnerText);
                if (playlist[i].SelectSingleNode("url") != null)
                {
                    Urls.Add(playlist[i]["url"].InnerText);
                }
                else
                {
                    Urls.Add("No Url");
                }
                if (playlist[i].SelectSingleNode("lyrics_id") != null)
                {
                    AudioIds.Add(playlist[i]["lyrics_id"].InnerText);
                }
                else
                {
                    AudioIds.Add("No Id");
                }
                listBox2.Items.Add(Urls[i]);
                
            }
            
        }

        private string GetAudioText(string audioId)
        {
            XmlDocument doc1 = Program.Api.AudioText(audioId);
            if (doc1.SelectSingleNode("response") != null)
            {
                doc1.Save("lyrics.xml");
                if (doc1["response"].SelectSingleNode("lyrics") != null)
                {
                    return doc1["response"]["lyrics"]["text"].InnerText;
                }
                else
                {
                    return "No Text";
                }
            }
            else
            {
                return "No Text";
            }
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox1.SelectedIndex = listBox2.SelectedIndex;
                Clipboard.SetText(Urls[listBox1.SelectedIndex]);
                this.Text = "Текст скопирован";
                Thread.Sleep(100);
                this.Text = "VK.Player";
            }
            else
            {
                listBox2.SelectedIndex = listBox1.SelectedIndex;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            trackBar1.Maximum = (int)wmp.currentMedia.duration;
            trackBar1.Value = (int)wmp.controls.currentPosition;
            int h = (int)wmp.controls.currentPosition / 3600 % 3600;
            int m = (int)wmp.controls.currentPosition / 60 % 60;
            int s = (int)wmp.controls.currentPosition % 60;

            int h1 = (int)wmp.currentMedia.duration / 3600 % 3600;
            int m1 = (int)wmp.currentMedia.duration / 60 % 60;
            int s1 = (int)wmp.currentMedia.duration % 60;

            label1.Text = String.Format("{0:00} : {1:00} : {2:00} / {3:00} : {4:00} : {5:00}", h, m, s, h1, m1, s1);
            if (wmp.currentMedia.duration-1 < wmp.controls.currentPosition&&wmp.currentMedia.duration>10)
            {
                listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
            }
        }
        
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            wmp.controls.currentPosition = 0;
            wmp.URL = Urls[listBox1.SelectedIndex];
            listBox2.SelectedIndex = listBox1.SelectedIndex;
            richTextBoxText.Text = "Loading Text";
            richTextBoxText.Text = GetAudioText(AudioIds[listBox1.SelectedIndex]);

            timer1.Start();

            trackBar1.Minimum = 0;

            //wmp.controls.stop();
            ButtonPlay.Text = "Stop";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            wmp.controls.currentPosition = trackBar1.Value;
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            if(ButtonPlay.Text == "Stop")
            {
                wmp.controls.pause();
                ButtonPlay.Text = "Play";
                timer1.Stop();
            }
            else if(ButtonPlay.Text == "Play")
            {
                wmp.controls.play();
                ButtonPlay.Text = "Stop";
                timer1.Start();
            }
        }
    }
}
