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

            /*List<string> toWrite = new List<String>();
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

            File.WriteAllLines(@"E:\articles.txt", toWrite.ToArray());*/
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
                OutSpan.InnerHtml += "<img src=\"";
                OutSpan.InnerHtml += res.article.image.ToString();
                OutSpan.InnerHtml += "\" border=\"0\" height=\"100\" width=\"150\">";
                OutSpan.InnerHtml += "<br /> <a href=\"";
                OutSpan.InnerHtml += OutSpan.InnerHtml += res.article.link.ToString();
                OutSpan.InnerHtml += "\">";
                OutSpan.InnerHtml += res.article.title;
                OutSpan.InnerHtml += "</a ><br /> <i>";
                int i = 0;
                if (res.summaryFoundIndex == -1)
                {
                    OutSpan.InnerHtml += res.article.summary;
                }
                else
                {
                    while (i != res.summaryFoundIndex)
                    {
                        OutSpan.InnerHtml += res.article.summary[i];
                        i++;
                    }
                    OutSpan.InnerHtml += "<u>";
                    int j = 1;
                    while (j <= s.Length)
                    {
                        OutSpan.InnerHtml += res.article.summary[i];
                        i++;
                        j++;
                    }
                    OutSpan.InnerHtml += "</u>";
                    while (i < res.article.summary.Length)
                    {
                        OutSpan.InnerHtml += res.article.summary[i];
                        i++;
                    }
                }
                OutSpan.InnerHtml += res.article.summary;
                OutSpan.InnerHtml += "</i> <br />";
                if (res.contentFoundIndex == -1)
                {
                    OutSpan.InnerHtml += res.article.content;
                }
                else
                {
                    i = res.contentFoundIndex;
                    int j;
                    if (i > 10)
                    {
                        j = i - 10;
                        OutSpan.InnerHtml += "..";
                        while (j != i)
                        {
                            OutSpan.InnerHtml += res.article.content[j];
                            j++;
                        }
                    }
                    OutSpan.InnerHtml += "<u>";
                    j = 1;
                    while (j <= s.Length)
                    {
                        OutSpan.InnerHtml += res.article.content[i];
                        i++;
                        j++;
                    }
                    OutSpan.InnerHtml += "</u>";
                    j = 0;
                    while (j < 10 && i < res.article.content.Length)
                    {
                        OutSpan.InnerHtml += res.article.content[i];
                        i++;
                        j++;
                    }
                    OutSpan.InnerHtml += "..";
                }
                OutSpan.InnerHtml += "<br />";
                OutSpan.InnerHtml += "<br />";
            }
        }
    }
}