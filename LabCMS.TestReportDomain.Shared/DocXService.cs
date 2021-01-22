using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.Shared
{
    public class DocXService
    {
        public void ReplaceToken(DocX doc, string token, string? content)
        {
            if (content is null) { return; }
            IEnumerable<Paragraph> paragraphs = doc.Paragraphs.Where(item => item.Text.Contains(token));
            foreach (Paragraph paragraph in paragraphs)
            { paragraph.ReplaceText(token, content);}
        }
    }
}
