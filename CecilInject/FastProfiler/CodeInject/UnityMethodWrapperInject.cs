using System;
using System.Collections.Generic;
using System.Security.Policy;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FastProfiler
{
    public static partial class CodeInject
    {
        #region Fast Profiler Unity Wrapper Name
        private static string UnityMethodWrapper = "UnityMethodWrapper";
        private static string UnityMethod_GameObject_SetActive = "UnityMethod_GameObject_SetActive";
        #endregion
        
        #region Fast Profiler Unity Wrapper Method Reference
        static MethodReference _unityWrapper_GameObject_SetActive = null;
        #endregion
        
        public static void InitWrapperMethodReference(AssemblyDefinition fastProfileAssemblyDefinition)
        {
            var module = fastProfileAssemblyDefinition.MainModule;
            foreach (var typeDef in module.Types)
            {
                if (typeDef.Name == UnityMethodWrapper)
                {
                    foreach (var methodDef in typeDef.Methods)
                    {
                        if (methodDef.Name == UnityMethod_GameObject_SetActive)
                        {
                            _unityWrapper_GameObject_SetActive = methodDef;
                        }
                    }
                }
            }
        }
        
        public static void ReplaceAllMethodNeedUnityWrapper(AssemblyDefinition assembly)
        {
            var module = assembly.MainModule;
            foreach (var typeDef in module.Types)
            {
                foreach (var methodDef in typeDef.Methods)
                {
                    if (methodDef.HasBody)
                    {
                        if (ReplaceMethodNeedToWrapper(methodDef.Body))
                        {
                            
                        }
                    }
                }
            } 
        }

        private static bool ReplaceMethodNeedToWrapper(MethodBody method)
        {
            var ret = false;
            var ilProcessor = method.GetILProcessor();
            if (null != ilProcessor)
            {
                var count = method.Instructions.Count;
                for (int i = count-1; i>= 0; --i)
                {
                    var curIL = method.Instructions[i];
                    if (curIL.OpCode.Code == Code.Callvirt)
                    {
                        var methodRef = curIL.Operand as MethodReference;
                        if (null != methodRef)
                        {
                            if (GetMethodName(methodRef) == "GameObject::SetActive")
                            {
                                var wrapperUnity = method.Method.Module.ImportReference(_unityWrapper_GameObject_SetActive);
                                ilProcessor.Replace(curIL,  ilProcessor.Create(OpCodes.Call, wrapperUnity));
                                ret = true;
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
}