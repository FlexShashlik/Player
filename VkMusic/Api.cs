using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections.Specialized;

namespace VkMusic
{
    class VkApi
    {
        string _userid;
        string _accesstoken;
        public VkApi(string UserId, string AccessToken)
        {
            _userid = UserId;
            _accesstoken = AccessToken;
        }
        public string UserId
        {
            get { return _userid; }
        }
        private XmlDocument ExecuteCommand(string command, NameValueCollection parameters)
        {
            XmlDocument result = new XmlDocument();

            string strParam = String.Empty;

            strParam = String.Join("&", from item in parameters.AllKeys select item + "=" + parameters[item]);

            result.Load(String.Format("https://api.vkontakte.ru/method/{0}.xml?access_token={1}&{2}", command, _accesstoken, strParam));

            return result;
        }
        public XmlDocument GetAudiosById(string ID,int CountAudio)
        {
            XmlDocument result = new XmlDocument();
            string command = "audio.get";
            NameValueCollection parameters = new NameValueCollection();
            parameters["owner_id"] = ID;
            //parameters["count"] = CountAudio.ToString();
            result = ExecuteCommand(command, parameters);

            return result;
        }
        public XmlDocument AudioText(string AudioId)
        {
            XmlDocument doc = new XmlDocument();
            string command = "audio.getLyrics";
            NameValueCollection parameters = new NameValueCollection();
            parameters["lyrics_id"] = AudioId;
            doc = ExecuteCommand(command, parameters);
            return doc;
        }
    }
}
