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
    }
}
