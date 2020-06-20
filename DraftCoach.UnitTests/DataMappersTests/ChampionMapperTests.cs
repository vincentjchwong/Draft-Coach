using DraftCoach.DataMappers;
using DraftCoach.DataModels;
using Xunit;

namespace DraftCoach.UnitTests.DataMappersTests
{
    public class ChampionMapperTests
    {
        [Fact]
        public void ToChampion_ShouldReturnChampionWithCorrectName()
        {
            // Arrange
            var championName = "Amumu";

            // Action
            var champion = championName.ToChampion();

            // Assert
            Assert.Equal(championName, champion.Name);
        }
    }
}
