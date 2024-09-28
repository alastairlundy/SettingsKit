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

using LocalizationKit.Interfaces;

namespace SettingsKit;

/// <summary>
/// A class to represent and help manage a logical representation of a Settings File.
/// </summary>
public class SettingsFile : ISettingsFile
{
    protected Dictionary<string, string> Settings { get; set; }
    
    public ILocalizationFileProvider SettingsProvider { get; protected set; }
    
    public string FilePath { get; protected set; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">The file path to use for Saving the Settings File.</param>
    /// <param name="provider">The Settings Provider to use.</param>
    public SettingsFile(string filePath, ILocalizationFileProvider provider)
    {
        FilePath = filePath;
        SettingsProvider = provider;
    }

    /// <summary>
    /// Add a KeyValuePair to the list of KeyValuePairs.
    /// </summary>
    /// <param name="pair"></param>
    public void AddValue(KeyValuePair<string, string> pair)
    {
        Settings.Add(pair.Key, pair.Value);
    }
    
    /// <summary>
    /// Remove a KeyValuePair from the list of KeyValuePairs.
    /// </summary>
    /// <param name="pair"></param>
    public void RemoveValue(KeyValuePair<string, string> pair)
    {
        Settings.Remove(pair.Key);
    }

    /// <summary>
    /// Return an Array of KeyValuePairs.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, string> GetValues()
    {
        return Settings;
    }

    /// <summary>
    /// Return the Value associated with the specified Key.
    /// </summary>
    /// <param name="key">The specified Key</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public string GetValue(string key)
    {
        foreach (var pair in Settings)
        {
            if (pair.Key == null || pair.Value == null)
            {
                throw new NullReferenceException();
            }

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
        SettingsProvider.WriteToFile(GetValues().ToArray(), FilePath);
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