namespace JFramework.Interface
{
    /// <summary>
    /// UI 层级接口
    /// </summary>
    public interface UILayer
    {
    }

    /// <summary>
    /// 默认层 (最底层)
    /// </summary>
    public interface UINormal : UILayer
    {
    }

    /// <summary>
    /// 底层
    /// </summary>
    public interface UIBottom : UILayer
    {
    }

    /// <summary>
    /// 中层
    /// </summary>
    public interface UIMiddle : UILayer
    {
    }

    /// <summary>
    /// 高层
    /// </summary>
    public interface UIHeight : UILayer
    {
    }

    /// <summary>
    /// 忽视射线层 (最高层)
    /// </summary>
    public interface UIIgnore : UILayer
    {
    }
}