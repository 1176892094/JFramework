// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:08
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using JFramework;
using JFramework.Core;
using JFramework.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JFramework
{
    [Serializable]
    public struct SecretInt
    {
        public int origin;
        public int buffer;
        public int offset;

        public int Value
        {
            get
            {
                if (offset == 0)
                {
                    this = new SecretInt(0);
                }

                var target = buffer - offset;
                if (!origin.Equals(target))
                {
                    GlobalManager.Cheat();
                }

                return target;
            }
            set
            {
                origin = value;
                unchecked
                {
                    offset = Random.Range(1, int.MaxValue - value);
                    buffer = value + offset;
                }
            }
        }

        public SecretInt(int value)
        {
            origin = 0;
            buffer = 0;
            offset = 0;
            Value = value;
        }

        public static implicit operator int(SecretInt secret)
        {
            return secret.Value;
        }

        public static implicit operator SecretInt(int value)
        {
            return new SecretInt(value);
        }

        public static implicit operator bool(SecretInt secret)
        {
            return secret.Value != 0;
        }

        public static implicit operator SecretInt(bool secret)
        {
            return new SecretInt(secret ? 1 : 0);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [Serializable]
    public struct SecretFloat
    {
        public float origin;
        public float buffer;
        public int offset;

        public float Value
        {
            get
            {
                if (offset == 0)
                {
                    this = new SecretFloat(0);
                }

                var target = buffer - offset;
                if (Math.Abs(origin - target) > 0.1f)
                {
                    GlobalManager.Cheat();
                }

                return target;
            }
            set
            {
                origin = value;
                unchecked
                {
                    offset = Random.Range(1, short.MaxValue - (int)value);
                    buffer = value + offset;
                }
            }
        }

        public SecretFloat(float value)
        {
            origin = 0;
            buffer = 0;
            offset = 0;
            Value = value;
        }

        public static implicit operator float(SecretFloat secret)
        {
            return secret.Value;
        }

        public static implicit operator SecretFloat(float value)
        {
            return new SecretFloat(value);
        }

        public override string ToString()
        {
            return Value.ToString("F");
        }
    }

    [Serializable]
    public struct SecretString
    {
        public string origin;
        public byte[] buffer;
        public int offset;

        public string Value
        {
            get
            {
                if (origin.IsEmpty())
                {
                    this = new SecretString("");
                }

                var target = new byte[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    target[i] = (byte)(buffer[i] - offset);
                }

                if (!origin.Equals(Encoding.UTF8.GetString(target)))
                {
                    GlobalManager.Cheat();
                }

                return origin;
            }
            set
            {
                origin = value;
                offset = Random.Range(0, byte.MaxValue);
                buffer = Encoding.UTF8.GetBytes(origin);
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = (byte)(buffer[i] + offset);
                }
            }
        }

        public SecretString(string value)
        {
            origin = "";
            buffer = null;
            offset = 0;
            Value = value;
        }

        public static implicit operator string(SecretString secret)
        {
            return secret.Value;
        }

        public static implicit operator SecretString(string value)
        {
            return new SecretString(value);
        }

        public override string ToString()
        {
            return Value;
        }
    }

    [Serializable]
    public class UIScroll<TItem, TGrid> where TGrid : IGrid<TItem> where TItem : new()
    {
        private readonly Dictionary<int, TGrid> grids = new Dictionary<int, TGrid>();
        private RectTransform content;
        private List<TItem> items;
        private int oldMinIndex = -1;
        private int oldMaxIndex = -1;
        private int row;
        private int column;
        private float width;
        private float height;
        private string path;

        public void SetContent(RectTransform content, string path)
        {
            this.path = path;
            this.content = content;
        }

        public void InitGrids(int width, int height, int row, int column)
        {
            this.width = width;
            this.height = height;
            this.column = column;
            this.row = row * height;
        }

        public void Refresh(List<TItem> items)
        {
            foreach (var i in grids.Keys)
            {
                if (grids.TryGetValue(i, out var grid))
                {
                    if (grid != null)
                    {
                        PoolManager.Push(grid.gameObject);
                    }
                }
            }

            grids.Clear();
            this.items = items;
            content.anchoredPosition = Vector2.zero;
            content.sizeDelta = new Vector2(0, Mathf.CeilToInt((float)items.Count / column) * height + 1);
        }

        public async void OnUpdate()
        {
            if (items == null) return;
            var fixHeight = height;
            var position = content.anchoredPosition;
            var minIndex = Math.Max(0, (int)(position.y / fixHeight) * column);
            var maxIndex = Math.Min((int)((position.y + row) / fixHeight) * column + column - 1, items.Count - 1);

            if (minIndex != oldMinIndex || maxIndex != oldMaxIndex)
            {
                for (int i = oldMinIndex; i < minIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            PoolManager.Push(grid.gameObject);
                        }

                        grids.Remove(i);
                    }
                }

                for (int i = maxIndex + 1; i <= oldMaxIndex; ++i)
                {
                    if (grids.TryGetValue(i, out var grid))
                    {
                        if (grid != null)
                        {
                            PoolManager.Push(grid.gameObject);
                        }

                        grids.Remove(i);
                    }
                }
            }

            oldMinIndex = minIndex;
            oldMaxIndex = maxIndex;

            for (int index = minIndex; index <= maxIndex; ++index)
            {
                if (!grids.TryAdd(index, default)) continue;
                var obj = await PoolManager.Pop(path);
                obj.transform.SetParent(content);
                obj.transform.localScale = Vector3.one;
                var posX = index % column * width + width / 2;
                var posY = -(index / column) * fixHeight - height / 2;
                obj.transform.localPosition = new Vector3(posX, posY, 0);
                if (obj.TryGetComponent(out TGrid grid))
                {
                    grid.SetItem(items[index]);
                }

                if (grids.ContainsKey(index))
                {
                    grids[index] = grid;
                }
                else
                {
                    PoolManager.Push(obj);
                }
            }
        }
    }
}