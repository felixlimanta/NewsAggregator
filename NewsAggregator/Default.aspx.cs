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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PatternMatch(object sender, EventArgs e)
        {
            if (b.Checked)
            {
                BoyerMoore(sender, e);
            }
            else if (k.Checked)
            {
                KMP(sender, e);
            }
            else if (r.Checked)
            {
                Regex(sender, e);
            }
        }
        protected void BoyerMoore(object sender, EventArgs e)
        {
            string s = key.Text;
            OutSpan.InnerText = "BooyerMoore " + s;
        }
        protected void KMP(object sender, EventArgs e)
        {
            string s = key.Text;
            OutSpan.InnerText = "KMP " + s;
        }
        protected void Regex(object sender, EventArgs e)
        {
            string s = key.Text;
            OutSpan.InnerText = "Regex " + s;
        }
    }
}