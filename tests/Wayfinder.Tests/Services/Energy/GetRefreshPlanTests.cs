using FluentAssertions;
using FluentAssertions.Execution;
using Wayfinder.Services.Energy;

namespace Wayfinder.Tests.Services.Energy;

public class GetRefreshPlanTests
{
    [Fact]
    public void Throw_exception_if_end_is_before_start()
    {
        var startTime = DateTime.Today;
        var startEnergy = 0;
        var endTime = startTime.AddHours(-1);
        var endEnergy = 0;

        Action action = () => EnergyService.GetEnergyPlan(EnergyType.Normal, startTime, startEnergy, endTime, endEnergy);
        action.Should().Throw<ArgumentException>();
    }


    [Theory]
    [InlineData(134)]
    [InlineData(124)]
    public void Do_nothing_to_reach_max_energy(int startEnergy)
    {
        var startTime = DateTime.Today;
        var endTime = startTime.AddHours(1);
        var endEnergy = 144;

        var plan = EnergyService.GetEnergyPlan(EnergyType.Normal, startTime, startEnergy, endTime, endEnergy);

        using var _ = new AssertionScope();
        plan.Steps.Should().HaveCount(1);
        plan.Steps.First().Should().BeOfType<DoNothing>();
    }

    [Fact]
    public void Spend_to_reach_max_energy()
    {
        var startTime = DateTime.Today;
        var endTime = startTime.AddHours(1);
        var startEnergy = 135;
        var endEnergy = 144;
        var expectedSpend = 1;  // 135 + 10 (natural energy) - 144


        var plan = EnergyService.GetEnergyPlan(EnergyType.Normal, startTime, startEnergy, endTime, endEnergy);

        // Assert
        using var _ = new AssertionScope();
        plan.Steps.Should().HaveCount(1);

        var step = plan.Steps.First();
        var spendStep = step.Should().BeOfType<SpendEnergy>().Subject;
        spendStep.Energy.Should().Be(expectedSpend);
        spendStep.SpendAt.Should().BeCloseTo(startTime, EnergyType.Normal.RefreshRate);
        spendStep.EndEnergy.Should().Be(startEnergy - expectedSpend);
    }

    [Fact]
    public void Claim_bonus_energy_to_reach_max_energy()
    {
        var endTime = DateTime.Today;
        var startTime = endTime.AddHours(-3);
        var startEnergy = 69;  // 144 - 45 - 30
        var endEnergy = 144;

        var plan = EnergyService.GetEnergyPlan(EnergyType.Normal, startTime, startEnergy, endTime, endEnergy);

        // Assert
        using var _ = new AssertionScope();
        var step = plan.Steps.Should().HaveCount(1).And.Subject.First();
        var collectStep = step.Should().BeOfType<CollectBonusEnergy>().Subject;
        collectStep.CollectAt.TimeOfDay.Should().Be(TimeSpan.FromHours(21));
        collectStep.EndEnergy.Should().Be(startEnergy + EnergyType.Normal.BonusEnergy);
    }

    [Fact]
    public void Spend_to_reach_max_energy_with_bonus_energy()
    {
        var endTime = DateTime.Today;
        var startTime = endTime.AddHours(-3).AddTicks(1);
        var startEnergy = 79;
        var endEnergy = 144;
        var expectedSpend = 10;

        var plan = EnergyService.GetEnergyPlan(EnergyType.Normal, startTime, startEnergy, endTime, endEnergy);

        // Assert
        using var _ = new AssertionScope();
        plan.Steps.Should().HaveCount(2);

        var spendStep = plan.Steps.First().Should().BeOfType<SpendEnergy>().Subject;
        spendStep.Energy.Should().Be(expectedSpend);

        var collectStep = plan.Steps.ElementAt(1).Should().BeOfType<CollectBonusEnergy>().Subject;
        collectStep.CollectAt.Should().BeCloseTo(startTime, TimeSpan.FromMicroseconds(1));
        collectStep.EndEnergy.Should().Be(startEnergy - expectedSpend + EnergyType.Normal.BonusEnergy);
    }

    [Fact]
    public void Spend_and_collect_energy_at_the_same_time()
    {
        var endTime = DateTime.Today;
        var startTime = endTime.AddHours(-3);
        var startEnergy = 79;
        var endEnergy = 144;
        var expectedSpend = 10;

        var plan = EnergyService.GetEnergyPlan(EnergyType.Normal, startTime, startEnergy, endTime, endEnergy);

        // Assert
        using var _ = new AssertionScope();
        plan.Steps.Should().HaveCount(2);

        var spendStep = plan.Steps.First().Should().BeOfType<SpendEnergy>().Subject;
        spendStep.Energy.Should().Be(expectedSpend);

        var collectStep = plan.Steps.ElementAt(1).Should().BeOfType<CollectBonusEnergy>().Subject;
        collectStep.CollectAt.Should().Be(startTime);
        collectStep.EndEnergy.Should().Be(startEnergy - expectedSpend + EnergyType.Normal.BonusEnergy);
    }


    [Fact]
    public void Replenish_energy_from_day_to_day()
    {
        var endTime = DateTime.Today;
        var startTime = endTime.AddDays(-1);

        var startEnergy = 144;
        var endEnergy = 144;

        var plan = EnergyService.GetEnergyPlan(EnergyType.Normal, startTime, startEnergy, endTime, endEnergy);

        // Assert
        plan.Steps.Should().HaveCount(3);

        using (var _ = new AssertionScope())
        {
            var collectStep = plan.Steps.ElementAt(2).Should().BeOfType<CollectBonusEnergy>().Subject;
            collectStep.CollectAt.TimeOfDay.Should().Be(TimeSpan.FromHours(21));
            collectStep.EndEnergy.Should().Be(114);
        }

        using (var _ = new AssertionScope())
        {
            var spendStep = plan.Steps.ElementAt(1).Should().BeOfType<SpendEnergy>().Subject;
            spendStep.SpendAt.TimeOfDay.Should().Be(TimeSpan.FromHours(18));
            spendStep.Energy.Should().Be(6);
            spendStep.EndEnergy.Should().Be(39);
        }

        using (var _ = new AssertionScope())
        {
            var collectStep = plan.Steps.ElementAt(0).Should().BeOfType<CollectBonusEnergy>().Subject;
            collectStep.CollectAt.TimeOfDay.Should().Be(TimeSpan.FromHours(18));
            collectStep.EndEnergy.Should().Be(45);
        }
    }
}
