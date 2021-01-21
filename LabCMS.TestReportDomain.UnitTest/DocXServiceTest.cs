using LabCMS.TestReportDomain.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;
using Xunit;
using Xunit.Sdk;

namespace LabCMS.TestReportDomain.UnitTest
{
    public class DocXServiceTest
    {
        private readonly DocXService docXService = new();
        [Fact]
        public void ReplaceTokeTest()
        {
            using DocX docX = DocX.Load("template1.docx");
            docXService.ReplaceToken<TestClassSample>(docX, item => item.TestProperty, "Replaced Value");
            docX.SaveAs($"output1-{DateTimeOffset.Now.ToString("yyyy-MM-dd HH-mm-ss")}");
        }
    }
}
