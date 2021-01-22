using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.Shared
{
    public class DynamicType
    {
        public string Namespace { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string FullName => $"{Namespace}.{Name}";
        public string Path { get; set; } = null!;
    }
}
