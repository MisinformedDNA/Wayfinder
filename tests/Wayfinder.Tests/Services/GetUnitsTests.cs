using FluentAssertions;
using Wayfinder.Services;

namespace Wayfinder.Tests.Services;

public class GetUnitsTests
{
    [Fact]
    public async Task Has_multiple_units()
    {
        var units = await GameService.GetUnitsAsync();
        units.Should().HaveCountGreaterThan(100);
    }
}
