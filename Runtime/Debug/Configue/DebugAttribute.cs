using System;

namespace JYJFramework
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class DebugAttribute : Attribute
    {
        public readonly Type InspectedType;
        public DebugAttribute(Type type) => InspectedType = type;
    }
}
