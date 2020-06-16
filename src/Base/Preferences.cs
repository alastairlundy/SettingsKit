/* BSD 3-Clause License

Copyright (c) 2020, AluminiumTech All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;

using AluminiumTech.DevKit;

namespace AluminiumTech.SettingsKit.Base
{
    public class Preferences<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return ToHashMapWrapper().ToDictionary();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public HashMapWrapper<TKey, TValue> ToHashMapWrapper()
        {
            HashMapWrapper<TKey, TValue> hashMapWrapper = new HashMapWrapper<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> pairs in this)
            {
                hashMapWrapper.Put(pairs.Key, pairs.Value);
            }

            return hashMapWrapper;
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