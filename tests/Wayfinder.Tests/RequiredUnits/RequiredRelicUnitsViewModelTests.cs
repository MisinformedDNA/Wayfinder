using Microsoft.Extensions.Localization;
using Wayfinder.App;
using Wayfinder.App.Features.RequiredUnits;

namespace Wayfinder.Tests.RequiredUnits
{
    public class RequiredRelicUnitsViewModelTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly RequiredRelicUnitsViewModel _viewModel;

        public RequiredRelicUnitsViewModelTests()
        {
            var localizerMock = Substitute.For<IStringLocalizer<Resources>>();
            localizerMock[Arg.Any<string>()].Returns(new LocalizedString("", _fixture.Create<string>()));

            _viewModel = new RequiredRelicUnitsViewModel(localizerMock);
        }

        [Fact]
        public async Task Challenges_are_loaded_by_default()
        {
            await _viewModel.InitializeAsync();

            _viewModel.Challenges.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task All_units_are_loaded_by_default()
        {
            await _viewModel.InitializeAsync();

            _viewModel.AllUnits.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task Multiple_challenges_are_selected_by_default()
        {
            await _viewModel.InitializeAsync();

            _viewModel.SelectedChallenges.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task Multiple_requirements_are_set_by_default()
        {
            await _viewModel.InitializeAsync();

            _viewModel.Requirements.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task Challenges_without_relic_requirements_are_not_included()
        {
            await _viewModel.InitializeAsync();

            _viewModel.Challenges.Should().NotContain(x => x.ChallengeId.Contains("CALKESTIS"));
        }

        [Fact]
        public async Task Ship_requirements_are_not_included()
        {
            await _viewModel.InitializeAsync();

            _viewModel.Requirements.Should().NotContain(x => x.UnitId.Contains("OUTRIDER"));
            _viewModel.Requirements.Should().NotContain(x => x.UnitId.Contains("LEVIATHAN"));
        }

        [Fact]
        public async Task Changing_selected_challenges_does_not_affect_challenges()
        {
            await _viewModel.InitializeAsync();

            _viewModel.Challenges.Should().HaveCountGreaterThan(1);
            _viewModel.SelectedChallenges.Clear();

            _viewModel.Challenges.Should().NotBeEmpty();
        }

        [Fact]
        public async Task If_no_challenges_are_selected_no_requirements_are_included()
        {
            await _viewModel.InitializeAsync();

            _viewModel.SelectedChallenges.Clear();
            _viewModel.LoadRequiredUnits();

            _viewModel.Requirements.Should().BeEmpty();
        }

        [Fact]
        public async Task Requirements_are_returned_in_alphabetical_order()
        {
            await _viewModel.InitializeAsync();

            _viewModel.Requirements.Should().BeInAscendingOrder(x => x.UnitId);
        }
    }
}
