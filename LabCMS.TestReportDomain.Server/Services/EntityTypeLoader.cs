using LabCMS.TestReportDomain.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.Server.Services
{
    public class EntityTypeLoader
    {
        public Type Get(EntityType entityType)
        {
            if (!_loadedTypesDict.ContainsKey(entityType.FullName))
            {
                Type type = Assembly.LoadFrom(entityType.AssemblyPath)!.GetType(entityType.FullName, true)!;
                _loadedTypesDict.TryAdd(entityType.FullName, type);
                return type;
            }
            else
            {
                return _loadedTypesDict[entityType.FullName];
            }
        }

        public Type? FindInCache(string fullName)
        {
            _loadedTypesDict.TryGetValue(fullName, out Type? type);
            return type;
        }

        private readonly ConcurrentDictionary<string,Type> _loadedTypesDict = new();
    }
}
