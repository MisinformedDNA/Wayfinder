using FluentAssertions;
using Wayfinder.Services.Energy;

namespace Wayfinder.Tests.Services.Energy
{
    public class GetBonusWindowsTests
    {
        [Fact]
        public void End_date_is_before_the_start_date()
        {
            var startTime = DateTime.Today;
            var endTime = DateTime.Today.AddMinutes(-1);

            Action action = () => EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void No_refreshes_found()
        {
            var startTime = DateTime.Today;
            var endTime = DateTime.Today.AddMinutes(1);

            var windows = EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            windows.Should().HaveCount(0);
        }

        [Fact]
        public void First_normal_refresh_found()
        {
            var startTime = DateTime.Today.AddHours(12);
            var endTime = startTime.AddHours(1);

            var windows = EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            // Assert
            windows.Should().HaveCount(1);
            var window = windows.First();
            window.Date.Should().NotBeNull().And.Be(DateOnly.FromDateTime(startTime));
            window.StartTime.Hour.Should().Be(12);
            window.EndTime.Hour.Should().Be(14);
        }

        [Fact]
        public void Second_normal_refresh_found()
        {
            var startTime = DateTime.Today.AddHours(18);
            var endTime = startTime.AddHours(1);

            var windows = EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            windows.Should().HaveCount(1);
            var window = windows.First();
            window.Date.Should().NotBeNull().And.Be(DateOnly.FromDateTime(startTime));
            window.StartTime.Hour.Should().Be(18);
            window.EndTime.Hour.Should().Be(20);
        }

        [Fact]
        public void Third_normal_refresh_found()
        {
            var startTime = DateTime.Today.AddHours(21);
            var endTime = startTime.AddHours(1);

            var windows = EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            windows.Should().HaveCount(1);
            var window = windows.First();
            window.Date.Should().NotBeNull().And.Be(DateOnly.FromDateTime(startTime));
            window.StartTime.Hour.Should().Be(21);
            window.EndTime.Hour.Should().Be(23);
        }

        [Fact]
        public void All_refreshes_found_over_multiple_days()
        {
            var startTime = DateTime.Today;
            var endTime = startTime.AddDays(2);

            var windows = EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            windows.Should().HaveCount(6);
        }

        [Fact]
        public void All_refreshes_found_over_multiple_days_starting_after_first_refresh()
        {
            var startTime = DateTime.Today.AddHours(15);
            var endTime = startTime.AddDays(2);

            var windows = EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            windows.Should().HaveCount(6);
            var firstWindow = windows.First();
            firstWindow.Date.Should().NotBeNull().And.Be(DateOnly.FromDateTime(startTime));
            firstWindow.StartTime.Hour.Should().Be(18);
            firstWindow.EndTime.Hour.Should().Be(20);

            var lastWindow = windows.Last();
            lastWindow.Date.Should().NotBeNull().And.Be(DateOnly.FromDateTime(endTime));
            lastWindow.StartTime.Hour.Should().Be(12);
            lastWindow.EndTime.Hour.Should().Be(14);
        }

        [Fact]
        public void Refresh_found_in_small_window_of_time()
        {
            var startTime = DateTime.Today.AddHours(12).AddMinutes(1);
            var endTime = startTime.AddMinutes(1);

            var windows = EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            windows.Should().HaveCount(1);
        }

        [Fact]
        public void Refresh_is_exclusive_at_upper_end()
        {
            var startTime = DateTime.Today.AddHours(14);
            var endTime = startTime.AddMinutes(1);

            var windows = EnergyService.GetBonusWindows(EnergyType.Normal, startTime, endTime);

            windows.Should().HaveCount(0);
        }
    }
}
