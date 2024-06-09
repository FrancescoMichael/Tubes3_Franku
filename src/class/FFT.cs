using System.Windows;

namespace FrankuGUI
{
    class FFT
    {
        private static int LOGN = 18;
        private static List<long>[] iw = new List<long>[LOGN];
        private static List<long>[] rv = new List<long>[LOGN];
        private static List<long>[] w = new List<long>[LOGN];
        private static long mod = 998244353;
        private static long g = 3;
        private static double LOWER_BOUND_TO_CONTINUE = 0.75;

        public static long powMod(long x, long y, long p)
        {
            long res = 1;
            x %= p;
            if(x == 0)
            {
                return 0;
            }
            while(y > 0)
            {
                if((y & 1L) == 1)
                {
                    res = ((res % p) * (x % p)) % p;
                }
                y >>= 1;
                x = ((x % p) * (x % p)) % p;
            }
            return res;
        }

        public static long inverseMod(long x, long p)
        {
            return powMod(x, p - 2, p);
        }

        public static void initFFT()
        {
            long wb = powMod(g, (mod - 1) / (1 << LOGN), mod);
            for(int i = 0; i < LOGN; i++)
            {
                iw[i] = new List<long>();
                rv[i] = new List<long>();
                w[i] = new List<long>();
            }
            for(int st = 0; st < LOGN; st++)
            {
                for(int i = 0; i < (1 << st); i++)
                {
                    w[st].Add(1);
                    iw[st].Add(1);
                    rv[st].Add(0);
                }
                long bw = powMod(wb, 1 << (LOGN - st - 1), mod);
                long ibw = inverseMod(bw, mod);
                long cw = 1;
                long icw = 1;
                for(int k = 0; k < (1 << st); k++)
                {
                    w[st][k] = cw;
                    iw[st][k] = icw;
                    cw = (cw * bw) % mod;
                    icw = (icw * ibw) % mod;
                }
                if(st == 0)
                {
                    rv[st][0] = 0;
                    continue;
                }
                int h = (1 << (st - 1));
                for(int k = 0; k < (1 << st); k++)
                {
                    long u = 1;
                    if(k < h) u = 0;
                    rv[st][k] = (rv[st - 1][k & (h - 1)] << 1) | u;
                }
            }
        }

        private static void fft(ref List<long> a, bool inverse)
        {
            long n = a.Count;
            long ln = (long)Math.Log2(n * 1.0);
            for(int i = 0; i < n; i++)
            {
                int ni = (int)rv[ln][i];
                if(i < ni)
                {
                    long temp = a[i];
                    a[i] = a[ni];
                    a[ni] = temp;
                }
            }

            for(int st = 0; (1 << st) < n; st++)
            {
                int len = (1 << st);
                for(int k = 0; k < n; k += (len << 1))
                {
                    for(int pos = k; pos < k + len; pos++)
                    {
                        long l = a[pos];
                        long r = (a[pos + len] * (inverse ? iw[st][pos - k] : w[st][pos - k])) % mod;
                        a[pos] = ((l + r < mod) ? l + r : l + r - mod);
                        a[pos + len] = ((l - r >= 0) ? l - r : mod + l - r);
                    }
                }
            }
            if(inverse)
            {
                long inValue = inverseMod(n, mod);
                for(int i = 0; i < n; i++)
                {
                    a[i] = (a[i] * inValue) % mod;
                }
            }
        }

        public static List<long> mul(ref List<long> a, ref List<long> b)
        {
            List<long> fa = new List<long>(a);
            List<long> fb = new List<long>(b);
            int n = 1;
            while(n < a.Count + b.Count){
                n <<= 1;
            }
            fa.AddRange(Enumerable.Repeat(0L, n - fa.Count));
            fb.AddRange(Enumerable.Repeat(0L, n - fb.Count));
            fft(ref fa, false);
            fft(ref fb, false);
            for(int i = 0; i < n; i++)
            {
                fa[i] = fa[i] * fb[i] % mod;
            }
            fft(ref fa, true);
            
            return fa;
        }

        public static (int, int) FindStart(List<string> queryString, List<string> baseString)
        {
            int N1 = queryString.Count;
            int M1 = queryString[0].Length;
            int N2 = baseString.Count;
            int M2 = baseString[0].Length;

            if(N1 > N2 || M1 > M2){
                return (-1, -1);
            }

            if(N1 == N2 && M1 == M2){
                return (0, 0);
            }
            List<long> realBase = new List<long>();
            List<long> invertedBase = new List<long>();
            List<long> realQuery = new List<long>();
            List<long> invertedQuery = new List<long>();
            for(int i = 0; i < N2; i++)
            {
                for(int j = 0; j < M2; j++)
                {
                    long val = baseString[i][j] - '0';
                    realBase.Add(val);
                    invertedBase.Add(val ^ 1L);
                }
            }
            int maxim = N1 * M2 - 1;
            for(int i = 0; i < N1; i++)
            {
                for(int j = 0; j < M1; j++)
                {
                    long val = queryString[i][j] - '0';
                    realQuery.Add(val);
                    invertedQuery.Add(val ^ 1L);
                }
                realQuery.AddRange(Enumerable.Repeat(0L, M2 - M1));
                invertedQuery.AddRange(Enumerable.Repeat(0L, M2 - M1));
            }
            realQuery.Reverse();
            invertedQuery.Reverse();
            if(realQuery.Count != maxim + 1 || invertedQuery.Count != maxim + 1)
            {
                //MessageBox.Show("?????W");
            }
            List<long> res1 = mul(ref realBase, ref realQuery);
            List<long> res2 = mul(ref invertedBase, ref invertedQuery);

            int r1c = res1.Count;
            int r2c = res2.Count;
            int curMaxi = -1, ansI = -1, ansJ = -1;
            for(int i = 0; i <= N2 - N1; i++)
            {
                for(int j = 0; j <= M2 - M1; j++)
                {
                    int diff = i * M2 + j;
                    diff += maxim;
                    int take = 0;
                    if(diff < r1c)
                    {
                        take += (int)res1[diff];
                    }
                    if(diff < r2c)
                    {
                        take += (int)res2[diff];
                    }
                    if(take > curMaxi)
                    {
                        curMaxi = take;
                        ansI = i;
                        ansJ = j;
                    }
                }
            }

            double percentage = 0;

            if(curMaxi != -1)
            {
                percentage = (curMaxi * 1.0) / (N1 * M1 * 1.0);
            }
            if(percentage > 1)
            {
                MessageBox.Show("???PER");
            }
            if(percentage < LOWER_BOUND_TO_CONTINUE)
            {
                return (-1, -1);
            }

            return (ansI, ansJ);
        }

    }
}