namespace Wayfinder.Services.RequiredUnits;

public class RequiredUnitsService
{
    private const string RequiredUnitsFileName = "Wayfinder.Services.data.required-units.json";
    private static readonly Assembly s_assembly = Assembly.GetExecutingAssembly();

    public static async Task<List<GoalUnit>> GetAllRequirementsAsync()
    {
        using var stream = s_assembly.GetManifestResourceStream(RequiredUnitsFileName)
            ?? throw new InvalidOperationException("Could not load embedded resource.");
        var goalUnits = await System.Text.Json.JsonSerializer.DeserializeAsync<List<GoalUnit>>(stream)
            ?? throw new InvalidOperationException("File could not be read.");

        return goalUnits;
    }
}
