using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocoptNet;

namespace Gemini.Commander.Core.Extensions
{
    public static class Ext
    {
        public static T Extract<T>(this IDictionary<string, ValueObject> args, string key, T defaultValue) => args.ContainsKey(key) && args[key]?.Value != null ? (T)Convert.ChangeType(args[key]?.Value?.ToString(), typeof(T)) : defaultValue;
        public static int ConvertTo(this string hours, string tag)
        {
            var pattern = $@"(?<c>\d+){tag}";
            if (!Regex.IsMatch(hours, pattern, RegexOptions.IgnoreCase)) return 0;
            var value = Regex.Match(hours, pattern, RegexOptions.IgnoreCase).Groups["c"]?.Value;
            return Convert.ToInt32(value ?? "0");
        }
    }
}