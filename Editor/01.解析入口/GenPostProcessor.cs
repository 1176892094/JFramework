// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-06-06  05:06
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Unity.CompilationPipeline.Common.ILPostProcessing;

namespace JFramework.Editor
{
    [Serializable]
    internal class GenPostProcessor : ILPostProcessor
    {
        /// <summary>
        /// 后处理日志
        /// </summary>
        private readonly Log logger = new Log();

        public override ILPostProcessor GetInstance() => this;

        public override bool WillProcess(ICompiledAssembly compiledAssembly)
        {
            return compiledAssembly.Name == Const.ASSEMBLY_NAME || FindAssembly(compiledAssembly);
        }

        private static bool FindAssembly(ICompiledAssembly compiledAssembly)
        {
            return compiledAssembly.References.Any(path => Path.GetFileNameWithoutExtension(path) == Const.ASSEMBLY_NAME);
        }

        public override ILPostProcessResult Process(ICompiledAssembly compiledAssembly)
        {
            using var ar = new AssemblyResolver(compiledAssembly, logger);
            using var ss = new MemoryStream(compiledAssembly.InMemoryAssembly.PdbData);
            var rp = new ReaderParameters
            {
                SymbolStream = ss,
                SymbolReaderProvider = new PortablePdbReaderProvider(),
                AssemblyResolver = ar,
                ReflectionImporterProvider = new ReflectionProvider(),
                ReadingMode = ReadingMode.Immediate
            };
            var pd = compiledAssembly.InMemoryAssembly.PeData;
            using var ms = new MemoryStream(pd);
            using var ad = AssemblyDefinition.ReadAssembly(ms, rp);
            ar.SetAssemblyDefinitionForCompiledAssembly(ad);
            var process = new Process(logger);
            if (!process.Execute(ad, ar, out var change) || !change)
            {
                return new ILPostProcessResult(compiledAssembly.InMemoryAssembly, logger.logs);
            }

            var mm = ad.MainModule;
            if (mm.AssemblyReferences.Any(assembly => assembly.Name == ad.Name.Name))
            {
                var an = mm.AssemblyReferences.First(assembly => assembly.Name == ad.Name.Name);
                mm.AssemblyReferences.Remove(an);
            }

            using var pe = new MemoryStream();
            using var pdb = new MemoryStream();

            var wp = new WriterParameters
            {
                SymbolWriterProvider = new PortablePdbWriterProvider(),
                SymbolStream = pdb,
                WriteSymbols = true
            };


            ad.Write(pe, wp);
            return new ILPostProcessResult(new InMemoryAssembly(pe.ToArray(), pdb.ToArray()), logger.logs);
        }
    }
}