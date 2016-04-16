using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YAXLib;

namespace Yandex.Synchronizer.Custom.Formatters
{
    public class YAXFormatter : MediaTypeFormatter
    {
        public YAXFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.Factory.StartNew(() =>
            {
                YAXSerializer serializer = new YAXSerializer(type);

                using (StreamReader sr = new StreamReader(readStream))
                {
                    string sXml = sr.ReadToEnd();

                    return serializer.Deserialize(sXml);
                }
            });
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                YAXSerializer serializer = new YAXSerializer(type);

                string xml = serializer.Serialize(value);

                byte[] buffer = Encoding.UTF8.GetBytes(xml);
                writeStream.Write(buffer, 0, buffer.Length);
                writeStream.Flush();
            });
        }
    }
}