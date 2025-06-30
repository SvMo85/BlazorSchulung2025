using Microsoft.AspNetCore.Components;

namespace BlazorSchulung2025.Components.List
{
    public class TextListItem : IListItem
    {
        private readonly string _text;

        public TextListItem(string text)
        {
            _text = text;
        }

        public string GetText()
        {
            return _text;
        }

        public RenderFragment GetRenderFragment()
        {
            return builder =>
            {
                builder.OpenElement(0, "p");
                builder.OpenElement(0, "span");
                builder.AddContent(1, GetText());
                builder.CloseElement();
                builder.CloseElement();
            };
        }
    }
}
