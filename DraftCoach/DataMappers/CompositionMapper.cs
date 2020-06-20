using DraftCoach.DataModels;
using DraftCoach.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DraftCoach.DataMappers
{
    public static class CompositionMapper
    {
        public static IList<Composition> ToCompositions(this IDictionary<string, DataTable> compositionsDataTables)
        {
            List<Composition> compositions = new List<Composition>();

            foreach (var entry in compositionsDataTables)
            {
                Composition composition = new Composition() { Name = entry.Key };

                composition.Top = entry.Value.AsEnumerable().Select(r => r.Field<string>(Role.Top).ToChampion()).Where(c => !string.IsNullOrEmpty(c.Name)).ToList();
                composition.Jungle = entry.Value.AsEnumerable().Select(r => r.Field<string>(Role.Jungle).ToChampion()).Where(c => !string.IsNullOrEmpty(c.Name)).ToList();
                composition.Mid = entry.Value.AsEnumerable().Select(r => r.Field<string>(Role.Mid).ToChampion()).Where(c => !string.IsNullOrEmpty(c.Name)).ToList();
                composition.ADC = entry.Value.AsEnumerable().Select(r => r.Field<string>(Role.ADC).ToChampion()).Where(c => !string.IsNullOrEmpty(c.Name)).ToList();
                composition.Support = entry.Value.AsEnumerable().Select(r => r.Field<string>(Role.Support).ToChampion()).Where(c => !string.IsNullOrEmpty(c.Name)).ToList();
                
                compositions.Add(composition);
            }

            return compositions;
        }
    }
}
