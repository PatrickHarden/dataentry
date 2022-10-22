using System;
using System.Collections.Generic;

namespace dataentry.Utility
{
    public class CaseInsensitiveDictionary<T> : Dictionary<string, T>
    {
        public CaseInsensitiveDictionary() : base(StringComparer.OrdinalIgnoreCase) { }
        public CaseInsensitiveDictionary(IDictionary<string, T> dictionary) : base(dictionary, StringComparer.OrdinalIgnoreCase) { }
        public CaseInsensitiveDictionary(IEnumerable<KeyValuePair<string, T>> collection) : base(collection, StringComparer.OrdinalIgnoreCase) { }
        public CaseInsensitiveDictionary(int capacity) : base(capacity, StringComparer.OrdinalIgnoreCase) { }
    }
}