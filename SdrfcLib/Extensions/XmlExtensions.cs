using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace FrequencyManagerCommon.Extensions
{
    public static class XmlExtensions
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));

                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var stringWriter = new StringWriter())
                using (XmlTextWriter writer = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented })
                {
                    xmlserializer.Serialize(streamWriter, value);
                    return Encoding.ASCII.GetString(memoryStream.ToArray()).Substring(3);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}
