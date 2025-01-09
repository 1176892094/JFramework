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
using System.Linq;
using JFramework.Net;
using Mono.Cecil;
using UnityEngine;

namespace JFramework.Editor
{
    internal static class Extensions
    {
        public static bool Is(this TypeReference self, Type type)
        {
            return type.IsGenericType ? self.GetElementType().FullName == type.FullName : self.FullName == type.FullName;
        }

        public static bool Is<T>(this TypeReference self)
        {
            return Is(self, typeof(T));
        }

        private static bool IsDerivedFrom(this TypeReference self, Type type)
        {
            var td = self.Resolve();
            if (!td.IsClass)
            {
                return false;
            }

            var tr = td.BaseType;
            if (tr == null)
            {
                return false;
            }

            if (tr.Is(type))
            {
                return true;
            }

            return tr.CanResolve() && IsDerivedFrom(tr.Resolve(), type);
        }

        public static bool IsDerivedFrom<T>(this TypeReference self)
        {
            return IsDerivedFrom(self, typeof(T));
        }

        public static bool CanResolve(this TypeReference self)
        {
            while (self != null)
            {
                if (self.Scope.Name == "Windows")
                {
                    return false;
                }

                if (self.Scope.Name == "mscorlib")
                {
                    var resolved = self.Resolve();
                    return resolved != null;
                }

                try
                {
                    self = self.Resolve().BaseType;
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public static IEnumerable<FieldDefinition> FindAllPublicFields(this TypeDefinition self)
        {
            while (self != null)
            {
                foreach (var field in self.Fields)
                {
                    if (field.IsStatic || field.IsPrivate || field.IsFamily)
                    {
                        continue;
                    }

                    if (field.IsAssembly)
                    {
                        continue;
                    }

                    if (field.IsNotSerialized)
                    {
                        continue;
                    }

                    yield return field;
                }

                try
                {
                    self = self.BaseType?.Resolve();
                }
                catch (AssemblyResolutionException)
                {
                    break;
                }
            }
        }

        public static IEnumerable<MethodDefinition> GetConstructors(this TypeDefinition self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return !self.HasMethods ? Array.Empty<MethodDefinition>() : self.Methods.Where(method => method.IsConstructor);
        }

        public static bool ImplementsInterface<T>(this TypeDefinition self)
        {
            var td = self;
            while (td != null)
            {
                if (td.Interfaces.Any(implementation => implementation.InterfaceType.Is<T>()))
                {
                    return true;
                }

                try
                {
                    var tr = td.BaseType;
                    td = tr?.Resolve();
                }
                catch (AssemblyResolutionException)
                {
                    break;
                }
            }

            return false;
        }

        public static bool HasCustomAttribute<T>(this ICustomAttributeProvider self)
        {
            return self.CustomAttributes.Any(attribute => attribute.AttributeType.Is<T>());
        }

        public static GenericInstanceType MakeGenericInstanceType(this TypeReference self, params TypeReference[] arguments)
        {
            var instanceType = new GenericInstanceType(self);
            foreach (var tr in arguments)
            {
                instanceType.GenericArguments.Add(tr);
            }

            return instanceType;
        }

        public static MethodReference MakeHostInstanceGeneric(this MethodReference self, ModuleDefinition md, GenericInstanceType generic)
        {
            var mr = new MethodReference(self.Name, self.ReturnType, generic)
            {
                HasThis = self.HasThis,
                ExplicitThis = self.ExplicitThis,
                CallingConvention = self.CallingConvention
            };

            foreach (var param in self.Parameters)
            {
                mr.Parameters.Add(new ParameterDefinition(param.ParameterType));
            }

            foreach (var param in self.GenericParameters)
            {
                mr.GenericParameters.Add(new GenericParameter(param.Name, mr));
            }

            return md.ImportReference(mr);
        }

        public static FieldReference SpecializeField(this FieldReference self, ModuleDefinition md, GenericInstanceType generic)
        {
            return md.ImportReference(new FieldReference(self.Name, self.FieldType, generic));
        }

        public static TypeReference GetEnumUnderlyingType(this TypeDefinition self)
        {
            foreach (var field in self.Fields.Where(field => !field.IsStatic))
            {
                return field.FieldType;
            }

            throw new ArgumentException($"无效的枚举类型：{self.FullName}");
        }

        public static bool IsMultidimensionalArray(this TypeReference self)
        {
            return self is ArrayType { Rank: > 1 };
        }

        public static MethodReference MakeGeneric(this MethodReference self, ModuleDefinition md, TypeReference tr)
        {
            var instance = new GenericInstanceMethod(self);
            instance.GenericArguments.Add(tr);
            var readFunc = md.ImportReference(instance);
            return readFunc;
        }

        public static MethodDefinition GetMethodInBaseType(this TypeDefinition self, string name)
        {
            var td = self;
            while (td != null)
            {
                foreach (var md in td.Methods.Where(method => method.Name == name))
                {
                    return md;
                }

                try
                {
                    var tr = td.BaseType;
                    td = tr?.Resolve();
                }
                catch (AssemblyResolutionException)
                {
                    break;
                }
            }

            return null;
        }

        public static MethodDefinition GetMethod(this TypeDefinition self, string methodName)
        {
            return self.Methods.FirstOrDefault(method => method.Name == methodName);
        }

        public static List<MethodDefinition> GetMethods(this TypeDefinition self, string methodName)
        {
            return self.Methods.Where(method => method.Name == methodName).ToList();
        }

        public static T GetFieldType<T>(this CustomAttribute self)
        {
            foreach (var custom in self.ConstructorArguments)
            {
                if (custom.Type.FullName == typeof(T).FullName)
                {
                    return (T)custom.Value;
                }
            }

            return default;
        }

        public static TypeReference ApplyGenericParameters(this TypeReference self, TypeReference child)
        {
            if (!self.IsGenericInstance) return self;
            var arguments = (GenericInstanceType)self;
            var generic = new GenericInstanceType(self.Resolve());
            foreach (var tr in arguments.GenericArguments)
            {
                generic.GenericArguments.Add(tr);
            }

            for (int i = 0; i < generic.GenericArguments.Count; i++)
            {
                if (!generic.GenericArguments[i].IsGenericParameter) continue;
                var tr = child.FindMatchingGenericArgument(generic.GenericArguments[i].Name);
                generic.GenericArguments[i] = self.Module.ImportReference(tr);
            }

            return generic;
        }

        private static TypeReference FindMatchingGenericArgument(this TypeReference self, string paramName)
        {
            var td = self.Resolve();
            if (!td.HasGenericParameters)
            {
                throw new InvalidOperationException("方法带有泛型参数，在子类中找不到它们。");
            }

            for (int i = 0; i < td.GenericParameters.Count; i++)
            {
                var param = td.GenericParameters[i];
                if (param.Name == paramName)
                {
                    GenericInstanceType generic = (GenericInstanceType)self;
                    return generic.GenericArguments[i];
                }
            }

            throw new InvalidOperationException("没有找到匹配的泛型");
        }


        public static FieldReference MakeHostInstanceGeneric(this FieldReference self)
        {
            var declaringType = new GenericInstanceType(self.DeclaringType);
            foreach (var parameter in self.DeclaringType.GenericParameters)
            {
                declaringType.GenericArguments.Add(parameter);
            }

            return new FieldReference(self.Name, self.FieldType, declaringType);
        }

        public static bool IsNetworkObjectField(this TypeReference tr)
        {
            return tr.Is<GameObject>() || tr.Is<NetworkObject>() || tr.IsDerivedFrom<NetworkBehaviour>() || tr.Is<NetworkBehaviour>();
        }

        public static CustomAttribute GetCustomAttribute<TAttribute>(this ICustomAttributeProvider self)
        {
            return self.CustomAttributes.FirstOrDefault(custom => custom.AttributeType.Is<TAttribute>());
        }
    }
}