using System.Collections.Generic;

namespace AlastairLundy.SettingsKit;

public interface ISettingsFileProvider2<TValue>
{
    KeyValuePair<string, TValue>[] Get(string pathToFile);

    void WriteToFile(KeyValuePair<string, TValue>[] data, string pathToFile);
}