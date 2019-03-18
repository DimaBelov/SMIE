using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SMIE.Core.CrossCutting.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToParamString(this Dictionary<string, string> parameters, bool encode)
        {
            string str = parameters.Aggregate(string.Empty,
                (current, kvp) => current + string.Format("{0}={1}&", kvp.Key, WebUtility.UrlEncode(kvp.Value)));
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }

        public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> first, Dictionary<TKey, TValue> second)
        {
            foreach (KeyValuePair<TKey, TValue> keyValuePair in second)
                first[keyValuePair.Key] = keyValuePair.Value;
        }
    }
}
