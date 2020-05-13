
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