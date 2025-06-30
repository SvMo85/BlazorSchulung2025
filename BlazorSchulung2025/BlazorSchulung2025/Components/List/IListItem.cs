using Microsoft.AspNetCore.Components;

namespace BlazorSchulung2025.Components.List
{
    public interface IListItem
    {
        string GetText();
        RenderFragment GetRenderFragment();
    }
}
