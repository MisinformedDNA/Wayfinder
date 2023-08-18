namespace Wayfinder.Services
{
    public record RewardTier
    {
        public required string Name { get; init; }
        public double Ordinal => Name[^1] switch
        {
            'M' => double.Parse(Name[0..^1]) * 1_000_000,
            'K' => double.Parse(Name[0..^1]) * 1_000,
            _ => 0
        };
        //public required string Tier { get; init; }
        public double? Crystals { get; init; }
        public double? Mk1RaidTokens { get; init; }
        public double? Mk2RaidTokens { get; init; }
        public double? Mk3RaidTokens { get; init; }
    }
}
