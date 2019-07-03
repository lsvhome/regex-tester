using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Xunit;

namespace Regex.TesterTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
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
                    Debug.Write($"{g.Name}={g.Value} |");
                    Console.Write($"{g.Name}={g.Value} |");
                    //gg.Value
                    //g.Name
                    //e.Success;
                }

                Debug.WriteLine("");
                Console.WriteLine("");
            }
        }
    }
}
