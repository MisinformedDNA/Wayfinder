using System.Text.Json.Serialization;

namespace Wayfinder.Services.Models;

public record GearLevel(
    [property: JsonPropertyName("tier")] int Tier,
    [property: JsonPropertyName("gear")] IReadOnlyList<string> Gear
);
