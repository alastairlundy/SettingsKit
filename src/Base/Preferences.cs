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

namespace AluminiumTech.DevKit.SettingsKit.Base
{
    public class Preferences<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {

        /// <summary>
        /// Returns preferences to a dictionary object.
        /// </summary>
        /// <returns></returns>
        public Dictionary<TKey, TValue> ToDictionary()
        {
            try
            {
                Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

                foreach (KeyValuePair<TKey, TValue> pairs in this)
                {
                    dictionary.Add(pairs.Key, pairs.Value);
                }

                return dictionary;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Modifies an existing preference and replacing it with a new one
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        public void Modify(TKey key, TValue newValue)
        {
            try
            {
                KeyValuePair<TKey, TValue> pair = new KeyValuePair<TKey, TValue>(key, newValue);

                Replace(this[GetPosition(key)], pair);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Replaces an existing preference with a new one.
        /// </summary>
        /// <param name="oldPreference"></param>
        /// <param name="newPreference"></param>
        public void Replace(KeyValuePair<TKey, TValue> oldPreference, KeyValuePair<TKey, TValue> newPreference)
        { 
            try
            {
                Insert(GetPosition(oldPreference), newPreference);
                Remove(oldPreference);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Gets the position of a preference within this Preferences object.
        /// <param name="preference"></param>
        /// <returns></returns>
        public int GetPosition(KeyValuePair<TKey, TValue> preference)
        {
            try
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Equals(preference))
                    {
                        return i;
                    }
                }

                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Get the position of the Key within a list of HashMapWrappers
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetPosition(TKey key)
        {           
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
    }
}