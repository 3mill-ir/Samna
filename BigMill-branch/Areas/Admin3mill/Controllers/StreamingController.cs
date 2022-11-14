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

namespace BigMill.Areas.Admin3mill.Controllers
{
    public class StreamingController : ApiController
    {
        /// <summary>
        /// in controller jahate stream kardane file haye soti va tasviri morede estefade gharar migirad
        /// </summary>
        /// <param name="type">noe file morede nazar</param>
        /// <param name="fileName">name file morede nazar</param>
        /// <param name="ext">pasvande file morede nazar</param>
        /// <returns>
        /// HttpResponseMessage
        /// </returns>
        [Route("Streaming/File/{type}/{fileName}/{ext}")]
        public HttpResponseMessage Get(string type, string fileName, string ext)
        {

            string videoPath = HostingEnvironment.MapPath(string.Format("~/App_Data/TicketMediaUpload/{0}.{1}", fileName, ext));
            if (File.Exists(videoPath))
            {
                FileInfo fi = new FileInfo(videoPath);
                var video = new VideoStream(videoPath);

                var response = Request.CreateResponse();


                response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)video.WriteToStream,
                    new MediaTypeHeaderValue(type+"/" + fi.Extension));

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
