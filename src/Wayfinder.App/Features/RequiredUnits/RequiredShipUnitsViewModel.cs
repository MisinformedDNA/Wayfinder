using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace Wayfinder.App.Features.RequiredUnits
{
    public partial class RequiredShipUnitsViewModel : RequiredUnitsViewModel
    {
        [GeneratedRegex("^STAR")]
        protected override partial Regex IsValidRequirement();

        public RequiredShipUnitsViewModel(IStringLocalizer<Resources> localizer)
            : base(localizer)
        { }
    }
}
