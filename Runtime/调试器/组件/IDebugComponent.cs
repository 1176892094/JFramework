namespace JFramework.Interface
{
    /// <summary>
    /// 自定义组件接口
    /// </summary>
    internal interface IDebugComponent
    {
        /// <summary>
        /// 组件类型
        /// </summary>
        object Target { set; }

        /// <summary>
        /// 在DebugScene中显示的界面
        /// </summary>
        void OnDebugScene();
    }
}