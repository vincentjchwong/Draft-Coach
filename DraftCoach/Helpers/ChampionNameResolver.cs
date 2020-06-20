using DraftCoach.DataMappers;
using DraftCoach.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DraftCoach.Helpers
{
    public static class ChampionNameResolver
    {
        private static List<Champion> Champions = new List<Champion>();

        public static void LoadAll(string championsDataLocations)
        {
            var championsDataTables = DataHelper.RetrieveDataTables(championsDataLocations);
            foreach (var entry in championsDataTables)
            {
                var championsToAdd = entry.Value.AsEnumerable().Select(r => r.Field<string>("Name").ToChampion()).Where(c => !string.IsNullOrEmpty(c.Name)).ToList();
                Champions.AddRange(championsToAdd.Where(cta => !Champions.Any(c => cta.Name == c.Name)));
            }
        }

        public static Champion ParseChampionFromUserInput()
        {
            var userInput = TextHelper.RemoveSpecialCharactersAndLowerCase(Console.ReadLine());
            var possibleChampions = Champions.Where(c => TextHelper.RemoveSpecialCharactersAndLowerCase(c.Name).Contains(userInput));

            if (possibleChampions.Where(c => userInput == TextHelper.RemoveSpecialCharactersAndLowerCase(c.Name)).Count() == 1)
            {
                return possibleChampions.Where(c => userInput == TextHelper.RemoveSpecialCharactersAndLowerCase(c.Name)).First();
            }

            if (possibleChampions.Count() == 0)
            {
                TextHelper.PrintErrorLine("No champions were found.");
            }
            else if (possibleChampions.Count() > 10)
            {
                TextHelper.PrintErrorLine("Too many possible champions to display, please be more specific.");
            }
            else
            {
                Console.WriteLine("Were you looking for one of the following?");
                foreach (var champion in possibleChampions)
                {
                    Console.WriteLine(champion.Name);
                }
            }

            return null;
        }
    }
}
