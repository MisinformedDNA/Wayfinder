using FluentAssertions;
using FluentAssertions.Execution;
using Wayfinder.Services;

namespace Wayfinder.Tests.Services
{
    public class GetPersonalKraytRewardsTests
    {
        [Fact]
        public void Has_correct_tier_count()
        {
            var gameService = new GameService();
            var tiers = gameService.GetPersonalKraytRaidRewards();
            tiers.Count.Should().Be(16);
        }

        [Fact]
        public void Get_highest_rewards()
        {
            var gameService = new GameService();
            var tiers = gameService.GetPersonalKraytRaidRewards();
            //var tier = tiers.OrderBy(x => x.Ordinal).Last();
            var tier = tiers.MaxBy(x => x.Ordinal);

            using var _ = new AssertionScope();
            tier!.Name.Should().Be("12.15M");
            tier.Ordinal.Should().Be(12_150_000);
            tier.Mk1RaidTokens.Should().Be(225);
            tier.Mk2RaidTokens.Should().Be(650);
            tier.Mk3RaidTokens.Should().Be(615);
        }

        [Fact]
        public void Get_lowest_rewards()
        {
            var gameService = new GameService();
            var tiers = gameService.GetPersonalKraytRaidRewards();
            var tier = tiers.MinBy(x => x.Ordinal);

            using var _ = new AssertionScope();
            tier!.Name.Should().Be("65K");
            tier.Ordinal.Should().Be(65_000);
            tier.Mk1RaidTokens.Should().Be(225);
            tier.Mk2RaidTokens.Should().Be(0);
            tier.Mk3RaidTokens.Should().Be(0);
        }
    }
}
