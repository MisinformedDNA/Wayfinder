using Wayfinder.Services.Journeys;

namespace Wayfinder.Tests.Services;

public class GetRequiredUnitsTests
{
    [Fact]
    public async Task Has_multiple_units()
    {
        var units = await JourneyService.GetAsync();
        units.Should().HaveCountGreaterThan(10);
    }

    [Theory]
    [InlineData("JOURNEY_LORDVADER")]
    [InlineData("JOURNEY_CAPITALEXECUTOR")]
    public async Task Has_unit(string unitId)
    {
        var units = await JourneyService.GetAsync();
        units.Should().Contain(x => x.Id.Equals(unitId, StringComparison.InvariantCultureIgnoreCase));
    }

    [Theory]
    [InlineData("JOURNEY_LORDVADER", 15)]
    public async Task Has_X_requirements(string unitId, int numOfRequirements)
    {
        var units = await JourneyService.GetAsync();
        var unit = units.First(x => x.Id.Equals(unitId, StringComparison.InvariantCultureIgnoreCase));
        unit.Requirements.Should().HaveCount(numOfRequirements);
    }
}
