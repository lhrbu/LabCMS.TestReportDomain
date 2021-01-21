using LabCMS.TestReportDomain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LabCMS.TestReportDomain.UnitTest
{

    public class PropertyMapperTest
    {
        private readonly PropertyMapper _propertyMapper = new();

        [Fact]
        public void GetTokenTest()
        {
            string token = _propertyMapper.GetToken(typeof(TestClassSample).GetProperty("TestProperty")!);
            Assert.Equal("${TestClassSample.TestProperty}", token);
        }

        [Fact]
        public void GetPropertyInfoFromTokenTest()
        {
            string propertyName = _propertyMapper.GetPropertyInfoFromToken<TestClassSample>("${TestClassSample.TestProperty}").Name;
            Assert.Equal(nameof(TestClassSample.TestProperty), propertyName);
        }

        [Fact]
        public void GetPropertyNameTest()
        {
            string name = PropertyMapper.PropertyInfo<TestClassSample>(item => item.TestProperty).Name;
            Assert.Equal(nameof(TestClassSample.TestProperty), name);
        }
    }
}
