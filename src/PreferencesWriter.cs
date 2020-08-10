/*
MIT License

Copyright (c) 2020 AluminiumTech

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
using System.IO;

using Newtonsoft.Json;

using AluminiumTech.DevKit.SettingsKit.Base;


namespace AluminiumTech.DevKit.SettingsKit
{    
    /// <summary>
    /// A class to write or create a .JSON preferences file.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class PreferencesWriter<TKey, TValue>
    {
        protected string _pathToJsonFile;
        
        /// <summary>
        /// Return the path to the json file as a string.
        /// </summary>
        /// <returns></returns>
        public string GetPathToJsonFile()
        {
            return _pathToJsonFile;
        }
        
        /// <summary>
        /// Sets the path to the json file as a string.
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        public void SetPathToJsonFile(string pathToJsonFile)
        {
            this._pathToJsonFile = pathToJsonFile;
        }
        
        /// <summary>
        /// Add a preference to an existing settings file or creates a new one.
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        /// <param name="preference"></param>
        public void AddPreference(KeyValuePair<TKey, TValue> preference)
        {
            JsonSerializer serializer = new JsonSerializer();
            
            using (StreamWriter sw = new StreamWriter(_pathToJsonFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, preference);
            }
        }
        
        /// <summary>
        /// Adds preferences to an existing settings file or creates a new one.
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        /// <param name="preferences"></param>
        public void AddPreferences(Preferences<TKey, TValue> preferences)
        {
            for (int i = 0; i < preferences.Count; i++)
            {
                KeyValuePair<TKey, TValue> preference = preferences[i];
                AddPreference(preference);
            }
        }
        
        /// <summary>
        /// Removes an old preference value and replaces it with a new preference value.
        /// </summary>
        /// <param name="oldPreference"></param>
        /// <param name="newPreference"></param>
        public void UpdatePreference(KeyValuePair<TKey, TValue> oldPreference, KeyValuePair<TKey, TValue> newPreference)
        {
           PreferencesReader<TKey, TValue> reader = new PreferencesReader<TKey, TValue>(_pathToJsonFile);
           var preferences = reader.GetPreferences(); 
           RemovePreference(oldPreference);
           preferences.Add(newPreference);
           AddPreferences(preferences);
        }
        
        /// <summary>
        /// Removes a preference from the Settings file.
        /// </summary>
        /// <param name="preference"></param>
        public void RemovePreference(KeyValuePair<TKey, TValue> preference)
        {
           PreferencesReader<TKey, TValue> reader = new PreferencesReader<TKey, TValue>(_pathToJsonFile);
           var preferences = reader.GetPreferences();
           preferences.Remove(preference);
           AddPreferences(preferences);
        }
    }
}