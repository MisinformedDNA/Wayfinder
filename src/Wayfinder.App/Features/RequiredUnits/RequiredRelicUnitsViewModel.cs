using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using Wayfinder.Services.Journeys;

namespace Wayfinder.App.Features.RequiredUnits
{
    public partial class RequiredRelicUnitsViewModel : RequiredUnitsViewModel
    {
        private readonly IStringLocalizer<Resources> _localizer;

        [GeneratedRegex("^RELIC")]
        private static partial Regex IsRelicRequirement();

        public RequiredRelicUnitsViewModel(IStringLocalizer<Resources> localizer)
        {
            _localizer = localizer;
        }

        public override async Task InitializeAsync()
        {
            await LoadAllUnitsAsync();
            await LoadJourneysAsync();

            SelectedJourneys = new(Journeys);
        }

        protected async Task LoadJourneysAsync()
        {
            await base.LoadAllJourneysAsync();

            List<Journey> journeys = new();
            foreach (var journey in AllJourneys)
            {
                var requirements = journey.Requirements
                    .Where(x => IsRelicRequirement().IsMatch(x.Level))
                    .ToList();

                if (requirements.Any())
                    journeys.Add(new Journey(journey.Id, requirements));
            }

            Journeys = journeys;
        }

        public override void LoadRequiredUnits()
        {
            var query = from g in SelectedJourneys
                        from r in g.Requirements
                        select new { GoalUnit = g.Id, RequiredUnit = r.UnitId, RequiredLevel = r.Level } into d
                        group d by d.RequiredUnit into ru
                        select new RequiredUnit(_localizer[ru.Key], ru.Select(x => new RequiredDetail(_localizer[x.GoalUnit], _localizer[x.RequiredLevel])).ToList());

            Requirements = query.ToList();
        }
    }
}
