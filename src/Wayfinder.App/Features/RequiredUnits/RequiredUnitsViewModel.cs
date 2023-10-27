using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using Wayfinder.Services.Journeys;

namespace Wayfinder.App.Features.RequiredUnits;

public abstract partial class RequiredUnitsViewModel : ObservableObject
{
    private IStringLocalizer<Resources> _localizer;

    public RequiredUnitsViewModel(IStringLocalizer<Resources> localizer)
    {
        _localizer = localizer;
    }

    [ObservableProperty] List<Journey> _selectedJourneys = new();
    [ObservableProperty] private List<RequiredUnit> _requirements = new();
    [ObservableProperty] private List<Journey> _journeys = new();

    partial void OnSelectedJourneysChanged(List<Journey> value)
    {
        LoadRequiredUnits();
    }

    protected abstract Regex IsValidRequirement();

    public async Task InitializeAsync()
    {
        await LoadJourneysAsync();

        SelectedJourneys = new(Journeys);
    }

    protected async Task LoadJourneysAsync()
    {
        var allJourneys = await JourneyService.GetAsync();

        List<Journey> journeys = new();
        foreach (var journey in allJourneys)
        {
            var matchingRequirements = journey.Requirements
                .Where(x => IsValidRequirement().IsMatch(x.Level))
                .ToList();

            if (matchingRequirements.Any())
                journeys.Add(new Journey(journey.Id, matchingRequirements));
        }

        Journeys = journeys
            .OrderBy(x => _localizer[x.Id].Value ?? x.Id)
            .ToList();
    }

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