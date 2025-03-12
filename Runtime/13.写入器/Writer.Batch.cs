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
    internal class WriterBatch
    {
        private readonly int maxCount;
        private readonly Queue<MemoryWriter> writers = new Queue<MemoryWriter>();
        private MemoryWriter writer;

        public WriterBatch(int maxCount)
        {
            this.maxCount = maxCount;
        }

        public void AddMessage(ArraySegment<byte> segment, double remoteTime)
        {
            var header = Service.Bit.Length((ulong)segment.Count);
            if (writer != null && writer.position + header + segment.Count > maxCount)
            {
                writers.Enqueue(writer);
                writer = null;
            }

            if (writer == null)
            {
                writer = MemoryWriter.Pop();
                writer.Write(remoteTime);
            }

            Service.Bit.Encode(writer, (ulong)segment.Count);
            writer.WriteBytes(segment.Array, segment.Offset, segment.Count);
        }

        public bool GetBatch(MemoryWriter target)
        {
            if (writers.Count > 0)
            {
                var first = writers.Dequeue();
                if (target.position != 0)
                {
                    throw new ArgumentException("拷贝目标不是空的！");
                }

                ArraySegment<byte> segment = first;
                target.WriteBytes(segment.Array, segment.Offset, segment.Count);
                MemoryWriter.Push(first);
                return true;
            }

            if (writer != null)
            {
                if (target.position != 0)
                {
                    throw new ArgumentException("拷贝目标不是空的！");
                }

                ArraySegment<byte> segment = writer;
                target.WriteBytes(segment.Array, segment.Offset, segment.Count);
                MemoryWriter.Push(writer);
                writer = null;
                return true;
            }

            return false;
        }
    }
}