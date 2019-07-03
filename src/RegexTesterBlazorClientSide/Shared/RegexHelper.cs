using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using System.Text;

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
        public static MarkupString Decor(string text, MatchCollection matches)
        {
            //System.Collections.Generic.
            gg[] g = new gg[text.Length];
            for(int i = 0; i< text.Length; i++)
            {
                g[i] = new gg { x = text[i].ToString() };

                if (g[i].x == "\r" || g[i].x == "\n")
                {
                    g[i].decorated = "<br />";
                }
            }

            if (matches != null)
            {
                foreach (Match m in matches)
                {
                    foreach (Group gr in m.Groups)
                    {
                        for (int i = gr.Index; i < gr.Index + gr.Length; i++)
                        {
                            g[i].decorated = "<span class=\"group_"+ gr.Name + "\">" + g[i].x + "</span>";
                        }

                    }
                }
            }

            StringBuilder ret = new StringBuilder();
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
