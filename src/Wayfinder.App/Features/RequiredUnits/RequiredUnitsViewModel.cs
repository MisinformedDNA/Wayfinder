using CommunityToolkit.Mvvm.ComponentModel;
using Wayfinder.Services;
using Wayfinder.Services.Journeys;
using Wayfinder.Services.Models;

namespace Wayfinder.App.Features.RequiredUnits;

public partial class RequiredUnitsViewModel : ObservableObject
{
    [ObservableProperty] List<Journey> _selectedJourneys = default!;
    [ObservableProperty] List<Unit> _selectedUnits = default!;
    [ObservableProperty] private List<RequiredUnit> _requirements = default!;
    [ObservableProperty] private List<Journey> _journeys = default!;

    public IReadOnlyList<Unit> AllUnits { get; set; } = default!;
    protected List<Journey> AllJourneys { get; set; } = default!;

    partial void OnSelectedJourneysChanged(List<Journey> value)
    {
        LoadRequiredUnits();
    }

    public virtual async Task InitializeAsync()
    {
        await LoadAllUnitsAsync();
        await LoadAllJourneysAsync();

        Journeys ??= AllJourneys;

        var query = from c in Journeys
                    from r in c.Requirements
                    join u in AllUnits on r.UnitId equals u.BaseId
                    orderby u.Name
                    select u;

        SelectedUnits = query.ToList();

        LoadRequiredUnits();
    }

    protected async Task LoadAllUnitsAsync() => AllUnits = await GameService.GetUnitsAsync();

    protected virtual async Task LoadAllJourneysAsync()
    {
        var resourceManager = Resources.ResourceManager;
        var journeys = await JourneyService.GetAsync();
        AllJourneys = journeys
            .OrderBy(x => resourceManager.GetString(x.Id) ?? x.Id)
            .ToList();
    }

    //internal static async Task<List<Journey>> GetAllJourneysAsync() => await JourneyService.GetAsync();

    public virtual void LoadRequiredUnits()
    {
        var selectedUnitIds = SelectedUnits.Select(x => x.BaseId).ToArray();
        var objectives = AllJourneys.Where(x => selectedUnitIds.Contains(x.Id));
        var requirements = objectives.SelectMany(x => x.Requirements).ToList();

        var unitDict = AllUnits.ToDictionary(x => x.BaseId, x => x.Name);

        var query = from g in objectives
                    from r in g.Requirements
                    select new { GoalUnit = g.Id, RequiredUnit = r.UnitId, RequiredLevel = r.Level } into d
                    group d by d.RequiredUnit into ru
                    select new RequiredUnit(ru.Key, ru.Select(x => new RequiredDetail(x.GoalUnit, x.RequiredLevel)).ToList());

        Requirements = query.ToList();
    }
}

public record RequiredDetail(string GoalUnitId, string Level);

public record RequiredUnit(string UnitId, List<RequiredDetail> Details);