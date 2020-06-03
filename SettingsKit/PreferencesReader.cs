/* BSD 3-Clause License

Copyright (c) 2020, AluminiumTech All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;

using System.IO;
using System.Linq;

using AluminiumTech.SettingsKit.Base;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AluminiumTech.SettingsKit
{
    public class PreferencesReader<TKey, TValue>
    {

        public Preference<TKey, TValue> GetPreference(string pathToJsonFile, TKey key)
        {
            var prefs = GetPreferences(pathToJsonFile);
           return prefs.Get(prefs.GetPosition(key));
        }
        
        public Preferences<TKey, TValue> GetPreferences(string pathToJsonFile)
        {
            Preferences<TKey, TValue> _preferences = new Preferences<TKey, TValue>();

            using (StreamReader file = File.OpenText(pathToJsonFile))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject json = (JObject) JToken.ReadFrom(reader);

                for (int i = 0; i < json.Count; i++)
                {
                    Preference<TKey, TValue> deserailizedPreference = JsonConvert.DeserializeObject<Preference<TKey, TValue>>(json[i].ToString());
                    _preferences.Add(deserailizedPreference);
                }
            }

            return _preferences;
        }
    }
}