using FluentAssertions;
using Wayfinder.Services.RequiredUnits;

namespace Wayfinder.Tests.Services;

public class GetRequiredUnitsTests
{
    [Fact]
    public async Task Has_multiple_units()
    {
        var units = await RequiredUnitsService.GetAllRequirementsAsync();
        units.Should().HaveCountGreaterThan(10);
    }

    [Theory]
    [InlineData("LV")]
    [InlineData("EXECUTOR")]
    public async Task Has_unit(string unitId)
    {
        var units = await RequiredUnitsService.GetAllRequirementsAsync();
        units.Should().Contain(x => x.UnitId.Equals(unitId, StringComparison.InvariantCultureIgnoreCase));
    }

    [Theory]
    [InlineData("LV", 15)]
    public async Task Has_X_requirements(string unitId, int numOfRequirements)
    {
        var units = await RequiredUnitsService.GetAllRequirementsAsync();
        var unit = units.First(x => x.UnitId.Equals(unitId, StringComparison.InvariantCultureIgnoreCase));
        unit.Requirements.Should().HaveCount(numOfRequirements);
    }
}
