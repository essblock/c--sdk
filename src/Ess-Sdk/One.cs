using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ess.Sdk
{
    public class One
    {
        public static readonly long Value = 10000000;

        public static implicit operator double(One d)
        {
            return 1;
        }

        //public static implicit operator long(One d)
        //{
        //    return Value;
        //}
    }
}
