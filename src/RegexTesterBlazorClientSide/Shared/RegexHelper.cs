using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Diagnostics;

namespace RegexTesterBlazorClientSide.Shared
{
    public class RegexHelper
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
            text += "\n";
            //System.Collections.Generic.
            gg[] g = new gg[text.Length];
            for(int i = 0; i< text.Length; i++)
            {
                g[i] = new gg { x = text[i].ToString() };

                if (g[i].x == "\r" || g[i].x == "\n")
                {
                    g[i].decorated = "<br />";
                    Debug.WriteLine($"Decor 1");
                }
            }

            StringBuilder ret = new StringBuilder();
            if (matches != null)
            {
                string[] names = new Regex(pattern).GetGroupNames();
                //Console.WriteLine("Named Groups:");
                //foreach (string s in names)
                //{
                //    ret.Append(s);
                //    ret.Append("<br />");
                //}
                foreach (Match m in matches)
                {
                    foreach (string s in names)
                    {
                        var gr = m.Groups[s];
                        for (int i = gr.Index; i < gr.Index + gr.Length; i++)
                        {
                            //var b1 = m.Groups["xx"];
                            //bool b = gr[].GetType().GetProperty("Name") != null;
                            //var b = gr.ToString();
                            g[i].decorated = $"<span class=\"group_{s}\">{g[i].x}</span>";
                        }

                    }

                    //foreach (Group gr in m.Groups)
                    //{
                    //    for (int i = gr.Index; i < gr.Index + gr.Length; i++)
                    //    {
                    //        var b = m.Groups["xx"];
                    //        //bool b = gr[].GetType().GetProperty("Name") != null;
                    //        //var b = gr.ToString();
                    //        g[i].decorated = $"<span class=\"group_{gr.Index}\">{gr.GetType().Name}|{b}|{g[i].x}</span>";
                    //    }

                    //}
                }
                int last = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    if (g[i].x == "\r" || g[i].x == "\n")
                    {
                        var q = from f in matches.Cast<Match>()
                                where f.Index >= last && f.Index+f.Length<=i
                                select f;
                        Debug.WriteLine($"{i} q.cnt={q.Count()}");
                        //string k = "";
                        //foreach (var each in q)
                        //{
                        //    var t = names.Select(x => $"{{{x}}}={each.Groups[x].Value}");
                        //    string s = $"{each.Value} {string.Join(" | ", t)}";
                        //    k += s;
                        //}

                        var qt = q.Select(each => {
                            var t = names.Select(x => $"{{{x}}}='{each.Groups[x].Value}'");
                            string s = $"[ {each.Value} : {string.Join(" | ", t)} ]";

                            return s; });

                        Debug.WriteLine($"{i} qt.cnt={qt.Count()}");

                        g[i].decorated = $"&nbsp;&nbsp;&nbsp;{string.Join("&nbsp;&nbsp;&nbsp;", qt)}<br />";
                        last = i;
                    }
                }
            }

            for (int i = 0; i < g.Length; i++)
            {
                ret.Append(string.IsNullOrWhiteSpace(g[i].decorated) ? g[i].x : g[i].decorated);
            }
            return (MarkupString)ret.ToString();
            //return (MarkupString)Regex.Replace(text, "(\r)|(\n)", "<br />");
        }

        class gg
        {
            public string x;
            public string decorated;
        }
    }
}
