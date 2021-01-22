using System;
using System.Reflection;

namespace LabCMS.TestReportDomain.Shared
{
    public record Binding(string BindingPath,dynamic? Value,PropertyInfo PropertyInfo);
}