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

namespace AlastairLundy.SettingsKit;

/// <summary>
/// A class to make managing Settings Files easier.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class SettingsManager<TKey, TValue>
{

    /// <summary>
    /// Gets a specified KeyValuePair from a KeyValuePair array.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public KeyValuePair<TKey, TValue> GetKeyValuePair(KeyValuePair<TKey, TValue>[] array, TKey key)
    {
        try
        {
            foreach (var pair in array)
            {
                if (pair.Key.Equals(key))
                {
                    return pair;
                }
            }

            throw new KeyNotFoundException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}