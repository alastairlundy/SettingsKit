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
using System.IO;
using System.Timers;
using AluminiumTech.DevKit.SettingsKit.enums;
using Newtonsoft.Json;

namespace AluminiumTech.DevKit.SettingsKit
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    // ReSharper disable once UnusedType.Global
    public class DataManager<TKey, TValue> : IDataManager<TKey, TValue>
    {
        protected SettingsSavingInformation SavingInformation { get; set; }
        
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        // ReSharper disable once InconsistentNaming
        protected List<KeyValueData<TKey, TValue>> listOfData;

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public string PathToJsonFile { get; set; }

        protected Timer _timer;

        protected bool ModifiedSinceLastSave;

        public DataManager(string pathToJsonFile)
        {
            PathToJsonFile = pathToJsonFile;
            listOfData = new List<KeyValueData<TKey, TValue>>();
            SavingInformation = new SettingsSavingInformation();
            SavingInformation.SavingMode = SettingsSavingMode.SaveAfterEveryChange;
            LoadTimer();
        }

        public DataManager(string pathToJsonFile, SettingsSavingInformation savingInformation)
        {
            PathToJsonFile = pathToJsonFile;
            listOfData = new List<KeyValueData<TKey, TValue>>();
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
                
                var deserializeObject = JsonConvert.DeserializeObject<KeyValueData<TKey, TValue>[]>(json);

                ImportData(deserializeObject);
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
            string contents = JsonConvert.SerializeObject(listOfData.ToArray());
            File.WriteAllText(pathToJsonFile, contents);
            ModifiedSinceLastSave = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Throws an exception if it is unable to find a preference with the specified key.</exception>
        public KeyValueData<TKey, TValue> GetData(TKey key)
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

        public void ImportData(KeyValueData<TKey, TValue>[] data)
        {
            foreach (var keyValueData in data)
            {
                AddData(keyValueData.Key, keyValueData.Value, keyValueData.DefaultValue);       
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <exception cref="Exception"></exception>
        public void ImportData(List<KeyValueData<TKey, TValue>> data)
        {
            try
            {
                foreach (var preference in data)
                {
                   AddData(preference.Key, preference.Value, preference.DefaultValue);
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
        /// <param name="default"></param>
        public void AddData(TKey key, TValue value, TValue @default)
        {
            try
            {
                if (DoesDataExist(key))
                {
                    throw new ArgumentException("Key already exists in dataset.");
                }
                
                KeyValueData<TKey, TValue> data = new KeyValueData<TKey, TValue>();
                data.Key = key;
                data.Value = value;
                data.DefaultValue = @default;
            
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

        public void UpdateData(TKey key, TValue value)
        {
            var oldDefault = GetData(key).DefaultValue;
            UpdateData(key, value, oldDefault);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="default"></param>
        /// <exception cref="Exception"></exception>
         public void UpdateData(TKey key, TValue value, TValue @default)
        {
            try
            {
                if (!DoesDataExist(key))
                {
                    AddData(key, value, @default);
                }
                else
                {
                    listOfData[GetDataPosition(key)].Value = value;
                    listOfData[GetDataPosition(key)].DefaultValue = @default;
                    listOfData[GetDataPosition(key)].LastEdited = DateTime.Now;
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
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TValue GetDataValueOrDefault(TKey key)
        {
            try
            {
                if (listOfData[GetDataPosition(key)].Value == null)
                {
                    return listOfData[GetDataPosition(key)].DefaultValue;
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

        public List<KeyValueData<TKey, TValue>> ToList()
        {
            return listOfData;
        }
    }
}