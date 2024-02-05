using System.Collections.Generic;

namespace SettingsKit;

public interface ISettingsFileProvider2<TValue>
{
    KeyValuePair<string, TValue>[] Get(string pathToFile);

    void WriteToFile(KeyValuePair<string, TValue>[] data, string pathToFile);
}