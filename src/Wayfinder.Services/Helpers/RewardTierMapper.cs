using CsvHelper.Configuration;

namespace Wayfinder.Services.Helpers
{
    internal class RewardTierMapper : ClassMap<RewardTier>
    {
        public RewardTierMapper()
        {
            Map(x => x.Name).Name("Points");
            Map(x => x.Mk1RaidTokens).Name("M1RT (total)");
            Map(x => x.Mk2RaidTokens).Name("M2RT (total)");
            Map(x => x.Mk3RaidTokens).Name("M3RT (total)");
        }
    }
}
