using System;
using System.Collections.Generic;
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
        }

        protected void PatternMatch(object sender, EventArgs e)
        {
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
                OutSpan.InnerText = res.article.title;
                OutSpan.InnerHtml = "<br />";
                OutSpan.InnerText = res.article.summary;
                OutSpan.InnerHtml = "<br />";
                OutSpan.InnerHtml = "<br />";
            }
        }
    }
}