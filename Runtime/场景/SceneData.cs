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

        public SceneData(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
}