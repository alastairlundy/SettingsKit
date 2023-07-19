/*
MIT License

Copyright (c) 2021-2023 Alastair Lundy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections.Generic;
using System.Timers;

using AlastairLundy.SettingsKit.AutoSaving;
using AlastairLundy.SettingsKit.enums;
// ReSharper disable InconsistentNaming

namespace AlastairLundy.SettingsKit;

public class SettingsFile<TKey, TValue> : ISettingsFile<TKey, TValue>
{
    internal List<KeyValuePair<TKey, TValue>> KeyValuePairs { get; set; }
    
    public ISettingsProvider<TKey, TValue> SettingsProvider { get; }
    
    public string FilePath { get; }


    private System.Timers.Timer _timer;
    
    public AutoSavePreference Preference { get;  }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">The file path to use for Saving the Settings File.</param>
    /// <param name="provider">The Settings Provider to use.</param>
    public SettingsFile(string filePath, ISettingsProvider<TKey, TValue> provider)
    {
        FilePath = filePath;
        SettingsProvider = provider;
        _timer = new Timer();
        KeyValuePairs = new List<KeyValuePair<TKey, TValue>>();
        Preference = new AutoSavePreference();
        Preference.SavingMode = SettingsSavingMode.SaveManuallyOnly;

        _timer.Enabled = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">The file path to use for Saving the Settings File.</param>
    /// <param name="provider">The Settings Provider to use.</param>
    /// <param name="autoSavePreference">The AutoSave preference to use.</param>
    public SettingsFile(string filePath, ISettingsProvider<TKey, TValue> provider, AutoSavePreference autoSavePreference)
    {
        FilePath = filePath;
        SettingsProvider = provider;
        _timer = new Timer();
        KeyValuePairs = new List<KeyValuePair<TKey, TValue>>();
        Preference = autoSavePreference;

        if (Preference.SavingMode == SettingsSavingMode.AutoSaveAfterTimeMinutes)
        {
            _timer.Enabled = true;
            _timer.Interval = (Preference.AutoSaveFrequencyMinutes * 60) * 1000;
            _timer.Start();
            
            _timer.Elapsed += TimerOnElapsed;
        }
    }

    private void TimerOnElapsed(object sender, ElapsedEventArgs e)
    {
        SaveFile();
    }

    /// <summary>
    /// Add a KeyValuePair to the list of KeyValuePairs.
    /// </summary>
    /// <param name="pair"></param>
    public void Add(KeyValuePair<TKey, TValue> pair)
    {
        switch (Preference.SavingMode)
        {
            case SettingsSavingMode.SaveManuallyOnly or SettingsSavingMode.AutoSaveAfterTimeMinutes:
                KeyValuePairs.Add(pair);
                break;
            case SettingsSavingMode.SaveAfterEveryChange:
                KeyValuePairs.Add(pair);
                SaveFile();
                break;
        }
    }
    
    /// <summary>
    /// Remove a KeyValuePair from the list of KeyValuePairs.
    /// </summary>
    /// <param name="pair"></param>
    public void Remove(KeyValuePair<TKey, TValue> pair)
    {
        switch (Preference.SavingMode)
        {
            case SettingsSavingMode.SaveManuallyOnly or SettingsSavingMode.AutoSaveAfterTimeMinutes:
                KeyValuePairs.Remove(pair);
                break;
            case SettingsSavingMode.SaveAfterEveryChange:
                KeyValuePairs.Remove(pair);
                SaveFile();
                break;
        }
    }

    /// <summary>
    /// Return an Array of KeyValuePairs.
    /// </summary>
    /// <returns></returns>
    public KeyValuePair<TKey, TValue>[] Get()
    {
        return KeyValuePairs.ToArray();
    }

    /// <summary>
    /// Return the Value associated with the specified Key.
    /// </summary>
    /// <param name="key">The specified Key</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public TValue Get(TKey key)
    {
        foreach (var pair in KeyValuePairs)
        {
            if (pair.Key.Equals(key))
            {
                return pair.Value;
            }
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Save the file.
    /// </summary>
    public void SaveFile()
    {
        SettingsProvider.WriteToFile(Get(), FilePath);
    }
}