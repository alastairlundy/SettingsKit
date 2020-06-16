/* BSD 3-Clause License

Copyright (c) 2020, AluminiumTech All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections.Generic;
using System.IO;

using AluminiumTech.DevKit;
using AluminiumTech.SettingsKit.Base;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AluminiumTech.SettingsKit
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