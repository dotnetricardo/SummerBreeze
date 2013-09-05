using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerBreeze
{
    public class BreezeNavigationPropertyMetadata
    {
        public string name { get; set; }
        public string entityTypeName { get; set; }
        public bool isScalar { get; set; }
        public string[] foreignKeyNames { get; set; }
        public string associationName { get; set; }
       
       
    }
}
