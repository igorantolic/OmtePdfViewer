using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmtePdfViewer
{

    public class Document
    {
        public string uuid { get; set; }
        public bool success { get; set; }
        public string referenceId { get; set; }
        public string format { get; set; }
        public string content { get; set; }
        public string sha256 { get; set; }
    }

    public class Metadata
    {
        public string uuid { get; set; }
        public DateTime time { get; set; }
    }

    public class Omte
    {
        public Metadata metadata { get; set; }
        public List<Document> documents { get; set; }
        public string status { get; set; }
    }
}
