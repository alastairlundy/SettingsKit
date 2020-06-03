/* BSD 3-Clause License

Copyright (c) 2020, AluminiumTech All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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

        public Preference<TKey, TValue>[] ToArray()
        {
            return _preferences.ToArray();
        }
    }
}