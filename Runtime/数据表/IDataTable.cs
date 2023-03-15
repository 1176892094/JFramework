namespace JFramework.Interface
{
    public interface IDataTable
    {
        int Count { get; }

        void AddData(object data);

        object GetData(int index);
    }
}