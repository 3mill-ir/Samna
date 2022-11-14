using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using BigMill.Models;

namespace BigMill.Controllers
{
    public class VideoController : ApiController
    {

        [Route("api/video/{ext}/{fileName}/{SpecPath}")]
        public HttpResponseMessage Get(string ext, string fileName, string SpecPath = "Default")
        {
            string videoPath = HostingEnvironment.MapPath(string.Format("~/Content/Videos/{0}.{1}", fileName, ext));
            if (SpecPath != "Default")
                videoPath = HostingEnvironment.MapPath(string.Format("~/Content/UserContent/PostVideos/{0}.{1}", fileName, ext));
            if (File.Exists(videoPath))
            {
                FileInfo fi = new FileInfo(videoPath);
                var video = new VideoStream(videoPath);

                var response = Request.CreateResponse();

                response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)video.WriteToStream,
                    new MediaTypeHeaderValue("video/" + ext));

                response.Content.Headers.Add("Content-Disposition", "attachment;filename=" + fi.Name.Replace(" ", ""));
                response.Content.Headers.Add("Content-Length", video.FileLength.ToString());

                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }


        /// <summary>
        /// in controller jahayte stream kardane file haye soti va tasviri bekar miravad
        /// </summary>
        /// <param name="type">noe file</param>
        /// <param name="fileName">name file</param>
        /// <param name="ext">pasvande file</param>
        /// <returns>
        /// HttpResponseMessage
        /// </returns>
        [Route("UserStreaming/File/{type}/{fileName}/{ext}")]
        public HttpResponseMessage Get(string type, string fileName, string ext,string t="")
        {

            string videoPath = HostingEnvironment.MapPath(string.Format("~/App_Data/TicketMediaUpload/{0}.{1}", fileName, ext));
            if (File.Exists(videoPath))
            {
                FileInfo fi = new FileInfo(videoPath);
                var video = new VideoStream(videoPath);

                var response = Request.CreateResponse();


                response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)video.WriteToStream,
                    new MediaTypeHeaderValue(type + "/" + fi.Extension));

                response.Content.Headers.Add("Content-Disposition", "attachment;filename=" + fi.Name.Replace(" ", ""));
                response.Content.Headers.Add("Content-Length", video.FileLength.ToString());

                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
