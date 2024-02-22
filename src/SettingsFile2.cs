using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

using AlastairLundy.SettingsKit;
using AlastairLundy.SettingsKit.enums;

namespace AlastairLundy.SettingsKit;

public class SettingsFile2<TValue> : ISettingsFile2<TValue>
{
        internal List<KeyValuePair<string, TValue>> KeyValuePairs { get; set; }
    
    public ISettingsFileProvider2<TValue> SettingsProvider { get; }
    
    public string FilePath { get; set; }


    private System.Timers.Timer _timer;
    
    public SavePreference Preference { get;  }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">The file path to use for Saving the Settings File.</param>
    /// <param name="provider">The Settings Provider to use.</param>
    public SettingsFile2(string filePath, ISettingsFileProvider2<TValue> provider)
    {
        FilePath = filePath;
        SettingsProvider = provider;
        _timer = new Timer();
        KeyValuePairs = new List<KeyValuePair<string, TValue>>();
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
    public SettingsFile2(string filePath, ISettingsFileProvider2<TValue> provider, SavePreference autoSavePreference)
    {
        FilePath = filePath;
        SettingsProvider = provider;
        _timer = new Timer();
        KeyValuePairs = new List<KeyValuePair<string, TValue>>();
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
    public void Add(KeyValuePair<string, TValue> pair)
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
    public void Remove(KeyValuePair<string, TValue> pair)
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
    public KeyValuePair<string, TValue>[] Get()
    {
        return KeyValuePairs.ToArray();
    }

    /// <summary>
    /// Return the Value associated with the specified Key.
    /// </summary>
    /// <param name="key">The specified Key</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public TValue Get(string key)
    {
        foreach (var pair in KeyValuePairs)
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
        SettingsProvider.WriteToFile(Get(), FilePath);
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