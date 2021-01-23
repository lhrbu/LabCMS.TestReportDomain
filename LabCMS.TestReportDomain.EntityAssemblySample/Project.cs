using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.EntityAssemblySample
{
    [Table("projects")]
    public class Project
    {
        [ExplicitKey]
        public string no { get; set; } = null!;
        public string name { get; set; } = null!;
        public string name_in_fin { get; set; } = null!;
    }
}
