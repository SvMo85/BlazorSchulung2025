using Microsoft.AspNetCore.Components;

namespace BlazorSchulung2025.Components.List
{
    public class IconListItem : IListItem
    {
        private readonly string _text;
        private readonly string _iconName;

        public IconListItem(string text, string iconName)
        {
            _text = text;
            _iconName = iconName;
        }

        public string GetText()
        {
            return _text;
        }

        public RenderFragment GetRenderFragment()
        {
            return builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", $"icon {GetIconName()}");
                builder.CloseElement();
                builder.OpenElement(2, "span");
                builder.AddContent(3, GetText());
                builder.CloseElement();
            };
        }


        public string GetIconName()
        {
            return _iconName;
        }
    }
}
