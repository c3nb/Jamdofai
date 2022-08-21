﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jamdofai
{
    public static class StringUtils
    {
        public const int Upper = 65;
        public const int UpperLast = 90;
        public const int Lower = 97;
        public const int LowerLast = 122;
        public const int ULDiff = Lower - Upper;
        public const char Undefined = char.MinValue;
        public static readonly Random Random = new Random(DateTime.Now.Millisecond);
        public static readonly Regex English = new Regex("^[A-Za-z]*$", RegexOptions.Compiled);
        public static readonly int[] Tops = { 0x3131, 0x3132, 0x3134, 0x3137, 0x3138, 0x3139, 0x3141, 0x3142, 0x3143, 0x3145, 0x3146, 0x3147, 0x3148, 0x3149, 0x314a, 0x314b, 0x314c, 0x314d, 0x314e };
        public static readonly int[] Mids = { 0x314f, 0x3150, 0x3151, 0x3152, 0x3153, 0x3154, 0x3155, 0x3156, 0x3157, 0x3158, 0x3159, 0x315a, 0x315b, 0x315c, 0x315d, 0x315e, 0x315f, 0x3160, 0x3161, 0x3162, 0x3163 };
        public static readonly int[] Bots = { 0, 0x3131, 0x3132, 0x3133, 0x3134, 0x3135, 0x3136, 0x3137, 0x3139, 0x313a, 0x313b, 0x313c, 0x313d, 0x313e, 0x313f, 0x3140, 0x3141, 0x3142, 0x3144, 0x3145, 0x3146, 0x3147, 0x3148, 0x314a, 0x314b, 0x314c, 0x314d, 0x314e };
        public static readonly char[] TopChars = { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
        public static readonly char[] MidChars = { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
        public static readonly char[] BotChars = { '\0', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
        public static bool IsUpper(this char c) => c >= Upper && c <= UpperLast;
        public static bool IsLower(this char c) => c >= Lower && c <= LowerLast;
        public static bool IsAlphabet(this char c) => IsUpper(c) || IsLower(c);
        public static char ToUpper(this char c) => c.IsUpper() ? c : (char)(c - 32);
        public static char ToLower(this char c) => c.IsLower() ? c : (char)(c + 32);
        public static char Invert(this char c) => IsUpper(c) ? c.ToLower() : c.ToUpper();
        public static unsafe string Invert(this string s)
        {
            if (s == null || s.Length <= 0) return s;
            fixed (char* ptr = s)
            {
                char* c = ptr;
                char v;
                while ((v = *c) != '\0')
                {
                    if (IsAlphabet(v))
                        *c = v.Invert();
                    c++;
                }
            }
            return s;
        }
        public static unsafe string InvertForce(this string s)
        {
            if (s == null || s.Length <= 0) return s;
            bool isLower = s[0].IsLower();
            fixed (char* ptr = s)
            {
                char* c = ptr;
                char v;
                while ((v = *c) != '\0')
                {
                    if (IsAlphabet(v))
                        *c = (isLower = !isLower) ? v.ToLower() : v.ToUpper();
                    c++;
                }
            }
            return s;
        }
        public static unsafe string ToUpperFast(this string s)
        {
            fixed (char* ptr = s)
            {
                char* c = ptr;
                char v;
                while ((v = *c) != '\0')
                {
                    if (IsAlphabet(v))
                        *c = v.ToUpper();
                    c++;
                }
            }
            return s;
        }
        public static unsafe string ToLowerFast(this string s)
        {
            fixed (char* ptr = s)
            {
                char* c = ptr;
                char v;
                while ((v = *c) != '\0')
                {
                    if (IsAlphabet(v))
                        *c = v.ToLower();
                    c++;
                }
            }
            return s;
        }
        public static string BreakGrammar(this string s)
        {
            if (s == null) return null;
            List<char> letterList = new List<char>();
            List<char> result = new List<char>();
            for (var i = 0; i < s.Length; i++)
            {
                int unicodeDec = s[i];
                if (unicodeDec < 12593)
                {
                    letterList.Add(s[i]);
                    letterList.Add(Undefined);
                    letterList.Add(Undefined);
                    continue;
                }
                else if (unicodeDec < 44032)
                {
                    letterList.Add(s[i]);
                    letterList.Add(Undefined);
                    letterList.Add(Undefined);
                    continue;
                }
                int letterIndex = unicodeDec - 44032;
                int topIndex = letterIndex / (21 * 28);
                int midIndex = letterIndex % (21 * 28) / 28;
                int botIndex = letterIndex % (21 * 28) % 28;
                letterList.Add(TopChars.Length <= topIndex ? Undefined : TopChars[topIndex]);
                letterList.Add(MidChars.Length <= midIndex ? Undefined : MidChars[midIndex]);
                letterList.Add(BotChars.Length <= botIndex ? Undefined : BotChars[botIndex]);
            }

            for (var i = 0; i < letterList.Count; i += 3)
            {
                if (((letterList[i] + "")[0] < 44032) && letterList.Count <= i + 1)
                {
                    result.Add(letterList[i]);
                    continue;
                }
                char top = letterList[i];
                char mid = letterList.Count <= i + 1 ? Undefined : letterList[i + 1];
                char bot = letterList.Count <= i + 2 ? Undefined : letterList[i + 2];
                if ((int)(Random.NextDouble() * 10000000 % 13) == 12)
                {
                    result.Add(top);
                    if (mid != Undefined)
                        result.Add(mid.BreakMidGrammar());
                    if (bot != Undefined)
                        result.Add(bot.BreakBotGrammar());
                    continue;
                }
                else
                {
                    if (mid != Undefined)
                    {
                        if (bot != Undefined)
                            result.Add((char)(((TopIndex(top) * 21) + MidIndex(mid)) * 28 + BotIndex(bot) + 44032));
                        else
                            result.Add((char)((((TopIndex(top) * 21) + MidIndex(mid)) * 28) + 44032));
                    }
                    else result.Add(top);
                }
            }
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < result.Count; i++)
                sb.Append(result[i].BreakWordGrammar());
            return sb.ToString().BreakStringGrammar();
        }
        public static string Separate(this string str)
        {
            if (str == null) return null;
            int a, b, c, x;
            StringBuilder sb = new StringBuilder();
            for (int cnt = 0; cnt < str.Length; cnt++)
            {
                x = str[cnt];
                if (x >= 0xAC00 && x <= 0xD7A3)
                {
                    c = x - 0xAC00;
                    a = c / (21 * 28);
                    c %= 21 * 28;
                    b = c / 28;
                    c %= 28;
                    sb.Append($"{(char)Tops[a]}{(char)Mids[b]}");
                    if (c != 0) sb.Append((char)Bots[c]);
                }
                else sb.Append((char)x);
            }
            return sb.ToString();
        }
        #region Private
        private static char BreakMidGrammar(this char c)
        {
            switch (c)
            {
                case 'ㅔ': return 'ㅐ';
                case 'ㅐ': return 'ㅔ';
                case 'ㅙ': return 'ㅚ';
                case 'ㅚ': return 'ㅙ';
                case 'ㅖ': return 'ㅒ';
                case 'ㅒ': return 'ㅖ';
                case 'ㅛ': return 'ㅕ';
                default: return c;
            }
        }
        private static char BreakBotGrammar(this char c)
        {
            switch (c)
            {
                case 'ㅅ': return 'ㅌ';
                case 'ㅆ': return 'ㅈ';
                case 'ㅎ': return 'ㅅ';
                case 'ㅉ': return 'ㅈ';
                case 'ㅄ': return 'ㅂ';
                default: return c;
            }
        }
        private static char BreakWordGrammar(this char c)
        {
            switch (c)
            {
                case '떡': return '떻';
                case '안': return '않';
                case '괜': return '괞';
                case '찮': return '찬';
                case '떻': return '떡';
                case '송': return '성';
                default:
                    return c;
            }
        }
        private static string BreakStringGrammar(this string s)
        {
            StringBuilder sb = new StringBuilder();
            if (s.Contains("..."))
            {
                s = s.Replace("...", "");
                int rand = (int)(Random.NextDouble() * 10000 % 5);
                for (int i = -7; i < rand; i++)
                {
                    if ((int)(Random.NextDouble() * 10000 % 5) == 4)
                        sb.Append('ㅜ');
                    sb.Append('ㅠ');
                }
                rand = (int)(Random.NextDouble() * 10000 % 4);
                for (var i = -7; i < rand; i++)
                {
                    if ((int)(Random.NextDouble() * 10000 % 5) == 4)
                        sb.Append("😢");
                    sb.Append("😭");
                }
            }
            else if (s.Contains("!!!"))
            {
                var rand = (int)(Random.NextDouble() * 10000 % 5);
                for (var i = -7; i < rand; i++)
                {
                    if ((int)(Random.NextDouble() * 10000 % 5) == 4)
                        sb.Append('@');
                    sb.Append(';');
                }
                rand = (int)(Random.NextDouble() * 10000 % 4);
                for (var i = -7; i < rand; i++)
                {
                    if ((int)(Random.NextDouble() * 10000 % 5) == 4)
                        sb.Append('!');
                    sb.Append("🤬");
                }
            }
            if (English.IsMatch(s))
                return s.ToLowerFast().Replace("ear", "are").Replace("ea", "ae").Replace("ar", "er").Replace("ey", "i").Replace("el", "le").Replace("ight", "yt").Replace("ei", "i").Replace("ie", "e").Replace("ll", "l").Replace("ou", "oau").Replace("au", "oau").Replace("ay", "aye").Replace("rt", "t").Replace("ntr", "NTR").Replace("er", "or").Replace("아니", "않이").Replace("읽으", "일그").Replace("합니", "함미").Replace("습니", "슴미") + sb.ToString();
            return s.Replace("아니", "않이").Replace("읽으", "일그").Replace("합니", "함미").Replace("습니", "슴미") + sb.ToString();
        }
        private static int TopIndex(this char c) => Array.FindIndex(Tops, t => t == c);
        private static int MidIndex(this char c)
        {
            char breaked = c.BreakMidGrammar();
            return Array.FindIndex(Mids, t => t == breaked);
        }
        private static int BotIndex(this char c)
        {
            char breaked = c.BreakBotGrammar();
            return Array.FindIndex(Bots, t => t == breaked);
        }
        #endregion
    }
}
