using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Wayfinder.App.Features.RequiredUnits;
using Wayfinder.Services.Challenges;

namespace Wayfinder.App.Pages
{
    public partial class RequiredRelics
    {
        private Func<Challenge, string> _challengeConverter = default!;
        private RequiredRelicUnitsViewModel _viewModel = default!;

        [Inject] private IStringLocalizer<Localization> Localizer { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _viewModel = ScopedServices.GetRequiredService<RequiredRelicUnitsViewModel>();
            await _viewModel.InitializeAsync();

            _challengeConverter = x => Localizer[x.ChallengeId];
        }

        private void OnSelectedChallengesChanged(IEnumerable<Challenge> challenges)
        {
            _viewModel.SelectedChallenges = challenges.ToList();
        }
    }
}