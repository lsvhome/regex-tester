namespace RegexTesterBlazorClientSide.Pages
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Components;
    using Microsoft.CodeAnalysis;

    public enum RegexSourceEnum
    {
        /// <summary>
        /// Use pattern for matches
        /// </summary>
        Pattern = 1,

        /// <summary>
        /// Use c# code for matches
        /// </summary>
        CsCode = 2,
    }

    public class IndexModel : ComponentBase
    {
        private RegexSourceEnum regexSource = RegexSourceEnum.Pattern;

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
            this.CsCode = "string pattern = \u0040\"dolo(?<lastchar>.?)\";";
        }

        [Inject]
        private HttpClient HttpClient { get; set; }

        public RegexSourceEnum RegexSource
        {
            get
            {
                return this.regexSource;
            }

            set
            {
                this.regexSource = value;
                this.Matches = null;
            }
        }

        public RegexOptions RegexOptions { get; set; } = RegexOptions.None;

        public bool RegexOptionsVisible { get; set; } = false;

        public string Pattern { get; set; }

        public string CsCode { get; set; }

        public string Text { get; set; }

        public bool Autorun { get; set; } = true;

        public MatchCollection Matches { get; set; }

        public string Error { get; set; }

        protected async Task RegexOptionsChanged(string optionName, bool optionValue)
        {
            Debug.WriteLine(optionName + "=" + optionValue);
            RegexOptions changedRegexOption = (RegexOptions)Enum.Parse(typeof(RegexOptions), optionName);

            if (optionValue)
            {
                this.RegexOptions = this.RegexOptions | changedRegexOption;
            }
            else
            {
                this.RegexOptions = this.RegexOptions ^ changedRegexOption;
            }

            Debug.WriteLine($"RegexOptions={(int)this.RegexOptions}");

            if (this.Autorun)
            {
                await this.ReCalc();
            }
        }

        protected async Task ReCalcCode(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            this.CsCode = (string)args.Value;

            if (this.Autorun)
            {
                await this.ReCalc();
            }
        }

        protected async Task ReCalcPattern(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            this.Pattern = (string)args.Value;

            if (this.Autorun)
            {
                await this.ReCalc();
            }
        }

        protected async Task ReCalcText(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            this.Text = (string)args.Value;

            if (this.Autorun)
            {
                await this.ReCalc();
            }
        }

        protected async Task ReCalc()
        {
            Debug.WriteLine("ReCalc");
            this.Matches = null;
            this.Error = null;
            if (this.RegexSource == RegexSourceEnum.Pattern)
            {
                Debug.WriteLine("Info #92387689506: Pattern");
                await Task.Factory.StartNew(() =>
                {
                    this.Matches = Regex.Matches(this.Text, this.Pattern, this.RegexOptions);
                });
            }
            else if (this.RegexSource == RegexSourceEnum.CsCode)
            {
                Debug.WriteLine("Info #92387689506: Code");
                Compiler.WhenReady(this.CompileCsCodeAndRunInternal);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected MarkupString GetDecoratedTextWithMatches()
        {
            return this.GetDecoratedTextWithMatches(this.Text, this.Pattern, this.Matches);
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

            string result = $@"
    <table width=""100%"">
        <tr>
            <td class=""text"">{(MarkupString)ret0.ToString()}</td>
            <td class=""results"">{(MarkupString)ret1.ToString()}</td>
        </tr>
    </table>
";
            return (MarkupString)result;
        }

        protected override Task OnInitAsync()
        {
            Compiler.InitializeMetadataReferences(this.HttpClient);
            return base.OnInitAsync();
        }

        private async Task CompileCsCodeAndRunInternal()
        {
            this.Matches = null;
            string code = @"using System;
class Program
{
    public static void Main(){}
    public static string GetPattern()
    {"
    + this.CsCode +
@"
    return pattern;
    }
}";
            Debug.WriteLine("Compiling and Running code");

            try
            {
                var (success, asm) = Compiler.LoadSource(code);
                if (success)
                {
                    Debug.WriteLine("Sumpilation succeeded");
                    MethodInfo entry = asm.DefinedTypes.Single().GetMethod("GetPattern", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static); // reflect for the async Task Main

                    var hasArgs = entry.GetParameters().Length > 0;
                    var result = entry.Invoke(null, hasArgs ? new object[] { this.Text } : null);
                    if (result is Task t)
                    {
                        Debug.WriteLine("Method going to be invoked");
                        await t;
                        Debug.WriteLine("Method invoked");
                    }
                    else
                    {
                        if (result is string pattern)
                        {
                            this.Matches = System.Text.RegularExpressions.Regex.Matches(this.Text, pattern);
                            Debug.WriteLine("Matches replaced");
                        }
                        else
                        {
                            Debug.WriteLine($"Wrong result type: {result?.GetType()?.Name}");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Compiler.LoadSource Fails: \r\n" + code);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.ToString());
            }

            this.StateHasChanged();
            Debug.WriteLine("Compiling and Running code ENd");
        }

        public class CharDecorator
        {
            public int Line { get; set; } = 0;

            public string Symbol { get; set; }

            public string DecoratedSymbol { get; set; }
        }
    }
}