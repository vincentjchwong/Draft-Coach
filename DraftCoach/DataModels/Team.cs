using System.Collections.Generic;

namespace DraftCoach.DataModels
{
    public class Team
    {
        public readonly string Colour;
        public IList<Champion> PickedChampions = new List<Champion>();
        public IList<Champion> BannedChampions = new List<Champion>();

        public Team(string colour)
        {
            Colour = colour;
        }
    }
}
