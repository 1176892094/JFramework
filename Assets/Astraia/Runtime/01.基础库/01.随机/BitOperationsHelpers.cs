using System;
using System.Runtime.CompilerServices;

namespace Astraia
{
    public static partial class Service
    {
        public static partial class Random
        {
            private static class BitOperationsHelpers
            {
                /// <summary>Rotates the specified value left by the specified number of bits.</summary>
                /// <param name="value">The value to rotate.</param>
                /// <param name="offset">
                ///     The number of bits to rotate by. Any value outside the range [0..31] is treated as congruent mod
                ///     32.
                /// </param>
                /// <returns>The rotated value.</returns>
                [CLSCompliant(false)]
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static uint RotateLeft(uint value, int offset)
                {
                    return value << offset | value >> 32 - offset;
                }

                /// <summary>Rotates the specified value left by the specified number of bits.</summary>
                /// <param name="value">The value to rotate.</param>
                /// <param name="offset">
                ///     The number of bits to rotate by. Any value outside the range [0..63] is treated as congruent mod
                ///     64.
                /// </param>
                /// <returns>The rotated value.</returns>
                [CLSCompliant(false)]
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public static ulong RotateLeft(ulong value, int offset)
                {
                    return value << offset | value >> 64 - offset;
                }
            }
        }
    }
}