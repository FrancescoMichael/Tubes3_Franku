using System.Text.RegularExpressions;

namespace FrankuGUI{
    class RegexSpec{
       public static bool regexMatch(string plainText, string comparedText){
            string regexText = "";

            for (int i = 0; i < plainText.Length; i++)
            {
                char c = plainText[i];

                if (c == 'a' || c == 'A')
                {
                    regexText += "(?:[Aa]|4)?";
                }
                else if (c == 'i' || c == 'I')
                {
                    regexText += "(?:[Ii]|1)?";
                }
                else if (c == 'u' || c == 'U')
                {
                    regexText += "(?:[Uu])?";
                }
                else if (c == 'e' || c == 'E')
                {
                    regexText += "(?:[Ee]|3)?";
                }
                else if (c == 'o' || c == 'O')
                {
                    regexText += "(?:[Oo]|0)?";
                }
                else if (c == 'g' || c == 'G')
                {
                    regexText += "(?:[Gg]|6|9)";
                }
                else if (c == 's' || c == 'S')
                {
                    regexText += "(?:[Ss]|5)";
                }
                else if (c == 'r' || c == 'R')
                {
                    regexText += "(?:[Rr]|12)";
                }
                else if (c == 'b' || c == 'B')
                {
                    regexText += "(?:[Bb]|13|8)";
                }
                else if (c == 't' || c == 'T')
                {
                    regexText += "(?:[Tt]|7)";
                }
                else if (c == 'l' || c == 'L')
                {
                    regexText += "(?:[Ll]|1)";
                }
                else
                {
                    regexText += "(?:[" + char.ToLower(c) + char.ToUpper(c) + "])";
                }
            }
            bool isMatch = Regex.IsMatch(comparedText, regexText);
            return isMatch;
        }
    }
}