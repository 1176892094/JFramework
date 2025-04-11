// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:30
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace JFramework
{
    internal class SetterBatch
    {
        private readonly Queue<MemorySetter> setters = new Queue<MemorySetter>();
        private readonly int maxLength;
        private MemorySetter setter;

        public SetterBatch(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public void AddMessage(ArraySegment<byte> segment, double remoteTime)
        {
            var length = Service.Length.Invoke((ulong)segment.Count);
            if (setter != null && setter.position + length + segment.Count > maxLength)
            {
                setters.Enqueue(setter);
                setter = null;
            }

            if (setter == null)
            {
                setter = MemorySetter.Pop();
                setter.Set(remoteTime);
            }

            Service.Length.Encode(setter, (ulong)segment.Count);
            setter.SetBytes(segment.Array, segment.Offset, segment.Count);
        }

        public bool GetBatch(MemorySetter target)
        {
            if (setters.Count > 0)
            {
                var cached = setters.Dequeue();
                if (target.position != 0)
                {
                    throw new ArgumentException("拷贝目标不是空的！");
                }

                ArraySegment<byte> segment = cached;
                target.SetBytes(segment.Array, segment.Offset, segment.Count);
                MemorySetter.Push(cached);
                return true;
            }

            if (setter != null)
            {
                if (target.position != 0)
                {
                    throw new ArgumentException("拷贝目标不是空的！");
                }

                ArraySegment<byte> segment = setter;
                target.SetBytes(segment.Array, segment.Offset, segment.Count);
                MemorySetter.Push(setter);
                setter = null;
                return true;
            }

            return false;
        }
    }
}