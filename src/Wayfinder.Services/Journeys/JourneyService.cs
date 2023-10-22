using System.Text.Json;

namespace Wayfinder.Services.Journeys;

public class JourneyService
{
    private const string JourneysFileName = "Wayfinder.Services.data.journeys.json";
    private static readonly Assembly s_assembly = Assembly.GetExecutingAssembly();

    public static async Task<List<Journey>> GetAsync()
    {
        using var stream = s_assembly.GetManifestResourceStream(JourneysFileName)
            ?? throw new InvalidOperationException("Could not load embedded resource.");
        var journeys = await JsonSerializer.DeserializeAsync<List<Journey>>(stream)
            ?? throw new InvalidOperationException("File could not be read.");

        return journeys;
    }
}
