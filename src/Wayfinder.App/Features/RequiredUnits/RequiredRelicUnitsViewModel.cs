using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using Wayfinder.Services.Challenges;

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
            await LoadChallengesAsync();

            SelectedChallenges = new(Challenges);
        }

        protected async Task LoadChallengesAsync()
        {
            await base.LoadAllChallengesAsync();

            List<Challenge> challenges = new();
            foreach (var challenge in AllChallenges)
            {
                var requirements = challenge.Requirements
                    .Where(x => IsRelicRequirement().IsMatch(x.Level))
                    .ToList();

                if (requirements.Any())
                    challenges.Add(new Challenge(challenge.ChallengeId, requirements));
            }

            Challenges = challenges;
        }

        public override void LoadRequiredUnits()
        {
            var query = from g in SelectedChallenges
                        from r in g.Requirements
                        select new { GoalUnit = g.ChallengeId, RequiredUnit = r.UnitId, RequiredLevel = r.Level } into d
                        group d by d.RequiredUnit into ru
                        select new RequiredUnit(_localizer[ru.Key], ru.Select(x => new RequiredDetail(_localizer[x.GoalUnit], _localizer[x.RequiredLevel])).ToList());

            Requirements = query.ToList();
        }
    }
}
