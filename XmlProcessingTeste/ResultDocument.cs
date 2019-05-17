using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace XmlProcessingTeste
{
    public class ResultDocument : Result
    {
        public XmlDocument Header { get; set; }
        public XmlDocument Body { get; set; }
        public long Size { get; set; }
    }
}
