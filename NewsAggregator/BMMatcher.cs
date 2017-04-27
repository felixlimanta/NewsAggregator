using System;
using System.Diagnostics;

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
            last = new int[256];

            for (int i = 0; i < 256; ++i)
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
                    Trace.WriteLine(text[i]);
                    Trace.WriteLine((int)text[i]);
                    int lo;
                    try
                    {
                        lo = last[text[i]];
                    } catch (IndexOutOfRangeException)
                    {
                        lo = -1;
                    }
                    i += m - Math.Min(j, lo + 1);
                    j = m - 1;
                }
            } while (i <= n - 1);

            return -1;
        }
    }
}