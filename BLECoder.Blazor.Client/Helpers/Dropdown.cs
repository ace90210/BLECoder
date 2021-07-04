using System;
using System.Collections.Generic;
using System.Linq;

namespace BLECoder.Blazor.Client.Helpers
{
    public static class DropdownHelpers
    {
        public static IEnumerable<object> CreateEnumList<T>(string nullOption = null) where T : struct, Enum
        {
            var list = new List<object>();

            if (!string.IsNullOrWhiteSpace(nullOption))
            {
                list.Add(new { Text = nullOption, Value = (T?)null });
            }

            list.AddRange(Enum.GetValues(typeof(T)).Cast<T>().Select(t => new { Text = $"{t}", Value = t }));
            return list;
        }
    }
}
