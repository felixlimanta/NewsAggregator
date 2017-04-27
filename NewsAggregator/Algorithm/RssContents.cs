using System;

namespace NewsAggregator
{
    class RssContents
    {
        public string title;
        public Uri link;
        public Uri image;
        public string summary;
        public DateTimeOffset publishDate;
        public string content;
    }
}
