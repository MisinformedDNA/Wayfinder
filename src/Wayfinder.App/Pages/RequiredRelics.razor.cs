using Wayfinder.Services.RequiredUnits;

namespace Wayfinder.App.Pages
{
    public partial class RequiredRelics
    {
        private readonly RequiredRelicUnitsViewModel _viewModel = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await _viewModel.InitializeAsync();
        }
    }
}