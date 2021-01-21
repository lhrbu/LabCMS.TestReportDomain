using LabCMS.TestReportDomain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;
using Xunit;

namespace LabCMS.TestReportDomain.UnitTest
{
    public class TestReportMapperTest
    {
        private readonly TestReportMapper _testReportMapper = new();
        [Fact]
        public void MapTest()
        {
            TestClassSample testClassSample = new() { TestProperty = 123, IgnoreProperty = 456 };
            using DocX docX = DocX.Load("template2.docx");
            _testReportMapper.Map(docX, testClassSample);
            docX.SaveAs($"output2-{DateTimeOffset.Now.ToString("yyyy-MM-dd HH-mm-ss")}");
        }

        [Fact] void MapComposeTest()
        {
            TestClassSampleCompose testClassSampleCompose = new();
            testClassSampleCompose.TestClassSample.TestProperty = 99999;
            using DocX docX = DocX.Load("template3.docx");
            _testReportMapper.Map(docX, testClassSampleCompose);
            docX.SaveAs($"output3-{DateTimeOffset.Now.ToString("yyyy-MM-dd HH-mm-ss")}");
        }
    }
}
