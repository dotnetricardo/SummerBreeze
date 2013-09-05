using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerBreeze
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class BreezeNavigationPropertyAttribute : System.Attribute
    {

        public string Association { get; private set; }
        public string[] ForeignKeyNames { get; private set; }
        public string InverseProperty { get; private set; }

        public BreezeNavigationPropertyAttribute(string association, params string[] foreignKeys)
        {
            Association = association;
            ForeignKeyNames = foreignKeys;
        }
    }
}
