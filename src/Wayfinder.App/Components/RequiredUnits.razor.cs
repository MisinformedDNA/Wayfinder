using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Wayfinder.App.Features.RequiredUnits;
using Wayfinder.Services.Journeys;

namespace Wayfinder.App.Components
{
    public partial class RequiredUnits
    {
        private Func<Journey, string> _journeyConverter = default!;
        private Func<List<string>, string> _multiSelectTextFunc = default!;

        [Parameter, EditorRequired] public string Title { get; set; } = default!;
        [Parameter, EditorRequired] public RequiredUnitsViewModel ViewModel { get; set; } = default!;

        [Inject] private IStringLocalizer<Resources> Localizer { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await ViewModel.InitializeAsync();

            _journeyConverter = x => Localizer[x.Id];
            _multiSelectTextFunc = x => x.Count == ViewModel.Journeys.Count
                ? "All"
                : string.Join(", ", x);
        }

        private void OnSelectedJourneysChanged(IEnumerable<Journey> journeys)
        {
            ViewModel.SelectedJourneys = journeys.ToList();
        }
    }
}