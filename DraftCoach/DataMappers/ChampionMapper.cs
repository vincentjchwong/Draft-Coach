using DraftCoach.DataModels;

namespace DraftCoach.DataMappers
{
    public static class ChampionMapper
    {
        public static Champion ToChampion(this string championName) => new Champion()
        {
            Name = championName
        };
    }
}
