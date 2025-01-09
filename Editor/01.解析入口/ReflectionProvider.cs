// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-06  05:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace JFramework.Editor
{
    /// <summary>
    /// 反射导入器提供程序
    /// </summary>
    internal class ReflectionProvider : IReflectionImporterProvider
    {
        public IReflectionImporter GetReflectionImporter(ModuleDefinition module) => new ReflectionImporter(module);
    }

    /// <summary>
    /// 默认的反射导入器
    /// </summary>
    internal class ReflectionImporter : DefaultReflectionImporter
    {
        private const string SystemPrivateCoreLib = "System.Private.CoreLib";
        private readonly AssemblyNameReference fixedCoreLib;

        public ReflectionImporter(ModuleDefinition module) : base(module)
        {
            fixedCoreLib = module.AssemblyReferences.FirstOrDefault(assembly => Equals(assembly.Name));
        }

        public override AssemblyNameReference ImportReference(AssemblyName name)
        {
            if (name.Name == SystemPrivateCoreLib && fixedCoreLib != null)
            {
                return fixedCoreLib;
            }

            return base.ImportReference(name);
        }

        private static bool Equals(string name)
        {
            return name is "mscorlib" or "netstandard" or SystemPrivateCoreLib;
        }
    }
}