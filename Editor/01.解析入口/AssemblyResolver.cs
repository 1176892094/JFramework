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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Mono.Cecil;
using Unity.CompilationPipeline.Common.ILPostProcessing;

namespace JFramework.Editor
{
    internal class AssemblyResolver : IAssemblyResolver
    {
        private readonly Dictionary<string, AssemblyDefinition> assemblyCache = new Dictionary<string, AssemblyDefinition>();
        private readonly Logger logger;
        private readonly string[] assemblyReferences;
        private readonly ICompiledAssembly compiledAssembly;
        private AssemblyDefinition selfAssembly;

        public AssemblyResolver(ICompiledAssembly compiledAssembly, Logger logger)
        {
            this.logger = logger;
            this.compiledAssembly = compiledAssembly;
            assemblyReferences = compiledAssembly.References;
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            return Resolve(name, new ReaderParameters(ReadingMode.Deferred));
        }

        public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            lock (assemblyCache)
            {
                if (name.Name == compiledAssembly.Name) return selfAssembly;

                var fileName = FindFile(name);
                if (fileName == null)
                {
                    logger.Warn($"无法找到文件: {name}");
                    return null;
                }

                var cacheKey = fileName + File.GetLastWriteTime(fileName);

                if (assemblyCache.TryGetValue(cacheKey, out var result))
                {
                    return result;
                }

                parameters.AssemblyResolver = this;

                var ms = GetMemoryStream(fileName);

                var pdb = fileName + ".pdb";
                if (File.Exists(pdb))
                {
                    parameters.SymbolStream = GetMemoryStream(pdb);
                }

                var ad = AssemblyDefinition.ReadAssembly(ms, parameters);
                assemblyCache.Add(cacheKey, ad);
                return ad;
            }
        }

        private string FindFile(AssemblyNameReference name)
        {
            var fileName = assemblyReferences.FirstOrDefault(r => Path.GetFileName(r) == name.Name + ".dll");
            if (fileName != null) return fileName;

            fileName = assemblyReferences.FirstOrDefault(r => Path.GetFileName(r) == name.Name + ".exe");
            if (fileName != null) return fileName;

            var dirs = assemblyReferences.Select(Path.GetDirectoryName).Distinct();
            return dirs.Select(parent => Path.Combine(parent, name.Name + ".dll")).FirstOrDefault(File.Exists);
        }

        private static MemoryStream GetMemoryStream(string fileName)
        {
            return Retry(10, TimeSpan.FromSeconds(1), () =>
            {
                byte[] bytes;
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    bytes = new byte[fs.Length];
                    int length = fs.Read(bytes, 0, (int)fs.Length);
                    if (length != fs.Length)
                    {
                        throw new InvalidOperationException("文件读取长度不是文件的完整长度.");
                    }
                }

                return new MemoryStream(bytes);
            });
        }

        private static MemoryStream Retry(int retryCount, TimeSpan waitTime, Func<MemoryStream> func)
        {
            try
            {
                return func();
            }
            catch (IOException)
            {
                if (retryCount == 0) throw;
                Console.WriteLine($"捕获IO异常，尝试{retryCount}更多次。");
                Thread.Sleep(waitTime);
                return Retry(retryCount - 1, waitTime, func);
            }
        }

        public void SetAssemblyDefinitionForCompiledAssembly(AssemblyDefinition assemblyDefinition)
        {
            selfAssembly = assemblyDefinition;
        }

        public void Dispose()
        {
        }
    }
}