using System.Collections.Generic;

namespace AlastairLundy.SettingsKit;

public interface ISettingsFile2<TValue>
{
    public ISettingsFileProvider2<TValue> SettingsProvider { get; }

    public void Add(KeyValuePair<string, TValue> pair);

    public void Remove(KeyValuePair<string, TValue> pair);

    public KeyValuePair<string, TValue>[] Get();
    public TValue Get(string key);
}