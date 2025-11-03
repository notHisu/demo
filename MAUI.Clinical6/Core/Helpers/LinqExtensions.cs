using System.Collections.Generic;

namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    /// <summary>
    /// Provides Linq-like functionality on top of what you get in the box
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// A convenience wrapper around the usual TryGetValue for dictionaries, allowing a soft get for missing keys
        /// </summary>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue = default(TValue))
        {
            if (source == null) return defaultValue;

            if (!source.TryGetValue(key, out TValue result)) return defaultValue;

            return result;
        }
    }
}