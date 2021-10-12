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

using System.Collections.Generic;
using System.IO;
using System;

using Newtonsoft.Json;

using AluminiumTech.DevKit.SettingsKit.Base;

namespace AluminiumTech.DevKit.SettingsKit
{
    /// <summary>
    /// A class to read a .JSON preferences file.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class PreferencesReader<TKey, TValue>{

        public PreferencesReader(){

        }

        /// <summary>
        /// Return an individual preference.
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyValuePair<TKey, TValue> GetPreference(string pathToJsonFile, TKey key)
        { 
            var prefs = GetPreferences(pathToJsonFile);
           return prefs[(prefs.GetPosition(key))];
        }
        
        /// <summary>
        /// Return a list of preferences.
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        /// <returns></returns>
        public Preferences<TKey, TValue> GetPreferences(string pathToJsonFile)
        {
            try
            {
                string json = File.ReadAllText(pathToJsonFile);
                
                Preferences<TKey, TValue> deserializedPreference = JsonConvert.DeserializeObject<Preferences<TKey, TValue>>(json);
                return deserializedPreference;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
    }
}