namespace NewsAggregator
{
    public abstract class StringMatcher
    {
        public string text { get; protected set; }
        public string keyword { get; protected set; }
        public int matchIndex { get; protected set; }

        public StringMatcher(string text, string keyword)
        {
            this.text = text;
            this.keyword = keyword;
        }
    }
}