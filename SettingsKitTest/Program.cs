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

            Preferences<string, string> preferences = new Preferences<string, string>(path);

            preferences.LoadPreferences(path);
            
            //preferences.AddPreference("aluminium_state", "awake", "awake");
            
            Console.WriteLine(preferences.GetPreference("aluminium_emotion").ToString());
            Console.WriteLine(preferences.GetPreference("aluminium_state").ToString());
            
        //    preferences.UpdatePreference("aluminium_emotion", "sad", "sad");
            
           // Console.WriteLine(preferences.GetPreference("aluminium_emotion").ToString());
        }
    }
}