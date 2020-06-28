using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace FastProfiler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var assmeblyPath = args[0];
            var fastProfilerAssmeblyPath = args[1];
            
            Console.WriteLine("开始启动");
            try
            {
                var assembly = AssemblyDefinition.ReadAssembly(assmeblyPath,
                    new ReaderParameters { ReadSymbols = true });
                
                var fastProfilerAssembly = AssemblyDefinition.ReadAssembly(fastProfilerAssmeblyPath,
                    new ReaderParameters { ReadSymbols = false });
                
                List<TypeDefinition> typesInject = new List<TypeDefinition>();
                List<MethodDefinition> methodsInject = new List<MethodDefinition>();
                List<PropertyDefinition> fieldsInject = new List<PropertyDefinition>();
                
                CodeInject.GetAllNeedToInject(assembly, typesInject, methodsInject, fieldsInject);
                Console.WriteLine("需要测试的类:"+typesInject.Count+" 需要测试的函数:"+methodsInject.Count+" 需要测试的域:"+fieldsInject.Count);
                
                CodeInject.InitAllMethodReference(fastProfilerAssembly);

                foreach (var methodDef in methodsInject)
                {
                    CodeInject.AddProfilerMethod(methodDef);
                }
                
                foreach (var typeDef in typesInject)
                {
                    CodeInject.AddProfilerClass(typeDef);
                }

                CodeInject.InitWrapperMethodReference(fastProfilerAssembly);
                
                CodeInject.ReplaceAllMethodNeedUnityWrapper(assembly);
                    
                assembly.Write(assmeblyPath, new WriterParameters { WriteSymbols = true });
            }
            
            catch(Exception e)
            {
                //如果读取不到符号则不读
                Console.WriteLine("Warning: read " + assmeblyPath + " fail for reason: "+e);
                return;
            }
            
            
            var str = Console.ReadLine() ;
            while (str != "q")
            {
                str = Console.ReadLine() ;
            }
        }
    }
}