using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace RegexTesterBlazorClientSide.Pages
{
    public enum RegexSourceEnum
    {
        Pattern = 1,
        CsCode = 2
    }
    public class IndexModel : ComponentBase
    {
        public IndexModel()
        {
            this.Text = @"Lorem ipsum dolor sit amet, consectetur adipiscing
elit, sed do eiusmod tempor incididunt ut labore et
dolore magna aliqua. Ut enim ad minim veniam,
quis nostrud exercitation ullamco laboris nisi ut
aliquip ex ea commodo consequat. Duis aute irure 
dolor in reprehenderit in voluptate velit esse cillum
dolore eu fugiat nulla pariatur. Excepteur sint
occaecat cupidatat non proident, sunt in culpa qui
officia deserunt mollit anim id est laborum.";

            this.Pattern = "conse(?<lastchar>.?)";
            this.CsCode = "System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(\u0040\"conse(?<lastchar>.?)\");";
        }

        public RegexSourceEnum RegexSource { get; set; } = RegexSourceEnum.Pattern;
        public RegexOptions RegexOptions { get; set; } = RegexOptions.None;
        public bool RegexOptionsVisible { get; set; } = false;
        public string Pattern { get; set; }
        public string CsCode { get; set; }
        public string Text { get; set; }
        public bool Autorun { get; set; } = true;
        public MatchCollection Matches { get; set; }


        protected async Task RegexOptionsChanged(string optionName, bool optionValue)
        {
            Debug.WriteLine(optionName + "=" + optionValue);
            RegexOptions changedRegexOption = (RegexOptions)Enum.Parse(typeof(RegexOptions), optionName);

            if (optionValue)
            {
                RegexOptions = RegexOptions | changedRegexOption;
            }
            else
            {
                RegexOptions = RegexOptions ^ changedRegexOption;
            }

            Debug.WriteLine($"RegexOptions={(int)RegexOptions}");

            if (Autorun)
            {
                await ReCalc();
            }
        }

        protected async Task ReCalcCode(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
        }

        protected async Task ReCalcPattern(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            Pattern = (string)args.Value;

            if (Autorun)
            {
                await ReCalc();
            }
        }

        protected async Task ReCalcText(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            Text = (string)args.Value;

            if (Autorun)
            {
                await ReCalc();
            }
        }

        protected async Task ReCalc()
        {
            Debug.WriteLine("ReCalc");
            await Task.Factory.StartNew(() => {
                Matches = Regex.Matches(Text, Pattern, RegexOptions);
            });
        }

        protected MarkupString GetDecoratedTextWithMatches()
        {
            return this.GetDecoratedTextWithMatches(Text, Pattern, Matches);
        }

        public MarkupString GetDecoratedTextWithMatches(string input, string pattern, MatchCollection matches)
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