using System;
using System.Linq;
using System.Reflection;

namespace JFramework.Excel
{
    internal static class ExcelExtend
    {
        public static FieldInfo GetDataKeyField(Type dataType)
        {
            var fields = dataType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var keyField = (from fieldInfo in fields let attrs = fieldInfo.GetCustomAttributes(typeof(KeyValueAttribute), false)
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
    
    public class KeyValueAttribute : Attribute
    {
		
    }
}