using JFramework.Interface;

namespace JFramework
{
    internal abstract class Field : IField
    {
        protected string name;
        protected string type;
        protected abstract string GetFieldLine();
        string IField.Name => name;
        string IField.GetFieldLine() => GetFieldLine();
    }
}