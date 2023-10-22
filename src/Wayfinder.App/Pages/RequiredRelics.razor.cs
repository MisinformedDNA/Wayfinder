using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Wayfinder.App.Features.RequiredUnits;
using Wayfinder.Services.Journeys;

namespace Wayfinder.App.Pages
{
    public partial class RequiredRelics
    {
        private Func<Journey, string> _journeyConverter = default!;
        private Func<List<string>, string> _multiSelectTextFunc = default!;
        private RequiredRelicUnitsViewModel _viewModel = default!;

        [Inject] private IStringLocalizer<Resources> Localizer { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _viewModel = ScopedServices.GetRequiredService<RequiredRelicUnitsViewModel>();
            await _viewModel.InitializeAsync();

            _journeyConverter = x => Localizer[x.Id];
            _multiSelectTextFunc = x => x.Count == _viewModel.Journeys.Count
                ? "All"
                : string.Join(", ", x);
        }

        private void OnSelectedJourneysChanged(IEnumerable<Journey> journeys)
        {
            _viewModel.SelectedJourneys = journeys.ToList();
        }
    }
}