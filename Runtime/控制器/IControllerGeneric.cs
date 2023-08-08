namespace JFramework.Interface
{
    public interface IController<in TCharacter> : IController where TCharacter : ICharacter
    {
        /// <summary>
        /// 控制器生成时调用
        /// </summary>
        /// <param name="owner">传入控制器的拥有者</param>
        /// <returns></returns>
        void Spawn(TCharacter owner);
    }
}