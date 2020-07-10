using System.Linq;
using Mono.Cecil;

namespace FastProfiler
{
    public static class CecilHelperFunc
    {
        //检查一个类 及其 基类是否特定属性
        public static  bool IsClassHasAttribute(TypeDefinition typeDef,  string attributeName, bool checkBase = false)
        {
            if (null == typeDef || string.IsNullOrEmpty(attributeName))
            {
                return false;
            }

            if (typeDef.HasCustomAttributes)
            {
                foreach (var attr in typeDef.CustomAttributes)
                {
                    if (attr.AttributeType.Name == attributeName)
                    {
                        return true;
                    }
                }
            }

            var ret = false;
            if (true == checkBase)
            {
                ret =IsClassHasAttribute(typeDef.BaseType?.Resolve(), attributeName);
            }
            return ret;
        }


        public static bool IsMethodHasAttribute(MethodDefinition methodDef,  string attributeName, bool checkBase = false)
        {
            if (null == methodDef || string.IsNullOrEmpty(attributeName))
            {
                return false;
            }
            
            if (methodDef.HasCustomAttributes)
            {
                foreach (var attr in methodDef.CustomAttributes)
                {
                    if (attr.AttributeType.Name == attributeName)
                    {
                        return true;
                    }
                }
            }

            var ret = false;
            if (true == checkBase)
            {
                foreach (var baseMethodDef in methodDef.Overrides)
                {
                    ret |= IsMethodHasAttribute(baseMethodDef?.Resolve(), attributeName);
                }              
            }
            
            return ret;
        }
    }
}