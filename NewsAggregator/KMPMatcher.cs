using System;

namespace NewsAggregator
{
    public class KMPMatcher : StringMatcher
    {
        private int[] fail;

        public KMPMatcher(string text, string keyword) : base(text, keyword)
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
                if (Char.ToUpper(keyword[j]) == Char.ToUpper(keyword[i]))
                {
                    fail[i++] = ++j;
                }
                else if (j > 0)
                {
                    j = fail[j - 1];
                }
                else
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
                if (Char.ToUpper(keyword[j]) == Char.ToUpper(text[i]))
                {
                    if (j == m - 1)
                    {
                        return i - m + 1;
                    }
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = fail[j - 1];
                }
                else
                {
                    i++;
                }
            }
            return -1;
        }
    }
}