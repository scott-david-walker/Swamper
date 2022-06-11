namespace Swamper;

public static class DictionaryExtensions<T>
{
    public static void AddOrUpdateCount(Dictionary<T, int> dict, T val)
    {
        if (dict.ContainsKey(val))
        {
            dict[val]++;
            return;
        }

        dict.Add(val, 1);
    }
    
    public static void MergeDictionaries(IDictionary<T, int> dict1, IDictionary<T, int> dict2)
    {
        foreach (var kvp2 in dict2)
        {
            if (dict1.ContainsKey(kvp2.Key))
            {
                dict1[kvp2.Key] += kvp2.Value;
                continue;
            }
            
            dict1.Add(kvp2);
        }
    }
}