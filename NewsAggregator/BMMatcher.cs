using System;

namespace NewsAggregator
{
    public class BMMatcher : StringMatcher
    {
        private int[] last;

        public BMMatcher(string text, string keyword) : base(text, keyword)
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
                if (Char.ToUpper(keyword[j]) == Char.ToUpper(text[i]))
                {
                    if (j == 0)
                    {
                        return i;
                    }
                    else
                    {
                        i--;
                        j--;
                    }
                }
                else
                {
                    int lo = last[text[i]];
                    i += m - Math.Min(j, lo + 1);
                    j = m - 1;
                }
            } while (i <= n - 1);

            return -1;
        }
    }
}