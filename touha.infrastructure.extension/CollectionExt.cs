using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace touha.infrastructure.extension
{
    /// <summary>
    /// 集合扩展
    /// </summary>
    public static partial class MyExtension
    {
        /// <summary>
        /// 根据类型的属性进行自定义排序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TPropertyType">属性类型</typeparam>
        /// <param name="collection">实体集</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="sortOrder">排序方式：desc（降序） 其他升序</param>
        /// <returns></returns>
        public static List<T> CustomSort<T, TPropertyType>
        (this IEnumerable<T> collection, string propertyName, string sortOrder)
        {
            List<T> sortedlist = null;

            ParameterExpression pe = Expression.Parameter(typeof(T), "p");
            Expression<Func<T, TPropertyType>> expr = Expression.Lambda<Func<T, TPropertyType>>(Expression.Property(pe, propertyName), pe);

            if (!string.IsNullOrEmpty(sortOrder) && sortOrder == "desc")
                sortedlist = collection.OrderByDescending<T, TPropertyType>(expr.Compile()).ToList();
            else
                sortedlist = collection.OrderBy<T, TPropertyType>(expr.Compile()).ToList();

            return sortedlist;
        }
    }
}
