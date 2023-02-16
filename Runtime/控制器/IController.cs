namespace JFramework.Interface
{
    /// <summary>
    /// 控制器接口
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 控制器醒来
        /// </summary>
        /// <param name="owner">控制器的所有者</param>
        void OnInit(object owner);
    }
}