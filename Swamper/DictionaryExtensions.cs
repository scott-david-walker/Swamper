using System.Collections.Concurrent;

namespace Swamper;

internal static class DictionaryExtensions<T> where T : notnull
{
    internal static void AddOrUpdateCount(ConcurrentDictionary<T, int> dict, T val)
    {
        if (dict.ContainsKey(val))
        {
            dict[val]++;
            return;
        }

        dict.TryAdd(val, 1);
    }
    
    internal static void MergeDictionaries(ConcurrentDictionary<T, int> dict1, ConcurrentDictionary<T, int> dict2)
    {
        foreach (var kvp2 in dict2)
        {
            if (dict1.ContainsKey(kvp2.Key))
            {
                dict1[kvp2.Key] += kvp2.Value;
                continue;
            }

            dict1.TryAdd(kvp2.Key, kvp2.Value);
        }
    }
}