namespace Wayfinder.Services.Challenges;

public record Challenge(string ChallengeId, List<Requirement> Requirements)
{
    public override int GetHashCode() => HashCode.Combine(ChallengeId);

    public override string ToString() => ChallengeId;
}

public record Requirement(string UnitId, string Level);
