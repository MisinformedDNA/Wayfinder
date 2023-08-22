using CsvHelper.Configuration.Attributes;

namespace Wayfinder.Services.Relics
{
    public record RelicCost
    {
        public required int RelicLevel { get; init; }
        [Default(0)] public int CarboniteCircuitBoard { get; init; }
        [Default(0)] public int BronziumWiring { get; init; }
        [Default(0)] public int ChromiumTransistor { get; init; }
        [Default(0)] public int AurodiumHeatsink { get; init; }
        [Default(0)] public int ElectriumConductor { get; init; }
        [Default(0)] public int ZinbiddleCard { get; init; }
        [Default(0)] public int Aeromagnifier { get; init; }
        [Default(0)] public int ImpulseDetector { get; init; }
        [Default(0)] public int GyrdaKeypad { get; init; }
        [Default(0)] public int DroidBrain { get; init; }
        [Default(0)] public int FragmentedSignal { get; init; }
        [Default(0)] public int IncompleteSignal { get; init; }
        [Default(0)] public int FlawedSignal { get; init; }

        public static RelicCost operator +(RelicCost a, RelicCost b) => new()
        {
            RelicLevel = a.RelicLevel + 1,
            CarboniteCircuitBoard = a.CarboniteCircuitBoard + b.CarboniteCircuitBoard,
            BronziumWiring = a.BronziumWiring + b.BronziumWiring,
            ChromiumTransistor = a.ChromiumTransistor + b.ChromiumTransistor,
            AurodiumHeatsink = a.AurodiumHeatsink + b.AurodiumHeatsink,
            ElectriumConductor = a.ElectriumConductor + b.ElectriumConductor,
            ZinbiddleCard = a.ZinbiddleCard + b.ZinbiddleCard,
            Aeromagnifier = a.Aeromagnifier + b.Aeromagnifier,
            ImpulseDetector = a.ImpulseDetector + b.ImpulseDetector,
            GyrdaKeypad = a.GyrdaKeypad + b.GyrdaKeypad,
            DroidBrain = a.DroidBrain + b.DroidBrain,
            FragmentedSignal = a.FragmentedSignal + b.FragmentedSignal,
            IncompleteSignal = a.IncompleteSignal + b.IncompleteSignal,
            FlawedSignal = a.FlawedSignal + b.FlawedSignal
        };
    }
}
