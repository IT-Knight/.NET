using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace CharacterStore
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedDictionary<char, int> charStore = new();

            var currentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);  
            string projectDirectory = currentDirectory.Parent.Parent.Parent.FullName;

            var fileText = File.ReadAllText(projectDirectory + "\\file.txt");

            foreach (var character in fileText)
            {
                if (charStore.ContainsKey(character))
                {
                    charStore[character]++;
                } 
                else
                {
                    charStore.Add(character, 1);
                }
            }

            char randomExixtingChar = charStore.Keys.ToArray()[new Random().Next(charStore.Keys.Count)];

            Console.WriteLine(JsonConvert.SerializeObject(charStore, Formatting.Indented) + "\n");

            RandomCheckForSure(randomExixtingChar, charStore, fileText);

            Console.WriteLine("Check for sure: " + CheckForSure(charStore, fileText));

        }

        public static void RandomCheckForSure(char c, SortedDictionary<char, int> dict, string fileText)
        {
            int res1 = dict[c];
            int res2 = fileText.Count(x => x == c);
            
            Console.WriteLine($"Char '{c}' from dict with value: {res1}. \nChar '{c}' from fileText with count: {res2}.");
            Console.WriteLine(res1 == res2 ? "Value1 == Value2" : "Value1 != Value2");
        }

        public static bool CheckForSure(SortedDictionary<char, int> dict, string fileText)
        {
            foreach (KeyValuePair<char, int> item in dict)
            {
                if (item.Value != fileText.Count(x => x == item.Key))
                {
                    return false;
                }

            }
            return true;
        }
    }
}
