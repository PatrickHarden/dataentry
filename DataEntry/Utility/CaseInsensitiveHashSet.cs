using System;
using System.Collections.Generic;

namespace dataentry.Utility
{
    public class CaseInsensitiveHashSet : HashSet<string>
    {
        public CaseInsensitiveHashSet() : base(StringComparer.OrdinalIgnoreCase) { }
        public CaseInsensitiveHashSet(IEnumerable<string> collection) : base(collection, StringComparer.OrdinalIgnoreCase) { }
        public CaseInsensitiveHashSet(int capacity) : base(capacity, StringComparer.OrdinalIgnoreCase) { }
    }
}