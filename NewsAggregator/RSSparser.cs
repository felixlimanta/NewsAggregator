using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using ReadSharp;
using System.Xml;
using System.Text.RegularExpressions;

namespace NewsAggregator
{
    class RssParser
    {
        public string source { get; private set; }
        private int limit = int.MaxValue;
        private Reader reader;
        public List<RssContents> articles { get; private set; }

        public RssParser(string source): this(source, int.MaxValue)
        {
            
        }

        public RssParser(string source, int limit)
        {
            this.source = source;
            this.limit = limit;
            articles = new List<RssContents>();
            reader = new Reader();            

            parseRss();
            articles.Sort((a, b) => b.publishDate.CompareTo(a.publishDate));
        }

        public void parseRss()
        {
            using (XmlReader xmlReader = XmlReader.Create(source))
            {
                try
                {
                    // Parse XML Data
                    SyndicationFeed feed = SyndicationFeed.Load(xmlReader);

                    // Process all parsed items
                    int n = 0;
                    foreach (SyndicationItem item in feed.Items)
                    {
                        if (n > limit)
                            break;

                        Article article = reader.Read(item.Links[0].Uri).Result;
                        if (article.ContentExtracted == false)
                        {
                            article.PlainContent =
                                Regex.Replace(
                                    Regex.Replace(article.Content, "<[^>]*(>|$)", string.Empty),
                                    "[ \t\r\n]+",
                                    " ").Trim();
                        }

                        articles.Add(new RssContents
                        {
                            title = article.Title.Trim(),
                            link = item.Links[0].Uri,
                            image = article.FrontImage,
                            summary = article.Description.Trim(),
                            publishDate = item.PublishDate,
                            content = article.PlainContent
                        });
                        n++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot read RSS from ", source);
                }
            }
        }
    }
}
