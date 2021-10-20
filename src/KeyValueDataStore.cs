﻿/*
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
using System.Reflection;

namespace AluminiumTech.DevKit.SettingsKit
{
    public class KeyValueDataStore<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        
        public TValue DefaultValue { get; set; }
        
        public DateTime LastEdited { get; set; }
        
        public Version SettingsKitVersion { get; set; }
        
        public Version DataCompatibilityVersion { get; set; }
        
        public int DataRevision { get; set; }

        public KeyValueDataStore()
        {
            LastEdited = DateTime.Now;

            SettingsKitVersion = Assembly.GetCallingAssembly().GetName().Version;
            
            
            DataRevision = 0;
        }

        public override string ToString()
        {
            return $"Key:{Key} Value:{Value} DefaultValue:{DefaultValue} LastEdited:{LastEdited} CreatedOn:{SettingsKitVersion} Compatibility:{DataCompatibilityVersion} DataRevision:{DataRevision}";
        }
    }
}