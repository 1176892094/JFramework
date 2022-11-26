using System;

namespace JFramework.Logger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class DebugAttribute : Attribute
    {
        public readonly Type InspectedType;
        public DebugAttribute(Type type) => InspectedType = type;
    }
}
