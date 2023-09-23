namespace Wayfinder.Services.RequiredUnits;

public record GoalUnit(string UnitId, List<Requirement> Requirements);

public record Requirement(string UnitId, string Level);
