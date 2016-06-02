using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VkMusic
{
    public partial class FormAuth : Form
    {
        public FormAuth()
        {
            InitializeComponent();
        }

        string AppId = "5477638";

        [Flags]
        private enum VkontakteScopeList
        {
            notify = 1,
            friends = 2,
            photos = 4,
            audio = 8,
            video = 16,
            offers = 32,
            questions = 64,
            pages = 128,
            link = 256,
            notes = 2048,
            messages = 4096,
            wall = 8192,
            docs = 131072

        }
        int AppScope = (int)(VkontakteScopeList.audio | VkontakteScopeList.docs | VkontakteScopeList.friends | VkontakteScopeList.link | VkontakteScopeList.messages | VkontakteScopeList.notes | VkontakteScopeList.notify | VkontakteScopeList.offers | VkontakteScopeList.pages | VkontakteScopeList.photos | VkontakteScopeList.questions | VkontakteScopeList.wall);

        string GetAccessToken(string Url)
        {
            string AccessToken = string.Empty;
            string parameters = (Url.Split('#'))[1];
            string[] splitedParams = parameters.Split('&');
            AccessToken = (splitedParams[0].Split('='))[1];


            return AccessToken;
        }

        string GetUserId(string Url)
        {
            string UserId = string.Empty;
            string parameters = (Url.Split('#'))[1];
            string[] splitedParams = parameters.Split('&');
            UserId = (splitedParams[2].Split('='))[1];
            return UserId;
        }

        private void FormAuth_Load(object sender, EventArgs e)
        {
            webBrowserVk.ScriptErrorsSuppressed = true;
           
            webBrowserVk.Navigate(String.Format("http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token", AppId, AppScope));
            
        }

        private void webBrowserVk_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBoxAddress.Text = e.Url.ToString();

            if (textBoxAddress.Text.Contains("access_token"))
            {
                Program.Api = new VkApi(GetUserId(textBoxAddress.Text), GetAccessToken(textBoxAddress.Text));
                this.Hide();
                Form f = new FormWork();
                f.Show();
            }     
        }
    }
}
