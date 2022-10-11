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

            DataManager<string, string> preferences = new DataManager<string, string>(path);
            
            preferences.LoadData(path);

         //   preferences.AddData("aluminium_state", "awake", "super");
         //   preferences.AddData("aluminium_emotion", "happy", "dope");
            
            Console.WriteLine(preferences.GetData("aluminium_emotion").ToString());
           Console.WriteLine(preferences.GetData("aluminium_state").ToString());
           Console.WriteLine(preferences.GetData("pascal_replying_to_me").ToString()); 
           
        //    preferences.UpdatePreference("aluminium_emotion", "sad", "sad");
        }
    }
}