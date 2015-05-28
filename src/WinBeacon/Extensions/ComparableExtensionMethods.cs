/*
 * Copyright 2015 Huysentruit Wouter
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace System
{
    /// <summary>
    /// IComparable extension methods.
    /// </summary>
    public static class ComparableExtensionMethods
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
