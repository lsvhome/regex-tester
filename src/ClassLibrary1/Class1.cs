using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ClassLibrary1
{
    public class Class1
    {
        public static void Test1()
        {
            string text =
@"abcdeabcde
abcdeabcde";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"bcd(?<xx>.*)");
            //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"bcd(.*)");
            //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"bcd.*");
            //System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"bcd");
            var m = regex.Matches(text);

            foreach (Match e in m)
            {
                foreach (Group g in e.Groups)
                {
                    bool b = g.GetType().GetProperty("Name") != null;
                    Debug.Write($"{g.GetType().Name}={g.Value} ({b})|");
                    //Console.Write($"{g.Index}={g.Value} |");
                    //gg.Value
                    
                    //e.Success;
                }

                Debug.WriteLine("");
                Console.WriteLine("");
            }
        }
    }
}
