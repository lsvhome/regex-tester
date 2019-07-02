using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace RegexTesterBlazorClientSide.Shared
{
    public class RegexHelper
    {
        public static string[] ff(string pattern, string text)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"abcd.*");
            var g = Regex.Matches(text, pattern);
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
    }
}
