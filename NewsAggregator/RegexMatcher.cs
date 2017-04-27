using System.Text.RegularExpressions;

namespace NewsAggregator
{
    public class RegexMatcher : StringMatcher
    {
        private Match match;

        public RegexMatcher(string text, string keyword) : base(text, keyword)
        {
            matchIndex = findMatch();
        }

        private int findMatch()
        {
            match = Regex.Match(text, keyword);
            keyword = match.Value;
            return match.Index;
        }
    }
}