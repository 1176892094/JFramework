using System.Collections.Generic;
using JFramework.Core;
using UnityEngine;

// ReSharper disable All
namespace JFramework.AStar
{
    /// <summary>
    /// 基于AStar初始化地图的委托
    /// </summary>
    public delegate ANodeType AStarDelegate();

    /// <summary>
    /// AStar寻路类型
    /// </summary>
    public enum AStarType
    {
        /// <summary>
        /// 四方向
        /// </summary>
        FourDirection,

        /// <summary>
        /// 八方向
        /// </summary>
        EightDirection
    }

    /// <summary>
    /// AStar管理器
    /// </summary>
    public class AStarManager: Singleton<AStarManager>
    {
        /// <summary>
        /// 初始化地图的事件
        /// </summary>
        public event AStarDelegate OnInitMap;

        /// <summary>
        /// 开启列表
        /// </summary>
        private readonly List<AStarNode> openList = new List<AStarNode>();
        
        /// <summary>
        /// 关闭列表
        /// </summary>
        private readonly List<AStarNode> closeList = new List<AStarNode>();
        
        /// <summary>
        /// AStar节点
        /// </summary>
        private AStarNode[,] nodes;
        
        /// <summary>
        /// 寻路地图宽
        /// </summary>
        private int width;
        
        /// <summary>
        /// 寻路地图高
        /// </summary>
        private int height;

        /// <summary>
        /// 初始化地图
        /// </summary>
        /// <param name="width">传入地图宽度</param>
        /// <param name="height">传入地图高度</param>
        public void InitMap(int width, int height)
        {
            this.width = width;
            this.height = height;
            nodes = new AStarNode[width, height];
            for (var i = 0; i < width; ++i)
            {
                for (var j = 0; j < height; ++j)
                {
                    var node = new AStarNode(i, j, OnInitMap);
                    nodes[i, j] = node;
                }
            }
        }

        /// <summary>
        /// 开始进行寻路
        /// </summary>
        /// <param name="start">传入起始点</param>
        /// <param name="end">传入目标点</param>
        /// <returns>返回节点列表</returns>
        public List<AStarNode> FindPath(Vector2 start, Vector2 end)
        {
            if (!PointCheck(start) || !PointCheck(end))
            {
                Debug.LogWarning("开始或结束点在地图格子范围外!");
                return null;
            }

            AStarNode startNode = nodes[(int)start.x, (int)start.y];
            AStarNode endNode = nodes[(int)end.x, (int)end.y];
            if (startNode.type == ANodeType.Stop || endNode.type == ANodeType.Stop)
            {
                Debug.LogWarning("开始或结束点被阻挡！");
                return null;
            }

            closeList.Clear();
            openList.Clear();
            startNode.Clear();
            closeList.Add(startNode);

            while (true)
            {

                FindNode(startNode.x, startNode.y - 1, 1, startNode, endNode);
                FindNode(startNode.x - 1, startNode.y, 1, startNode, endNode);
                FindNode(startNode.x + 1, startNode.y, 1, startNode, endNode);
                FindNode(startNode.x, startNode.y + 1, 1, startNode, endNode);
                if (GlobalManager.Instance.pathfinding == AStarType.EightDirection)
                {
                    FindNode(startNode.x - 1, startNode.y - 1, 1.4f, startNode, endNode);
                    FindNode(startNode.x + 1, startNode.y - 1, 1.4f, startNode, endNode);
                    FindNode(startNode.x - 1, startNode.y + 1, 1.4f, startNode, endNode);
                    FindNode(startNode.x + 1, startNode.y + 1, 1.4f, startNode, endNode);
                }

                if (openList.Count == 0)
                {
                    Debug.LogWarning("死路");
                    return null;
                }

                openList.Sort((a, b) => a.fCost >= b.fCost ? 1 : -1);
                foreach (var node in openList)
                {
                    Debug.Log($"[{node.x} , {node.y}] G:{node.gCost} - H:{node.hCost} - F:{node.fCost}");
                }

                closeList.Add(openList[0]);
                startNode = openList[0];
                openList.RemoveAt(0);

                if (startNode == endNode)
                {
                    List<AStarNode> path = new List<AStarNode> { endNode };
                    while (endNode.parent != null)
                    {
                        path.Add(endNode.parent);
                        endNode = endNode.parent;
                    }

                    path.Reverse();
                    return path;
                }
            }
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="x">传入节点在AStar的x轴位置</param>
        /// <param name="y">传入节点在AStar的y轴位置</param>
        /// <returns></returns>
        public AStarNode GetNode(int x, int y) => nodes[x, y];

        /// <summary>
        /// 位置检测
        /// </summary>
        /// <param name="point">传入一个坐标</param>
        /// <returns>返回坐标是否越界</returns>
        private bool PointCheck(Vector2 point)
        {
            if (point.x < 0) return false; //x轴越界
            if (point.x >= width) return false; //x轴越界
            if (point.y < 0) return false; //y轴越界
            if (point.y >= height) return false; //y轴越界
            return true;
        }

        /// <summary>
        /// 查找节点
        /// </summary>
        /// <param name="x">传入节点x轴</param>
        /// <param name="y">传入节点y轴</param>
        /// <param name="fCost">传入寻路消耗</param>
        /// <param name="parent">传入上一个节点</param>
        /// <param name="end">传入最终坐标点</param>
        private void FindNode(int x, int y, float fCost, AStarNode parent, AStarNode end)
        {
            if (x < 0 || x >= width || y < 0 || y >= height) return;
            AStarNode start = nodes[x, y];
            if (start == null || start.type == ANodeType.Stop) return;
            if (closeList.Contains(start) || openList.Contains(start)) return;
            start.parent = parent;
            start.gCost = parent.gCost + fCost;
            start.hCost = Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
            start.fCost = start.gCost + start.hCost;
            openList.Add(start);
        }
    }
}