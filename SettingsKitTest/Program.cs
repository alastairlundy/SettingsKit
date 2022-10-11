using System;
using System.Collections.Generic;
using System.IO;

using AluminiumTech.DevKit.SettingsKit;

namespace SettingsKitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Testing.json";

            SettingsManager<string, string> preferences = new SettingsManager<string, string>(path);
            
            //preferences.LoadData(path);

            preferences.Set("aluminium_state", "awake");
            preferences.Set("aluminium_emotion", "happy");
            preferences.Set("pascal_replying_to_me", "no");
            preferences.Set("time_to_eat", "not_now");

            preferences.WriteJsonFile(path);
            
            Console.WriteLine(preferences.Get("aluminium_emotion").ToString());
           Console.WriteLine(preferences.Get("aluminium_state").ToString());
           Console.WriteLine(preferences.Get("pascal_replying_to_me").ToString()); 
           
        //    preferences.UpdatePreference("aluminium_emotion", "sad", "sad");
        }
    }
}