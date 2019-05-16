using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XmlProcessingTeste
{
    public class Result
    {
        public string DifferenceTime { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public long Size { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
    }
}
