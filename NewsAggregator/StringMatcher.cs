using System;
using System.Text.RegularExpressions;

namespace NewsAggregator
{
    public abstract class StringMatcher
    {
        protected string text { get; set; }
        protected string keyword { get; set; }
        protected int matchIndex { get; set; }

        public StringMatcher(string text, string keyword)
        {
            this.text = text;
            this.keyword = keyword;
        }
    }

    public class KMPMatcher: StringMatcher
    {
        private int [] fail;

        public KMPMatcher(string text, string keyword): base(text, keyword)
        {
            computeFail();
            matchIndex = findMatch();
        }

        private void computeFail()
        {
            fail = new int[keyword.Length];
            fail[0] = 0;

            int m = keyword.Length;
            int j = 0;
            int i = 1;

            while (i < m)
            {
                if (keyword[j] == keyword[i])
                {
                    fail[i++] = ++j;
                } else if (j > 0)
                {
                    j = fail[j - 1];
                } else
                {
                    fail[i++] = 0;
                }
            }
        }

        private int findMatch()
        {
            int n = text.Length;
            int m = keyword.Length;

            int i = 0;
            int j = 0;

            while (i < n)
            {
                if (keyword[j] == text[i])
                {
                    if (j == m - 1)
                    {
                        return i - m + 1;
                    }
                    i++;
                    j++;
                } else if (j > 0)
                {
                    j = fail[j - 1];
                } else
                {
                    i++;
                }
            }
            return -1;
        }
    }

    public class BMMatcher: StringMatcher
    {
        private int[] last;

        public BMMatcher(string text, string keyword): base(text, keyword)
        {
            buildLast();
            matchIndex = findMatch();
        }

        private void buildLast()
        {
            last = new int[128];

            for (int i = 0; i < 128; ++i)
            {
                last[i] = -1;
            }

            for (int i = 0; i < keyword.Length; ++i)
            {
                last[keyword[i]] = i;
            }
        }

        private int findMatch()
        {
            int n = text.Length;
            int m = keyword.Length;
            int i = m - 1;

            if (i > n - 1)
            {
                return -1;
            }

            int j = m - 1;
            do
            {
                if (keyword[j] == text[i])
                {
                    if (j == 0)
                    {
                        return i;
                    } else
                    {
                        i--;
                        j--;
                    }
                } else
                {
                    int lo = last[text[i]];
                    i += m - Math.Min(j, lo + 1);
                    j = m - 1;
                }
            } while (i <= n - 1);

            return -1;
        }
    }

    public class RegexMatcher: StringMatcher
    {
        private Match match;

        public RegexMatcher(string text, string keyword): base(text, keyword)
        {
            matchIndex = findMatch();
        }

        private int findMatch()
        {
            match = Regex.Match(text, keyword);
            keyword = match.Value;
            return match.Index;
        }
    }
}