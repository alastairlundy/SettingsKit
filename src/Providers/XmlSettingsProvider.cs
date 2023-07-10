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
using System.Xml;
using System.Xml.Serialization;
using AlastairLundy.SettingsKit.Interfaces;

namespace AlastairLundy.SettingsKit;

public class XmlSettingsProvider<TKey, TValue> : ISettingsProvider<TKey, TValue>
{

    public KeyValuePair<TKey, TValue>[] Get(string pathToFile)
    {
        try
        {
            FileStream fileStream = new FileStream(pathToFile, FileMode.Open, FileAccess.Read);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<KeyValuePair<TKey, TValue>>));

            List<KeyValuePair<TKey, TValue>> deserialized =
                (List<KeyValuePair<TKey, TValue>>)xmlSerializer.Deserialize(fileStream);

            if (deserialized != null)
            {
                return deserialized.ToArray();
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


    public void WriteToFile(KeyValuePair<TKey, TValue>[] data, string pathToFile)
    {
        try
        {
            FileStream fileStream = new FileStream(pathToFile, FileMode.Create);
            
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<KeyValuePair<TKey, TValue>>));
            xmlSerializer.Serialize(fileStream, data);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            throw;
        }
    }
}