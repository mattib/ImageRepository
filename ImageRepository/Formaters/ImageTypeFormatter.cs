using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace ImageRepository.Formaters
{
    public class ImageTypeFormatter : MediaTypeFormatter
    {
        public ImageTypeFormatter()
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/gif"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));

        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(Stream); // type == typeof(RepositoryFileAttributes);
        }

        public override bool CanReadType(Type type)
        {
            return  type == typeof(byte[]);
        }

        public async override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, System.Net.TransportContext transportContext)
        {
            base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }

        public async override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {

            var taskCompletionSource = new TaskCompletionSource<object>();
            try
            {
                var memoryStream = new MemoryStream();
                CopyStream(readStream, memoryStream);
                taskCompletionSource.SetResult(memoryStream);
            }
            catch (Exception e)
            {
                taskCompletionSource.SetException(e);
            }
            //await taskCompletionSource.Task;
            return taskCompletionSource.Task;
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