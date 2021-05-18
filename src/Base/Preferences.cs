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

namespace AluminiumTech.DevKit.SettingsKit.Base
{
    public class Preferences<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> pairs in this)
            {
               dictionary.Add(pairs.Key, pairs.Value);
            }

            return dictionary;
        }

        public void Modify(KeyValuePair<TKey, TValue> oldPreference, KeyValuePair<TKey, TValue> newPreference)
        { 
            Insert(GetPosition(oldPreference), newPreference);
            Remove(oldPreference);
        }

        public int GetPosition(KeyValuePair<TKey, TValue> preference)
        {
            for(int i = 0; i < this.Count; i++)
            {
                if (this[i].Equals(preference))
                {
                    return i;
                }
            }

            return 0;
        }

        /// <summary>
        /// Get the position of the Key within a list of HashMapWrappers
        /// Note:
        ///     First int in Tuple is the selected HashMapWrapper in the list.
        ///     Second int in Tuple is the position in the selected HashMapWrapper
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetPosition(TKey key)
        {
            for (int index = 0; index < this.Count; index++)
            {
                if (this[index].Equals(key))
                {
                    return index;
                }
            }

            return 0;
        }
    }
}