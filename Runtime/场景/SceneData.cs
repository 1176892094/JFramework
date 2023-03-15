namespace JFramework
{
    /// <summary>
    /// 场景数据
    /// </summary>
    internal struct SceneData
    {
        /// <summary>
        /// 场景Id
        /// </summary>
        public int Id;

        /// <summary>
        /// 场景名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="Id">场景Id</param>
        /// <param name="Name">场景名称</param>
        public SceneData(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
}