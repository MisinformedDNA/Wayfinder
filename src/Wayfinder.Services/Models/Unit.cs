using System.Text.Json.Serialization;

namespace Wayfinder.Services.Models;

public record Unit(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("base_id")] string BaseId,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("image")] string Image,
    [property: JsonPropertyName("power")] int Power,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("combat_type")] int UnitType,
    [property: JsonPropertyName("gear_levels")] IReadOnlyList<GearLevel> GearLevels,
    [property: JsonPropertyName("alignment")] int Alignment,
    [property: JsonPropertyName("categories")] IReadOnlyList<string> Categories,
    [property: JsonPropertyName("ability_classes")] IReadOnlyList<string> AbilityClasses,
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("ship_base_id")] object ShipBaseId,
    [property: JsonPropertyName("ship_slot")] object ShipSlot,
    [property: JsonPropertyName("activate_shard_count")] int ActivateShardCount,
    [property: JsonPropertyName("is_capital_ship")] bool IsCapitalShip,
    [property: JsonPropertyName("is_galactic_legend")] bool IsGalacticLegend,
    [property: JsonPropertyName("made_available_on")] string MadeAvailableOn,
    [property: JsonPropertyName("crew_base_ids")] IReadOnlyList<object> CrewBaseIds,
    [property: JsonPropertyName("omicron_ability_ids")] IReadOnlyList<string> OmicronAbilityIds,
    [property: JsonPropertyName("zeta_ability_ids")] IReadOnlyList<string> ZetaAbilityIds
);

internal record UnitRoot(
    [property: JsonPropertyName("data")] IReadOnlyList<Unit> Units,
    [property: JsonPropertyName("message")] object Message
);
