using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerBreeze
{
    public class BreezeDataPropertyMetadata
    {
        public string name { get; set; }
        public string dataType { get; set; }
        public bool isNullable { get; set; }
        public bool isPartOfKey { get; set; }
        public bool isUnmapped { get; set; }
        public List<string> validators { get; set; }
       
    }
}
