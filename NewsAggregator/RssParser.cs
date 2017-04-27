using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using ReadSharp;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NewsAggregator
{
    class RssParser
    {
        public string source { get; private set; }
        private int limit = int.MaxValue;
        private Reader reader;
        public List<RssContents> articles { get; private set; }

        public RssParser(string source) : this(source, int.MaxValue)
        {

        }

        public RssParser(string source, int limit)
        {
            this.source = source;
            this.limit = limit;
            articles = new List<RssContents>();
            reader = new Reader();

            parseRss();
        }

        public void parseRss()
        {
            List<Task> tasks = new List<Task>();
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
                        parsePage(item);

                        Trace.WriteLine(item.Links[0].Uri.ToString());
                        n++;
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Cannot read RSS from " + source);
                }
            }
        }

        public void parsePage(SyndicationItem item)
        {
            Trace.WriteLine("Parsing " + item.Links[0].Uri.ToString());
            try
            {
                var t = new NReadability.NReadabilityWebTranscoder();
                bool b;
                string page = t.Transcode(item.Links[0].Uri.ToString(), out b);

                if (b)
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(page);

                    string title = doc.DocumentNode.SelectSingleNode("//title").InnerText;
                    string imgUrl = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']").Attributes["content"].Value;
                    string summary = Regex.Replace(
                                        Regex.Replace(item.Summary.Text, "<[^>]*(>|$)", string.Empty),
                                        "[ \t\r\n]+",
                                        " ").Trim();
                    string mainText = doc.DocumentNode.SelectSingleNode("//div[@id='readInner']").InnerText;

                    articles.Add(new RssContents
                    {
                        title = title,
                        link = item.Links[0].Uri,
                        image = new Uri(imgUrl),
                        summary = summary,
                        publishDate = item.PublishDate,
                        content = mainText
                    });
                    Trace.WriteLine(item.Links[0].Uri.ToString() + " parsed");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Cannot parse " + item.Links[0].Uri.ToString());
            }
        }
    }
}
