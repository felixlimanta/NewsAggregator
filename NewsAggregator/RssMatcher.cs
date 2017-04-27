using System.Collections.Generic;

namespace NewsAggregator
{
    class RssMatchObject
    {
        public RssContents article;
        public int titleFoundIndex;
        public int summaryFoundIndex;
        public int contentFoundIndex;
    }

    class RssMatcher
    {
        public string keyword { get; private set; }
        public int type { get; private set; }
        private List<RssContents> articles;
        public List<RssMatchObject> searchResults { get; private set; }

        public RssMatcher(List<RssContents> articles, string keyword, int type)
        {
            this.keyword = keyword;
            this.type = type;
            this.articles = articles;
            searchResults = new List<RssMatchObject>();

            findArticles();
        }

        public void findArticles()
        {
            foreach (RssContents article in articles)
            {
                RssMatchObject result = new RssMatchObject
                {
                    article = article,
                    titleFoundIndex = matchString(article.title, keyword),
                    summaryFoundIndex = matchString(article.summary, keyword),
                    contentFoundIndex = matchString(article.content, keyword)
                };

                if (result.titleFoundIndex * result.contentFoundIndex * result.summaryFoundIndex >= 0)
                {
                    searchResults.Add(result);
                }
            }
        }

        public int matchString(string text, string keyword)
        {
            switch (type)
            {
                case 1:
                    return (new KMPMatcher(text, keyword)).matchIndex;
                case 2:
                    return (new BMMatcher(text, keyword)).matchIndex;
                case 3:
                    return (new RegexMatcher(text, keyword)).matchIndex;
                default:
                    return -1;
            }
        }
    }
}
