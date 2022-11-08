using UnityEngine;

namespace JFramework.Pathfinding
{
    public class AStarNode
    {
        public int x;
        public int y;
        public float totalDis; //寻路消耗
        public float originDis; //离起点的距离
        public float targetDis; //离终点的距离
        public AStarNode father;
        public ANodeType type;

        public AStarNode(int x,int y, ANodeType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public void Clear()
        {
            father = null;
            totalDis = 0;
            originDis = 0;
            targetDis = 0;
        }
    }

    public enum ANodeType
    {
        Move,
        Stop,
    }
}