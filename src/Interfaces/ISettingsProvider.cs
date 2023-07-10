using System.Collections.Generic;

namespace AlastairLundy.SettingsKit.Interfaces;

public interface ISettingsProvider<TKey, TValue>
{
    KeyValuePair<TKey, TValue>[] Get(string pathToFile);

    void WriteToFile(KeyValuePair<TKey, TValue>[] data, string pathToFile);
}