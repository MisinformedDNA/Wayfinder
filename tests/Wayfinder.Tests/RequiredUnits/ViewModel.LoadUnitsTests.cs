using FluentAssertions;
using Wayfinder.App.Features.RequiredUnits;

namespace Wayfinder.Tests.RequiredUnits
{
    public class LoadUnitsTests
    {
        private readonly RequiredUnitsViewModel _viewModel = new();

        [Fact]
        public async Task List_all_units_for_one_character()
        {
            await _viewModel.InitializeAsync();

            var selectedUnits = _viewModel.AllUnits.Where(x => x.BaseId == "LORDVADER");
            _viewModel.SelectedUnits = selectedUnits.ToList();

            _viewModel.LoadRequiredUnits();

            _viewModel.Units.Should().HaveCount(15);
        }

        [Fact]
        public async Task All_goal_units_are_selected_by_default()
        {
            await _viewModel.InitializeAsync();

            _viewModel.SelectedUnits.Should().HaveSameCount(_viewModel.AllObjectives);
        }

        [Fact]
        public async Task All_goal_units_have_at_least_five_requirements()
        {
            await _viewModel.InitializeAsync();

            var selectedUnitIds = new[] { "JABBATHEHUTT", "GRANDMASTERLUKE" };
            var selectedUnits = _viewModel.AllUnits.Where(x => selectedUnitIds.Contains(x.BaseId));
            _viewModel.SelectedUnits = selectedUnits.ToList();

            _viewModel.LoadRequiredUnits();

            _viewModel.Units.Should().HaveCountGreaterThan(15).And.HaveCountLessThan(30);
        }

        [Fact]
        public async Task Both_have_requirements_for_CLS()
        {
            await _viewModel.InitializeAsync();

            var selectedUnitIds = new[] { "JABBATHEHUTT", "GRANDMASTERLUKE" };
            var selectedUnits = _viewModel.AllUnits.Where(x => selectedUnitIds.Contains(x.BaseId));
            _viewModel.SelectedUnits = selectedUnits.ToList();

            _viewModel.LoadRequiredUnits();

            var unit = _viewModel.Units.Should().Contain(x => x.UnitId == "JEDIKNIGHTLUKE").Subject;
            unit.Details.Should().HaveCount(2);
        }
    }
}
