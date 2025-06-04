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


        public string GetIconName()
        {
            return _iconName;
        }
    }
}
