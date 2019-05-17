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
        public static Task<ResultReader> ProcessWithReader(string xmlMessage)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var result = new ResultReader();

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

        public static Task<ResultDocument> ProcessWithDocument(string xmlMessage)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var result = new ResultDocument();
                    result.Header = new XmlDocument();
                    result.Body = new XmlDocument();
                    var doc = new XmlDocument();

                    result.StartingTime = DateTime.Now;
                    doc.LoadXml(xmlMessage);
                    result.EndingTime = DateTime.Now;

                    result.Header.InnerXml = doc.DocumentElement.GetElementsByTagName("soap:Header")[0]?.OuterXml;
                    result.Body.InnerXml = doc.DocumentElement.GetElementsByTagName("soap:Body")[0]?.OuterXml;

                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlMessage)))
                    {
                        result.Size = ms.Length;
                    }

                    result.DifferenceTime = result.EndingTime.Subtract(result.StartingTime).ToString();

                    return result;
                }
            );
        }

        public static Task<ResultReader> ProcessSimilarWSHWithReader(string xmlMessage)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var result = new ResultReader();

                    using (var reader = XmlReader.Create(new StringReader(xmlMessage)))
                    {
                        result.StartingTime = DateTime.Now;
                        while (reader.Read() && (string.IsNullOrEmpty(result.Header) || string.IsNullOrEmpty(result.Body)))
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "soap:Envelope")
                                {
                                    result.NameSpace = reader.NamespaceURI;
                                }

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

                    ValidateWsaAddressing(result);

                    return result;
                }
            );
        }

        public static Task<ResultDocument> ProcessSimilarWSHWithDocument(string xmlMessage)
        {
            return Task.Factory.StartNew(
                () =>
                {
                    var result = new ResultDocument();
                    result.Header = new XmlDocument();
                    result.Body = new XmlDocument();
                    var doc = new XmlDocument();

                    result.StartingTime = DateTime.Now;
                    doc.LoadXml(xmlMessage);
                    result.EndingTime = DateTime.Now;

                    result.Header.InnerXml = doc.DocumentElement.GetElementsByTagName("soap:Header")[0]?.OuterXml;
                    result.Body.InnerXml = doc.DocumentElement.GetElementsByTagName("soap:Body")[0]?.OuterXml;

                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlMessage)))
                    {
                        result.Size = ms.Length;
                    }

                    result.DifferenceTime = result.EndingTime.Subtract(result.StartingTime).ToString();

                    ValidateWsaAddressing(result);

                    return result;
                }
            );
        }

        private static void ValidateWsaAddressing(ResultReader result)
        {
            if (!string.IsNullOrEmpty(result.Header))
            {
                using (var reader = XmlReader.Create(new StringReader(result.Header)))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "wsa:To")
                                result.WsaTo = reader.Value;
                            else if (reader.Name == "wsa:MessageID")
                                result.MessageID = reader.Value;
                            else if (reader.Name == "wsa:From")
                                result.WsaFrom = reader.Value;
                            else if (reader.Name == "wsa:Action")
                                result.WsaAction = reader.Value;
                            else if (reader.Name == "wsa:ReplyTo")
                                result.WsaReplyTo = reader.Value;
                            else if (reader.Name == "wsa:RelatesTo")
                                result.WsaRelatesTo = reader.Value;
                            else if (reader.Name == "wsa:FaultTo")
                                result.WsaFaultTo = reader.Value;
                            else if (reader.Name == "wsa:AcceptFaultTo")
                                result.WsaAcceptFaultTo = reader.Value;
                            else if (reader.Name == "wsa:RetryMessages")
                                result.WsaRetryMessages = reader.Value;
                        }
                    }
                } 
            }
        }

        private static void ValidateWsaAddressing(ResultDocument result)
        {
            if (result.Header != null)
            {
                if (result.Header.GetElementsByTagName("wsa:To") != null && result.Header.GetElementsByTagName("wsa:To").Count > 0)
                    result.WsaTo = result.Header.GetElementsByTagName("wsa:To")[0].InnerText;

                if (result.Header.GetElementsByTagName("wsa:MessageID") != null && result.Header.GetElementsByTagName("wsa:MessageID").Count > 0)
                    result.MessageID = result.Header.GetElementsByTagName("wsa:MessageID")[0].InnerText;

                if (result.Header.GetElementsByTagName("wsa:From") != null && result.Header.GetElementsByTagName("wsa:From").Count > 0)
                    result.WsaFrom = result.Header.GetElementsByTagName("wsa:From")[0].InnerText;

                if (result.Header.GetElementsByTagName("wsa:Action") != null && result.Header.GetElementsByTagName("wsa:Action").Count > 0)
                    result.WsaAction = result.Header.GetElementsByTagName("wsa:Action")[0].InnerText;

                if (result.Header.GetElementsByTagName("wsa:ReplyTo") != null && result.Header.GetElementsByTagName("wsa:ReplyTo").Count > 0)
                    result.WsaReplyTo = result.Header.GetElementsByTagName("wsa:ReplyTo")[0].InnerText;

                if (result.Header.GetElementsByTagName("wsa:RelatesTo") != null && result.Header.GetElementsByTagName("wsa:RelatesTo").Count > 0)
                    result.WsaRelatesTo = result.Header.GetElementsByTagName("wsa:RelatesTo")[0].InnerText;

                if (result.Header.GetElementsByTagName("wsa:FaultTo") != null && result.Header.GetElementsByTagName("wsa:FaultTo").Count > 0)
                    result.WsaFaultTo = result.Header.GetElementsByTagName("wsa:FaultTo")[0].InnerText;

                if (result.Header.GetElementsByTagName("wsa:AcceptFaultTo") != null && result.Header.GetElementsByTagName("wsa:AcceptFaultTo").Count > 0)
                    result.WsaAcceptFaultTo = result.Header.GetElementsByTagName("wsa:AcceptFaultTo")[0].InnerText;

                if (result.Header.GetElementsByTagName("wsa:RetryMessages") != null && result.Header.GetElementsByTagName("wsa:RetryMessages").Count > 0)
                    result.WsaRetryMessages = result.Header.GetElementsByTagName("wsa:RetryMessages")[0].InnerText;
            }
        }
    }
}
