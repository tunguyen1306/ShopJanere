using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication1.Helper
{
    public static class UrlFriendly
    {
        public static string UrlFrendly(this string text, int maxlen = 80)
        {
            if (string.IsNullOrEmpty(text)) return "";
            bool prevdash = false;
            var sb = new StringBuilder(text.Length);
            foreach (char chr in text)
            {
                var c = NormalizeChar(chr);
                switch (c)
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        sb.Append(c);
                        prevdash = false;
                        break;

                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                        sb.Append((char)(c | 32));
                        prevdash = false;
                        break;

                    case ' ':
                    case ',':
                    case '.':
                    case '/':
                    case '\\':
                    case '-':
                    case '_':
                    case '=':
                        if (!prevdash && sb.Length > 0)
                        {
                            sb.Append('-');
                            prevdash = true;
                        }
                        break;
                }
                if (sb.Length == maxlen) break;
            }
            var cleaned = prevdash ? sb.ToString(0, sb.Length - 1) : sb.ToString();
            return cleaned.StringWordsRemove();
        }
        private static readonly List<string> wordsToRemove = "and or".Split(' ').ToList();

        public static string StringWordsRemove(this string stringToClean)
        {
            return string.Join(" ", stringToClean.Split(' ').Except(wordsToRemove));
        }
        private static char NormalizeChar(char c)
        {
            switch (c)
            {
                case '\u00C0':
                    return '\u0041';
                //  À → A    LATIN CAPITAL LETTER A WITH GRAVE → LATIN CAPITAL LETTER A
                case '\u00C1':
                    return '\u0041';
                //  Á → A    LATIN CAPITAL LETTER A WITH ACUTE → LATIN CAPITAL LETTER A
                case '\u00C2':
                    return '\u0041';
                //  Â → A    LATIN CAPITAL LETTER A WITH CIRCUMFLEX → LATIN CAPITAL LETTER A
                case '\u00C3':
                    return '\u0041';
                //  Ã → A    LATIN CAPITAL LETTER A WITH TILDE → LATIN CAPITAL LETTER A
                case '\u00C4':
                    return '\u0041';
                //  Ä → A    LATIN CAPITAL LETTER A WITH DIAERESIS → LATIN CAPITAL LETTER A
                case '\u00C5':
                    return '\u0041';
                //  Å → A    LATIN CAPITAL LETTER A WITH RING ABOVE → LATIN CAPITAL LETTER A
                case '\u0100':
                    return '\u0041';
                //  Ā → A    LATIN CAPITAL LETTER A WITH MACRON → LATIN CAPITAL LETTER A
                case '\u0102':
                    return '\u0041';
                //  Ă → A    LATIN CAPITAL LETTER A WITH BREVE → LATIN CAPITAL LETTER A
                case '\u0104':
                    return '\u0041';
                //  Ą → A    LATIN CAPITAL LETTER A WITH OGONEK → LATIN CAPITAL LETTER A
                case '\u01CD':
                    return '\u0041';
                //  Ǎ → A    LATIN CAPITAL LETTER A WITH CARON → LATIN CAPITAL LETTER A
                case '\u01DE':
                    return '\u0041';
                //  Ǟ → A    LATIN CAPITAL LETTER A WITH DIAERESIS AND MACRON → LATIN CAPITAL LETTER A
                case '\u01E0':
                    return '\u0041';
                //  Ǡ → A    LATIN CAPITAL LETTER A WITH DOT ABOVE AND MACRON → LATIN CAPITAL LETTER A
                case '\u01FA':
                    return '\u0041';
                //  Ǻ → A    LATIN CAPITAL LETTER A WITH RING ABOVE AND ACUTE → LATIN CAPITAL LETTER A
                case '\u0200':
                    return '\u0041';
                //  Ȁ → A    LATIN CAPITAL LETTER A WITH DOUBLE GRAVE → LATIN CAPITAL LETTER A
                case '\u0202':
                    return '\u0041';
                //  Ȃ → A    LATIN CAPITAL LETTER A WITH INVERTED BREVE → LATIN CAPITAL LETTER A
                case '\u0226':
                    return '\u0041';
                //  Ȧ → A    LATIN CAPITAL LETTER A WITH DOT ABOVE → LATIN CAPITAL LETTER A
                case '\u1E00':
                    return '\u0041';
                //  Ḁ → A    LATIN CAPITAL LETTER A WITH RING BELOW → LATIN CAPITAL LETTER A
                case '\u1EA0':
                    return '\u0041';
                //  Ạ → A    LATIN CAPITAL LETTER A WITH DOT BELOW → LATIN CAPITAL LETTER A
                case '\u1EA2':
                    return '\u0041';
                //  Ả → A    LATIN CAPITAL LETTER A WITH HOOK ABOVE → LATIN CAPITAL LETTER A
                case '\u1EA4':
                    return '\u0041';
                //  Ấ → A    LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND ACUTE → LATIN CAPITAL LETTER A
                case '\u1EA6':
                    return '\u0041';
                //  Ầ → A    LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND GRAVE → LATIN CAPITAL LETTER A
                case '\u1EA8':
                    return '\u0041';
                //  Ẩ → A    LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND HOOK ABOVE → LATIN CAPITAL LETTER A
                case '\u1EAA':
                    return '\u0041';
                //  Ẫ → A    LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND TILDE → LATIN CAPITAL LETTER A
                case '\u1EAC':
                    return '\u0041';
                //  Ậ → A    LATIN CAPITAL LETTER A WITH CIRCUMFLEX AND DOT BELOW → LATIN CAPITAL LETTER A
                case '\u1EAE':
                    return '\u0041';
                //  Ắ → A    LATIN CAPITAL LETTER A WITH BREVE AND ACUTE → LATIN CAPITAL LETTER A
                case '\u1EB0':
                    return '\u0041';
                //  Ằ → A    LATIN CAPITAL LETTER A WITH BREVE AND GRAVE → LATIN CAPITAL LETTER A
                case '\u1EB2':
                    return '\u0041';
                //  Ẳ → A    LATIN CAPITAL LETTER A WITH BREVE AND HOOK ABOVE → LATIN CAPITAL LETTER A
                case '\u1EB4':
                    return '\u0041';
                //  Ẵ → A    LATIN CAPITAL LETTER A WITH BREVE AND TILDE → LATIN CAPITAL LETTER A
                case '\u1EB6':
                    return '\u0041';
                //  Ặ → A    LATIN CAPITAL LETTER A WITH BREVE AND DOT BELOW → LATIN CAPITAL LETTER A
                case '\u0181':
                    return '\u0042';
                //  Ɓ → B    LATIN CAPITAL LETTER B WITH HOOK → LATIN CAPITAL LETTER B
                case '\u0182':
                    return '\u0042';
                //  Ƃ → B    LATIN CAPITAL LETTER B WITH TOPBAR → LATIN CAPITAL LETTER B
                case '\u1E02':
                    return '\u0042';
                //  Ḃ → B    LATIN CAPITAL LETTER B WITH DOT ABOVE → LATIN CAPITAL LETTER B
                case '\u1E04':
                    return '\u0042';
                //  Ḅ → B    LATIN CAPITAL LETTER B WITH DOT BELOW → LATIN CAPITAL LETTER B
                case '\u1E06':
                    return '\u0042';
                //  Ḇ → B    LATIN CAPITAL LETTER B WITH LINE BELOW → LATIN CAPITAL LETTER B
                case '\u00C7':
                    return '\u0043';
                //  Ç → C    LATIN CAPITAL LETTER C WITH CEDILLA → LATIN CAPITAL LETTER C
                case '\u0106':
                    return '\u0043';
                //  Ć → C    LATIN CAPITAL LETTER C WITH ACUTE → LATIN CAPITAL LETTER C
                case '\u0108':
                    return '\u0043';
                //  Ĉ → C    LATIN CAPITAL LETTER C WITH CIRCUMFLEX → LATIN CAPITAL LETTER C
                case '\u010A':
                    return '\u0043';
                //  Ċ → C    LATIN CAPITAL LETTER C WITH DOT ABOVE → LATIN CAPITAL LETTER C
                case '\u010C':
                    return '\u0043';
                //  Č → C    LATIN CAPITAL LETTER C WITH CARON → LATIN CAPITAL LETTER C
                case '\u0187':
                    return '\u0043';
                //  Ƈ → C    LATIN CAPITAL LETTER C WITH HOOK → LATIN CAPITAL LETTER C
                case '\u1E08':
                    return '\u0043';
                //  Ḉ → C    LATIN CAPITAL LETTER C WITH CEDILLA AND ACUTE → LATIN CAPITAL LETTER C
                case '\u010E':
                    return '\u0044';
                //  Ď → D    LATIN CAPITAL LETTER D WITH CARON → LATIN CAPITAL LETTER D
                case '\u0110':
                    return '\u0044';
                //  Đ → D    LATIN CAPITAL LETTER D WITH STROKE → LATIN CAPITAL LETTER D
                case '\u018A':
                    return '\u0044';
                //  Ɗ → D    LATIN CAPITAL LETTER D WITH HOOK → LATIN CAPITAL LETTER D
                case '\u018B':
                    return '\u0044';
                //  Ƌ → D    LATIN CAPITAL LETTER D WITH TOPBAR → LATIN CAPITAL LETTER D
                case '\u1E0A':
                    return '\u0044';
                //  Ḋ → D    LATIN CAPITAL LETTER D WITH DOT ABOVE → LATIN CAPITAL LETTER D
                case '\u1E0C':
                    return '\u0044';
                //  Ḍ → D    LATIN CAPITAL LETTER D WITH DOT BELOW → LATIN CAPITAL LETTER D
                case '\u1E0E':
                    return '\u0044';
                //  Ḏ → D    LATIN CAPITAL LETTER D WITH LINE BELOW → LATIN CAPITAL LETTER D
                case '\u1E10':
                    return '\u0044';
                //  Ḑ → D    LATIN CAPITAL LETTER D WITH CEDILLA → LATIN CAPITAL LETTER D
                case '\u1E12':
                    return '\u0044';
                //  Ḓ → D    LATIN CAPITAL LETTER D WITH CIRCUMFLEX BELOW → LATIN CAPITAL LETTER D
                case '\u00C8':
                    return '\u0045';
                //  È → E    LATIN CAPITAL LETTER E WITH GRAVE → LATIN CAPITAL LETTER E
                case '\u00C9':
                    return '\u0045';
                //  É → E    LATIN CAPITAL LETTER E WITH ACUTE → LATIN CAPITAL LETTER E
                case '\u00CA':
                    return '\u0045';
                //  Ê → E    LATIN CAPITAL LETTER E WITH CIRCUMFLEX → LATIN CAPITAL LETTER E
                case '\u00CB':
                    return '\u0045';
                //  Ë → E    LATIN CAPITAL LETTER E WITH DIAERESIS → LATIN CAPITAL LETTER E
                case '\u0112':
                    return '\u0045';
                //  Ē → E    LATIN CAPITAL LETTER E WITH MACRON → LATIN CAPITAL LETTER E
                case '\u0114':
                    return '\u0045';
                //  Ĕ → E    LATIN CAPITAL LETTER E WITH BREVE → LATIN CAPITAL LETTER E
                case '\u0116':
                    return '\u0045';
                //  Ė → E    LATIN CAPITAL LETTER E WITH DOT ABOVE → LATIN CAPITAL LETTER E
                case '\u0118':
                    return '\u0045';
                //  Ę → E    LATIN CAPITAL LETTER E WITH OGONEK → LATIN CAPITAL LETTER E
                case '\u011A':
                    return '\u0045';
                //  Ě → E    LATIN CAPITAL LETTER E WITH CARON → LATIN CAPITAL LETTER E
                case '\u0204':
                    return '\u0045';
                //  Ȅ → E    LATIN CAPITAL LETTER E WITH DOUBLE GRAVE → LATIN CAPITAL LETTER E
                case '\u0206':
                    return '\u0045';
                //  Ȇ → E    LATIN CAPITAL LETTER E WITH INVERTED BREVE → LATIN CAPITAL LETTER E
                case '\u0228':
                    return '\u0045';
                //  Ȩ → E    LATIN CAPITAL LETTER E WITH CEDILLA → LATIN CAPITAL LETTER E
                case '\u1E14':
                    return '\u0045';
                //  Ḕ → E    LATIN CAPITAL LETTER E WITH MACRON AND GRAVE → LATIN CAPITAL LETTER E
                case '\u1E16':
                    return '\u0045';
                //  Ḗ → E    LATIN CAPITAL LETTER E WITH MACRON AND ACUTE → LATIN CAPITAL LETTER E
                case '\u1E18':
                    return '\u0045';
                //  Ḙ → E    LATIN CAPITAL LETTER E WITH CIRCUMFLEX BELOW → LATIN CAPITAL LETTER E
                case '\u1E1A':
                    return '\u0045';
                //  Ḛ → E    LATIN CAPITAL LETTER E WITH TILDE BELOW → LATIN CAPITAL LETTER E
                case '\u1E1C':
                    return '\u0045';
                //  Ḝ → E    LATIN CAPITAL LETTER E WITH CEDILLA AND BREVE → LATIN CAPITAL LETTER E
                case '\u1EB8':
                    return '\u0045';
                //  Ẹ → E    LATIN CAPITAL LETTER E WITH DOT BELOW → LATIN CAPITAL LETTER E
                case '\u1EBA':
                    return '\u0045';
                //  Ẻ → E    LATIN CAPITAL LETTER E WITH HOOK ABOVE → LATIN CAPITAL LETTER E
                case '\u1EBC':
                    return '\u0045';
                //  Ẽ → E    LATIN CAPITAL LETTER E WITH TILDE → LATIN CAPITAL LETTER E
                case '\u1EBE':
                    return '\u0045';
                //  Ế → E    LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND ACUTE → LATIN CAPITAL LETTER E
                case '\u1EC0':
                    return '\u0045';
                //  Ề → E    LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND GRAVE → LATIN CAPITAL LETTER E
                case '\u1EC2':
                    return '\u0045';
                //  Ể → E    LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND HOOK ABOVE → LATIN CAPITAL LETTER E
                case '\u1EC4':
                    return '\u0045';
                //  Ễ → E    LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND TILDE → LATIN CAPITAL LETTER E
                case '\u1EC6':
                    return '\u0045';
                //  Ệ → E    LATIN CAPITAL LETTER E WITH CIRCUMFLEX AND DOT BELOW → LATIN CAPITAL LETTER E
                case '\u0191':
                    return '\u0046';
                //  Ƒ → F    LATIN CAPITAL LETTER F WITH HOOK → LATIN CAPITAL LETTER F
                case '\u1E1E':
                    return '\u0046';
                //  Ḟ → F    LATIN CAPITAL LETTER F WITH DOT ABOVE → LATIN CAPITAL LETTER F
                case '\u011C':
                    return '\u0047';
                //  Ĝ → G    LATIN CAPITAL LETTER G WITH CIRCUMFLEX → LATIN CAPITAL LETTER G
                case '\u011E':
                    return '\u0047';
                //  Ğ → G    LATIN CAPITAL LETTER G WITH BREVE → LATIN CAPITAL LETTER G
                case '\u0120':
                    return '\u0047';
                //  Ġ → G    LATIN CAPITAL LETTER G WITH DOT ABOVE → LATIN CAPITAL LETTER G
                case '\u0122':
                    return '\u0047';
                //  Ģ → G    LATIN CAPITAL LETTER G WITH CEDILLA → LATIN CAPITAL LETTER G
                case '\u0193':
                    return '\u0047';
                //  Ɠ → G    LATIN CAPITAL LETTER G WITH HOOK → LATIN CAPITAL LETTER G
                case '\u01E4':
                    return '\u0047';
                //  Ǥ → G    LATIN CAPITAL LETTER G WITH STROKE → LATIN CAPITAL LETTER G
                case '\u01E6':
                    return '\u0047';
                //  Ǧ → G    LATIN CAPITAL LETTER G WITH CARON → LATIN CAPITAL LETTER G
                case '\u01F4':
                    return '\u0047';
                //  Ǵ → G    LATIN CAPITAL LETTER G WITH ACUTE → LATIN CAPITAL LETTER G
                case '\u1E20':
                    return '\u0047';
                //  Ḡ → G    LATIN CAPITAL LETTER G WITH MACRON → LATIN CAPITAL LETTER G
                case '\u0124':
                    return '\u0048';
                //  Ĥ → H    LATIN CAPITAL LETTER H WITH CIRCUMFLEX → LATIN CAPITAL LETTER H
                case '\u0126':
                    return '\u0048';
                //  Ħ → H    LATIN CAPITAL LETTER H WITH STROKE → LATIN CAPITAL LETTER H
                case '\u021E':
                    return '\u0048';
                //  Ȟ → H    LATIN CAPITAL LETTER H WITH CARON → LATIN CAPITAL LETTER H
                case '\u1E22':
                    return '\u0048';
                //  Ḣ → H    LATIN CAPITAL LETTER H WITH DOT ABOVE → LATIN CAPITAL LETTER H
                case '\u1E24':
                    return '\u0048';
                //  Ḥ → H    LATIN CAPITAL LETTER H WITH DOT BELOW → LATIN CAPITAL LETTER H
                case '\u1E26':
                    return '\u0048';
                //  Ḧ → H    LATIN CAPITAL LETTER H WITH DIAERESIS → LATIN CAPITAL LETTER H
                case '\u1E28':
                    return '\u0048';
                //  Ḩ → H    LATIN CAPITAL LETTER H WITH CEDILLA → LATIN CAPITAL LETTER H
                case '\u1E2A':
                    return '\u0048';
                //  Ḫ → H    LATIN CAPITAL LETTER H WITH BREVE BELOW → LATIN CAPITAL LETTER H
                case '\u00CC':
                    return '\u0049';
                //  Ì → I    LATIN CAPITAL LETTER I WITH GRAVE → LATIN CAPITAL LETTER I
                case '\u00CD':
                    return '\u0049';
                //  Í → I    LATIN CAPITAL LETTER I WITH ACUTE → LATIN CAPITAL LETTER I
                case '\u00CE':
                    return '\u0049';
                //  Î → I    LATIN CAPITAL LETTER I WITH CIRCUMFLEX → LATIN CAPITAL LETTER I
                case '\u00CF':
                    return '\u0049';
                //  Ï → I    LATIN CAPITAL LETTER I WITH DIAERESIS → LATIN CAPITAL LETTER I
                case '\u0128':
                    return '\u0049';
                //  Ĩ → I    LATIN CAPITAL LETTER I WITH TILDE → LATIN CAPITAL LETTER I
                case '\u012A':
                    return '\u0049';
                //  Ī → I    LATIN CAPITAL LETTER I WITH MACRON → LATIN CAPITAL LETTER I
                case '\u012C':
                    return '\u0049';
                //  Ĭ → I    LATIN CAPITAL LETTER I WITH BREVE → LATIN CAPITAL LETTER I
                case '\u012E':
                    return '\u0049';
                //  Į → I    LATIN CAPITAL LETTER I WITH OGONEK → LATIN CAPITAL LETTER I
                case '\u0130':
                    return '\u0049';
                //  İ → I    LATIN CAPITAL LETTER I WITH DOT ABOVE → LATIN CAPITAL LETTER I
                case '\u0197':
                    return '\u0049';
                //  Ɨ → I    LATIN CAPITAL LETTER I WITH STROKE → LATIN CAPITAL LETTER I
                case '\u01CF':
                    return '\u0049';
                //  Ǐ → I    LATIN CAPITAL LETTER I WITH CARON → LATIN CAPITAL LETTER I
                case '\u0208':
                    return '\u0049';
                //  Ȉ → I    LATIN CAPITAL LETTER I WITH DOUBLE GRAVE → LATIN CAPITAL LETTER I
                case '\u020A':
                    return '\u0049';
                //  Ȋ → I    LATIN CAPITAL LETTER I WITH INVERTED BREVE → LATIN CAPITAL LETTER I
                case '\u1E2C':
                    return '\u0049';
                //  Ḭ → I    LATIN CAPITAL LETTER I WITH TILDE BELOW → LATIN CAPITAL LETTER I
                case '\u1E2E':
                    return '\u0049';
                //  Ḯ → I    LATIN CAPITAL LETTER I WITH DIAERESIS AND ACUTE → LATIN CAPITAL LETTER I
                case '\u1EC8':
                    return '\u0049';
                //  Ỉ → I    LATIN CAPITAL LETTER I WITH HOOK ABOVE → LATIN CAPITAL LETTER I
                case '\u1ECA':
                    return '\u0049';
                //  Ị → I    LATIN CAPITAL LETTER I WITH DOT BELOW → LATIN CAPITAL LETTER I
                case '\u0134':
                    return '\u004A';
                //  Ĵ → J    LATIN CAPITAL LETTER J WITH CIRCUMFLEX → LATIN CAPITAL LETTER J
                case '\u0136':
                    return '\u004B';
                //  Ķ → K    LATIN CAPITAL LETTER K WITH CEDILLA → LATIN CAPITAL LETTER K
                case '\u0198':
                    return '\u004B';
                //  Ƙ → K    LATIN CAPITAL LETTER K WITH HOOK → LATIN CAPITAL LETTER K
                case '\u01E8':
                    return '\u004B';
                //  Ǩ → K    LATIN CAPITAL LETTER K WITH CARON → LATIN CAPITAL LETTER K
                case '\u1E30':
                    return '\u004B';
                //  Ḱ → K    LATIN CAPITAL LETTER K WITH ACUTE → LATIN CAPITAL LETTER K
                case '\u1E32':
                    return '\u004B';
                //  Ḳ → K    LATIN CAPITAL LETTER K WITH DOT BELOW → LATIN CAPITAL LETTER K
                case '\u1E34':
                    return '\u004B';
                //  Ḵ → K    LATIN CAPITAL LETTER K WITH LINE BELOW → LATIN CAPITAL LETTER K
                case '\u0139':
                    return '\u004C';
                //  Ĺ → L    LATIN CAPITAL LETTER L WITH ACUTE → LATIN CAPITAL LETTER L
                case '\u013B':
                    return '\u004C';
                //  Ļ → L    LATIN CAPITAL LETTER L WITH CEDILLA → LATIN CAPITAL LETTER L
                case '\u013D':
                    return '\u004C';
                //  Ľ → L    LATIN CAPITAL LETTER L WITH CARON → LATIN CAPITAL LETTER L
                case '\u0141':
                    return '\u004C';
                //  Ł → L    LATIN CAPITAL LETTER L WITH STROKE → LATIN CAPITAL LETTER L
                case '\u1E36':
                    return '\u004C';
                //  Ḷ → L    LATIN CAPITAL LETTER L WITH DOT BELOW → LATIN CAPITAL LETTER L
                case '\u1E38':
                    return '\u004C';
                //  Ḹ → L    LATIN CAPITAL LETTER L WITH DOT BELOW AND MACRON → LATIN CAPITAL LETTER L
                case '\u1E3A':
                    return '\u004C';
                //  Ḻ → L    LATIN CAPITAL LETTER L WITH LINE BELOW → LATIN CAPITAL LETTER L
                case '\u1E3C':
                    return '\u004C';
                //  Ḽ → L    LATIN CAPITAL LETTER L WITH CIRCUMFLEX BELOW → LATIN CAPITAL LETTER L
                case '\u1E3E':
                    return '\u004D';
                //  Ḿ → M    LATIN CAPITAL LETTER M WITH ACUTE → LATIN CAPITAL LETTER M
                case '\u1E40':
                    return '\u004D';
                //  Ṁ → M    LATIN CAPITAL LETTER M WITH DOT ABOVE → LATIN CAPITAL LETTER M
                case '\u1E42':
                    return '\u004D';
                //  Ṃ → M    LATIN CAPITAL LETTER M WITH DOT BELOW → LATIN CAPITAL LETTER M
                case '\u00D1':
                    return '\u004E';
                //  Ñ → N    LATIN CAPITAL LETTER N WITH TILDE → LATIN CAPITAL LETTER N
                case '\u0143':
                    return '\u004E';
                //  Ń → N    LATIN CAPITAL LETTER N WITH ACUTE → LATIN CAPITAL LETTER N
                case '\u0145':
                    return '\u004E';
                //  Ņ → N    LATIN CAPITAL LETTER N WITH CEDILLA → LATIN CAPITAL LETTER N
                case '\u0147':
                    return '\u004E';
                //  Ň → N    LATIN CAPITAL LETTER N WITH CARON → LATIN CAPITAL LETTER N
                case '\u019D':
                    return '\u004E';
                //  Ɲ → N    LATIN CAPITAL LETTER N WITH LEFT HOOK → LATIN CAPITAL LETTER N
                case '\u01F8':
                    return '\u004E';
                //  Ǹ → N    LATIN CAPITAL LETTER N WITH GRAVE → LATIN CAPITAL LETTER N
                case '\u0220':
                    return '\u004E';
                //  Ƞ → N    LATIN CAPITAL LETTER N WITH LONG RIGHT LEG → LATIN CAPITAL LETTER N
                case '\u1E44':
                    return '\u004E';
                //  Ṅ → N    LATIN CAPITAL LETTER N WITH DOT ABOVE → LATIN CAPITAL LETTER N
                case '\u1E46':
                    return '\u004E';
                //  Ṇ → N    LATIN CAPITAL LETTER N WITH DOT BELOW → LATIN CAPITAL LETTER N
                case '\u1E48':
                    return '\u004E';
                //  Ṉ → N    LATIN CAPITAL LETTER N WITH LINE BELOW → LATIN CAPITAL LETTER N
                case '\u1E4A':
                    return '\u004E';
                //  Ṋ → N    LATIN CAPITAL LETTER N WITH CIRCUMFLEX BELOW → LATIN CAPITAL LETTER N
                case '\u00D2':
                    return '\u004F';
                //  Ò → O    LATIN CAPITAL LETTER O WITH GRAVE → LATIN CAPITAL LETTER O
                case '\u00D3':
                    return '\u004F';
                //  Ó → O    LATIN CAPITAL LETTER O WITH ACUTE → LATIN CAPITAL LETTER O
                case '\u00D4':
                    return '\u004F';
                //  Ô → O    LATIN CAPITAL LETTER O WITH CIRCUMFLEX → LATIN CAPITAL LETTER O
                case '\u00D5':
                    return '\u004F';
                //  Õ → O    LATIN CAPITAL LETTER O WITH TILDE → LATIN CAPITAL LETTER O
                case '\u00D6':
                    return '\u004F';
                //  Ö → O    LATIN CAPITAL LETTER O WITH DIAERESIS → LATIN CAPITAL LETTER O
                case '\u00D8':
                    return '\u004F';
                //  Ø → O    LATIN CAPITAL LETTER O WITH STROKE → LATIN CAPITAL LETTER O
                case '\u014C':
                    return '\u004F';
                //  Ō → O    LATIN CAPITAL LETTER O WITH MACRON → LATIN CAPITAL LETTER O
                case '\u014E':
                    return '\u004F';
                //  Ŏ → O    LATIN CAPITAL LETTER O WITH BREVE → LATIN CAPITAL LETTER O
                case '\u0150':
                    return '\u004F';
                //  Ő → O    LATIN CAPITAL LETTER O WITH DOUBLE ACUTE → LATIN CAPITAL LETTER O
                case '\u019F':
                    return '\u004F';
                //  Ɵ → O    LATIN CAPITAL LETTER O WITH MIDDLE TILDE → LATIN CAPITAL LETTER O
                case '\u01A0':
                    return '\u004F';
                //  Ơ → O    LATIN CAPITAL LETTER O WITH HORN → LATIN CAPITAL LETTER O
                case '\u01D1':
                    return '\u004F';
                //  Ǒ → O    LATIN CAPITAL LETTER O WITH CARON → LATIN CAPITAL LETTER O
                case '\u01EA':
                    return '\u004F';
                //  Ǫ → O    LATIN CAPITAL LETTER O WITH OGONEK → LATIN CAPITAL LETTER O
                case '\u01EC':
                    return '\u004F';
                //  Ǭ → O    LATIN CAPITAL LETTER O WITH OGONEK AND MACRON → LATIN CAPITAL LETTER O
                case '\u01FE':
                    return '\u004F';
                //  Ǿ → O    LATIN CAPITAL LETTER O WITH STROKE AND ACUTE → LATIN CAPITAL LETTER O
                case '\u020C':
                    return '\u004F';
                //  Ȍ → O    LATIN CAPITAL LETTER O WITH DOUBLE GRAVE → LATIN CAPITAL LETTER O
                case '\u020E':
                    return '\u004F';
                //  Ȏ → O    LATIN CAPITAL LETTER O WITH INVERTED BREVE → LATIN CAPITAL LETTER O
                case '\u022A':
                    return '\u004F';
                //  Ȫ → O    LATIN CAPITAL LETTER O WITH DIAERESIS AND MACRON → LATIN CAPITAL LETTER O
                case '\u022C':
                    return '\u004F';
                //  Ȭ → O    LATIN CAPITAL LETTER O WITH TILDE AND MACRON → LATIN CAPITAL LETTER O
                case '\u022E':
                    return '\u004F';
                //  Ȯ → O    LATIN CAPITAL LETTER O WITH DOT ABOVE → LATIN CAPITAL LETTER O
                case '\u0230':
                    return '\u004F';
                //  Ȱ → O    LATIN CAPITAL LETTER O WITH DOT ABOVE AND MACRON → LATIN CAPITAL LETTER O
                case '\u1E4C':
                    return '\u004F';
                //  Ṍ → O    LATIN CAPITAL LETTER O WITH TILDE AND ACUTE → LATIN CAPITAL LETTER O
                case '\u1E4E':
                    return '\u004F';
                //  Ṏ → O    LATIN CAPITAL LETTER O WITH TILDE AND DIAERESIS → LATIN CAPITAL LETTER O
                case '\u1E50':
                    return '\u004F';
                //  Ṑ → O    LATIN CAPITAL LETTER O WITH MACRON AND GRAVE → LATIN CAPITAL LETTER O
                case '\u1E52':
                    return '\u004F';
                //  Ṓ → O    LATIN CAPITAL LETTER O WITH MACRON AND ACUTE → LATIN CAPITAL LETTER O
                case '\u1ECC':
                    return '\u004F';
                //  Ọ → O    LATIN CAPITAL LETTER O WITH DOT BELOW → LATIN CAPITAL LETTER O
                case '\u1ECE':
                    return '\u004F';
                //  Ỏ → O    LATIN CAPITAL LETTER O WITH HOOK ABOVE → LATIN CAPITAL LETTER O
                case '\u1ED0':
                    return '\u004F';
                //  Ố → O    LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND ACUTE → LATIN CAPITAL LETTER O
                case '\u1ED2':
                    return '\u004F';
                //  Ồ → O    LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND GRAVE → LATIN CAPITAL LETTER O
                case '\u1ED4':
                    return '\u004F';
                //  Ổ → O    LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND HOOK ABOVE → LATIN CAPITAL LETTER O
                case '\u1ED6':
                    return '\u004F';
                //  Ỗ → O    LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND TILDE → LATIN CAPITAL LETTER O
                case '\u1ED8':
                    return '\u004F';
                //  Ộ → O    LATIN CAPITAL LETTER O WITH CIRCUMFLEX AND DOT BELOW → LATIN CAPITAL LETTER O
                case '\u1EDA':
                    return '\u004F';
                //  Ớ → O    LATIN CAPITAL LETTER O WITH HORN AND ACUTE → LATIN CAPITAL LETTER O
                case '\u1EDC':
                    return '\u004F';
                //  Ờ → O    LATIN CAPITAL LETTER O WITH HORN AND GRAVE → LATIN CAPITAL LETTER O
                case '\u1EDE':
                    return '\u004F';
                //  Ở → O    LATIN CAPITAL LETTER O WITH HORN AND HOOK ABOVE → LATIN CAPITAL LETTER O
                case '\u1EE0':
                    return '\u004F';
                //  Ỡ → O    LATIN CAPITAL LETTER O WITH HORN AND TILDE → LATIN CAPITAL LETTER O
                case '\u1EE2':
                    return '\u004F';
                //  Ợ → O    LATIN CAPITAL LETTER O WITH HORN AND DOT BELOW → LATIN CAPITAL LETTER O
                case '\u01A4':
                    return '\u0050';
                //  Ƥ → P    LATIN CAPITAL LETTER P WITH HOOK → LATIN CAPITAL LETTER P
                case '\u1E54':
                    return '\u0050';
                //  Ṕ → P    LATIN CAPITAL LETTER P WITH ACUTE → LATIN CAPITAL LETTER P
                case '\u1E56':
                    return '\u0050';
                //  Ṗ → P    LATIN CAPITAL LETTER P WITH DOT ABOVE → LATIN CAPITAL LETTER P
                case '\u0154':
                    return '\u0052';
                //  Ŕ → R    LATIN CAPITAL LETTER R WITH ACUTE → LATIN CAPITAL LETTER R
                case '\u0156':
                    return '\u0052';
                //  Ŗ → R    LATIN CAPITAL LETTER R WITH CEDILLA → LATIN CAPITAL LETTER R
                case '\u0158':
                    return '\u0052';
                //  Ř → R    LATIN CAPITAL LETTER R WITH CARON → LATIN CAPITAL LETTER R
                case '\u0210':
                    return '\u0052';
                //  Ȑ → R    LATIN CAPITAL LETTER R WITH DOUBLE GRAVE → LATIN CAPITAL LETTER R
                case '\u0212':
                    return '\u0052';
                //  Ȓ → R    LATIN CAPITAL LETTER R WITH INVERTED BREVE → LATIN CAPITAL LETTER R
                case '\u1E58':
                    return '\u0052';
                //  Ṙ → R    LATIN CAPITAL LETTER R WITH DOT ABOVE → LATIN CAPITAL LETTER R
                case '\u1E5A':
                    return '\u0052';
                //  Ṛ → R    LATIN CAPITAL LETTER R WITH DOT BELOW → LATIN CAPITAL LETTER R
                case '\u1E5C':
                    return '\u0052';
                //  Ṝ → R    LATIN CAPITAL LETTER R WITH DOT BELOW AND MACRON → LATIN CAPITAL LETTER R
                case '\u1E5E':
                    return '\u0052';
                //  Ṟ → R    LATIN CAPITAL LETTER R WITH LINE BELOW → LATIN CAPITAL LETTER R
                case '\u015A':
                    return '\u0053';
                //  Ś → S    LATIN CAPITAL LETTER S WITH ACUTE → LATIN CAPITAL LETTER S
                case '\u015C':
                    return '\u0053';
                //  Ŝ → S    LATIN CAPITAL LETTER S WITH CIRCUMFLEX → LATIN CAPITAL LETTER S
                case '\u015E':
                    return '\u0053';
                //  Ş → S    LATIN CAPITAL LETTER S WITH CEDILLA → LATIN CAPITAL LETTER S
                case '\u0160':
                    return '\u0053';
                //  Š → S    LATIN CAPITAL LETTER S WITH CARON → LATIN CAPITAL LETTER S
                case '\u0218':
                    return '\u0053';
                //  Ș → S    LATIN CAPITAL LETTER S WITH COMMA BELOW → LATIN CAPITAL LETTER S
                case '\u1E60':
                    return '\u0053';
                //  Ṡ → S    LATIN CAPITAL LETTER S WITH DOT ABOVE → LATIN CAPITAL LETTER S
                case '\u1E62':
                    return '\u0053';
                //  Ṣ → S    LATIN CAPITAL LETTER S WITH DOT BELOW → LATIN CAPITAL LETTER S
                case '\u1E64':
                    return '\u0053';
                //  Ṥ → S    LATIN CAPITAL LETTER S WITH ACUTE AND DOT ABOVE → LATIN CAPITAL LETTER S
                case '\u1E66':
                    return '\u0053';
                //  Ṧ → S    LATIN CAPITAL LETTER S WITH CARON AND DOT ABOVE → LATIN CAPITAL LETTER S
                case '\u1E68':
                    return '\u0053';
                //  Ṩ → S    LATIN CAPITAL LETTER S WITH DOT BELOW AND DOT ABOVE → LATIN CAPITAL LETTER S
                case '\u0162':
                    return '\u0054';
                //  Ţ → T    LATIN CAPITAL LETTER T WITH CEDILLA → LATIN CAPITAL LETTER T
                case '\u0164':
                    return '\u0054';
                //  Ť → T    LATIN CAPITAL LETTER T WITH CARON → LATIN CAPITAL LETTER T
                case '\u0166':
                    return '\u0054';
                //  Ŧ → T    LATIN CAPITAL LETTER T WITH STROKE → LATIN CAPITAL LETTER T
                case '\u01AC':
                    return '\u0054';
                //  Ƭ → T    LATIN CAPITAL LETTER T WITH HOOK → LATIN CAPITAL LETTER T
                case '\u01AE':
                    return '\u0054';
                //  Ʈ → T    LATIN CAPITAL LETTER T WITH RETROFLEX HOOK → LATIN CAPITAL LETTER T
                case '\u021A':
                    return '\u0054';
                //  Ț → T    LATIN CAPITAL LETTER T WITH COMMA BELOW → LATIN CAPITAL LETTER T
                case '\u1E6A':
                    return '\u0054';
                //  Ṫ → T    LATIN CAPITAL LETTER T WITH DOT ABOVE → LATIN CAPITAL LETTER T
                case '\u1E6C':
                    return '\u0054';
                //  Ṭ → T    LATIN CAPITAL LETTER T WITH DOT BELOW → LATIN CAPITAL LETTER T
                case '\u1E6E':
                    return '\u0054';
                //  Ṯ → T    LATIN CAPITAL LETTER T WITH LINE BELOW → LATIN CAPITAL LETTER T
                case '\u1E70':
                    return '\u0054';
                //  Ṱ → T    LATIN CAPITAL LETTER T WITH CIRCUMFLEX BELOW → LATIN CAPITAL LETTER T
                case '\u00D9':
                    return '\u0055';
                //  Ù → U    LATIN CAPITAL LETTER U WITH GRAVE → LATIN CAPITAL LETTER U
                case '\u00DA':
                    return '\u0055';
                //  Ú → U    LATIN CAPITAL LETTER U WITH ACUTE → LATIN CAPITAL LETTER U
                case '\u00DB':
                    return '\u0055';
                //  Û → U    LATIN CAPITAL LETTER U WITH CIRCUMFLEX → LATIN CAPITAL LETTER U
                case '\u00DC':
                    return '\u0055';
                //  Ü → U    LATIN CAPITAL LETTER U WITH DIAERESIS → LATIN CAPITAL LETTER U
                case '\u0168':
                    return '\u0055';
                //  Ũ → U    LATIN CAPITAL LETTER U WITH TILDE → LATIN CAPITAL LETTER U
                case '\u016A':
                    return '\u0055';
                //  Ū → U    LATIN CAPITAL LETTER U WITH MACRON → LATIN CAPITAL LETTER U
                case '\u016C':
                    return '\u0055';
                //  Ŭ → U    LATIN CAPITAL LETTER U WITH BREVE → LATIN CAPITAL LETTER U
                case '\u016E':
                    return '\u0055';
                //  Ů → U    LATIN CAPITAL LETTER U WITH RING ABOVE → LATIN CAPITAL LETTER U
                case '\u0170':
                    return '\u0055';
                //  Ű → U    LATIN CAPITAL LETTER U WITH DOUBLE ACUTE → LATIN CAPITAL LETTER U
                case '\u0172':
                    return '\u0055';
                //  Ų → U    LATIN CAPITAL LETTER U WITH OGONEK → LATIN CAPITAL LETTER U
                case '\u01AF':
                    return '\u0055';
                //  Ư → U    LATIN CAPITAL LETTER U WITH HORN → LATIN CAPITAL LETTER U
                case '\u01D3':
                    return '\u0055';
                //  Ǔ → U    LATIN CAPITAL LETTER U WITH CARON → LATIN CAPITAL LETTER U
                case '\u01D5':
                    return '\u0055';
                //  Ǖ → U    LATIN CAPITAL LETTER U WITH DIAERESIS AND MACRON → LATIN CAPITAL LETTER U
                case '\u01D7':
                    return '\u0055';
                //  Ǘ → U    LATIN CAPITAL LETTER U WITH DIAERESIS AND ACUTE → LATIN CAPITAL LETTER U
                case '\u01D9':
                    return '\u0055';
                //  Ǚ → U    LATIN CAPITAL LETTER U WITH DIAERESIS AND CARON → LATIN CAPITAL LETTER U
                case '\u01DB':
                    return '\u0055';
                //  Ǜ → U    LATIN CAPITAL LETTER U WITH DIAERESIS AND GRAVE → LATIN CAPITAL LETTER U
                case '\u0214':
                    return '\u0055';
                //  Ȕ → U    LATIN CAPITAL LETTER U WITH DOUBLE GRAVE → LATIN CAPITAL LETTER U
                case '\u0216':
                    return '\u0055';
                //  Ȗ → U    LATIN CAPITAL LETTER U WITH INVERTED BREVE → LATIN CAPITAL LETTER U
                case '\u1E72':
                    return '\u0055';
                //  Ṳ → U    LATIN CAPITAL LETTER U WITH DIAERESIS BELOW → LATIN CAPITAL LETTER U
                case '\u1E74':
                    return '\u0055';
                //  Ṵ → U    LATIN CAPITAL LETTER U WITH TILDE BELOW → LATIN CAPITAL LETTER U
                case '\u1E76':
                    return '\u0055';
                //  Ṷ → U    LATIN CAPITAL LETTER U WITH CIRCUMFLEX BELOW → LATIN CAPITAL LETTER U
                case '\u1E78':
                    return '\u0055';
                //  Ṹ → U    LATIN CAPITAL LETTER U WITH TILDE AND ACUTE → LATIN CAPITAL LETTER U
                case '\u1E7A':
                    return '\u0055';
                //  Ṻ → U    LATIN CAPITAL LETTER U WITH MACRON AND DIAERESIS → LATIN CAPITAL LETTER U
                case '\u1EE4':
                    return '\u0055';
                //  Ụ → U    LATIN CAPITAL LETTER U WITH DOT BELOW → LATIN CAPITAL LETTER U
                case '\u1EE6':
                    return '\u0055';
                //  Ủ → U    LATIN CAPITAL LETTER U WITH HOOK ABOVE → LATIN CAPITAL LETTER U
                case '\u1EE8':
                    return '\u0055';
                //  Ứ → U    LATIN CAPITAL LETTER U WITH HORN AND ACUTE → LATIN CAPITAL LETTER U
                case '\u1EEA':
                    return '\u0055';
                //  Ừ → U    LATIN CAPITAL LETTER U WITH HORN AND GRAVE → LATIN CAPITAL LETTER U
                case '\u1EEC':
                    return '\u0055';
                //  Ử → U    LATIN CAPITAL LETTER U WITH HORN AND HOOK ABOVE → LATIN CAPITAL LETTER U
                case '\u1EEE':
                    return '\u0055';
                //  Ữ → U    LATIN CAPITAL LETTER U WITH HORN AND TILDE → LATIN CAPITAL LETTER U
                case '\u1EF0':
                    return '\u0055';
                //  Ự → U    LATIN CAPITAL LETTER U WITH HORN AND DOT BELOW → LATIN CAPITAL LETTER U
                case '\u01B2':
                    return '\u0056';
                //  Ʋ → V    LATIN CAPITAL LETTER V WITH HOOK → LATIN CAPITAL LETTER V
                case '\u1E7C':
                    return '\u0056';
                //  Ṽ → V    LATIN CAPITAL LETTER V WITH TILDE → LATIN CAPITAL LETTER V
                case '\u1E7E':
                    return '\u0056';
                //  Ṿ → V    LATIN CAPITAL LETTER V WITH DOT BELOW → LATIN CAPITAL LETTER V
                case '\u0174':
                    return '\u0057';
                //  Ŵ → W    LATIN CAPITAL LETTER W WITH CIRCUMFLEX → LATIN CAPITAL LETTER W
                case '\u1E80':
                    return '\u0057';
                //  Ẁ → W    LATIN CAPITAL LETTER W WITH GRAVE → LATIN CAPITAL LETTER W
                case '\u1E82':
                    return '\u0057';
                //  Ẃ → W    LATIN CAPITAL LETTER W WITH ACUTE → LATIN CAPITAL LETTER W
                case '\u1E84':
                    return '\u0057';
                //  Ẅ → W    LATIN CAPITAL LETTER W WITH DIAERESIS → LATIN CAPITAL LETTER W
                case '\u1E86':
                    return '\u0057';
                //  Ẇ → W    LATIN CAPITAL LETTER W WITH DOT ABOVE → LATIN CAPITAL LETTER W
                case '\u1E88':
                    return '\u0057';
                //  Ẉ → W    LATIN CAPITAL LETTER W WITH DOT BELOW → LATIN CAPITAL LETTER W
                case '\u1E8A':
                    return '\u0058';
                //  Ẋ → X    LATIN CAPITAL LETTER X WITH DOT ABOVE → LATIN CAPITAL LETTER X
                case '\u1E8C':
                    return '\u0058';
                //  Ẍ → X    LATIN CAPITAL LETTER X WITH DIAERESIS → LATIN CAPITAL LETTER X
                case '\u00DD':
                    return '\u0059';
                //  Ý → Y    LATIN CAPITAL LETTER Y WITH ACUTE → LATIN CAPITAL LETTER Y
                case '\u0176':
                    return '\u0059';
                //  Ŷ → Y    LATIN CAPITAL LETTER Y WITH CIRCUMFLEX → LATIN CAPITAL LETTER Y
                case '\u0178':
                    return '\u0059';
                //  Ÿ → Y    LATIN CAPITAL LETTER Y WITH DIAERESIS → LATIN CAPITAL LETTER Y
                case '\u01B3':
                    return '\u0059';
                //  Ƴ → Y    LATIN CAPITAL LETTER Y WITH HOOK → LATIN CAPITAL LETTER Y
                case '\u0232':
                    return '\u0059';
                //  Ȳ → Y    LATIN CAPITAL LETTER Y WITH MACRON → LATIN CAPITAL LETTER Y
                case '\u1E8E':
                    return '\u0059';
                //  Ẏ → Y    LATIN CAPITAL LETTER Y WITH DOT ABOVE → LATIN CAPITAL LETTER Y
                case '\u1EF2':
                    return '\u0059';
                //  Ỳ → Y    LATIN CAPITAL LETTER Y WITH GRAVE → LATIN CAPITAL LETTER Y
                case '\u1EF4':
                    return '\u0059';
                //  Ỵ → Y    LATIN CAPITAL LETTER Y WITH DOT BELOW → LATIN CAPITAL LETTER Y
                case '\u1EF6':
                    return '\u0059';
                //  Ỷ → Y    LATIN CAPITAL LETTER Y WITH HOOK ABOVE → LATIN CAPITAL LETTER Y
                case '\u1EF8':
                    return '\u0059';
                //  Ỹ → Y    LATIN CAPITAL LETTER Y WITH TILDE → LATIN CAPITAL LETTER Y
                case '\u0179':
                    return '\u005A';
                //  Ź → Z    LATIN CAPITAL LETTER Z WITH ACUTE → LATIN CAPITAL LETTER Z
                case '\u017B':
                    return '\u005A';
                //  Ż → Z    LATIN CAPITAL LETTER Z WITH DOT ABOVE → LATIN CAPITAL LETTER Z
                case '\u017D':
                    return '\u005A';
                //  Ž → Z    LATIN CAPITAL LETTER Z WITH CARON → LATIN CAPITAL LETTER Z
                case '\u01B5':
                    return '\u005A';
                //  Ƶ → Z    LATIN CAPITAL LETTER Z WITH STROKE → LATIN CAPITAL LETTER Z
                case '\u0224':
                    return '\u005A';
                //  Ȥ → Z    LATIN CAPITAL LETTER Z WITH HOOK → LATIN CAPITAL LETTER Z
                case '\u1E90':
                    return '\u005A';
                //  Ẑ → Z    LATIN CAPITAL LETTER Z WITH CIRCUMFLEX → LATIN CAPITAL LETTER Z
                case '\u1E92':
                    return '\u005A';
                //  Ẓ → Z    LATIN CAPITAL LETTER Z WITH DOT BELOW → LATIN CAPITAL LETTER Z
                case '\u1E94':
                    return '\u005A';
                //  Ẕ → Z    LATIN CAPITAL LETTER Z WITH LINE BELOW → LATIN CAPITAL LETTER Z
                case '\u00E0':
                    return '\u0061';
                //  à → a    LATIN SMALL LETTER A WITH GRAVE → LATIN SMALL LETTER A
                case '\u00E1':
                    return '\u0061';
                //  á → a    LATIN SMALL LETTER A WITH ACUTE → LATIN SMALL LETTER A
                case '\u00E2':
                    return '\u0061';
                //  â → a    LATIN SMALL LETTER A WITH CIRCUMFLEX → LATIN SMALL LETTER A
                case '\u00E3':
                    return '\u0061';
                //  ã → a    LATIN SMALL LETTER A WITH TILDE → LATIN SMALL LETTER A
                case '\u00E4':
                    return '\u0061';
                //  ä → a    LATIN SMALL LETTER A WITH DIAERESIS → LATIN SMALL LETTER A
                case '\u00E5':
                    return '\u0061';
                //  å → a    LATIN SMALL LETTER A WITH RING ABOVE → LATIN SMALL LETTER A
                case '\u0101':
                    return '\u0061';
                //  ā → a    LATIN SMALL LETTER A WITH MACRON → LATIN SMALL LETTER A
                case '\u0103':
                    return '\u0061';
                //  ă → a    LATIN SMALL LETTER A WITH BREVE → LATIN SMALL LETTER A
                case '\u0105':
                    return '\u0061';
                //  ą → a    LATIN SMALL LETTER A WITH OGONEK → LATIN SMALL LETTER A
                case '\u01CE':
                    return '\u0061';
                //  ǎ → a    LATIN SMALL LETTER A WITH CARON → LATIN SMALL LETTER A
                case '\u01DF':
                    return '\u0061';
                //  ǟ → a    LATIN SMALL LETTER A WITH DIAERESIS AND MACRON → LATIN SMALL LETTER A
                case '\u01E1':
                    return '\u0061';
                //  ǡ → a    LATIN SMALL LETTER A WITH DOT ABOVE AND MACRON → LATIN SMALL LETTER A
                case '\u01FB':
                    return '\u0061';
                //  ǻ → a    LATIN SMALL LETTER A WITH RING ABOVE AND ACUTE → LATIN SMALL LETTER A
                case '\u0201':
                    return '\u0061';
                //  ȁ → a    LATIN SMALL LETTER A WITH DOUBLE GRAVE → LATIN SMALL LETTER A
                case '\u0203':
                    return '\u0061';
                //  ȃ → a    LATIN SMALL LETTER A WITH INVERTED BREVE → LATIN SMALL LETTER A
                case '\u0227':
                    return '\u0061';
                //  ȧ → a    LATIN SMALL LETTER A WITH DOT ABOVE → LATIN SMALL LETTER A
                case '\u1E01':
                    return '\u0061';
                //  ḁ → a    LATIN SMALL LETTER A WITH RING BELOW → LATIN SMALL LETTER A
                case '\u1E9A':
                    return '\u0061';
                //  ẚ → a    LATIN SMALL LETTER A WITH RIGHT HALF RING → LATIN SMALL LETTER A
                case '\u1EA1':
                    return '\u0061';
                //  ạ → a    LATIN SMALL LETTER A WITH DOT BELOW → LATIN SMALL LETTER A
                case '\u1EA3':
                    return '\u0061';
                //  ả → a    LATIN SMALL LETTER A WITH HOOK ABOVE → LATIN SMALL LETTER A
                case '\u1EA5':
                    return '\u0061';
                //  ấ → a    LATIN SMALL LETTER A WITH CIRCUMFLEX AND ACUTE → LATIN SMALL LETTER A
                case '\u1EA7':
                    return '\u0061';
                //  ầ → a    LATIN SMALL LETTER A WITH CIRCUMFLEX AND GRAVE → LATIN SMALL LETTER A
                case '\u1EA9':
                    return '\u0061';
                //  ẩ → a    LATIN SMALL LETTER A WITH CIRCUMFLEX AND HOOK ABOVE → LATIN SMALL LETTER A
                case '\u1EAB':
                    return '\u0061';
                //  ẫ → a    LATIN SMALL LETTER A WITH CIRCUMFLEX AND TILDE → LATIN SMALL LETTER A
                case '\u1EAD':
                    return '\u0061';
                //  ậ → a    LATIN SMALL LETTER A WITH CIRCUMFLEX AND DOT BELOW → LATIN SMALL LETTER A
                case '\u1EAF':
                    return '\u0061';
                //  ắ → a    LATIN SMALL LETTER A WITH BREVE AND ACUTE → LATIN SMALL LETTER A
                case '\u1EB1':
                    return '\u0061';
                //  ằ → a    LATIN SMALL LETTER A WITH BREVE AND GRAVE → LATIN SMALL LETTER A
                case '\u1EB3':
                    return '\u0061';
                //  ẳ → a    LATIN SMALL LETTER A WITH BREVE AND HOOK ABOVE → LATIN SMALL LETTER A
                case '\u1EB5':
                    return '\u0061';
                //  ẵ → a    LATIN SMALL LETTER A WITH BREVE AND TILDE → LATIN SMALL LETTER A
                case '\u1EB7':
                    return '\u0061';
                //  ặ → a    LATIN SMALL LETTER A WITH BREVE AND DOT BELOW → LATIN SMALL LETTER A
                case '\u0180':
                    return '\u0062';
                //  ƀ → b    LATIN SMALL LETTER B WITH STROKE → LATIN SMALL LETTER B
                case '\u0183':
                    return '\u0062';
                //  ƃ → b    LATIN SMALL LETTER B WITH TOPBAR → LATIN SMALL LETTER B
                case '\u0253':
                    return '\u0062';
                //  ɓ → b    LATIN SMALL LETTER B WITH HOOK → LATIN SMALL LETTER B
                case '\u1E03':
                    return '\u0062';
                //  ḃ → b    LATIN SMALL LETTER B WITH DOT ABOVE → LATIN SMALL LETTER B
                case '\u1E05':
                    return '\u0062';
                //  ḅ → b    LATIN SMALL LETTER B WITH DOT BELOW → LATIN SMALL LETTER B
                case '\u1E07':
                    return '\u0062';
                //  ḇ → b    LATIN SMALL LETTER B WITH LINE BELOW → LATIN SMALL LETTER B
                case '\u00E7':
                    return '\u0063';
                //  ç → c    LATIN SMALL LETTER C WITH CEDILLA → LATIN SMALL LETTER C
                case '\u0107':
                    return '\u0063';
                //  ć → c    LATIN SMALL LETTER C WITH ACUTE → LATIN SMALL LETTER C
                case '\u0109':
                    return '\u0063';
                //  ĉ → c    LATIN SMALL LETTER C WITH CIRCUMFLEX → LATIN SMALL LETTER C
                case '\u010B':
                    return '\u0063';
                //  ċ → c    LATIN SMALL LETTER C WITH DOT ABOVE → LATIN SMALL LETTER C
                case '\u010D':
                    return '\u0063';
                //  č → c    LATIN SMALL LETTER C WITH CARON → LATIN SMALL LETTER C
                case '\u0188':
                    return '\u0063';
                //  ƈ → c    LATIN SMALL LETTER C WITH HOOK → LATIN SMALL LETTER C
                case '\u0255':
                    return '\u0063';
                //  ɕ → c    LATIN SMALL LETTER C WITH CURL → LATIN SMALL LETTER C
                case '\u1E09':
                    return '\u0063';
                //  ḉ → c    LATIN SMALL LETTER C WITH CEDILLA AND ACUTE → LATIN SMALL LETTER C
                case '\u010F':
                    return '\u0064';
                //  ď → d    LATIN SMALL LETTER D WITH CARON → LATIN SMALL LETTER D
                case '\u0111':
                    return '\u0064';
                //  đ → d    LATIN SMALL LETTER D WITH STROKE → LATIN SMALL LETTER D
                case '\u018C':
                    return '\u0064';
                //  ƌ → d    LATIN SMALL LETTER D WITH TOPBAR → LATIN SMALL LETTER D
                case '\u0221':
                    return '\u0064';
                //  ȡ → d    LATIN SMALL LETTER D WITH CURL → LATIN SMALL LETTER D
                case '\u0256':
                    return '\u0064';
                //  ɖ → d    LATIN SMALL LETTER D WITH TAIL → LATIN SMALL LETTER D
                case '\u0257':
                    return '\u0064';
                //  ɗ → d    LATIN SMALL LETTER D WITH HOOK → LATIN SMALL LETTER D
                case '\u1E0B':
                    return '\u0064';
                //  ḋ → d    LATIN SMALL LETTER D WITH DOT ABOVE → LATIN SMALL LETTER D
                case '\u1E0D':
                    return '\u0064';
                //  ḍ → d    LATIN SMALL LETTER D WITH DOT BELOW → LATIN SMALL LETTER D
                case '\u1E0F':
                    return '\u0064';
                //  ḏ → d    LATIN SMALL LETTER D WITH LINE BELOW → LATIN SMALL LETTER D
                case '\u1E11':
                    return '\u0064';
                //  ḑ → d    LATIN SMALL LETTER D WITH CEDILLA → LATIN SMALL LETTER D
                case '\u1E13':
                    return '\u0064';
                //  ḓ → d    LATIN SMALL LETTER D WITH CIRCUMFLEX BELOW → LATIN SMALL LETTER D
                case '\u00E8':
                    return '\u0065';
                //  è → e    LATIN SMALL LETTER E WITH GRAVE → LATIN SMALL LETTER E
                case '\u00E9':
                    return '\u0065';
                //  é → e    LATIN SMALL LETTER E WITH ACUTE → LATIN SMALL LETTER E
                case '\u00EA':
                    return '\u0065';
                //  ê → e    LATIN SMALL LETTER E WITH CIRCUMFLEX → LATIN SMALL LETTER E
                case '\u00EB':
                    return '\u0065';
                //  ë → e    LATIN SMALL LETTER E WITH DIAERESIS → LATIN SMALL LETTER E
                case '\u0113':
                    return '\u0065';
                //  ē → e    LATIN SMALL LETTER E WITH MACRON → LATIN SMALL LETTER E
                case '\u0115':
                    return '\u0065';
                //  ĕ → e    LATIN SMALL LETTER E WITH BREVE → LATIN SMALL LETTER E
                case '\u0117':
                    return '\u0065';
                //  ė → e    LATIN SMALL LETTER E WITH DOT ABOVE → LATIN SMALL LETTER E
                case '\u0119':
                    return '\u0065';
                //  ę → e    LATIN SMALL LETTER E WITH OGONEK → LATIN SMALL LETTER E
                case '\u011B':
                    return '\u0065';
                //  ě → e    LATIN SMALL LETTER E WITH CARON → LATIN SMALL LETTER E
                case '\u0205':
                    return '\u0065';
                //  ȅ → e    LATIN SMALL LETTER E WITH DOUBLE GRAVE → LATIN SMALL LETTER E
                case '\u0207':
                    return '\u0065';
                //  ȇ → e    LATIN SMALL LETTER E WITH INVERTED BREVE → LATIN SMALL LETTER E
                case '\u0229':
                    return '\u0065';
                //  ȩ → e    LATIN SMALL LETTER E WITH CEDILLA → LATIN SMALL LETTER E
                case '\u1E15':
                    return '\u0065';
                //  ḕ → e    LATIN SMALL LETTER E WITH MACRON AND GRAVE → LATIN SMALL LETTER E
                case '\u1E17':
                    return '\u0065';
                //  ḗ → e    LATIN SMALL LETTER E WITH MACRON AND ACUTE → LATIN SMALL LETTER E
                case '\u1E19':
                    return '\u0065';
                //  ḙ → e    LATIN SMALL LETTER E WITH CIRCUMFLEX BELOW → LATIN SMALL LETTER E
                case '\u1E1B':
                    return '\u0065';
                //  ḛ → e    LATIN SMALL LETTER E WITH TILDE BELOW → LATIN SMALL LETTER E
                case '\u1E1D':
                    return '\u0065';
                //  ḝ → e    LATIN SMALL LETTER E WITH CEDILLA AND BREVE → LATIN SMALL LETTER E
                case '\u1EB9':
                    return '\u0065';
                //  ẹ → e    LATIN SMALL LETTER E WITH DOT BELOW → LATIN SMALL LETTER E
                case '\u1EBB':
                    return '\u0065';
                //  ẻ → e    LATIN SMALL LETTER E WITH HOOK ABOVE → LATIN SMALL LETTER E
                case '\u1EBD':
                    return '\u0065';
                //  ẽ → e    LATIN SMALL LETTER E WITH TILDE → LATIN SMALL LETTER E
                case '\u1EBF':
                    return '\u0065';
                //  ế → e    LATIN SMALL LETTER E WITH CIRCUMFLEX AND ACUTE → LATIN SMALL LETTER E
                case '\u1EC1':
                    return '\u0065';
                //  ề → e    LATIN SMALL LETTER E WITH CIRCUMFLEX AND GRAVE → LATIN SMALL LETTER E
                case '\u1EC3':
                    return '\u0065';
                //  ể → e    LATIN SMALL LETTER E WITH CIRCUMFLEX AND HOOK ABOVE → LATIN SMALL LETTER E
                case '\u1EC5':
                    return '\u0065';
                //  ễ → e    LATIN SMALL LETTER E WITH CIRCUMFLEX AND TILDE → LATIN SMALL LETTER E
                case '\u1EC7':
                    return '\u0065';
                //  ệ → e    LATIN SMALL LETTER E WITH CIRCUMFLEX AND DOT BELOW → LATIN SMALL LETTER E
                case '\u0192':
                    return '\u0066';
                //  ƒ → f    LATIN SMALL LETTER F WITH HOOK → LATIN SMALL LETTER F
                case '\u1E1F':
                    return '\u0066';
                //  ḟ → f    LATIN SMALL LETTER F WITH DOT ABOVE → LATIN SMALL LETTER F
                case '\u011D':
                    return '\u0067';
                //  ĝ → g    LATIN SMALL LETTER G WITH CIRCUMFLEX → LATIN SMALL LETTER G
                case '\u011F':
                    return '\u0067';
                //  ğ → g    LATIN SMALL LETTER G WITH BREVE → LATIN SMALL LETTER G
                case '\u0121':
                    return '\u0067';
                //  ġ → g    LATIN SMALL LETTER G WITH DOT ABOVE → LATIN SMALL LETTER G
                case '\u0123':
                    return '\u0067';
                //  ģ → g    LATIN SMALL LETTER G WITH CEDILLA → LATIN SMALL LETTER G
                case '\u01E5':
                    return '\u0067';
                //  ǥ → g    LATIN SMALL LETTER G WITH STROKE → LATIN SMALL LETTER G
                case '\u01E7':
                    return '\u0067';
                //  ǧ → g    LATIN SMALL LETTER G WITH CARON → LATIN SMALL LETTER G
                case '\u01F5':
                    return '\u0067';
                //  ǵ → g    LATIN SMALL LETTER G WITH ACUTE → LATIN SMALL LETTER G
                case '\u0260':
                    return '\u0067';
                //  ɠ → g    LATIN SMALL LETTER G WITH HOOK → LATIN SMALL LETTER G
                case '\u1E21':
                    return '\u0067';
                //  ḡ → g    LATIN SMALL LETTER G WITH MACRON → LATIN SMALL LETTER G
                case '\u0125':
                    return '\u0068';
                //  ĥ → h    LATIN SMALL LETTER H WITH CIRCUMFLEX → LATIN SMALL LETTER H
                case '\u0127':
                    return '\u0068';
                //  ħ → h    LATIN SMALL LETTER H WITH STROKE → LATIN SMALL LETTER H
                case '\u021F':
                    return '\u0068';
                //  ȟ → h    LATIN SMALL LETTER H WITH CARON → LATIN SMALL LETTER H
                case '\u0266':
                    return '\u0068';
                //  ɦ → h    LATIN SMALL LETTER H WITH HOOK → LATIN SMALL LETTER H
                case '\u1E23':
                    return '\u0068';
                //  ḣ → h    LATIN SMALL LETTER H WITH DOT ABOVE → LATIN SMALL LETTER H
                case '\u1E25':
                    return '\u0068';
                //  ḥ → h    LATIN SMALL LETTER H WITH DOT BELOW → LATIN SMALL LETTER H
                case '\u1E27':
                    return '\u0068';
                //  ḧ → h    LATIN SMALL LETTER H WITH DIAERESIS → LATIN SMALL LETTER H
                case '\u1E29':
                    return '\u0068';
                //  ḩ → h    LATIN SMALL LETTER H WITH CEDILLA → LATIN SMALL LETTER H
                case '\u1E2B':
                    return '\u0068';
                //  ḫ → h    LATIN SMALL LETTER H WITH BREVE BELOW → LATIN SMALL LETTER H
                case '\u1E96':
                    return '\u0068';
                //  ẖ → h    LATIN SMALL LETTER H WITH LINE BELOW → LATIN SMALL LETTER H
                case '\u00EC':
                    return '\u0069';
                //  ì → i    LATIN SMALL LETTER I WITH GRAVE → LATIN SMALL LETTER I
                case '\u00ED':
                    return '\u0069';
                //  í → i    LATIN SMALL LETTER I WITH ACUTE → LATIN SMALL LETTER I
                case '\u00EE':
                    return '\u0069';
                //  î → i    LATIN SMALL LETTER I WITH CIRCUMFLEX → LATIN SMALL LETTER I
                case '\u00EF':
                    return '\u0069';
                //  ï → i    LATIN SMALL LETTER I WITH DIAERESIS → LATIN SMALL LETTER I
                case '\u0129':
                    return '\u0069';
                //  ĩ → i    LATIN SMALL LETTER I WITH TILDE → LATIN SMALL LETTER I
                case '\u012B':
                    return '\u0069';
                //  ī → i    LATIN SMALL LETTER I WITH MACRON → LATIN SMALL LETTER I
                case '\u012D':
                    return '\u0069';
                //  ĭ → i    LATIN SMALL LETTER I WITH BREVE → LATIN SMALL LETTER I
                case '\u012F':
                    return '\u0069';
                //  į → i    LATIN SMALL LETTER I WITH OGONEK → LATIN SMALL LETTER I
                case '\u01D0':
                    return '\u0069';
                //  ǐ → i    LATIN SMALL LETTER I WITH CARON → LATIN SMALL LETTER I
                case '\u0209':
                    return '\u0069';
                //  ȉ → i    LATIN SMALL LETTER I WITH DOUBLE GRAVE → LATIN SMALL LETTER I
                case '\u020B':
                    return '\u0069';
                //  ȋ → i    LATIN SMALL LETTER I WITH INVERTED BREVE → LATIN SMALL LETTER I
                case '\u0268':
                    return '\u0069';
                //  ɨ → i    LATIN SMALL LETTER I WITH STROKE → LATIN SMALL LETTER I
                case '\u1E2D':
                    return '\u0069';
                //  ḭ → i    LATIN SMALL LETTER I WITH TILDE BELOW → LATIN SMALL LETTER I
                case '\u1E2F':
                    return '\u0069';
                //  ḯ → i    LATIN SMALL LETTER I WITH DIAERESIS AND ACUTE → LATIN SMALL LETTER I
                case '\u1EC9':
                    return '\u0069';
                //  ỉ → i    LATIN SMALL LETTER I WITH HOOK ABOVE → LATIN SMALL LETTER I
                case '\u1ECB':
                    return '\u0069';
                //  ị → i    LATIN SMALL LETTER I WITH DOT BELOW → LATIN SMALL LETTER I
                case '\u0135':
                    return '\u006A';
                //  ĵ → j    LATIN SMALL LETTER J WITH CIRCUMFLEX → LATIN SMALL LETTER J
                case '\u01F0':
                    return '\u006A';
                //  ǰ → j    LATIN SMALL LETTER J WITH CARON → LATIN SMALL LETTER J
                case '\u029D':
                    return '\u006A';
                //  ʝ → j    LATIN SMALL LETTER J WITH CROSSED-TAIL → LATIN SMALL LETTER J
                case '\u0137':
                    return '\u006B';
                //  ķ → k    LATIN SMALL LETTER K WITH CEDILLA → LATIN SMALL LETTER K
                case '\u0199':
                    return '\u006B';
                //  ƙ → k    LATIN SMALL LETTER K WITH HOOK → LATIN SMALL LETTER K
                case '\u01E9':
                    return '\u006B';
                //  ǩ → k    LATIN SMALL LETTER K WITH CARON → LATIN SMALL LETTER K
                case '\u1E31':
                    return '\u006B';
                //  ḱ → k    LATIN SMALL LETTER K WITH ACUTE → LATIN SMALL LETTER K
                case '\u1E33':
                    return '\u006B';
                //  ḳ → k    LATIN SMALL LETTER K WITH DOT BELOW → LATIN SMALL LETTER K
                case '\u1E35':
                    return '\u006B';
                //  ḵ → k    LATIN SMALL LETTER K WITH LINE BELOW → LATIN SMALL LETTER K
                case '\u013A':
                    return '\u006C';
                //  ĺ → l    LATIN SMALL LETTER L WITH ACUTE → LATIN SMALL LETTER L
                case '\u013C':
                    return '\u006C';
                //  ļ → l    LATIN SMALL LETTER L WITH CEDILLA → LATIN SMALL LETTER L
                case '\u013E':
                    return '\u006C';
                //  ľ → l    LATIN SMALL LETTER L WITH CARON → LATIN SMALL LETTER L
                case '\u0140':
                    return '\u006C';
                //  ŀ → l    LATIN SMALL LETTER L WITH MIDDLE DOT → LATIN SMALL LETTER L
                case '\u0142':
                    return '\u006C';
                //  ł → l    LATIN SMALL LETTER L WITH STROKE → LATIN SMALL LETTER L
                case '\u019A':
                    return '\u006C';
                //  ƚ → l    LATIN SMALL LETTER L WITH BAR → LATIN SMALL LETTER L
                case '\u0234':
                    return '\u006C';
                //  ȴ → l    LATIN SMALL LETTER L WITH CURL → LATIN SMALL LETTER L
                case '\u026B':
                    return '\u006C';
                //  ɫ → l    LATIN SMALL LETTER L WITH MIDDLE TILDE → LATIN SMALL LETTER L
                case '\u026C':
                    return '\u006C';
                //  ɬ → l    LATIN SMALL LETTER L WITH BELT → LATIN SMALL LETTER L
                case '\u026D':
                    return '\u006C';
                //  ɭ → l    LATIN SMALL LETTER L WITH RETROFLEX HOOK → LATIN SMALL LETTER L
                case '\u1E37':
                    return '\u006C';
                //  ḷ → l    LATIN SMALL LETTER L WITH DOT BELOW → LATIN SMALL LETTER L
                case '\u1E39':
                    return '\u006C';
                //  ḹ → l    LATIN SMALL LETTER L WITH DOT BELOW AND MACRON → LATIN SMALL LETTER L
                case '\u1E3B':
                    return '\u006C';
                //  ḻ → l    LATIN SMALL LETTER L WITH LINE BELOW → LATIN SMALL LETTER L
                case '\u1E3D':
                    return '\u006C';
                //  ḽ → l    LATIN SMALL LETTER L WITH CIRCUMFLEX BELOW → LATIN SMALL LETTER L
                case '\u0271':
                    return '\u006D';
                //  ɱ → m    LATIN SMALL LETTER M WITH HOOK → LATIN SMALL LETTER M
                case '\u1E3F':
                    return '\u006D';
                //  ḿ → m    LATIN SMALL LETTER M WITH ACUTE → LATIN SMALL LETTER M
                case '\u1E41':
                    return '\u006D';
                //  ṁ → m    LATIN SMALL LETTER M WITH DOT ABOVE → LATIN SMALL LETTER M
                case '\u1E43':
                    return '\u006D';
                //  ṃ → m    LATIN SMALL LETTER M WITH DOT BELOW → LATIN SMALL LETTER M
                case '\u00F1':
                    return '\u006E';
                //  ñ → n    LATIN SMALL LETTER N WITH TILDE → LATIN SMALL LETTER N
                case '\u0144':
                    return '\u006E';
                //  ń → n    LATIN SMALL LETTER N WITH ACUTE → LATIN SMALL LETTER N
                case '\u0146':
                    return '\u006E';
                //  ņ → n    LATIN SMALL LETTER N WITH CEDILLA → LATIN SMALL LETTER N
                case '\u0148':
                    return '\u006E';
                //  ň → n    LATIN SMALL LETTER N WITH CARON → LATIN SMALL LETTER N
                case '\u019E':
                    return '\u006E';
                //  ƞ → n    LATIN SMALL LETTER N WITH LONG RIGHT LEG → LATIN SMALL LETTER N
                case '\u01F9':
                    return '\u006E';
                //  ǹ → n    LATIN SMALL LETTER N WITH GRAVE → LATIN SMALL LETTER N
                case '\u0235':
                    return '\u006E';
                //  ȵ → n    LATIN SMALL LETTER N WITH CURL → LATIN SMALL LETTER N
                case '\u0272':
                    return '\u006E';
                //  ɲ → n    LATIN SMALL LETTER N WITH LEFT HOOK → LATIN SMALL LETTER N
                case '\u0273':
                    return '\u006E';
                //  ɳ → n    LATIN SMALL LETTER N WITH RETROFLEX HOOK → LATIN SMALL LETTER N
                case '\u1E45':
                    return '\u006E';
                //  ṅ → n    LATIN SMALL LETTER N WITH DOT ABOVE → LATIN SMALL LETTER N
                case '\u1E47':
                    return '\u006E';
                //  ṇ → n    LATIN SMALL LETTER N WITH DOT BELOW → LATIN SMALL LETTER N
                case '\u1E49':
                    return '\u006E';
                //  ṉ → n    LATIN SMALL LETTER N WITH LINE BELOW → LATIN SMALL LETTER N
                case '\u1E4B':
                    return '\u006E';
                //  ṋ → n    LATIN SMALL LETTER N WITH CIRCUMFLEX BELOW → LATIN SMALL LETTER N
                case '\u00F2':
                    return '\u006F';
                //  ò → o    LATIN SMALL LETTER O WITH GRAVE → LATIN SMALL LETTER O
                case '\u00F3':
                    return '\u006F';
                //  ó → o    LATIN SMALL LETTER O WITH ACUTE → LATIN SMALL LETTER O
                case '\u00F4':
                    return '\u006F';
                //  ô → o    LATIN SMALL LETTER O WITH CIRCUMFLEX → LATIN SMALL LETTER O
                case '\u00F5':
                    return '\u006F';
                //  õ → o    LATIN SMALL LETTER O WITH TILDE → LATIN SMALL LETTER O
                case '\u00F6':
                    return '\u006F';
                //  ö → o    LATIN SMALL LETTER O WITH DIAERESIS → LATIN SMALL LETTER O
                case '\u00F8':
                    return '\u006F';
                //  ø → o    LATIN SMALL LETTER O WITH STROKE → LATIN SMALL LETTER O
                case '\u014D':
                    return '\u006F';
                //  ō → o    LATIN SMALL LETTER O WITH MACRON → LATIN SMALL LETTER O
                case '\u014F':
                    return '\u006F';
                //  ŏ → o    LATIN SMALL LETTER O WITH BREVE → LATIN SMALL LETTER O
                case '\u0151':
                    return '\u006F';
                //  ő → o    LATIN SMALL LETTER O WITH DOUBLE ACUTE → LATIN SMALL LETTER O
                case '\u01A1':
                    return '\u006F';
                //  ơ → o    LATIN SMALL LETTER O WITH HORN → LATIN SMALL LETTER O
                case '\u01D2':
                    return '\u006F';
                //  ǒ → o    LATIN SMALL LETTER O WITH CARON → LATIN SMALL LETTER O
                case '\u01EB':
                    return '\u006F';
                //  ǫ → o    LATIN SMALL LETTER O WITH OGONEK → LATIN SMALL LETTER O
                case '\u01ED':
                    return '\u006F';
                //  ǭ → o    LATIN SMALL LETTER O WITH OGONEK AND MACRON → LATIN SMALL LETTER O
                case '\u01FF':
                    return '\u006F';
                //  ǿ → o    LATIN SMALL LETTER O WITH STROKE AND ACUTE → LATIN SMALL LETTER O
                case '\u020D':
                    return '\u006F';
                //  ȍ → o    LATIN SMALL LETTER O WITH DOUBLE GRAVE → LATIN SMALL LETTER O
                case '\u020F':
                    return '\u006F';
                //  ȏ → o    LATIN SMALL LETTER O WITH INVERTED BREVE → LATIN SMALL LETTER O
                case '\u022B':
                    return '\u006F';
                //  ȫ → o    LATIN SMALL LETTER O WITH DIAERESIS AND MACRON → LATIN SMALL LETTER O
                case '\u022D':
                    return '\u006F';
                //  ȭ → o    LATIN SMALL LETTER O WITH TILDE AND MACRON → LATIN SMALL LETTER O
                case '\u022F':
                    return '\u006F';
                //  ȯ → o    LATIN SMALL LETTER O WITH DOT ABOVE → LATIN SMALL LETTER O
                case '\u0231':
                    return '\u006F';
                //  ȱ → o    LATIN SMALL LETTER O WITH DOT ABOVE AND MACRON → LATIN SMALL LETTER O
                case '\u1E4D':
                    return '\u006F';
                //  ṍ → o    LATIN SMALL LETTER O WITH TILDE AND ACUTE → LATIN SMALL LETTER O
                case '\u1E4F':
                    return '\u006F';
                //  ṏ → o    LATIN SMALL LETTER O WITH TILDE AND DIAERESIS → LATIN SMALL LETTER O
                case '\u1E51':
                    return '\u006F';
                //  ṑ → o    LATIN SMALL LETTER O WITH MACRON AND GRAVE → LATIN SMALL LETTER O
                case '\u1E53':
                    return '\u006F';
                //  ṓ → o    LATIN SMALL LETTER O WITH MACRON AND ACUTE → LATIN SMALL LETTER O
                case '\u1ECD':
                    return '\u006F';
                //  ọ → o    LATIN SMALL LETTER O WITH DOT BELOW → LATIN SMALL LETTER O
                case '\u1ECF':
                    return '\u006F';
                //  ỏ → o    LATIN SMALL LETTER O WITH HOOK ABOVE → LATIN SMALL LETTER O
                case '\u1ED1':
                    return '\u006F';
                //  ố → o    LATIN SMALL LETTER O WITH CIRCUMFLEX AND ACUTE → LATIN SMALL LETTER O
                case '\u1ED3':
                    return '\u006F';
                //  ồ → o    LATIN SMALL LETTER O WITH CIRCUMFLEX AND GRAVE → LATIN SMALL LETTER O
                case '\u1ED5':
                    return '\u006F';
                //  ổ → o    LATIN SMALL LETTER O WITH CIRCUMFLEX AND HOOK ABOVE → LATIN SMALL LETTER O
                case '\u1ED7':
                    return '\u006F';
                //  ỗ → o    LATIN SMALL LETTER O WITH CIRCUMFLEX AND TILDE → LATIN SMALL LETTER O
                case '\u1ED9':
                    return '\u006F';
                //  ộ → o    LATIN SMALL LETTER O WITH CIRCUMFLEX AND DOT BELOW → LATIN SMALL LETTER O
                case '\u1EDB':
                    return '\u006F';
                //  ớ → o    LATIN SMALL LETTER O WITH HORN AND ACUTE → LATIN SMALL LETTER O
                case '\u1EDD':
                    return '\u006F';
                //  ờ → o    LATIN SMALL LETTER O WITH HORN AND GRAVE → LATIN SMALL LETTER O
                case '\u1EDF':
                    return '\u006F';
                //  ở → o    LATIN SMALL LETTER O WITH HORN AND HOOK ABOVE → LATIN SMALL LETTER O
                case '\u1EE1':
                    return '\u006F';
                //  ỡ → o    LATIN SMALL LETTER O WITH HORN AND TILDE → LATIN SMALL LETTER O
                case '\u1EE3':
                    return '\u006F';
                //  ợ → o    LATIN SMALL LETTER O WITH HORN AND DOT BELOW → LATIN SMALL LETTER O
                case '\u01A5':
                    return '\u0070';
                //  ƥ → p    LATIN SMALL LETTER P WITH HOOK → LATIN SMALL LETTER P
                case '\u1E55':
                    return '\u0070';
                //  ṕ → p    LATIN SMALL LETTER P WITH ACUTE → LATIN SMALL LETTER P
                case '\u1E57':
                    return '\u0070';
                //  ṗ → p    LATIN SMALL LETTER P WITH DOT ABOVE → LATIN SMALL LETTER P
                case '\u02A0':
                    return '\u0071';
                //  ʠ → q    LATIN SMALL LETTER Q WITH HOOK → LATIN SMALL LETTER Q
                case '\u0155':
                    return '\u0072';
                //  ŕ → r    LATIN SMALL LETTER R WITH ACUTE → LATIN SMALL LETTER R
                case '\u0157':
                    return '\u0072';
                //  ŗ → r    LATIN SMALL LETTER R WITH CEDILLA → LATIN SMALL LETTER R
                case '\u0159':
                    return '\u0072';
                //  ř → r    LATIN SMALL LETTER R WITH CARON → LATIN SMALL LETTER R
                case '\u0211':
                    return '\u0072';
                //  ȑ → r    LATIN SMALL LETTER R WITH DOUBLE GRAVE → LATIN SMALL LETTER R
                case '\u0213':
                    return '\u0072';
                //  ȓ → r    LATIN SMALL LETTER R WITH INVERTED BREVE → LATIN SMALL LETTER R
                case '\u027C':
                    return '\u0072';
                //  ɼ → r    LATIN SMALL LETTER R WITH LONG LEG → LATIN SMALL LETTER R
                case '\u027D':
                    return '\u0072';
                //  ɽ → r    LATIN SMALL LETTER R WITH TAIL → LATIN SMALL LETTER R
                case '\u1E59':
                    return '\u0072';
                //  ṙ → r    LATIN SMALL LETTER R WITH DOT ABOVE → LATIN SMALL LETTER R
                case '\u1E5B':
                    return '\u0072';
                //  ṛ → r    LATIN SMALL LETTER R WITH DOT BELOW → LATIN SMALL LETTER R
                case '\u1E5D':
                    return '\u0072';
                //  ṝ → r    LATIN SMALL LETTER R WITH DOT BELOW AND MACRON → LATIN SMALL LETTER R
                case '\u1E5F':
                    return '\u0072';
                //  ṟ → r    LATIN SMALL LETTER R WITH LINE BELOW → LATIN SMALL LETTER R
                case '\u015B':
                    return '\u0073';
                //  ś → s    LATIN SMALL LETTER S WITH ACUTE → LATIN SMALL LETTER S
                case '\u015D':
                    return '\u0073';
                //  ŝ → s    LATIN SMALL LETTER S WITH CIRCUMFLEX → LATIN SMALL LETTER S
                case '\u015F':
                    return '\u0073';
                //  ş → s    LATIN SMALL LETTER S WITH CEDILLA → LATIN SMALL LETTER S
                case '\u0161':
                    return '\u0073';
                //  š → s    LATIN SMALL LETTER S WITH CARON → LATIN SMALL LETTER S
                case '\u0219':
                    return '\u0073';
                //  ș → s    LATIN SMALL LETTER S WITH COMMA BELOW → LATIN SMALL LETTER S
                case '\u0282':
                    return '\u0073';
                //  ʂ → s    LATIN SMALL LETTER S WITH HOOK → LATIN SMALL LETTER S
                case '\u1E61':
                    return '\u0073';
                //  ṡ → s    LATIN SMALL LETTER S WITH DOT ABOVE → LATIN SMALL LETTER S
                case '\u1E63':
                    return '\u0073';
                //  ṣ → s    LATIN SMALL LETTER S WITH DOT BELOW → LATIN SMALL LETTER S
                case '\u1E65':
                    return '\u0073';
                //  ṥ → s    LATIN SMALL LETTER S WITH ACUTE AND DOT ABOVE → LATIN SMALL LETTER S
                case '\u1E67':
                    return '\u0073';
                //  ṧ → s    LATIN SMALL LETTER S WITH CARON AND DOT ABOVE → LATIN SMALL LETTER S
                case '\u1E69':
                    return '\u0073';
                //  ṩ → s    LATIN SMALL LETTER S WITH DOT BELOW AND DOT ABOVE → LATIN SMALL LETTER S
                case '\u0163':
                    return '\u0074';
                //  ţ → t    LATIN SMALL LETTER T WITH CEDILLA → LATIN SMALL LETTER T
                case '\u0165':
                    return '\u0074';
                //  ť → t    LATIN SMALL LETTER T WITH CARON → LATIN SMALL LETTER T
                case '\u0167':
                    return '\u0074';
                //  ŧ → t    LATIN SMALL LETTER T WITH STROKE → LATIN SMALL LETTER T
                case '\u01AB':
                    return '\u0074';
                //  ƫ → t    LATIN SMALL LETTER T WITH PALATAL HOOK → LATIN SMALL LETTER T
                case '\u01AD':
                    return '\u0074';
                //  ƭ → t    LATIN SMALL LETTER T WITH HOOK → LATIN SMALL LETTER T
                case '\u021B':
                    return '\u0074';
                //  ț → t    LATIN SMALL LETTER T WITH COMMA BELOW → LATIN SMALL LETTER T
                case '\u0236':
                    return '\u0074';
                //  ȶ → t    LATIN SMALL LETTER T WITH CURL → LATIN SMALL LETTER T
                case '\u0288':
                    return '\u0074';
                //  ʈ → t    LATIN SMALL LETTER T WITH RETROFLEX HOOK → LATIN SMALL LETTER T
                case '\u1E6B':
                    return '\u0074';
                //  ṫ → t    LATIN SMALL LETTER T WITH DOT ABOVE → LATIN SMALL LETTER T
                case '\u1E6D':
                    return '\u0074';
                //  ṭ → t    LATIN SMALL LETTER T WITH DOT BELOW → LATIN SMALL LETTER T
                case '\u1E6F':
                    return '\u0074';
                //  ṯ → t    LATIN SMALL LETTER T WITH LINE BELOW → LATIN SMALL LETTER T
                case '\u1E71':
                    return '\u0074';
                //  ṱ → t    LATIN SMALL LETTER T WITH CIRCUMFLEX BELOW → LATIN SMALL LETTER T
                case '\u1E97':
                    return '\u0074';
                //  ẗ → t    LATIN SMALL LETTER T WITH DIAERESIS → LATIN SMALL LETTER T
                case '\u00F9':
                    return '\u0075';
                //  ù → u    LATIN SMALL LETTER U WITH GRAVE → LATIN SMALL LETTER U
                case '\u00FA':
                    return '\u0075';
                //  ú → u    LATIN SMALL LETTER U WITH ACUTE → LATIN SMALL LETTER U
                case '\u00FB':
                    return '\u0075';
                //  û → u    LATIN SMALL LETTER U WITH CIRCUMFLEX → LATIN SMALL LETTER U
                case '\u00FC':
                    return '\u0075';
                //  ü → u    LATIN SMALL LETTER U WITH DIAERESIS → LATIN SMALL LETTER U
                case '\u0169':
                    return '\u0075';
                //  ũ → u    LATIN SMALL LETTER U WITH TILDE → LATIN SMALL LETTER U
                case '\u016B':
                    return '\u0075';
                //  ū → u    LATIN SMALL LETTER U WITH MACRON → LATIN SMALL LETTER U
                case '\u016D':
                    return '\u0075';
                //  ŭ → u    LATIN SMALL LETTER U WITH BREVE → LATIN SMALL LETTER U
                case '\u016F':
                    return '\u0075';
                //  ů → u    LATIN SMALL LETTER U WITH RING ABOVE → LATIN SMALL LETTER U
                case '\u0171':
                    return '\u0075';
                //  ű → u    LATIN SMALL LETTER U WITH DOUBLE ACUTE → LATIN SMALL LETTER U
                case '\u0173':
                    return '\u0075';
                //  ų → u    LATIN SMALL LETTER U WITH OGONEK → LATIN SMALL LETTER U
                case '\u01B0':
                    return '\u0075';
                //  ư → u    LATIN SMALL LETTER U WITH HORN → LATIN SMALL LETTER U
                case '\u01D4':
                    return '\u0075';
                //  ǔ → u    LATIN SMALL LETTER U WITH CARON → LATIN SMALL LETTER U
                case '\u01D6':
                    return '\u0075';
                //  ǖ → u    LATIN SMALL LETTER U WITH DIAERESIS AND MACRON → LATIN SMALL LETTER U
                case '\u01D8':
                    return '\u0075';
                //  ǘ → u    LATIN SMALL LETTER U WITH DIAERESIS AND ACUTE → LATIN SMALL LETTER U
                case '\u01DA':
                    return '\u0075';
                //  ǚ → u    LATIN SMALL LETTER U WITH DIAERESIS AND CARON → LATIN SMALL LETTER U
                case '\u01DC':
                    return '\u0075';
                //  ǜ → u    LATIN SMALL LETTER U WITH DIAERESIS AND GRAVE → LATIN SMALL LETTER U
                case '\u0215':
                    return '\u0075';
                //  ȕ → u    LATIN SMALL LETTER U WITH DOUBLE GRAVE → LATIN SMALL LETTER U
                case '\u0217':
                    return '\u0075';
                //  ȗ → u    LATIN SMALL LETTER U WITH INVERTED BREVE → LATIN SMALL LETTER U
                case '\u1E73':
                    return '\u0075';
                //  ṳ → u    LATIN SMALL LETTER U WITH DIAERESIS BELOW → LATIN SMALL LETTER U
                case '\u1E75':
                    return '\u0075';
                //  ṵ → u    LATIN SMALL LETTER U WITH TILDE BELOW → LATIN SMALL LETTER U
                case '\u1E77':
                    return '\u0075';
                //  ṷ → u    LATIN SMALL LETTER U WITH CIRCUMFLEX BELOW → LATIN SMALL LETTER U
                case '\u1E79':
                    return '\u0075';
                //  ṹ → u    LATIN SMALL LETTER U WITH TILDE AND ACUTE → LATIN SMALL LETTER U
                case '\u1E7B':
                    return '\u0075';
                //  ṻ → u    LATIN SMALL LETTER U WITH MACRON AND DIAERESIS → LATIN SMALL LETTER U
                case '\u1EE5':
                    return '\u0075';
                //  ụ → u    LATIN SMALL LETTER U WITH DOT BELOW → LATIN SMALL LETTER U
                case '\u1EE7':
                    return '\u0075';
                //  ủ → u    LATIN SMALL LETTER U WITH HOOK ABOVE → LATIN SMALL LETTER U
                case '\u1EE9':
                    return '\u0075';
                //  ứ → u    LATIN SMALL LETTER U WITH HORN AND ACUTE → LATIN SMALL LETTER U
                case '\u1EEB':
                    return '\u0075';
                //  ừ → u    LATIN SMALL LETTER U WITH HORN AND GRAVE → LATIN SMALL LETTER U
                case '\u1EED':
                    return '\u0075';
                //  ử → u    LATIN SMALL LETTER U WITH HORN AND HOOK ABOVE → LATIN SMALL LETTER U
                case '\u1EEF':
                    return '\u0075';
                //  ữ → u    LATIN SMALL LETTER U WITH HORN AND TILDE → LATIN SMALL LETTER U
                case '\u1EF1':
                    return '\u0075';
                //  ự → u    LATIN SMALL LETTER U WITH HORN AND DOT BELOW → LATIN SMALL LETTER U
                case '\u028B':
                    return '\u0076';
                //  ʋ → v    LATIN SMALL LETTER V WITH HOOK → LATIN SMALL LETTER V
                case '\u1E7D':
                    return '\u0076';
                //  ṽ → v    LATIN SMALL LETTER V WITH TILDE → LATIN SMALL LETTER V
                case '\u1E7F':
                    return '\u0076';
                //  ṿ → v    LATIN SMALL LETTER V WITH DOT BELOW → LATIN SMALL LETTER V
                case '\u0175':
                    return '\u0077';
                //  ŵ → w    LATIN SMALL LETTER W WITH CIRCUMFLEX → LATIN SMALL LETTER W
                case '\u1E81':
                    return '\u0077';
                //  ẁ → w    LATIN SMALL LETTER W WITH GRAVE → LATIN SMALL LETTER W
                case '\u1E83':
                    return '\u0077';
                //  ẃ → w    LATIN SMALL LETTER W WITH ACUTE → LATIN SMALL LETTER W
                case '\u1E85':
                    return '\u0077';
                //  ẅ → w    LATIN SMALL LETTER W WITH DIAERESIS → LATIN SMALL LETTER W
                case '\u1E87':
                    return '\u0077';
                //  ẇ → w    LATIN SMALL LETTER W WITH DOT ABOVE → LATIN SMALL LETTER W
                case '\u1E89':
                    return '\u0077';
                //  ẉ → w    LATIN SMALL LETTER W WITH DOT BELOW → LATIN SMALL LETTER W
                case '\u1E98':
                    return '\u0077';
                //  ẘ → w    LATIN SMALL LETTER W WITH RING ABOVE → LATIN SMALL LETTER W
                case '\u1E8B':
                    return '\u0078';
                //  ẋ → x    LATIN SMALL LETTER X WITH DOT ABOVE → LATIN SMALL LETTER X
                case '\u1E8D':
                    return '\u0078';
                //  ẍ → x    LATIN SMALL LETTER X WITH DIAERESIS → LATIN SMALL LETTER X
                case '\u00FD':
                    return '\u0079';
                //  ý → y    LATIN SMALL LETTER Y WITH ACUTE → LATIN SMALL LETTER Y
                case '\u00FF':
                    return '\u0079';
                //  ÿ → y    LATIN SMALL LETTER Y WITH DIAERESIS → LATIN SMALL LETTER Y
                case '\u0177':
                    return '\u0079';
                //  ŷ → y    LATIN SMALL LETTER Y WITH CIRCUMFLEX → LATIN SMALL LETTER Y
                case '\u01B4':
                    return '\u0079';
                //  ƴ → y    LATIN SMALL LETTER Y WITH HOOK → LATIN SMALL LETTER Y
                case '\u0233':
                    return '\u0079';
                //  ȳ → y    LATIN SMALL LETTER Y WITH MACRON → LATIN SMALL LETTER Y
                case '\u1E8F':
                    return '\u0079';
                //  ẏ → y    LATIN SMALL LETTER Y WITH DOT ABOVE → LATIN SMALL LETTER Y
                case '\u1E99':
                    return '\u0079';
                //  ẙ → y    LATIN SMALL LETTER Y WITH RING ABOVE → LATIN SMALL LETTER Y
                case '\u1EF3':
                    return '\u0079';
                //  ỳ → y    LATIN SMALL LETTER Y WITH GRAVE → LATIN SMALL LETTER Y
                case '\u1EF5':
                    return '\u0079';
                //  ỵ → y    LATIN SMALL LETTER Y WITH DOT BELOW → LATIN SMALL LETTER Y
                case '\u1EF7':
                    return '\u0079';
                //  ỷ → y    LATIN SMALL LETTER Y WITH HOOK ABOVE → LATIN SMALL LETTER Y
                case '\u1EF9':
                    return '\u0079';
                //  ỹ → y    LATIN SMALL LETTER Y WITH TILDE → LATIN SMALL LETTER Y
                case '\u017A':
                    return '\u007A';
                //  ź → z    LATIN SMALL LETTER Z WITH ACUTE → LATIN SMALL LETTER Z
                case '\u017C':
                    return '\u007A';
                //  ż → z    LATIN SMALL LETTER Z WITH DOT ABOVE → LATIN SMALL LETTER Z
                case '\u017E':
                    return '\u007A';
                //  ž → z    LATIN SMALL LETTER Z WITH CARON → LATIN SMALL LETTER Z
                case '\u01B6':
                    return '\u007A';
                //  ƶ → z    LATIN SMALL LETTER Z WITH STROKE → LATIN SMALL LETTER Z
                case '\u0225':
                    return '\u007A';
                //  ȥ → z    LATIN SMALL LETTER Z WITH HOOK → LATIN SMALL LETTER Z
                case '\u0290':
                    return '\u007A';
                //  ʐ → z    LATIN SMALL LETTER Z WITH RETROFLEX HOOK → LATIN SMALL LETTER Z
                case '\u0291':
                    return '\u007A';
                //  ʑ → z    LATIN SMALL LETTER Z WITH CURL → LATIN SMALL LETTER Z
                case '\u1E91':
                    return '\u007A';
                //  ẑ → z    LATIN SMALL LETTER Z WITH CIRCUMFLEX → LATIN SMALL LETTER Z
                case '\u1E93':
                    return '\u007A';
                //  ẓ → z    LATIN SMALL LETTER Z WITH DOT BELOW → LATIN SMALL LETTER Z
                case '\u1E95':
                    return '\u007A';
                //  ẕ → z    LATIN SMALL LETTER Z WITH LINE BELOW → LATIN SMALL LETTER Z
                default:
                    return c;
            }
        }
    }
}