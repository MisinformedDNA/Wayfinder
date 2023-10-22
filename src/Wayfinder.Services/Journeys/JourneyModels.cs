namespace Wayfinder.Services.Journeys;

public record Journey(string Id, List<Requirement> Requirements)
{
    public override int GetHashCode() => HashCode.Combine(Id);

    public override string ToString() => Id;
}

public record Requirement(string UnitId, string Level);
