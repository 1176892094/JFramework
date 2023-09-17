namespace JFramework
{
    /// <summary>
    /// 资源下载进度条
    /// </summary>
    public struct AssetProgress
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public readonly string name;

        /// <summary>
        /// 当前进度
        /// </summary>
        public readonly float current;

        /// <summary>
        /// 最大进度
        /// </summary>
        public readonly float progress;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="current"></param>
        /// <param name="progress"></param>
        public AssetProgress(string name, float current, float progress)
        {
            this.name = name;
            this.current = current;
            this.progress = progress;
        }
    }
}