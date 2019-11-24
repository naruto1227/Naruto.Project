using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fate.Infrastructure.Infrastructure
{
    /// <summary>
    /// 去重
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyComparer<T> : IEqualityComparer<T>
    {

        private Func<T, object> getPropertyValueFunc = null;

        /// <summary>
        /// 通过propertyName 获取PropertyInfo对象 (需要按照什么字段过滤)
        /// </summary>
        /// <param name="propertyName"></param>
        public PropertyComparer(string propertyName)
        {
            PropertyInfo _PropertyInfo = typeof(T).GetProperty(propertyName,
            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (_PropertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.",
                    propertyName, typeof(T)));
            }
            ParameterExpression expPara = Expression.Parameter(typeof(T), "obj");
            MemberExpression me = Expression.Property(expPara, _PropertyInfo);
            getPropertyValueFunc = Expression.Lambda<Func<T, object>>(me, expPara).Compile();
        }

        #region IEqualityComparer<T> Members
        public bool Equals(T x, T y)
        {
            object xValue = getPropertyValueFunc(x);
            object yValue = getPropertyValueFunc(y);

            if (xValue == null)
                return yValue == null;

            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            object propertyValue = getPropertyValueFunc(obj);

            if (propertyValue == null)
                return 0;
            else
                return propertyValue.GetHashCode();
        }

        #endregion
    }
}
