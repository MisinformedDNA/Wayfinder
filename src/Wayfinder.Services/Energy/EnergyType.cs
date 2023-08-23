using Ardalis.SmartEnum;
using System.Runtime.CompilerServices;

namespace Wayfinder.Services.Energy;

public sealed class EnergyType : SmartEnum<EnergyType>
{
    public static readonly EnergyType Normal = new(
        value: 1,
        TimeSpan.FromMinutes(6),
        bonusEnergy: 45,
        new()
        {
            new(new(12, 00), new (14, 00)),
            new(new(18, 00), new TimeOnly(20, 00)),
            new(new(21, 00), new TimeOnly(23, 00))
        });

    private EnergyType(
        int value,
        TimeSpan refreshRate,
        int bonusEnergy,
        List<BonusEnergyWindow> bonusEnergyWindows,
        [CallerMemberName] string name = null!) : base(name, value)
    {
        RefreshRate = refreshRate;
        BonusEnergy = bonusEnergy;
        BonusEnergyWindows = bonusEnergyWindows;
    }

    public int BonusEnergy { get; }
    public List<BonusEnergyWindow> BonusEnergyWindows { get; }
    public TimeSpan RefreshRate { get; }

    public int GetNaturalEnergy(DateTime start, DateTime end) =>
        (int)Math.Floor((end - start) / RefreshRate);
}
