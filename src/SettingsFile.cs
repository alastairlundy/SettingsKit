/*
MIT License

Copyright (c) 2021-2024 Alastair Lundy

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using LocalizationKit.Interfaces;
using SettingsKit.enums;

namespace SettingsKit;

/// <summary>
/// A class to represent and help manage a logical representation of a Settings File.
/// </summary>
public class SettingsFile : ISettingsFile
{
    internal Dictionary<string, string> Settings { get; set; }
    
    public ILocalizationFileProvider SettingsProvider { get; }
    
    public string FilePath { get; set; }


    private System.Timers.Timer _timer;
    
    public SavePreference Preference { get;  }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">The file path to use for Saving the Settings File.</param>
    /// <param name="provider">The Settings Provider to use.</param>
    public SettingsFile(string filePath, ILocalizationFileProvider provider)
    {
        FilePath = filePath;
        SettingsProvider = provider;
        _timer = new Timer();
        Settings = new Dictionary<string, string>();
        Preference = new SavePreference();
        Preference.SavingMode = SettingsSavingMode.SaveManuallyOnly;

        _timer.Enabled = false;
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">The file path to use for Saving the Settings File.</param>
    /// <param name="provider">The Settings Provider to use.</param>
    /// <param name="autoSavePreference">The AutoSave preference to use.</param>
    public SettingsFile(string filePath, ILocalizationFileProvider provider, SavePreference autoSavePreference)
    {
        FilePath = filePath;
        SettingsProvider = provider;
        _timer = new Timer();
        Settings = new Dictionary<string, string>();
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
    public void Add(KeyValuePair<string, string> pair)
    {
        switch (Preference.SavingMode)
        {
            case SettingsSavingMode.SaveManuallyOnly or SettingsSavingMode.AutoSaveAfterTimeMinutes:
                Settings.Add(pair.Key, pair.Value);
                break;
            case SettingsSavingMode.SaveAfterEveryChange:
                Settings.Add(pair.Key, pair.Value);
                SaveFile();
                break;
        }
    }
    
    /// <summary>
    /// Remove a KeyValuePair from the list of KeyValuePairs.
    /// </summary>
    /// <param name="pair"></param>
    public void Remove(KeyValuePair<string, string> pair)
    {
        switch (Preference.SavingMode)
        {
            case SettingsSavingMode.SaveManuallyOnly or SettingsSavingMode.AutoSaveAfterTimeMinutes:
                Settings.Remove(pair.Key);
                break;
            case SettingsSavingMode.SaveAfterEveryChange:
                Settings.Remove(pair.Key);
                SaveFile();
                break;
        }
    }

    /// <summary>
    /// Return an Array of KeyValuePairs.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> Get()
    {
        return Settings;
    }

    /// <summary>
    /// Return the Value associated with the specified Key.
    /// </summary>
    /// <param name="key">The specified Key</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public string Get(string key)
    {
        foreach (var pair in Settings)
        {
            if (pair.Key == null || pair.Value == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                if (pair.Key.Equals(key))
                {
                    return pair.Value;
                }
            }
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Save the file.
    /// </summary>
    public void SaveFile()
    {
        SettingsProvider.WriteToFile(Get().ToArray(), FilePath);
    }

    /// <summary>
    /// Move the Settings File to a new location.
    /// </summary>
    /// <param name="newPath">The path where the File should be moved to.</param>
    public void MoveFile(string newPath)
    {
      File.Move(FilePath, newPath);
      FilePath = newPath;
    }

    /// <summary>
    /// Makes a copy of the Settings File to a specified location.
    /// </summary>
    /// <param name="pathToCopyTo">The path where the File should be copied to.</param>
    public void CopyFile(string pathToCopyTo)
    {
        File.Copy(FilePath, pathToCopyTo);
    }

    /// <summary>
    /// Deletes the file.
    /// </summary>
    public void DeleteFile()
    {
        File.Delete(FilePath);
    }
}