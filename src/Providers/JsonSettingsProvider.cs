/*
MIT License

Copyright (c) 2021-2023 Alastair Lundy

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
using System.IO;
using System.Text.Json;

using AlastairLundy.SettingsKit.Interfaces;

namespace AlastairLundy.SettingsKit.Providers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    // ReSharper disable once UnusedType.Global
    public class JsonSettingsProvider<TKey, TValue> : ISettingsProvider<TKey, TValue>
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Global

        /// <summary>
        /// Retrieves KeyValuePair objects saved as a Json File.
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        /// <exception cref="Exception"></exception>
        public KeyValuePair<TKey, TValue>[] Get(string pathToJsonFile)
        {
            try
            {
                string json = File.ReadAllText(pathToJsonFile);

                if (json.Contains("key:"))
                {
                   json = json.Replace("key:", "Key:");
                }

                if (json.Contains("value:"))
                {
                    json = json.Replace("value:", "Value:");
                }

                List<KeyValuePair<TKey, TValue>> deserializeObject = JsonSerializer.Deserialize<List<KeyValuePair<TKey, TValue>>>(json);
                
                return deserializeObject.ToArray();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        /// <summary>
        ///  Writes the specified data to a Json file.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pathToJsonFile"></param>
        public void WriteToFile(KeyValuePair<TKey, TValue>[] data, string pathToJsonFile)
        {
            try
            {
                string contents = JsonSerializer.Serialize(data);

                #if DEBUG
                    Console.WriteLine(contents);
                #endif

                File.WriteAllText(pathToJsonFile, contents);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }
    }
}