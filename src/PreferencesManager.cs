/*
MIT License

Copyright (c) 2021 AluminiumTech

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

using Newtonsoft.Json;

namespace AluminiumTech.DevKit.SettingsKit
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    // ReSharper disable once UnusedType.Global
    public class PreferencesManager<TKey, TValue>
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once InconsistentNaming
        protected List<KeyValueDataStore<TKey, TValue>> _preferences;

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public string PathToJsonFile { get; set; }
        
        public PreferencesManager(string pathToJsonFile)
        {
            PathToJsonFile = pathToJsonFile;

            _preferences = new List<KeyValueDataStore<TKey, TValue>>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        /// <exception cref="Exception"></exception>
        public void LoadPreferences(string pathToJsonFile)
        {
            try
            {
                string json = File.ReadAllText(pathToJsonFile);
                
                var deserializedPreference = JsonConvert.DeserializeObject<KeyValueDataStore<TKey, TValue>[]>(json);

                if (deserializedPreference != null) ImportPreferences(deserializedPreference.ToList());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        protected void WriteJsonFile(string pathToJsonFile)
        {
            string contents = JsonConvert.SerializeObject(_preferences.ToArray());
            File.WriteAllText(pathToJsonFile, contents);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Throws an exception if it is unable to find a preference with the specified key.</exception>
        public KeyValueDataStore<TKey, TValue> GetPreference(TKey key)
        {
            foreach (var preference in _preferences)
            {
                if (key is string)
                {
                    if (preference.Key.ToString().ToLower().Equals(key.ToString().ToLower()))
                    {
                        return preference;
                    }
                }
                else
                {
                    if (preference.Key.Equals(key))
                    {
                        return preference;
                    }
                }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preferences"></param>
        /// <exception cref="Exception"></exception>
        public void ImportPreferences(List<KeyValueDataStore<TKey, TValue>> preferences)
        { 
            ImportPreferences(preferences.ToArray());
        }

        public void ImportPreferences(KeyValueDataStore<TKey, TValue>[] preferences)
        {
            try
            {
                foreach (var preference in preferences)
                {
                    AddPreference(preference.Key, preference.Value, preference.DefaultValue);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="default"></param>
        public void AddPreference(TKey key, TValue value, TValue @default)
        {
            KeyValueDataStore<TKey, TValue> preference = new KeyValueDataStore<TKey, TValue>();
            preference.Key = key;
            preference.Value = value;
            preference.DefaultValue = @default;
            
            _preferences.Add(preference);
            
            WriteJsonFile(PathToJsonFile);
        }

        /// <summary>
        /// Updates the preference whilst keeping the same old default value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdatePreference(TKey key, TValue value)
        {
            var oldDefault = GetPreference(key).DefaultValue;
            UpdatePreference(key, value, oldDefault);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="default"></param>
        /// <exception cref="Exception"></exception>
        public void UpdatePreference(TKey key, TValue value, TValue @default)
        {
            try
            {
                _preferences[GetPreferencePosition(key)].Value = value;
                _preferences[GetPreferencePosition(key)].DefaultValue = @default;
                _preferences[GetPreferencePosition(key)].LastEdited = DateTime.Now;

                WriteJsonFile(PathToJsonFile);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void RemovePreference(TKey key)
        {
            _preferences.Remove(_preferences[GetPreferencePosition(key)]);
            WriteJsonFile(PathToJsonFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public int GetPreferencePosition(TKey key)
        {
            for (int index = 0; index < _preferences.Count; index++)
            {
                if (_preferences[index].Key.Equals(key))
                {
                    return index;
                }
            }

            throw new KeyNotFoundException();
        }

        public TValue GetPreferenceValue(TKey key)
        {
            return _preferences[GetPreferencePosition(key)].Value;
        }

        /// <summary>
        /// Attempts to get a preference value or the default value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TValue GetPreferenceValueOrDefault(TKey key)
        {
            try
            {
                if (_preferences[GetPreferencePosition(key)].Value == null)
                {
                    return _preferences[GetPreferencePosition(key)].DefaultValue;
                }
                else
                {
                    return _preferences[GetPreferencePosition(key)].Value;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
    
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void RemoveAllPreferencesWithValue(TValue value)
        {
            for (int index = 0; index < _preferences.Count; index++)
            {
                if (_preferences[index].Value.Equals(value))
                {
                    _preferences.RemoveAt(index);
                }
            }
            
            WriteJsonFile(PathToJsonFile);
        }

        /// <summary>
        /// Enables easy access to the internal list in the class.
        /// </summary>
        /// <returns></returns>
        public List<KeyValueDataStore<TKey, TValue>> ToList()
        {
            return _preferences;
        }
    }
}