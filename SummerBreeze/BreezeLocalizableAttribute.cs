using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerBreeze
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class BreezeLocalizableAttribute : System.Attribute
    {

        public string[] Include { get; private set; }
        public bool UnMapAll { get; private set; }

        public BreezeLocalizableAttribute() : this(false) { }
        
        public BreezeLocalizableAttribute(bool unmapAll, params string[] include)
        {
            UnMapAll = unmapAll;
            Include = include;
        }

      
    }
}
