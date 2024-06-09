namespace FrankuGUI{
    class KMP{
        public static List<int> computeLPSArray(string pat, int M) 
        {
            int len = 0;
            int i = 1;
            List<int> lps = new List<int>(M);
            for(int j = 0; j < M; j++)
            {
                lps.Add(0);
            }
            while(i < M) 
            {
                if(pat[i] == pat[len]) 
                {
                    len++;
                    lps[i] = len;
                    i++;
                } 
                else 
                {
                    if(len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }
            return lps;
        }

        public static (bool, int) KMPSearch(string pat, string txt) 
        {
            int M = pat.Length;
            int N = txt.Length;
            List<int> lps = computeLPSArray(pat, M);
            int i = 0, j = 0;
            while(i < N) 
            {
                if(pat[j] == txt[i]) 
                {
                    j++;
                    i++;
                }
                if(j == M)
                {
                    return (true, i - M);
                } 
                else if (i < N && pat[j] != txt[i]) 
                {
                    if(j != 0) j = lps[j - 1];
                    else i++;
                }
            } 
            return (false, -1);
        }
    }
}