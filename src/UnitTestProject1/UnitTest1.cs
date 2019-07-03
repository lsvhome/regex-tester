using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Trace.WriteLine("test trace");
            Debug.WriteLine("test debug");
            //TestContext.WriteLine("test context");
            Console.WriteLine("test console");
            ClassLibrary1.Class1.Test1();
        }

        [TestMethod]
        public void TestMethod2()
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
                    Debug.Write($"{g.GetType().Name}={g.Value} |");
                    Console.Write($"{g.Index}={g.Value} |");
                    //gg.Value
                    string s = g.Name;
                    //e.Success;
                }

                Debug.WriteLine("");
                Console.WriteLine("");
            }
        }
    }
}