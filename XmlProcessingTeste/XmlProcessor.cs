using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlProcessingTeste
{
    public static class XmlProcessor
    {
        public static Task<Result> ProcessWithReader(string xmlMessage)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var result = new Result();

                    using (var reader = XmlReader.Create(new StringReader(xmlMessage)))
                    {
                        result.StartingTime = DateTime.Now;
                        while (reader.Read() && (string.IsNullOrEmpty(result.Header) || string.IsNullOrEmpty(result.Body)))
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "soap:Header")
                                {
                                    result.Header = reader.ReadOuterXml();
                                }

                                if (reader.Name == "soap:Body")
                                {
                                    result.Body = reader.ReadOuterXml();
                                }
                            }
                        }
                        result.EndingTime = DateTime.Now;
                    }

                    result.DifferenceTime = result.EndingTime.Subtract(result.StartingTime).ToString();

                    return result;
                }
            );
        }

        public static Task<Result> ProcessWithDocument(string xmlMessage)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var result = new Result();
                    var doc = new XmlDocument();

                    result.StartingTime = DateTime.Now;
                    doc.LoadXml(xmlMessage);
                    result.EndingTime = DateTime.Now;

                    result.Header = doc.DocumentElement.GetElementsByTagName("soap:Header")[0]?.OuterXml;
                    result.Body = doc.DocumentElement.GetElementsByTagName("soap:Body")[0]?.OuterXml;

                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlMessage)))
                    {
                        result.Size = ms.Length;
                    }

                    result.DifferenceTime = result.EndingTime.Subtract(result.StartingTime).ToString();

                    return result;
                }
            );
        }
    }
}
