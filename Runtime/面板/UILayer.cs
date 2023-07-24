namespace JFramework.Interface
{
    /// <summary>
    /// 用户面板层级接口
    /// </summary>
    public interface UILayer
    {
    }

    /// <summary>
    /// 最底层
    /// </summary>
    public interface UILayer1 : UILayer
    {
    }

    /// <summary>
    /// 较底层
    /// </summary>
    public interface UILayer2 : UILayer
    {
    }

    /// <summary>
    /// 中间层
    /// </summary>
    public interface UILayer3 : UILayer
    {
    }

    /// <summary>
    /// 较高层
    /// </summary>
    public interface UILayer4 : UILayer
    {
    }

    /// <summary>
    /// 最高层 (忽视射线)
    /// </summary>
    public interface UILayer5 : UILayer
    {
    }
}