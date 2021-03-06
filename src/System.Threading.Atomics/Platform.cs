using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if NET40
using Interlocked = System.Threading.Thread;
#endif

namespace System.Threading.Atomics
{
    public static class Platform
    {
        public const int CacheLineSize = 64;

        /// <summary>
        /// Reads value from provided <paramref name="location"/> without any synchronization
        /// </summary>
        /// <typeparam name="T">The reference (<paramref name="location"/>) type</typeparam>
        /// <param name="location">The location to read</param>
        /// <returns>Value stored at provided <paramref name="location"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(ref T location)
        {
            return location;
        }

        /// <summary>
        /// Reads value from provided <paramref name="location"/> with acquire semantics
        /// </summary>
        /// <typeparam name="T">The reference (<paramref name="location"/>) type</typeparam>
        /// <param name="location">The location to read</param>
        /// <returns>Value stored at provided <paramref name="location"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadAcquire<T>(ref T location)
        {
            return ReadSeqCst(ref location);
        }

        /// <summary>
        /// Reads value from provided <paramref name="location"/> with sequential consistent semantics
        /// </summary>
        /// <typeparam name="T">The reference (<paramref name="location"/>) type</typeparam>
        /// <param name="location">The location to read</param>
        /// <returns>Value stored at provided <paramref name="location"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadSeqCst<T>(ref T location)
        {
#if ARM_CPU
            var tmp = location;
            MemoryBarrier();
            return tmp;
#else
            return location;
#endif
        }

        /// <summary>
        /// Writes <paramref name="value"/> to provided <paramref name="location"/>
        /// </summary>
        /// <typeparam name="T">The reference (<paramref name="location"/>) type</typeparam>
        /// <param name="location">The location to store the <paramref name="value"/></param>
        /// <param name="value">The value to be written to provided <paramref name="location"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(ref T location, ref T value)
        {
            location = value;
        }

        /// <summary>
        /// Writes <paramref name="value"/> to provided <paramref name="location"/> with release semantics
        /// </summary>
        /// <typeparam name="T">The reference (<paramref name="location"/>) type</typeparam>
        /// <param name="location">The location to store the <paramref name="value"/></param>
        /// <param name="value">The value to be written to provided <paramref name="location"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRelease<T>(ref T location, ref T value)
        {
#if ARM_CPU
            MemoryBarrier();
#endif
            location = value;
        }

        /// <summary>
        /// Writes <paramref name="value"/> to provided <paramref name="location"/> with sequential consistent semantics
        /// </summary>
        /// <typeparam name="T">The reference (<paramref name="location"/>) type</typeparam>
        /// <param name="location">The location to store the <paramref name="value"/></param>
        /// <param name="value">The value to be written to provided <paramref name="location"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSeqCst<T>(ref T location, ref T value)
        {
            MemoryBarrier();
            location = value;
#if ARM_CPU
            MemoryBarrier();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MemoryBarrier()
        {
#if NET40
            Thread.MemoryBarrier();
#else
            Interlocked.MemoryBarrier();
#endif
        }

        internal static unsafe TTo reinterpret_cast<TFrom, TTo>(ref TFrom source)
        {
            var sourcePtr = __makeref(source);

            TTo dest = default(TTo);
            var destPtr = __makeref(dest);
            
            *(void**)&destPtr = *(void**)&sourcePtr;

            return __refvalue(destPtr,TTo);
        }

        internal static TTo static_cast<TFrom, TTo>(ref TFrom source)
        {
            return __refvalue(__makeref(source),TTo);
        }

        [StructLayout(LayoutKind.Explicit, Size = sizeof(int))]
        internal struct Data32
        {
            [FieldOffset(0)]
            public int Int32Value;
            [FieldOffset(0)]
            public float SingleValue;
        }

        [StructLayout(LayoutKind.Explicit, Size = sizeof(long))]
        internal struct Data64
        {
            [FieldOffset(0)]
            public long Int64Value;
            [FieldOffset(0)]
            public double DoubleValue;
        }
    }
}