namespace AluminiumTech.SettingsKit.Base
{
    public class Preference<TKey, TValue>
    {
        public TKey KEY { get; set; }
        public TValue VALUE { get; set; }
    }
}