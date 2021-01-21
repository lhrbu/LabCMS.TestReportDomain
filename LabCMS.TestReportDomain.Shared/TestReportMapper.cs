using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace LabCMS.TestReportDomain.Shared
{
    public class TestReportMapper
    {
        private readonly DocXService _docxService;
        public TestReportMapper(DocXService? docxService=null)
        { _docxService = docxService ?? new(); }
        public DocX Map<TEntity>(DocX templateDoc,TEntity report)
        {
            PropertyInfo[] propertyInfos = typeof(TEntity).GetProperties();
            foreach(PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetCustomAttribute<IgnoreInMappingAttribute>() is null)
                {
                    dynamic? value = propertyInfo.GetValue(report);
                    string? content = value switch
                    {
                        string contentString => contentString,
                        DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("yyyy-MM-dd"),
                        DateTime dateTime => dateTime.ToString("yyyy-MM-dd"),
                        _ when propertyInfo.PropertyType.IsPrimitive=> value?.ToString(),
                        _ => null
                    };
                    _docxService.ReplaceToken<TEntity>(templateDoc, propertyInfo, content);
                    if ((content is null) && (value is not Delegate))
                    {
                        Map(templateDoc, value);
                    }
                }
            }
            return templateDoc;
        }
    }
}
