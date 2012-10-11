using System;
using System.Collections.Generic;

namespace Melchior.Common
{
    public static class StringUtils
    {
        public static string Join(string[] values, string separator)
        {
            string expression = null;
            for (int i = 0; i < values.Length; i++) expression += values[i] + separator;
            return expression;
        }

        public static string Join(List<string> values, string separator)
        {
            string expression = null;
            for (int i = 0, length = values.Count; i < length; i++) expression += values[i] + separator;
            return expression;
        }
    }
}