namespace JFramework.Interface
{
    /// <summary>
    /// 默认层 (最底层)
    /// </summary>
    public interface UINormal
    {
    }

    /// <summary>
    /// 底层
    /// </summary>
    public interface UIBottom : UINormal
    {
    }

    /// <summary>
    /// 中层
    /// </summary>
    public interface UIMiddle : UINormal
    {
    }

    /// <summary>
    /// 高层
    /// </summary>
    public interface UIHeight : UINormal
    {
    }

    /// <summary>
    /// 忽视射线层 (最高层)
    /// </summary>
    public interface UIIgnore : UINormal
    {
    }
}