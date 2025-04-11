// *********************************************************************************
// # Project: Astraia
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-11-29 13:11:20
// # Recently: 2024-12-22 20:12:06
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using Astraia.Common;

namespace Astraia
{
    internal class Segment
    {
        public readonly MemoryStream data = new MemoryStream(Kcp.MTU_DEF);
        public uint cmd;
        public uint conv;
        public uint fast_ack;
        public uint frg;
        public uint rsd_c;
        public uint rsd_ts;
        public int rto;
        public uint sn;
        public uint ts;
        public uint una;
        public uint wnd;

        public int Encode(byte[] ptr, int offset)
        {
            var position = offset;
            offset += Utils.Encode32U(ptr, offset, conv);
            offset += Utils.Encode8U(ptr, offset, (byte)cmd);
            offset += Utils.Encode8U(ptr, offset, (byte)frg);
            offset += Utils.Encode16U(ptr, offset, (ushort)wnd);
            offset += Utils.Encode32U(ptr, offset, ts);
            offset += Utils.Encode32U(ptr, offset, sn);
            offset += Utils.Encode32U(ptr, offset, una);
            offset += Utils.Encode32U(ptr, offset, (uint)data.Position);
            return offset - position;
        }

        public void Reset()
        {
            conv = 0;
            cmd = 0;
            frg = 0;
            wnd = 0;
            una = 0;
            ts = 0;
            sn = 0;
            rsd_c = 0;
            rsd_ts = 0;
            fast_ack = 0;
            rto = 0;
            data.SetLength(0);
        }

        public static Segment Dequeue()
        {
            return HeapManager.Dequeue<Segment>();
        }

        public static void Enqueue(Segment segment)
        {
            HeapManager.Enqueue(segment);
        }
    }
}