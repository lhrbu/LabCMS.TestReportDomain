using System;

namespace LabCMS.TestReportDomain.Shared
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StringFormatAttribute:Attribute
    {
        public string Format {get;}
        public StringFormatAttribute(string format)=>Format = format;
    }
}