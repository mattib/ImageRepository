using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            var repositoryFolder = ConfigurationManager.AppSettings.Get("RepositoryFolder");
            if (string.IsNullOrEmpty(repositoryFolder))
            {

                repositoryFolder = HttpRuntime.AppDomainAppPath;
            }

            var fullDirectory = Path.Combine(repositoryFolder, "Repository");
            if (!Directory.Exists(fullDirectory))
            {
                Directory.CreateDirectory(fullDirectory);
            }
            var fullPath = Path.Combine(fullDirectory, fileId);
            Stream fs = File.OpenRead(fullPath);

            httpResponseMessage.Content = new StreamContent(fs);

            httpResponseMessage.Content.Headers.ContentType =
                new MediaTypeHeaderValue("image/*");

            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
 
            return httpResponseMessage;
        }


        public HttpResponseMessage Post(string fileId)
        {
            var task = this.Request.Content.ReadAsStreamAsync();
            task.Wait();
            Stream requestStream = task.Result;

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var currentDirectory = HttpContext.Current.Server.MapPath("");
                var fullDirectory = Path.Combine(currentDirectory, "Repository");
                if (Directory.Exists(fullDirectory))
                {
                    Directory.CreateDirectory(fullDirectory);
                }

                var fullPath = Path.Combine(fullDirectory, fileId);

                using (Stream file = File.OpenWrite(fullPath))
                {
                    CopyStream(requestStream, file);
                }
                requestStream.Close();

                response.StatusCode = HttpStatusCode.Created;
            }
            catch
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }

          
            return response;
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

    }
}
