using System.Collections.Generic;

namespace AlastairLundy.SettingsKit;

public interface ISettingsFile<TKey, TValue>
{
    internal List<KeyValuePair<TKey, TValue>> KeyValuePairs { get; set; }
    
    public ISettingsProvider<TKey, TValue> SettingsProvider { get; }

    public void Add(KeyValuePair<TKey, TValue> pair);

    public void Remove(KeyValuePair<TKey, TValue> pair);

    public KeyValuePair<TKey, TValue>[] Get();
    public TValue Get(TKey key);

    internal void SaveFile();
}