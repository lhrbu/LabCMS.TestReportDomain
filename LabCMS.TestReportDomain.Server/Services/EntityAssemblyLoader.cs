using LabCMS.TestReportDomain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.Server.Services
{
    public class EntityAssemblyLoader
    {
        public Type Load(DynamicType dynamicType)=>
            Assembly.LoadFile(dynamicType.Path).GetType(dynamicType.FullName,true);
        
    }
}
