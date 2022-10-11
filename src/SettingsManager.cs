/*
MIT License

Copyright (c) 2021-2022 AluminiumTech

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
using System.Timers;
using AluminiumTech.DevKit.SettingsKit.enums;

using System.Text.Json;

namespace AluminiumTech.DevKit.SettingsKit
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    // ReSharper disable once UnusedType.Global
    public class SettingsManager<TKey, TValue>
    { 
        protected SettingsSavingInformation SavingInformation { get; set; }
        
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once InconsistentNaming
        protected List<Preference<TKey, TValue>> listOfData;

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public string PathToJsonFile { get; set; }

        protected Timer _timer;

        protected bool ModifiedSinceLastSave;

        public SettingsManager(string pathToJsonFile)
        {
            PathToJsonFile = pathToJsonFile;
            listOfData = new List<Preference<TKey, TValue>>();
            SavingInformation = new SettingsSavingInformation();
            SavingInformation.SavingMode = SettingsSavingMode.SaveAfterEveryChange;
            LoadTimer();
        }

        public SettingsManager(string pathToJsonFile, SettingsSavingInformation savingInformation)
        {
            PathToJsonFile = pathToJsonFile;
            listOfData = new List<Preference<TKey, TValue>>();
            SavingInformation = savingInformation;
            LoadTimer();
        }

        protected void LoadTimer()
        {
            _timer = new Timer
            {
                AutoReset = false,
                Enabled = SavingInformation.SavingMode == SettingsSavingMode.AutoSaveAfterTimeMinutes,
                Interval = SavingInformation.AutoSaveFrequencyMinutes * ((1000) * 60),
            };
            
            _timer.Elapsed += TimerOnElapsed;

            ModifiedSinceLastSave = false;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (ModifiedSinceLastSave && SavingInformation.SavingMode == SettingsSavingMode.AutoSaveAfterTimeMinutes)
            {
                WriteJsonFile(PathToJsonFile);

                if (SavingInformation.AutoSaveFrequencyMinutes > 5)
                {
                    SavingInformation.AutoSaveFrequencyMinutes /= 2;
                }
            }
            else if(!ModifiedSinceLastSave && SavingInformation.SavingMode == SettingsSavingMode.AutoSaveAfterTimeMinutes)
            {
                if (SavingInformation.AutoSaveFrequencyMinutes < 100)
                {
                    SavingInformation.AutoSaveFrequencyMinutes *= 2;
                }
            }
        }
        
        public void LoadData(string pathToJsonFile)
        {
            try
            {
                string json = File.ReadAllText(pathToJsonFile);

               List<Preference<TKey, TValue>> deserializeObject = JsonSerializer.Deserialize<List<Preference<TKey, TValue>>>(json);
                
                //var deserializeObject = JsonConvert.DeserializeObject<Data<TKey, TValue>[]>(json);

                ImportData(deserializeObject.ToArray());
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToJsonFile"></param>
        public void WriteJsonFile(string pathToJsonFile)
        {
            string contents = JsonSerializer.Serialize(listOfData.ToArray());
            File.WriteAllText(pathToJsonFile, contents);
            ModifiedSinceLastSave = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Throws an exception if it is unable to find a preference with the specified key.</exception>
        public KeyValuePair<TKey, TValue> GetData(TKey key)
        {
            foreach (var data in listOfData)
            {
                if (key is string)
                {
                    if (data.Key.ToString().ToLower().Equals(key.ToString().ToLower()))
                    {
                        return new KeyValuePair<TKey, TValue>(data.Key, data.Value);
                    }
                }
                else
                {
                    if (data.Key.Equals(key))
                    {
                        return new KeyValuePair<TKey, TValue>(data.Key, data.Value);
                    }
                }
            }

            throw new KeyNotFoundException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Throws an exception if it is unable to find a preference with the specified key.</exception>
        public Preference<TKey, TValue> GetPreference(TKey key)
        {
            foreach (var data in listOfData)
            {
                if (key is string)
                {
                    if (data.Key.ToString().ToLower().Equals(key.ToString().ToLower()))
                    {
                        return data;
                    }
                }
                else
                {
                    if (data.Key.Equals(key))
                    {
                        return data;
                    }
                }
            }

            throw new KeyNotFoundException();
        }

        public void ImportData(KeyValuePair<TKey, TValue>[] data)
        {
            foreach (var keyValueData in data)
            {
                AddData(keyValueData.Key, keyValueData.Value);       
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="Exception"></exception>
        public void ImportData(Preference<TKey, TValue>[] data)
        {
            try
            {
                foreach (var preference in data)
                {
                   AddData(preference.Key, preference.Value, preference.Default);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddData(TKey key, TValue value)
        {
            try
            {
                if (DoesDataExist(key))
                {
                    throw new ArgumentException("Key already exists in dataset.");
                }
                
                listOfData.Add(new Preference<TKey, TValue>()
                {
                    Key = key,
                    Value = value
                });
            
                if (SavingInformation.SavingMode == SettingsSavingMode.SaveAfterEveryChange)
                {
                    WriteJsonFile(PathToJsonFile);
                }
                else
                {
                    ModifiedSinceLastSave = true;
                }
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public void AddData(TKey key, TValue value, TValue defaultValue)
        {
            try
            {
                if (DoesDataExist(key))
                {
                    throw new ArgumentException("Key already exists in dataset.");
                }

                Preference<TKey, TValue> data = new Preference<TKey, TValue>();
                data.Default = defaultValue;
                data.Key = key;
                data.Value = value;

                listOfData.Add(data);
            
                if (SavingInformation.SavingMode == SettingsSavingMode.SaveAfterEveryChange)
                {
                    WriteJsonFile(PathToJsonFile);
                }
                else
                {
                    ModifiedSinceLastSave = true;
                }
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// /// <exception cref="Exception"></exception>
         public void UpdateData(TKey key, TValue value)
         {
            try
            {
                if (!DoesDataExist(key))
                {
                    AddData(key, value);
                }
                else
                {
                    listOfData[GetDataPosition(key)] = new Preference<TKey, TValue>()
                    {
                        Key = key,
                        Value = value
                    };
                }

                if (SavingInformation.SavingMode == SettingsSavingMode.SaveAfterEveryChange)
                {
                    WriteJsonFile(PathToJsonFile);
                }
                else
                {
                    ModifiedSinceLastSave = true;
                }
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// /// <exception cref="Exception"></exception>
        public void UpdateData(TKey key, TValue value, TValue defaultValue)
        {
            try
            {
                if (!DoesDataExist(key))
                {
                    AddData(key, value);
                }
                else
                {
                    listOfData[GetDataPosition(key)] = new Preference<TKey, TValue>()
                    {
                        Default = defaultValue,
                        Key = key,
                        Value = value,
                    };
                }

                if (SavingInformation.SavingMode == SettingsSavingMode.SaveAfterEveryChange)
                {
                    WriteJsonFile(PathToJsonFile);
                }
                else
                {
                    ModifiedSinceLastSave = true;
                }
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void RemoveData(TKey key)
        {
            try
            {
                if (DoesDataExist(key))
                {
                    listOfData.Remove(listOfData[GetDataPosition(key)]);

                    if (SavingInformation.SavingMode == SettingsSavingMode.SaveAfterEveryChange)
                    {
                        WriteJsonFile(PathToJsonFile);
                    }
                    else
                    {
                        ModifiedSinceLastSave = true;
                    }
                }
            }
            catch(Exception exception)
            {
                throw new KeyNotFoundException();
            }
        }

        public bool DoesDataExist(TKey key)
        {
            for (int index = 0; index < listOfData.Count; index++)
            {
                if (listOfData[index].Key.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public int GetDataPosition(TKey key)
        {
            if (DoesDataExist(key))
            {
                for (int index = 0; index < listOfData.Count; index++)
                {
                    if (listOfData[index].Key.Equals(key))
                    {
                        return index;
                    }
                }
            }
            
            throw new KeyNotFoundException();
        }

        public TValue GetDataValue(TKey key)
        {
            if (DoesDataExist(key))
            {
                return listOfData[GetDataPosition(key)].Value;
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Attempts to get a Data value or the default value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TValue GetDataValueOrDefault(TKey key, TValue defaultValue)
        {
            try
            {
                if (listOfData[GetDataPosition(key)].Value == null)
                {
                    return defaultValue;
                }
                else
                {
                    return listOfData[GetDataPosition(key)].Value;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        public void RemoveAllDataWithValue(TValue value)
        {
            for (int index = 0; index < listOfData.Count; index++)
            {
                if (listOfData[index].Value.Equals(value))
                {
                    listOfData.RemoveAt(index);
                }
            }
            
            if (SavingInformation.SavingMode == SettingsSavingMode.SaveAfterEveryChange)
            {
                WriteJsonFile(PathToJsonFile);
            }
            else
            {
                ModifiedSinceLastSave = true;
            }
        }

        public List<Preference<TKey, TValue>> ToList()
        {
            return listOfData;
        }
    }
}