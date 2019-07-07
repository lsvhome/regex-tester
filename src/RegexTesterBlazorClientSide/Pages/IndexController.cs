using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Diagnostics;

namespace RegexTesterBlazorClientSide.Shared
{
    public class IndexController
    {
        /*
        public static string[] ff(string pattern, string text)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"abcd.*");
            var g = Regex.Matches(text, pattern);
            string h = "";
            RegexOptions s = RegexOptions.None;
            //Enum.Parse(typeof(RegexOptions),h)
            //bool c = s & Enum.GetValues(typeof(RegexOptions))
            Dictionary<string, string> RegexOpt = new Dictionary<string, string>();
            //RegexOpt.ContainsKey()
            //Enum.GetValues()
            //string optionName = "";
            //RegexOptions x = (RegexOptions)Enum.Parse(typeof(RegexOptions), optionName);
            //var g = Regex.Matches(text, pattern, regexOptions);

            foreach (Match e in g)
            {
                foreach (Group gg in e.Groups)
                {
                    //gg.Value

                    //e.Success;
                }
            }

            return null;
        }
        */
        public static MarkupString GetDecoratedTextWithMatches(string input, string pattern, MatchCollection matches)
        {
            input = input + "\n";
            CharDecorator[] decoratedInput = new CharDecorator[input.Length];
            int row = 0;
            for (int i = 0; i < input.Length; i++)
            {
                decoratedInput[i] = new CharDecorator { Symbol = input[i].ToString(), DecoratedSymbol = input[i].ToString(), Line = row };

                if (decoratedInput[i].Symbol == "\n")
                {
                    decoratedInput[i].DecoratedSymbol = "<br />";
                    row++;
                }
            }

            StringBuilder ret0 = new StringBuilder();
            StringBuilder ret1 = new StringBuilder();
            if (matches != null)
            {
                string[] names = new Regex(pattern).GetGroupNames();

                foreach (Match match in matches)
                {
                    for (int n = 0; n < names.Length; n++)
                    {
                        string groupName = names[n];
                        var group = match.Groups[groupName];
                        for (int charIndex = group.Index; charIndex < group.Index + group.Length; charIndex++)
                        {
                            decoratedInput[charIndex].DecoratedSymbol = $"<span class=\"group_{n}\">{decoratedInput[charIndex].Symbol}</span>";
                        }
                    }
                }

                for (int i = 0; i < decoratedInput.Length; i++)
                {
                    ret0.Append(string.IsNullOrWhiteSpace(decoratedInput[i].DecoratedSymbol) ? decoratedInput[i].Symbol : decoratedInput[i].DecoratedSymbol);
                }

                int last = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    if (decoratedInput[i].Symbol == "\n")
                    {
                        var q = from f in matches.Cast<Match>()
                                where f.Index >= last && f.Index + f.Length <= i
                                select f;
                        Debug.WriteLine($"{i} q.cnt={q.Count()}");


                        var qt = q.Select(each =>
                        {
                            var t = names.Select(x => $"{{{x}}}='{each.Groups[x].Value}'");
                            string s = $"[ {each.Value} : {string.Join(" | ", t)} ]";

                            return s;
                        });

                        Debug.WriteLine($"{i} qt.cnt={qt.Count()}");

                        decoratedInput[i].DecoratedSymbol = $"&nbsp;&nbsp;&nbsp;{string.Join("&nbsp;&nbsp;&nbsp;", qt)}<br />";
                        last = i;
                        ret1.Append(string.IsNullOrWhiteSpace(decoratedInput[i].DecoratedSymbol) ? decoratedInput[i].Symbol : decoratedInput[i].DecoratedSymbol);
                    }
                }

            }

            string ff = $@"
    <table width=""100%"">
        <tr>
            <td class=""text"">{(MarkupString)ret0.ToString()}</td>
            <td class=""results"">{ (MarkupString)ret1.ToString()}</td>
        </tr>
    </table>
";
            return (MarkupString)ff;// new MarkupString[2] { (MarkupString)ret0.ToString(), (MarkupString)ret1.ToString() };
        }

        public class CharDecorator
        {
            public int Line { get; set; } = 0;
            public string Symbol { get; set; }
            public string DecoratedSymbol { get; set; }
        }
    }
}
