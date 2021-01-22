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
    public class BindingMapperTest
    {
        private readonly BindingMapper _bindingMapper=new();
        [Fact]
        public void ParseDataContextTest()
        {
            TestClassSampleCompose sample = new();
            var items = _bindingMapper.ParseDataContext(sample).ToList();
        }
    }
}