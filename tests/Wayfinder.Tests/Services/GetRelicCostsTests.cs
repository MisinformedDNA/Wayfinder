using FluentAssertions;
using Wayfinder.Services.Relics;

namespace Wayfinder.Tests.Services;

public class GetRelicCostsTests
{
    [Fact]
    public void Has_correct_tier_count()
    {
        var relicCosts = RelicService.GetRelicCosts();
        relicCosts.Should().HaveCount(9);
    }
}
