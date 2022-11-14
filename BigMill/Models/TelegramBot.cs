using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace BigMill.Models
{
    public class TelegramBot
    {
        /// <summary>
        /// in tabe jahate ersale payam be chanele telegram morede estefade gharar migirad
        /// </summary>
        /// <param name="message">payame morede nazar jahate ersal</param>
        /// <returns>
        /// string
        /// true : ersal ba movaffaghiyyat
        /// false : khata dar ersal be telegram
        /// </returns>
        public string SendMessage(string message)
        {
            // idie channel
            string Chat_Id = System.Configuration.ConfigurationManager.AppSettings["Chat_Id"];
            // Tokene robot
            string token = System.Configuration.ConfigurationManager.AppSettings["TelegramToken"];
            string url = string.Format("https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}", token, Chat_Id, message);
            var req = (HttpWebRequest)WebRequest.Create(url);
          var respons=  req.GetResponse();
         var result= new StreamReader(respons.GetResponseStream()).ReadToEnd();
         int index = result.IndexOf(":");
         string IsOk = result.Substring(index + 1, result.IndexOf(",") - (index + 1));
         return IsOk;
        }

        /// <summary>
        /// in tabe jahate ersale Tasvir (ya ba caption ya bedune caption) be channele telegram morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">Tasvire morede nazar jahate ersal</param>
        /// <returns>
        /// string
        /// true : ersal ba movaffaghiyyat
        /// false : khata dar ersal be telegram
        /// </returns>
        public string SendPhoto(TelegramModel model,HttpPostedFileBase Path)
        {
            string Chat_Id = System.Configuration.ConfigurationManager.AppSettings["Chat_Id"];
            string result;
            using (var client = new System.Net.Http.HttpClient())
            {
                string token = System.Configuration.ConfigurationManager.AppSettings["TelegramToken"];
                var uri = new Uri("https://api.telegram.org/bot" + token + "/sendPhoto?chat_id=" + Chat_Id + "&caption=" + model.Caption);
                string corrected = Path.FileName.Replace(" ", "");
                byte[] fileBytes = null;
                using (var multipartFormDataContent = new System.Net.Http.MultipartFormDataContent())
                {
                    using (var binaryReader = new BinaryReader(Path.InputStream))
                    {
                        fileBytes = binaryReader.ReadBytes(Path.ContentLength);
                    }

                    var streamContent = new System.Net.Http.StreamContent(new System.IO.MemoryStream(fileBytes));
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"photo\"; filename=" + corrected);

                    multipartFormDataContent.Add(streamContent, "file", Path.FileName);
                    using (var message = client.PostAsync(uri, multipartFormDataContent))
                    {
                        var contentString =  message.Result.Content.ReadAsStringAsync().Result;
                        result = contentString.ToString();
                    }
                    int index = result.IndexOf(":");
                    string IsOk = result.Substring(index + 1, result.IndexOf(",") - (index + 1));
                    return IsOk;
                }

            }
        }


        /// <summary>
        /// in tabe jahate ersale Video (ya ba caption ya bedune caption) be channele telegram morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">Video ye morede nazar jahate ersal</param>
        /// <returns>
        /// string
        /// true : ersal ba movaffaghiyyat
        /// false : khata dar ersal be telegram
        /// </returns>
        public string SendVideo(TelegramModel model,HttpPostedFileBase Path)
        {
            string Chat_Id = System.Configuration.ConfigurationManager.AppSettings["Chat_Id"];
            string result;
            using (var client = new System.Net.Http.HttpClient())
            {
                string token = System.Configuration.ConfigurationManager.AppSettings["TelegramToken"];
                var uri = new Uri("https://api.telegram.org/bot" + token + "/sendVideo?chat_id=" + Chat_Id + "&caption=" + model.Caption);
                string corrected = Path.FileName.Replace(" ", "");
                byte[] fileBytes = null;
                using (var multipartFormDataContent = new System.Net.Http.MultipartFormDataContent())
                {
                    using (var binaryReader = new BinaryReader(Path.InputStream))
                    {
                        fileBytes = binaryReader.ReadBytes(Path.ContentLength);
                    }

                    var streamContent = new System.Net.Http.StreamContent(new System.IO.MemoryStream(fileBytes));
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"video\"; filename=" + corrected);

                    multipartFormDataContent.Add(streamContent, "Video", Path.FileName);

                    using (var message =  client.PostAsync(uri, multipartFormDataContent))
                    {
                        var contentString =  message.Result.Content.ReadAsStringAsync().Result;
                        result = contentString.ToString();
                    }
                    int index = result.IndexOf(":");
                    string IsOk = result.Substring(index + 1, result.IndexOf(",") - (index + 1));
                    return IsOk;
                }

            }
        }

        /// <summary>
        /// in tabe jahate ersale File Soti be channele telegram morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">File Soti ye morede nazar jahate ersal</param>
        /// <returns>
        /// string
        /// true : ersal ba movaffaghiyyat
        /// false : khata dar ersal be telegram
        /// </returns>
        public string SendVoice(TelegramModel model,HttpPostedFileBase Path)
        {
            string Chat_Id = System.Configuration.ConfigurationManager.AppSettings["Chat_Id"];
            string result;
            using (var client = new System.Net.Http.HttpClient())
            {
                string token = System.Configuration.ConfigurationManager.AppSettings["TelegramToken"];
                var uri = new Uri("https://api.telegram.org/bot" + token + "/sendVoice?chat_id=" + Chat_Id);
                string corrected = Path.FileName.Replace(" ", "");
                byte[] fileBytes = null;
                using (var multipartFormDataContent = new System.Net.Http.MultipartFormDataContent())
                {
                    using (var binaryReader = new BinaryReader(Path.InputStream))
                    {
                        fileBytes = binaryReader.ReadBytes(Path.ContentLength);
                    }

                    var streamContent = new System.Net.Http.StreamContent(new System.IO.MemoryStream(fileBytes));
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"voice\"; filename=" + corrected);
                    multipartFormDataContent.Add(streamContent, "Voice", Path.FileName);
                    using (var message =  client.PostAsync(uri, multipartFormDataContent))
                    {
                        var contentString =  message.Result.Content.ReadAsStringAsync().Result;
                        result = contentString.ToString();
                    }
                    int index = result.IndexOf(":");
                    string IsOk = result.Substring(index + 1, result.IndexOf(",") - (index + 1));
                    
                    return IsOk;
                }

            }
        }

        /// <summary>
        /// in tabe jahate ersale File be channele telegram morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">File ye morede nazar jahate ersal</param>
        /// <returns>
        /// string
        /// true : ersal ba movaffaghiyyat
        /// false : khata dar ersal be telegram
        /// </returns>
        public string SendDocument(TelegramModel model,HttpPostedFileBase Path)
        {
            string Chat_Id = System.Configuration.ConfigurationManager.AppSettings["Chat_Id"];
            string result;
            using (var client = new System.Net.Http.HttpClient())
            {
                string token = System.Configuration.ConfigurationManager.AppSettings["TelegramToken"];
                var uri = new Uri("https://api.telegram.org/bot" + token + "/sendDocument?chat_id=" + Chat_Id);
                string corrected = Path.FileName.Replace(" ", "");
                byte[] fileBytes = null;
                using (var multipartFormDataContent = new System.Net.Http.MultipartFormDataContent())
                {
                    using (var binaryReader = new BinaryReader(Path.InputStream))
                    {
                        fileBytes = binaryReader.ReadBytes(Path.ContentLength);
                    }

                    var streamContent = new System.Net.Http.StreamContent(new System.IO.MemoryStream(fileBytes));
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"document\"; filename=" + corrected);

                    multipartFormDataContent.Add(streamContent, "Document", Path.FileName);

                    using (var message =  client.PostAsync(uri, multipartFormDataContent))
                    {
                        var contentString =  message.Result.Content.ReadAsStringAsync().Result;
                        result = contentString.ToString();
                    }
                    int index = result.IndexOf(":");
                    string IsOk = result.Substring(index + 1, result.IndexOf(",") - (index + 1));
                    return IsOk;
                }

            }
        }

        /// <summary>
        /// in tabe jahate ersale Tasvir be channele telegram morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">Tasvir ye morede nazar jahate ersal</param>
        /// <returns>
        /// string
        /// true : ersal ba movaffaghiyyat
        /// false : khata dar ersal be telegram
        /// </returns>
        public string SendContent(TelegramModel model)
        {
            string Chat_Id = System.Configuration.ConfigurationManager.AppSettings["Chat_Id"];
            string result;
            using (var client = new System.Net.Http.HttpClient())
            {
                string token = System.Configuration.ConfigurationManager.AppSettings["TelegramToken"];
                var uri = new Uri("https://api.telegram.org/bot" + token + "/sendPhoto?chat_id=" + Chat_Id + "&caption=" + model.Caption);
         
                using (var multipartFormDataContent = new System.Net.Http.MultipartFormDataContent())
                {
                    string y = System.Web.HttpContext.Current.Server.MapPath("~/Content/UplodedImages/PostImages/" + model.Path);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(y);

                    var streamContent = new System.Net.Http.StreamContent(new System.IO.MemoryStream(fileBytes));
                    streamContent.Headers.Add("Content-Type", "application/octet-stream");
                    streamContent.Headers.Add("Content-Disposition", "form-data; name=\"photo\"; filename=" + model.Path);

                    multipartFormDataContent.Add(streamContent, "file", model.Path);
                    using (var message = client.PostAsync(uri, multipartFormDataContent))
                    {
                        var contentString = message.Result.Content.ReadAsStringAsync().Result;
                        result = contentString.ToString();
                    }
                    int index = result.IndexOf(":");
                    string IsOk = result.Substring(index + 1, result.IndexOf(",") - (index + 1));
                    return IsOk;
                }

            }
        }
    }
}