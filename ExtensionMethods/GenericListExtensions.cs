using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    public static class GenericListExtensions
    {
        public static bool IsNullOrEmpty<T>(this IList<T> entityList)
        {
            return entityList == null || !entityList.Any();
        }
    }
}
