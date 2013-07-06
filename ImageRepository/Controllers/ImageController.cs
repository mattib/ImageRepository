using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ImageRepository.Controllers
{
    public class ImageController : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get(string fileId)
        {
             
            //return new string[] { "value1", "value2" };

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            Stream fs = File.OpenRead(@"H:\Matti's Document's\GitHub\ImageRepository\ImageRepository\Repository\ben-avraham.jpg");

            httpResponseMessage.Content = new StreamContent(fs);

            httpResponseMessage.Content.Headers.ContentType =
                new MediaTypeHeaderValue("image/*");

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
            //httpResponseMessage.Content.Headers.ContentDisposition =
            //    new ContentDispositionHeaderValue("attachment")
            //    {
            //        FileName = "ben-avraham.jpg"
            //    };
            return httpResponseMessage;
        }

    }
}
