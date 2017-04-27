using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NewsAggregator
{
    public partial class _Default : Page
    {
        private List<RssParser> rssParsers;

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] sources =
            {
                "http://rss.detik.com/index.php/detikcom",
                "http://rss.vivanews.com/get/all",
                "http://www.antaranews.com/rss/terkini",
                "https://rss.tempo.co/index.php/teco/news/feed/start/0/"
            };

            rssParsers = new List<RssParser>();
            foreach (string source in sources)
            {
                RssParser rss = new RssParser(source, 20);
                rssParsers.Add(rss);
            }

            List<string> toWrite = new List<String>();
            foreach (RssParser rss in rssParsers)
            {
                foreach (RssContents article in rss.articles)
                {
                    string temp =
                        article.title + "\n" +
                        article.link + "\n" +
                        article.image + "\n" +
                        article.summary + "\n" +
                        article.publishDate + "\n" +
                        article.content + "\n";
                    toWrite.Add(temp);
                }
            }
        }

        protected void PatternMatch(object sender, EventArgs e)
        {
            OutSpan.InnerHtml = "";
            string s = key.Text;
            int type;
            if (b.Checked)
            {
                type = 2;
            }
            else if (k.Checked)
            {
                type = 1;
            }
            else
            {
                type = 3;
            }

            List<RssMatchObject> result = new List<RssMatchObject>();
            foreach (RssParser rssParser in rssParsers)
            {                                
                RssMatcher rm = new RssMatcher(rssParser.articles, s, type);
                result.AddRange(rm.searchResults);
            }

            foreach (RssMatchObject res in result)
            {
                OutSpan.InnerHtml += res.article.title;
                OutSpan.InnerHtml += "<br />";
                OutSpan.InnerHtml += res.article.summary;
                OutSpan.InnerHtml += "<br />";
                OutSpan.InnerHtml += "<br />";
            }
        }
    }
}