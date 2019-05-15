
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace XmlProcessingTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Find below the time spent on eahc compilation");

            var curDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            var path = Directory.GetParent(curDirectory).FullName;

            PerformStreamReading($"{ path }\\SoapEnvelopMessage.xml").Wait();
            Console.WriteLine($"{ Environment.NewLine }******************************************************************************{ Environment.NewLine }");
            PerformSDocumentReading($"{ path }\\SoapEnvelopMessage.xml").Wait();

            Console.Read();
        }

        private static async Task PerformStreamReading(string path)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var header = string.Empty;
                    var body = string.Empty;
                    DateTime init;
                    DateTime end;

                    using (var reader = XmlReader.Create(path))
                    {
                        init = DateTime.Now;
                        while (reader.Read() && (string.IsNullOrEmpty(header) || string.IsNullOrEmpty(body)))
                        {
                            if (reader.IsStartElement())
                            {
                                if (reader.Name == "soap:Header")
                                {
                                    header = reader.ReadOuterXml();
                                }

                                if (reader.Name == "soap:Body")
                                {
                                    body = reader.ReadOuterXml();
                                }
                            }
                        }
                        end = DateTime.Now;
                    }
                    Console.WriteLine($"The Reader started at { init } and finished at { end }. So the processing time was { end.Subtract(init) }");
                    // Console.WriteLine($"Header:{ Environment.NewLine }{ header }{ Environment.NewLine }Body:{ Environment.NewLine }{ body }");
                }
            );
        }

        private static async Task PerformSDocumentReading(string path)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var header = string.Empty;
                    var body = string.Empty;
                    var doc = new XmlDocument();
                    var size = 0L;
                    DateTime init = DateTime.Now;
                    using (var stream = File.Open(path, FileMode.Open))
                    {
                        size = stream.Length;
                        doc.Load(stream);
                    }
                    DateTime end = DateTime.Now;

                    header = doc.DocumentElement.GetElementsByTagName("soap:Header")[0].OuterXml;
                    body = doc.DocumentElement.GetElementsByTagName("soap:Body")[0].OuterXml;

                    Console.WriteLine($"The Documente size is { size } bytes.");
                    Console.WriteLine($"The Documente started loading at { init } and finished at { end }. So the processing time was { end.Subtract(init) }");
                    // Console.WriteLine($"Header:{ Environment.NewLine }{ header }{ Environment.NewLine }Body:{ Environment.NewLine }{ body }");
                }
            );
        }
    }
}
