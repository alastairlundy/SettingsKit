using System.Collections.Generic;

namespace AluminiumTech.DevKit.SettingsKit
{
    public interface IDataManager<TKey, TValue>
    {
        public bool DoesDataExist(TKey key);
        
        public void LoadData(string pathToJsonFile);

         void WriteJsonFile(string pathToJsonFile);

        public KeyValueData<TKey, TValue> GetData(TKey key);

        public void ImportData(List<KeyValueData<TKey, TValue>> data);

        public void AddData(TKey key, TValue value, TValue @default);

        public void UpdateData(TKey key, TValue value);

        public void UpdateData(TKey key, TValue value, TValue @default);

        public void RemoveData(TKey key);

        public int GetDataPosition(TKey key);

        public TValue GetDataValue(TKey key);

        public TValue GetDataValueOrDefault(TKey key);

        public void RemoveAllDataWithValue(TValue value);

        public List<KeyValueData<TKey, TValue>> ToList();
    }
}