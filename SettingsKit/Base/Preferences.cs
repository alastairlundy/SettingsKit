using System.Collections.Generic;
using System.Drawing;

namespace AluminiumTech.SettingsKit.Base
{
    public class Preferences<TKey, TValue>
    {
        protected List<Preference<TKey, TValue>> _preferences;

        public Preferences()
        {
            _preferences = new List<Preference<TKey, TValue>>();
        }

        public void Add(Preference<TKey, TValue> preference)
        {
            _preferences.Add(preference);
        }

        public void Remove(Preference<TKey, TValue> preference)
        {
            _preferences.Remove(preference);
        }

        public void Modify(Preference<TKey, TValue> oldPreference, Preference<TKey, TValue> newPreference)
        {
            _preferences.Remove(oldPreference);
            _preferences.Insert(GetPosition(oldPreference), newPreference);
        }
        
        public Preference<TKey, TValue> Get(int i)
        {
           return _preferences[i];
        }

        public int GetPosition(Preference<TKey, TValue> preference)
        {
            for(int i = 0; i < _preferences.Count; i++)
            {
                if (_preferences[i].Equals(preference))
                {
                    return i;
                }
            }

            return 0;
        }

        public int GetPosition(TKey key)
        {
            foreach (Preference<TKey, TValue> prefs in _preferences)
            {
                if (prefs.KEY.Equals(key))
                {
                    return GetPosition(prefs);
                }
            }

            return 0;
        }

        public int Size()
        {
            return _preferences.Count;
        }
    }
}