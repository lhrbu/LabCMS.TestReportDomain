using System;
using System.Reflection;

namespace LabCMS.TestReportDomain.Shared
{
    public class TokenDecorator
    {
        public string GetToken(string bindingPath)=>$"${{{bindingPath}}}";
        public string GetBindingPath(string token)
        {
            if(token.StartsWith("${")&&token.EndsWith("}")){return token.Substring(2,token.Length-3);}
            else { throw new ArgumentException($"{token} is not a valid token", nameof(token)); }
        }
        public string GetBindingPath(PropertyInfo propertyInfo,string? prefix=null)=>
            prefix is null?propertyInfo.Name:$"{prefix}.{propertyInfo.Name}";
    }
}