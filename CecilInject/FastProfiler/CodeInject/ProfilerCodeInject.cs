using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FastProfiler
{
    //性能检测代码注入
    public static partial class CodeInject
    {
        //
        private static string FastProfileClassAttribute = "FastProfileClassAttribute";
        private static string FastProfileMethodAttribute = "FastProfileMethodAttribute";

        //
        private static string FastProfiler = "FastProfiler";
        private static string StartRecordMethod = "StartRecordMethod";
        private static string EndRecordMethod = "EndRecordMethod";
        
        private static string ClassInstanceCreated = "ClassInstanceCreated";
        private static string ClassInstanceWithdraw = "ClassInstanceWithdraw";
        /// <summary>
        /// 获取所有需要插入代码的元素 类和方法
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="classInject"></param>
        /// <param name="methodInject"></param>
        public static void GetAllNeedToInject(AssemblyDefinition assembly, List<TypeDefinition> classesInject, List<MethodDefinition> methodsInject, List<PropertyDefinition> propertysInject)
        {
            classesInject.Clear();
            methodsInject.Clear();
            propertysInject.Clear();
            
            var module = assembly.MainModule;
            foreach (var typeDef in module.Types)
            {
                if (typeDef.Name == "ChildMonoClass")
                {
                    Console.WriteLine("afaf");
                }
                
                if (CecilHelperFunc.IsClassHasAttribute(typeDef, FastProfileClassAttribute))
                {

                    if (Program.isOutDebug)
                    {
                        Console.WriteLine("attribute "+FastProfileClassAttribute+" class name is "+typeDef.Name);
                    }
                    
                    classesInject.Add(typeDef);                    
                }

                var hasMethodAttribute = false;
                foreach (var methodDef in typeDef.Methods)
                {
                    hasMethodAttribute = CecilHelperFunc.IsMethodHasAttribute(methodDef, FastProfileMethodAttribute);
                    
                    if (hasMethodAttribute)
                    {
                        if (Program.isOutDebug)
                        {
                            Console.WriteLine("attribute "+FastProfileMethodAttribute+" class name is "+typeDef.Name);
                        }                            
                        methodsInject.Add(methodDef);
                    }
                }
                
                foreach (var propertyDef in typeDef.Properties)
                {
                    if (propertyDef.HasCustomAttributes)
                    {
                        foreach (var attr in propertyDef.CustomAttributes)
                        {
                            if (attr.AttributeType.Name == FastProfileMethodAttribute)
                            {
                                if (Program.isOutDebug)
                                {
                                    Console.WriteLine("property attribute "+attr.AttributeType.Name+" class name is "+typeDef.Name+" property name is "+propertyDef.Name);
                                }                                
                                propertysInject.Add(propertyDef);
                            }
                        }
                    }
                }
            }
        }
        
        public static void InitAllMethodReference(AssemblyDefinition fastProfileAssemblyDefinition)
        {
            var module = fastProfileAssemblyDefinition.MainModule;
            foreach (var typeDef in module.Types)
            {
                if (typeDef.Name == FastProfiler)
                {
                    foreach (var methodDef in typeDef.Methods)
                    {
                        if (methodDef.Name == StartRecordMethod)
                        {
                            startMethodReference = methodDef;
                        }
                        if (methodDef.Name == EndRecordMethod)
                        {
                            endMethodReference = methodDef;
                        }
                        if (methodDef.Name == ClassInstanceCreated)
                        {
                            classInstanceCreated = methodDef;
                        }
                        if (methodDef.Name == ClassInstanceWithdraw)
                        {
                            classInstanceWithdraw = methodDef;
                        }
                    }
                }
            }
        }

        static MethodReference startMethodReference = null;
        static MethodReference endMethodReference = null;
        static MethodReference classInstanceCreated = null;
        static MethodReference classInstanceWithdraw = null;
        
        public static void AddProfilerMethod(MethodDefinition methodDef)
        {
            var startRef = methodDef.Module.ImportReference(startMethodReference);
            var endRef = methodDef.Module.ImportReference(endMethodReference);
            // 开始注入IL代码
            var ilProcessor = methodDef.Body.GetILProcessor();
            
            var startPoint = methodDef.Body.Instructions[0];
            Console.WriteLine(methodDef.DeclaringType.Name+"::"+methodDef.Name+" inject the method");
            
            ilProcessor.InsertBefore(startPoint, ilProcessor.Create(OpCodes.Ldstr, methodDef.DeclaringType.Name+"::"+methodDef.Name));
            ilProcessor.InsertBefore(startPoint, ilProcessor.Create(OpCodes.Call, startRef));
            
            var retInstructions = methodDef.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret).ToList();
            //将所有ret都直接修改为注入代码 这样所有跳转和移除处理的引用都变成了 注入代码。然后再注入代码后面添加上ret语句
            foreach (var retIns in retInstructions)
            {
                Console.WriteLine(retIns);
                retIns.OpCode = OpCodes.Ldstr;
                retIns.Operand = methodDef.DeclaringType.Name + "::" + methodDef.Name;
                Console.WriteLine(retIns);
                
                ilProcessor.InsertAfter(retIns, ilProcessor.Create(OpCodes.Ret));
                ilProcessor.InsertAfter(retIns, ilProcessor.Create(OpCodes.Call, endRef));
            }
        }
        
        public static void AddProfilerClass(TypeDefinition typeDef)
        {
            bool hasFinalizeMethod = false;
            
            // 开始注入IL代码
            foreach (var methodDef in typeDef.Methods)
            {
                if (false == methodDef.HasBody)
                { 
                    continue;
                }
                
                if (methodDef.IsConstructor)
                {
                    Console.WriteLine("type "+typeDef.Name+" the method "+methodDef.Name+" is cconstructor");
                    
                    var createRef = methodDef.Module.ImportReference(classInstanceCreated);
                    // 开始注入IL代码
                    var ilProcessor = methodDef.Body.GetILProcessor();
                    var startPoint = methodDef.Body.Instructions[0];
                    
                    ilProcessor.InsertBefore(startPoint, ilProcessor.Create(OpCodes.Call, createRef));
                }


                if (methodDef.Name == "Finalize")
                {
                    hasFinalizeMethod = true;
                    
                    Console.WriteLine("type "+typeDef.Name+" the method "+methodDef.Name+" is Finalize");
                    
                    var finalizeRef = methodDef.Module.ImportReference(classInstanceWithdraw);
                    // 开始注入IL代码
                    var ilProcessor = methodDef.Body.GetILProcessor();
                    var startPoint = methodDef.Body.Instructions[0];
                    
                    ilProcessor.InsertBefore(startPoint, ilProcessor.Create(OpCodes.Call, finalizeRef));
                }
            }

            if (false == hasFinalizeMethod)
            { 
                var finalizeRef = typeDef.Module.ImportReference(classInstanceWithdraw);
                
                Console.WriteLine("create finalize method for type "+typeDef.Name);
                var finalizeMethodDef = new MethodDefinition("Finalize",
                    MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.Virtual |
                    MethodAttributes.ReuseSlot, typeDef.Module.Import(typeof (void)));
                
/*
                var systemType = typeDef.Module.ImportReference(typeof(System.Object));
                var baseFinalizeMethodDef = systemType.Resolve().Methods.Single(m => m.Name == "Finalize");
                typeDef.Module.ImportReference(baseFinalizeMethodDef);
*/            

                var ilProcessor = finalizeMethodDef.Body.GetILProcessor();
                
                var nop = ilProcessor.Create(OpCodes.Nop);
                var ret = ilProcessor.Create (OpCodes.Ret);
                var leave = ilProcessor.Create (OpCodes.Leave, ret);
                
                var ldarg = ilProcessor.Create(OpCodes.Ldarg_0);
                var callBase = ilProcessor.Create(OpCodes.Call, finalizeRef);
                var final = ilProcessor.Create(OpCodes.Endfinally);
                
                ilProcessor.Append( nop);
                ilProcessor.InsertAfter(nop, leave);
                ilProcessor.InsertAfter(leave, ldarg);
                ilProcessor.InsertAfter(ldarg, callBase);
                ilProcessor.InsertAfter(callBase, final);
                ilProcessor.InsertAfter(final, ret);
                
                var handler = new ExceptionHandler (ExceptionHandlerType.Finally) {
                    TryStart = leave,
                    TryEnd = ldarg,
                    HandlerStart = ldarg,
                    HandlerEnd = ret,
                };
                
                finalizeMethodDef.Body.ExceptionHandlers.Add (handler);
                typeDef.Methods.Add(finalizeMethodDef);
            }
        }


        public static string GetMethodName(MethodReference methodRef)
        {
            return methodRef.DeclaringType.Name + "::" + methodRef.Name;
        }
    }
}

