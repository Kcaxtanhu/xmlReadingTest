
using System;
using System.IO;
using System.Linq;
using System.Text;
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

            // PerformFileReaderReading($"{ path }\\SoapEnvelopMessage.xml").Wait();
            // Console.WriteLine($"{ Environment.NewLine }******************************************************************************{ Environment.NewLine }");
            // PerformFileDocumentReading($"{ path }\\SoapEnvelopMessage.xml").Wait();

            #region FromString
            var xmlString = $@"
            <soap:Envelope xmlns:wsrm=""http://schemas.xmlsoap.org/ws/2005/02/rm"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soap:Header>
                    <wsa:RelatesTo>r320715b-1b75-41af-a5bb-E03ea9885cEm</wsa:RelatesTo> 
                    <wsa:ReplyTo>http://demo9647435.mockable.io/routerSendDummy</wsa:ReplyTo>
                    <wsa:Action>teste</wsa:Action>
                    <!--<wsrm:SequenceFault>sdfdsfsdfds</wsrm:SequenceFault>
                    <wsrm:Sequence>sdfsd</wsrm:Sequence>
                    <wsrm:SequenceAcknowledgement>erer</wsrm:SequenceAcknowledgement>
                    <wsrm:AckRequested>wer</wsrm:AckRequested>-->
                    <wsa:To>rerer</wsa:To>
                    <wsa:MessageID>350f7679-c326-445d-880c-9eb7f419e624</wsa:MessageID>
                    <wsa:From>IMT</wsa:From>
                    <wsa:FaultTo>teste.io</wsa:FaultTo>
                </soap:Header>
                <soap:Body>
                    <urn:InstaurarProcessoRequest xmlns:urn=""urn:instauracaoAnulacaoProcesso"">
                    <urn:Declaracao>
                        <urn:numeroFiscal>123456789</urn:numeroFiscal>
                        <urn:codEntidade>2EA</urn:codEntidade>
                        <urn:sf>0019</urn:sf>
                        <urn:autoNoticia>
                        <urn:id>111222333101</urn:id>
                        <urn:dataEmissao>2019-03-13T00:00:00.000+01:00</urn:dataEmissao>
                        </urn:autoNoticia>
                                        <urn:infracoes>
                        <urn:id>10</urn:id>
                        <urn:codigo>A00141</urn:codigo>
                        <urn:dataInfracao>2018-10-25T00:00:00.000+01:00</urn:dataInfracao>
                        <urn:parametrosAdicionais>
                            <urn:codigo>15</urn:codigo>
                            <urn:valor>Testes com IMT</urn:valor>
                        </urn:parametrosAdicionais>
                        <urn:parametrosAdicionais>
                            <urn:codigo>40</urn:codigo>
                            <urn:valor>111222333100_DL</urn:valor>
                        </urn:parametrosAdicionais>
                        <urn:parametrosAdicionais>
                            <urn:codigo>41</urn:codigo>
                            <urn:valor>N</urn:valor>
                        </urn:parametrosAdicionais>
                        </urn:infracoes>
                    </urn:Declaracao>
                    </urn:InstaurarProcessoRequest>
                </soap:Body>
            </soap:Envelope>";

            PerformStringReaderReading(xmlString).Wait();
            Console.WriteLine($"{ Environment.NewLine }******************************************************************************{ Environment.NewLine }");
            PerformStringDocumentReading(xmlString).Wait();
            #endregion

            Console.Read();
        }

        private static async Task PerformFileReaderReading(string path)
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

        private static async Task PerformFileDocumentReading(string path)
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
    
        private static async Task PerformStringReaderReading(string xmlString)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var header = string.Empty;
                    var body = string.Empty;
                    DateTime init;
                    DateTime end;

                    using (var reader = XmlReader.Create(new StringReader(xmlString)))
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

        private static async Task PerformStringDocumentReading(string xmlString)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var header = string.Empty;
                    var body = string.Empty;
                    var doc = new XmlDocument();
                    var size = 0L;
                    DateTime init = DateTime.Now;
                    doc.LoadXml(xmlString);
                    DateTime end = DateTime.Now;

                    header = doc.DocumentElement.GetElementsByTagName("soap:Header")[0].OuterXml;
                    body = doc.DocumentElement.GetElementsByTagName("soap:Body")[0].OuterXml;

                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
                    {
                        size = ms.Length;
                    }

                    Console.WriteLine($"The Documente size is { size } bytes.");
                    Console.WriteLine($"The Documente started loading at { init } and finished at { end }. So the processing time was { end.Subtract(init) }");
                }
            );
        }
    }
}
