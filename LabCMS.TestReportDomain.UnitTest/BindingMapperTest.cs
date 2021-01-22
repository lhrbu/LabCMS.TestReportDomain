using LabCMS.TestReportDomain.Shared;
using System.Text.Json;
using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LabCMS.TestReportDomain.UnitTest
{
    public class A
    {
        public int Key { get; set; } = 123;
        public byte[] Content { get; set; } = Encoding.UTF8.GetBytes("haha");
    }
    public class BindingMapperTest
    {
        private readonly BindingMapper _bindingMapper=new();
        [Fact]
        public void ParseDataContextTest()
        {
            TestClassSampleCompose sample = new();
            var items = _bindingMapper.ParseDataContext(sample).ToList();
        }

        [Fact]
        public void MapTest()
        {
            using DocX doc = DocX.Load("BindingMapperDoc.docx");
            TestClassSampleCompose sample = new() { TestHeader = "MapTest" };
            _bindingMapper.Map(doc, sample);
            doc.SaveAs($"output-BindingMapperDoc");
        }
    }
}