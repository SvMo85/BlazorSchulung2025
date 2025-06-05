using Blazorise;
using Microsoft.AspNetCore.Components;

namespace BlazorSchulung2025.Components.Helper
{
    public partial class SourceCode : ComponentBase
    {
        [Parameter] public string Code { get; set; } = string.Empty;

        private IconName _icon = IconName.Copy;
        private string DisplayCode { get; set; } = string.Empty;
        protected override void OnParametersSet()
        {
            var trimmed = Code.Trim();
            base.OnParametersSet();
            DisplayCode = trimmed;
        }
    }
}
