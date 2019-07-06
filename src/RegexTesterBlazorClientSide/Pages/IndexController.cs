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
        public static string[] ff(string pattern, string text)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"abcd.*");
            var g = Regex.Matches(text, pattern);
            string h = "";
            RegexOptions s = RegexOptions.None;
            //Enum.Parse(typeof(RegexOptions),h)
            //bool c = s & Enum.GetValues(typeof(RegexOptions))
            Dictionary<string, string> RegexOpt  = new Dictionary<string, string>();
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
        public static MarkupString Decor(string text,string pattern, MatchCollection matches)
        {
            Debug.WriteLine($"Decor 0");
            //text = text.Replace("\r\n","\n") + "\n";
            text = text + "\n";
            gg[] g = new gg[text.Length];
            int row = 0;
            for(int i = 0; i< text.Length; i++)
            {
                g[i] = new gg { x = text[i].ToString(), decorated = text[i].ToString() };

                if (g[i].x == "\n")
                {
                    g[i].decorated = "<br />";
                    g[i].row = row;
                    Debug.WriteLine($"Decor 1");
                    row++;
                }
            }

            StringBuilder ret0 = new StringBuilder();
            StringBuilder ret1 = new StringBuilder();
            if (matches != null)
            {
                string[] names = new Regex(pattern).GetGroupNames();

                foreach (Match m in matches)
                {
                    //foreach (string s in names)
                    for (int n = 0; n < names.Length; n++)
                    {
                        string s = names[n];
                        var gr = m.Groups[s];
                        for (int i = gr.Index; i < gr.Index + gr.Length; i++)
                        {
                            //var b1 = m.Groups["xx"];
                            //bool b = gr[].GetType().GetProperty("Name") != null;
                            //var b = gr.ToString();
                            g[i].decorated = $"<span class=\"group_{n}\">{g[i].x}</span>";
                        }

                    }
                }

                for (int i = 0; i < g.Length; i++)
                {
                    ret0.Append(string.IsNullOrWhiteSpace(g[i].decorated) ? g[i].x : g[i].decorated);
                }

                //ret0 = ret0.Replace("<br />< br />", "<br />");

                int last = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    if (g[i].x == "\n")
                    {
                        var q = from f in matches.Cast<Match>()
                                where f.Index >= last && f.Index+f.Length<=i
                                select f;
                        Debug.WriteLine($"{i} q.cnt={q.Count()}");


                        var qt = q.Select(each => {
                            var t = names.Select(x => $"{{{x}}}='{each.Groups[x].Value}'");
                            string s = $"[ {each.Value} : {string.Join(" | ", t)} ]";

                            return s; });

                        Debug.WriteLine($"{i} qt.cnt={qt.Count()}");

                        g[i].decorated = $"&nbsp;&nbsp;&nbsp;{string.Join("&nbsp;&nbsp;&nbsp;", qt)}<br />";
                        last = i;
                        ret1.Append(string.IsNullOrWhiteSpace(g[i].decorated) ? g[i].x : g[i].decorated);
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

        class gg
        {
            public int row = 0;
            public string x;
            public string decorated;
        }
    }
}
