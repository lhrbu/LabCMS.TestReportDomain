using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace LabCMS.TestReportDomain.Shared
{
    public class PropertyMapper
    {
        public string GetToken(PropertyInfo propertyInfo) => 
            $"${{{propertyInfo.DeclaringType!.Name}.{propertyInfo.Name}}}";
        public PropertyInfo GetPropertyInfoFromToken<TEntity>(string token)
        {
            if(token.StartsWith("${")&&token.EndsWith("}")&&
                token.Count(c=>c=='.')==1)
            {
                string propertyNameElement = token.Split('.')[1];
                int length = propertyNameElement.Length;
                string propertyName = propertyNameElement.Substring(0, length - 1);
                return typeof(TEntity).GetProperty(propertyName)!;
            }
            else { throw new ArgumentException($"{token} is not a valid token", nameof(token)); }
        }
        public static PropertyInfo PropertyInfo<TEntity>(Expression<Func<TEntity, object>> expresion) =>
            expresion.Body switch
            {
                MemberExpression m => typeof(TEntity).GetProperty(m.Member.Name)!,
                UnaryExpression u when u.Operand is MemberExpression m => typeof(TEntity).GetProperty(m.Member.Name)!,
                _ => throw new InvalidOperationException($"Can't hanle this type expression {expresion.Body.Type}")
            };
    }
}
