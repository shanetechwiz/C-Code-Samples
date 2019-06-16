using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class NullableIntExtenstions
    {
        public static bool IsNullOrZero(this int? value)
        {
            return value == 0 || value == null;
        }
    }
}
