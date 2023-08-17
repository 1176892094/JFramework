namespace JFramework.Interface
{
    /// <summary>
    /// 控制器接口
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// 控制器生成
        /// </summary>
        /// <param name="character"></param>
        void Spawn(ICharacter character);
    }
}