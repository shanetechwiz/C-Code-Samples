using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Classes
{
    public class Global
    {
        public static Expression<Func<T, bool>> ExpressionBuilder<T>(long id)
        {
            var item = System.Linq.Expressions.Expression.Parameter(typeof(T));
            var property = System.Linq.Expressions.Expression.Property(item, "Id");
            var value = System.Linq.Expressions.Expression.Constant(id);
            var equal = System.Linq.Expressions.Expression.Equal(property, value);
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equal, item);
            return lambda;
        }
    }
}
