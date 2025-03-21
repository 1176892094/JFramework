// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-11 03:01:58
// # Recently: 2025-01-11 18:01:34
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Runtime.CompilerServices;

namespace JFramework
{
    public static partial class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBit(this Variable<byte> variable, int shift, int mask)
        {
            return (variable >> shift) & (1 << mask) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable<int> SetBit(this Variable<byte> variable, int shift, int mask, int value)
        {
            return (variable & ~((1 << mask) - 1 << shift)) | ((value & (1 << mask) - 1) << shift);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBit(this Variable<short> variable, int shift, int mask)
        {
            return (variable >> shift) & (1 << mask) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable<int> SetBit(this Variable<short> variable, int shift, int mask, int value)
        {
            return (variable & ~((1 << mask) - 1 << shift)) | ((value & (1 << mask) - 1) << shift);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBit(this Variable<int> variable, int shift, int mask)
        {
            return (variable >> shift) & (1 << mask) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable<int> SetBit(this Variable<int> variable, int shift, int mask, int value)
        {
            return (variable & ~((1 << mask) - 1 << shift)) | ((value & (1 << mask) - 1) << shift);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetBit(this Variable<long> variable, int shift, int mask)
        {
            return (int)((variable >> shift) & (1L << mask) - 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variable<long> SetBit(this Variable<long> variable, int shift, int mask, int value)
        {
            return (variable & ~((1L << mask) - 1 << shift)) | ((value & (1L << mask) - 1) << shift);
        }
    }
}