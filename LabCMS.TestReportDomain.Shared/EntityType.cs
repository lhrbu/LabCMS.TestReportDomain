using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.Shared
{
    public class EntityType
    {
        public string TableName { get; set; } = null!;
        public string Namespace { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string FullName => $"{Namespace}.{Name}";
        public string AssemblyPath { get; set; } = null!;
    }
}
