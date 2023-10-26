using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using Wayfinder.Services;
using Wayfinder.Services.Journeys;
using Wayfinder.Services.Models;

namespace Wayfinder.App.Features.RequiredUnits;

public abstract partial class RequiredUnitsViewModel : ObservableObject
{
    private IStringLocalizer<Resources> _localizer;

    public RequiredUnitsViewModel(IStringLocalizer<Resources> localizer)
    {
        _localizer = localizer;
    }

    [ObservableProperty] List<Journey> _selectedJourneys = new();
    [ObservableProperty] List<Unit> _selectedUnits = new();
    [ObservableProperty] private List<RequiredUnit> _requirements = new();
    [ObservableProperty] private List<Journey> _journeys = new();

    public IReadOnlyList<Unit> AllUnits { get; set; } = default!;
    protected List<Journey> AllJourneys { get; set; } = new();

    partial void OnSelectedJourneysChanged(List<Journey> value)
    {
        LoadRequiredUnits();
    }

    protected abstract Regex IsValidRequirement();

    public async Task InitializeAsync()
    {
        await LoadAllUnitsAsync();
        await LoadJourneysAsync();

        SelectedJourneys = new(Journeys);
    }

    protected async Task LoadJourneysAsync()
    {
        await LoadAllJourneysAsync();

        List<Journey> journeys = new();
        foreach (var journey in AllJourneys)
        {
            var requirements = journey.Requirements
                .Where(x => IsValidRequirement().IsMatch(x.Level))
                .ToList();

            if (requirements.Any())
                journeys.Add(new Journey(journey.Id, requirements));
        }

        Journeys = journeys;
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

    public void LoadRequiredUnits()
    {
        var query = from g in SelectedJourneys
                    from r in g.Requirements
                    select new { GoalUnit = g.Id, RequiredUnit = r.UnitId, RequiredLevel = r.Level } into d
                    group d by d.RequiredUnit into ru
                    select new RequiredUnit(_localizer[ru.Key], ru.Select(x => new RequiredDetail(_localizer[x.GoalUnit], _localizer[x.RequiredLevel])).ToList());

        Requirements = query.ToList();
    }
}

public record RequiredDetail(string GoalUnitId, string Level);

public record RequiredUnit(string UnitId, List<RequiredDetail> Details);