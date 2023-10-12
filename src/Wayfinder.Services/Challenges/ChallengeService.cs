namespace Wayfinder.Services.Challenges;

public class ChallengeService
{
    private const string ChallengesFileName = "Wayfinder.Services.data.challenges.json";
    private static readonly Assembly s_assembly = Assembly.GetExecutingAssembly();

    public static async Task<List<Challenge>> GetAsync()
    {
        using var stream = s_assembly.GetManifestResourceStream(ChallengesFileName)
            ?? throw new InvalidOperationException("Could not load embedded resource.");
        var challenges = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Challenge>>(stream)
            ?? throw new InvalidOperationException("File could not be read.");

        return challenges;
    }
}
