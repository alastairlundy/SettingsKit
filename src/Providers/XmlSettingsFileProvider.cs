/*
MIT License

Copyright (c) 2021-2024 Alastair Lundy

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
using System.Xml.Serialization;

namespace SettingsKit.Providers;

/// <summary>
/// A class to read and write Settings to XML files.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class XmlSettingsFileProvider<TValue> : ISettingsFileProvider<TValue>
{

    /// <summary>
    /// Read Settings from a Settings XML file.
    /// </summary>
    /// <param name="pathToFile"></param>
    /// <returns></returns>
    public KeyValuePair<string, TValue>[] Get(string pathToFile)
    {
        try
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<KeyValuePair<string, TValue>>));

            KeyValuePair<string, TValue>[] pairs;
            
            using (Stream reader = new FileStream(pathToFile, FileMode.Open, FileAccess.Read))
            {
                // Call the Deserialize method to restore the object's state.
                pairs = (KeyValuePair<string, TValue>[])xmlSerializer.Deserialize(reader);
            }

            if (pairs != null)
            {
                return pairs;
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            throw;
        }
    }

    /// <summary>
    /// Write the settings to the XML file.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pathToFile"></param>
    public void WriteToFile(KeyValuePair<string, TValue>[] data, string pathToFile)
    {
        try
        {
            FileStream fileStream = new FileStream(pathToFile, FileMode.Create);
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<KeyValuePair<string, TValue>>));
            xmlSerializer.Serialize(fileStream, data);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            throw;
        }
    }
}