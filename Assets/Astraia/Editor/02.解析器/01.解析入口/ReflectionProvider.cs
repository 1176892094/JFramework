// *********************************************************************************
// # Project: Astraia
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 03:12:36
// # Recently: 2024-12-22 20:12:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Reflection;
using Mono.Cecil;

namespace Astraia.Editor
{
    internal class ReflectionProvider : IReflectionImporterProvider
    {
        public IReflectionImporter GetReflectionImporter(ModuleDefinition definition)
        {
            return new ReflectionImporter(definition);
        }
    }
    
    internal class ReflectionImporter : DefaultReflectionImporter
    {
        private readonly AssemblyNameReference assemblyName;

        public ReflectionImporter(ModuleDefinition module) : base(module)
        {
            AssemblyNameReference assemblyData = null;
            foreach (var assembly in module.AssemblyReferences)
            {
                if (assembly.Name is "mscorlib" or "netstandard" or "System.Private.CoreLib")
                {
                    assemblyData = assembly;
                    break;
                }
            }

            assemblyName = assemblyData;
        }

        public override AssemblyNameReference ImportReference(AssemblyName name)
        {
            if (assemblyName != null && name.Name == "System.Private.CoreLib")
            {
                return assemblyName;
            }

            return base.ImportReference(name);
        }
    }
}