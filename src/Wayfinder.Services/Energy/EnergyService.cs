namespace Wayfinder.Services.Energy
{
    public class EnergyService
    {
        public static EnergyPlan GetEnergyPlan(EnergyType energyType, DateTime startTime, int startEnergy, DateTime endTime, int endEnergy)
        {
            if (endTime < startTime)
                throw new ArgumentException("End time must be after start time.");

            var bonusWindows = GetBonusWindows(energyType, startTime, endTime);
            bonusWindows.Reverse();

            var energyPlan = new EnergyPlan();

            var currentTime = endTime;
            var currentEnergy = endEnergy;
            while (currentTime > startTime && currentEnergy > 0)
            {
                if (bonusWindows.Any())
                {
                    var window = bonusWindows.First();
                    var windowEnd = window.EndAt();
                    if (currentTime < windowEnd)
                    {
                        // Add to beginning of bonus window
                        var naturalEnergy = energyType.GetNaturalEnergy(window.StartAt(), currentTime);
                        var energyAtBonusStart = currentEnergy - naturalEnergy;

                        if (energyAtBonusStart - energyType.BonusEnergy < 0)
                        {
                            var spendEnergy = energyType.BonusEnergy - energyAtBonusStart;
                            energyPlan.Steps.Add(new SpendEnergy(spendEnergy, energyAtBonusStart, window.StartAt()));

                            energyPlan.Steps.Add(new CollectBonusEnergy(energyType.BonusEnergy, energyType.BonusEnergy, window.StartAt()));
                            currentEnergy = 0;

                            break;
                        }
                        else
                        {
                            energyPlan.Steps.Add(new CollectBonusEnergy(energyType.BonusEnergy, energyAtBonusStart, window.StartAt()));
                        }

                        currentEnergy -= energyType.BonusEnergy;
                        bonusWindows.RemoveAt(0);
                    }
                }

                currentTime -= energyType.RefreshRate;
                currentEnergy--;
            }

            if (currentEnergy > 0)
            {
                var excessEnergy = startEnergy - currentEnergy;
                if (excessEnergy > 0)
                {
                    energyPlan.Steps.Add(new SpendEnergy(excessEnergy, currentEnergy, currentTime));
                }
            }

            if (energyPlan.Steps.Count == 0)
            {
                energyPlan.Steps.Add(new DoNothing());
            }

            energyPlan.Steps.Reverse();
            return energyPlan;
        }

        public static List<BonusEnergyWindow> GetBonusWindows(EnergyType energyType, DateTime startTime, DateTime endTime)
        {
            if (endTime < startTime)
                throw new ArgumentException("End time must be after start time.");

            var bonusWindows = new List<BonusEnergyWindow>();
            foreach (var bonusWindow in energyType.BonusEnergyWindows)
            {
                var currentDate = startTime.Date;
                while (currentDate <= endTime.Date)
                {
                    var windowStart = CombineDateTime(currentDate, bonusWindow.StartTime);
                    var windowEnd = CombineDateTime(currentDate, bonusWindow.EndTime);
                    if ((windowEnd > startTime) && (windowStart <= endTime))
                    {
                        bonusWindows.Add(new BonusEnergyWindow(
                            TimeOnly.FromDateTime(windowStart),
                            TimeOnly.FromDateTime(windowEnd),
                            DateOnly.FromDateTime(currentDate)));
                    }

                    currentDate = currentDate.AddDays(1);
                }
            }

            return bonusWindows
                .OrderBy(x => x.Date)
                .ThenBy(x => x.StartTime)
                .ToList();
        }

        private static DateTime CombineDateTime(DateTime currentDate, TimeOnly startTime) =>
                new(currentDate.Year, currentDate.Month, currentDate.Day, startTime.Hour, startTime.Minute, 0);
    }
}
