using System.IO;

using AluminiumTech.SettingsKit.Base;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AluminiumTech.SettingsKit
{
    public class PreferencesWriter<TKey, TValue>
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        /// <param name="preference"></param>
        public void AddPreference(string pathToJsonFile, Preference<TKey, TValue> preference)
        {
            JsonSerializer serializer = new JsonSerializer();
            
            using (StreamWriter sw = new StreamWriter(pathToJsonFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, preference);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        /// <param name="preferences"></param>
        public void AddPreferences(string pathToJsonFile, Preferences<TKey, TValue> preferences)
        {
            for (int i = 0; i < preferences.Size(); i++)
            {
                Preference<TKey, TValue> preference = preferences.Get(i);
                AddPreference(pathToJsonFile, preference);
            }
        }
    }
}