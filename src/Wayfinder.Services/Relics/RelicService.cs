namespace Wayfinder.Services.Relics;

public class RelicService
{
    private const string RelicCostFileName = "Wayfinder.Services.data.relic-costs.csv";
    private static readonly Assembly s_assembly = Assembly.GetExecutingAssembly();
    private static readonly CsvConfiguration s_configuration = new(CultureInfo.InvariantCulture) { HasHeaderRecord = true };

    public static List<RelicCost> GetRelicCosts()
    {
        using var stream = s_assembly.GetManifestResourceStream(RelicCostFileName)
            ?? throw new InvalidOperationException("Could not load embedded resource.");
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, s_configuration);

        var relicCosts = csv.GetRecords<RelicCost>();
        return relicCosts.ToList();
    }
}
