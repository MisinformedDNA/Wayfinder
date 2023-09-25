using CommunityToolkit.Mvvm.ComponentModel;
using Wayfinder.Services.Models;

namespace Wayfinder.Services.RequiredUnits;

public partial class RequiredUnitsViewModel : ObservableObject
{
    [ObservableProperty] List<Unit> _selectedUnits = default!;
    [ObservableProperty] private List<RequiredUnit> _units = default!;

    public IReadOnlyList<Unit> AllUnits { get; set; } = default!;
    public List<GoalUnit> AllObjectives { get; set; } = default!;


    public async Task InitializeAsync()
    {
        await LoadUnitsAsync();
        await LoadRequirementsAsync();

        var query = from r in AllObjectives
                    join u in AllUnits on r.UnitId equals u.BaseId
                    orderby u.Name
                    select u;

        SelectedUnits = query.ToList();
    }

    private async Task LoadUnitsAsync() => AllUnits = await GameService.GetUnitsAsync();

    private async Task LoadRequirementsAsync() => AllObjectives = await RequiredUnitsService.GetAllRequirementsAsync();

    public void LoadRequiredUnits()
    {
        var selectedUnitIds = SelectedUnits.Select(x => x.BaseId).ToArray();
        var objectives = AllObjectives.Where(x => selectedUnitIds.Contains(x.UnitId));
        var requirements = objectives.SelectMany(x => x.Requirements).ToList();

        var unitDict = AllUnits.ToDictionary(x => x.BaseId, x => x.Name);

        var query = from g in objectives
                    from r in g.Requirements
                    select new { GoalUnit = g.UnitId, RequiredUnit = r.UnitId, RequiredLevel = r.Level } into d
                    group d by d.RequiredUnit into ru
                    select new RequiredUnit(ru.Key, ru.Select(x => new RequiredDetail(x.GoalUnit, x.RequiredLevel)).ToList())                    ;

        Units = query.ToList();
    }
}

public record RequiredDetail(string GoalUnitId, string Level);

public record RequiredUnit(string UnitId, List<RequiredDetail> Details);