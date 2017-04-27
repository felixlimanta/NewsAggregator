using System.Collections.Generic;
using System.Diagnostics;

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
                Trace.WriteLine("Searching {0}", article.link.ToString());
                RssMatchObject result = new RssMatchObject
                {
                    article = article,
                    titleFoundIndex = matchString(article.title, keyword),
                    summaryFoundIndex = matchString(article.summary, keyword),
                    contentFoundIndex = matchString(article.content, keyword)
                };
                Trace.WriteLine("Title {0}", result.titleFoundIndex.ToString());
                Trace.WriteLine("Title {0}", result.summaryFoundIndex.ToString());
                Trace.WriteLine("Title {0}", result.contentFoundIndex.ToString());

                if (result.titleFoundIndex != -1 ||
                    result.summaryFoundIndex != -1 ||
                    result.contentFoundIndex != -1)
                {
                    Trace.WriteLine("Found {0}", article.link.ToString());
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
