namespace JFramework.AStar
{
    /// <summary>
    /// A星节点类
    /// </summary>
    public class AStarNode
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        public readonly ANodeType type;
        
        /// <summary>
        /// x轴位置
        /// </summary>
        public readonly int x;
        
        /// <summary>
        /// y轴位置
        /// </summary>
        public readonly int y;
        
        /// <summary>
        /// 寻路消耗
        /// </summary>
        public float fCost;
        
        /// <summary>
        /// 离起点的距离
        /// </summary>
        public float gCost;
        
        /// <summary>
        /// 离终点的距离
        /// </summary>
        public float hCost;
        
        /// <summary>
        /// 父节点
        /// </summary>
        public AStarNode parent;

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="x">传入x轴位置</param>
        /// <param name="y">传入y轴位置</param>
        /// <param name="action">传入回调方法</param>
        public AStarNode(int x, int y, AStarDelegate action)
        {
            this.x = x;
            this.y = y;
            type = action?.Invoke() ?? ANodeType.Move;
        }

        /// <summary>
        /// 清除节点值
        /// </summary>
        public void Clear()
        {
            parent = null;
            fCost = 0;
            gCost = 0;
            hCost = 0;
        }
    }

    /// <summary>
    /// 节点类型枚举
    /// </summary>
    public enum ANodeType
    {
        Move,
        Stop,
    }
}