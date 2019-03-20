/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

namespace System
{
    /// <summary>
    /// IComparable extension methods.
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// Same as CompareTo but returns null instead of 0 if both items are equal.
        /// </summary>
        /// <typeparam name="T">IComparable type.</typeparam>
        /// <param name="this">This instance.</param>
        /// <param name="other">The other instance.</param>
        /// <returns>Lexical relation between this and the other instance or null if both are equal.</returns>
        public static int? NullableCompareTo<T>(this T @this, T other) where T : IComparable
        {
            var result = @this.CompareTo(other);
            return result != 0 ? result : (int?)null;
        }
    }
}
