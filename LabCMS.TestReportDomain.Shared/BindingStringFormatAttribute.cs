using System;

namespace LabCMS.TestReportDomain.Shared
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BindingStringFormatAttribute:Attribute
    {
        public string Format {get;}
        public BindingStringFormatAttribute(string format)=>Format = format;
    }
}