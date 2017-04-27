using System;
using System.Xml;
using System.IO;
using System.ServiceModel.Syndication;
using System.Net;
using HtmlAgilityPack;

namespace NewsAggregator
{
    struct rssFeed
    {
        public string url;
        public string article_block;
    }

    struct rssContents
    {
        public string title;
        public string link;
        public string summary;
        public string publishDate;
        public string contents;
    }

    class RssParser
    {
        static rssContents[] parseRSS(rssFeed source)
        {
            // Read XML Data
            rssContents[] feed_items = new rssContents[20];
            using (XmlReader reader = XmlReader.Create(source.url))
            {
                int i = 0;

                // Parse XML Data
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                // Process all parsed items
                foreach (SyndicationItem item in feed.Items)
                {
                    if (i >= 20)
                        break;

                    // Read parsed XML Data
                    feed_items[i].title = item.Title.Text;
                    feed_items[i].summary = item.Summary.Text;
                    feed_items[i].link = item.Links[0].Uri.ToString();
                    feed_items[i].publishDate = item.PublishDate.ToString();

                    // Check link validity
                    if (feed_items[i].link.Contains("http"))
                    {
                        string result;
                        using (var client = new WebClient())
                        {
                            result = client.DownloadString(feed_items[i].link);
                        }

                        HtmlDocument doc_to_parse = new HtmlDocument();
                        doc_to_parse.LoadHtml(result);
                        HtmlNode form_node = doc_to_parse.DocumentNode.SelectSingleNode(source.article_block);

                        if (form_node != null)
                        {
                            feed_items[i].contents = form_node.InnerText;
                        }
                        else
                        {
                            feed_items[i].contents = "<No content>";
                        }
                    }
                    else
                    {
                        feed_items[i].contents = "Invalid Feed URL";
                    }
                    Console.WriteLine(feed_items[i].link + ": " + feed_items[i].title);
                    i++;
                }
            }

            return feed_items;
        }

        static void Main()
        {
            //rss source and news content sections
            rssFeed[] sources = new rssFeed[]
            {
                new rssFeed {url = "http://rss.detik.com/index.php/detikcom", article_block = "//div[@class='detail_text']"},
                new rssFeed {url = "http://rss.vivanews.com/get/all", article_block = "//span[@itemprop='description']"},
                new rssFeed {url = "http://www.antaranews.com/rss/terkini", article_block = "//div[@id='content_news']"}
            };

            //variables
            int i = 0;
            string[] to_write = new string[100];

            foreach (rssFeed source in sources)
            {
                rssContents[] feedItems = parseRSS(source);
                foreach (rssContents feedItem in feedItems)
                {
                    to_write[i++] = feedItem.title + "\n" +
                                    feedItem.summary + "\n" +
                                    feedItem.link + "\n" +
                                    feedItem.publishDate + "\n\n" +
                                    feedItem.contents + "\n===============================\n";
                }
            }

            //write all gathered data
            File.WriteAllLines(@"E:\out.txt", to_write);
            Console.WriteLine("main RSS successfully read. Press any key to continue.");
            Console.ReadLine();
        }
    }
}