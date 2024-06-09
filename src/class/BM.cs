
namespace FrankuGUI{
    class BM{
    private static int ALPHABET_SIZE = 2;
    private static char FIRST_CHARACTER = '0';
        public static (bool, int) BMSearch(string pat, string txt) {
            int M = pat.Length;
            int N = txt.Length;
            List<int> badChar = new List<int>();
            for(int u = 0; u < ALPHABET_SIZE; u++) badChar.Add(-1);
            for(int u = 0; u < txt.Length; u++) badChar[txt[u] - FIRST_CHARACTER] = u;
            int i = 0;
            while(i <= N - M) {
                int j = M - 1;
                while(j >= 0 && pat[j] == txt[i + j]){
                    j--;
                }
                if(j < 0){
                    return (true, i);
                }
                i += Math.Max(1, j - badChar[txt[i + j] - FIRST_CHARACTER]);
            } 
            return (false, -1);
        }
    }
}