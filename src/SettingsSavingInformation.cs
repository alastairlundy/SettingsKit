using AluminiumTech.DevKit.SettingsKit.enums;

namespace AluminiumTech.DevKit.SettingsKit
{
    public class SettingsSavingInformation
    {
        public SettingsSavingMode SavingMode { get; set; }
        
        public double AutoSaveFrequencyMinutes { get; set; }

        public SettingsSavingInformation()
        {
            AutoSaveFrequencyMinutes = 5;
            SavingMode = SettingsSavingMode.SaveAfterEveryChange;
        }
    }
}