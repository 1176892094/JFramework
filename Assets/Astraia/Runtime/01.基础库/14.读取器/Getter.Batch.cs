// *********************************************************************************
// # Project: Astraia
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:40
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;

namespace Astraia
{
    internal class GetterBatch
    {
        private readonly Queue<MemorySetter> setters = new Queue<MemorySetter>();
        private readonly MemoryGetter getter = new MemoryGetter();
        private double remoteTime;
        public int Count => setters.Count;


        public bool AddBatch(ArraySegment<byte> segment)
        {
            if (segment.Count < sizeof(double))
            {
                return false;
            }

            var setter = MemorySetter.Pop();
            setter.SetBytes(segment.Array, segment.Offset, segment.Count);
            if (setters.Count == 0)
            {
                getter.Reset(setter);
                remoteTime = getter.Get<double>();
            }

            setters.Enqueue(setter);
            return true;
        }

        public bool GetMessage(out ArraySegment<byte> segment, out double newTime)
        {
            newTime = 0;
            segment = default;
            if (setters.Count == 0)
            {
                return false;
            }

            if (getter.buffer.Count == 0)
            {
                return false;
            }

            if (getter.buffer.Count - getter.position == 0)
            {
                var setter = setters.Dequeue();
                MemorySetter.Push(setter);
                if (setters.Count > 0)
                {
                    setter = setters.Peek();
                    getter.Reset(setter);
                    remoteTime = getter.Get<double>();
                }
                else
                {
                    return false;
                }
            }

            newTime = remoteTime;
            if (getter.buffer.Count - getter.position == 0)
            {
                return false;
            }

            var length = (int)Service.Length.Decode(getter);

            if (getter.buffer.Count - getter.position < length)
            {
                return false;
            }

            segment = getter.GetArraySegment(length);
            return true;
        }
    }
}