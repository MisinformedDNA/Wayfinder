namespace Wayfinder.Services.Energy;

public abstract record EnergyAction();

public record DoNothing : EnergyAction
{
    public override string ToString() => "Nothing to do. You are on track!";
}

public record SpendEnergy(int Energy, int EndEnergy, DateTime SpendAt) : EnergyAction
{
    public SpendEnergy(int Energy, DateTime SpendAt) : this(Energy, 0, SpendAt) { }

    public override string ToString() => $"Spend {Energy} energy at {SpendAt} to have {EndEnergy} energy.";
}

public record CollectBonusEnergy(int Energy, int EndEnergy, DateTime CollectAt) : EnergyAction
{
    public CollectBonusEnergy(int Energy, DateTime CollectAt) : this(Energy, 0, CollectAt) { }

    public override string ToString() => $"Collect bonus energy at {CollectAt} to have {EndEnergy}.";
}
