namespace Wayfinder.Services.RequiredUnits
{
    public class RequiredRelicUnitsViewModel : RequiredUnitsViewModel
    {
        public override void LoadRequiredUnits()
        {
            var selectedUnitIds = SelectedUnits.Select(x => x.BaseId).ToArray();
            var objectives = AllObjectives.Where(x => selectedUnitIds.Contains(x.UnitId));
            var requirements = objectives
                .SelectMany(x => x.Requirements)
                .Where(x => x.Level.StartsWith("RELIC"))
                .ToList();

            var unitDict = AllUnits.ToDictionary(x => x.BaseId, x => x.Name);

            var query = from g in objectives
                        from r in g.Requirements
                        select new { GoalUnit = g.UnitId, RequiredUnit = r.UnitId, RequiredLevel = r.Level } into d
                        group d by d.RequiredUnit into ru
                        select new RequiredUnit(ru.Key, ru.Select(x => new RequiredDetail(x.GoalUnit, x.RequiredLevel)).ToList());

            Units = query.ToList();
        }
    }
}
