using System.Text.Json;
using Wayfinder.Services.Helpers;
using Wayfinder.Services.Models;

namespace Wayfinder.Services
{
    public class GameService
    {
        private const string KraytGuildFileName = "Wayfinder.Services.data.krayt-guild.csv";
        private const string KraytPersonalFileName = "Wayfinder.Services.data.krayt-personal.csv";
        private const string UnitsFileName = "Wayfinder.Services.data.swgoh.gg.units.json";
        private static readonly Assembly s_assembly = Assembly.GetExecutingAssembly();
        private static readonly CsvConfiguration s_configuration = new(CultureInfo.InvariantCulture) { HasHeaderRecord = true };

        public Rewards GetPersonalRaidRewards(Raid raid, int points)
        {
            if (raid == Raid.Krayt)
            {
                _ = GetPersonalKraytRaidRewards();
                return null;
            }
            //Resources
            throw new NotImplementedException();
        }

        public List<RewardTier> GetPersonalKraytRaidRewards() => GetRewardTiers(KraytPersonalFileName);

        public List<RewardTier> GetGuildKraytRewards() => GetRewardTiers(KraytGuildFileName);

        public static async Task<IReadOnlyList<Unit>> GetUnitsAsync()
        {
            using var stream = s_assembly.GetManifestResourceStream(UnitsFileName)
                ?? throw new InvalidOperationException("Could not load embedded resource.");
            var result = await JsonSerializer.DeserializeAsync<UnitRoot>(stream)
                ?? throw new InvalidOperationException("No data found.");
            return result.Units;
        }

        private List<RewardTier> GetRewardTiers(string name)
        {
            using var stream = s_assembly.GetManifestResourceStream(name)
                ?? throw new InvalidOperationException("Could not load embedded resource.");
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, s_configuration);
            csv.Context.RegisterClassMap<RewardTierMapper>();

            var rewardTiers = csv.GetRecords<RewardTier>();
            return rewardTiers.ToList();
        }
    }
}
