namespace JYJFramework.Pathfinding
{
    public class AStarNode
    {
        public int x;
        public int y;
        public float f; //寻路消耗
        public float g; //离起点的距离
        public float h; //离终点的距离
        public AStarNode father;
        public ANodeType type;

        public AStarNode(int x, int y, ANodeType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public void Clear()
        {
            father = null;
            f = 0;
            g = 0;
            h = 0;
        }
    }

    public enum ANodeType
    {
        Move,
        Stop,
    }
}