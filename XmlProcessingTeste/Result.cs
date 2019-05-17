using System;

namespace XmlProcessingTeste
{
    public class Result
    {
        public string DifferenceTime { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        public string NameSpace { get; set; }
        public string WsaTo { get; set; }
        public string WsaAction { get; set; }
        public string WsaRelatesTo { get; set; }
        public string WsaReplyTo { get; set; }
        public string WsaFaultTo { get; set; }
        public string WsaRetryMessages { get; set; }
        public string WsaAcceptFaultTo { get; set; }
        public string WsaFrom { get; set; }
        public string MessageID { get; set; }
        public string Message { get; set; }
    }
}