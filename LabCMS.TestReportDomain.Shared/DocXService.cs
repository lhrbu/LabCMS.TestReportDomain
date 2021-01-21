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
    public class DocXService
    {
        private readonly PropertyMapper _propertyMapper;
        public DocXService(PropertyMapper? propertyMapper=null)
        { _propertyMapper = propertyMapper ?? new(); }

        public void ReplaceToken<TEntity>(DocX doc, Expression<Func<TEntity, object>> propertyExpresion, string? content) =>
            ReplaceToken(doc, _propertyMapper.GetToken(PropertyMapper.PropertyInfo(propertyExpresion)), content);
        public void ReplaceToken<TEntity>(DocX doc, PropertyInfo propertyInfo, string? content) =>
            ReplaceToken(doc, _propertyMapper.GetToken(propertyInfo), content);
        public void ReplaceToken(DocX doc, string token, string? content)
        {
            if (content is null) { return; }
            IEnumerable<Paragraph> paragraphs = doc.Paragraphs.Where(item => item.Text.Contains(token));
            foreach (Paragraph paragraph in paragraphs)
            { paragraph.ReplaceText(token, content);}
        }
    }
}
