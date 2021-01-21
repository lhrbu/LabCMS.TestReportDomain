using LabCMS.TestReportDomain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.UnitTest
{
    public class TestClassSample
    {
        public int TestProperty { get; set; }
        [IgnoreInMapping]
        public int IgnoreProperty { get; set; }
    }
}
