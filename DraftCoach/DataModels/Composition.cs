using System.Collections.Generic;

namespace DraftCoach.DataModels
{
    public class Composition
    {
        public string Name;

        public IList<Composition> StrongAgainst;
        public IList<Composition> WeakAgainst;

        public IList<Champion> Top;
        public IList<Champion> Mid;
        public IList<Champion> Jungle;
        public IList<Champion> ADC;
        public IList<Champion> Support;
    }
}
