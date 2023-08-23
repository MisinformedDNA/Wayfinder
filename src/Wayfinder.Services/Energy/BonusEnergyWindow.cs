namespace Wayfinder.Services.Energy;

public record BonusEnergyWindow(TimeOnly StartTime, TimeOnly EndTime, DateOnly? Date = null)
{
    public DateTime StartAt()
    {
        if (!Date.HasValue) throw new InvalidOperationException($"'{nameof(Date)}' must be set to use this method.");

        var date = Date.Value;
        return new(date.Year, date.Month, date.Day, StartTime.Hour, StartTime.Minute, 0);
    }

    public DateTime EndAt()
    {
        if (!Date.HasValue) throw new InvalidOperationException($"'{nameof(Date)}' must be set to use this method.");

        var date = Date.Value;
        return new(date.Year, date.Month, date.Day, EndTime.Hour, EndTime.Minute, 0);
    }
}
