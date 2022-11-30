using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JFramework.Excel
{
    internal static class ExcelUtility
    {
        public static FieldInfo GetRowDataKeyField(Type rowDataType)
        {
            var fields = rowDataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var keyField = (from fieldInfo in fields let attrs = fieldInfo.GetCustomAttributes(typeof(ExcelKeyAttribute), false)
                where attrs.Length > 0 select fieldInfo).FirstOrDefault();
            return keyField;
        }

        public static Assembly GetSourceAssembly()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ExcelContainer)));
                var dataCollectionTypes = types as Type[] ?? types.ToArray();
                if (dataCollectionTypes.Any()) return assembly;
            }

            return typeof(ExcelContainer).Assembly;
        }
    }
    
    public class ExcelKeyAttribute : Attribute
    {
		
    }
    
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class LabelAttribute : Attribute
    {
        public readonly string label;
        public LabelAttribute(string label) => this.label = label;
    }
}