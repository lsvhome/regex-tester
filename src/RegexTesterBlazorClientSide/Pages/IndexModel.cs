using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexTesterBlazorClientSide.Pages
{
    public enum RegexSourceEnum
    {
        Pattern = 1,
        CsCode = 2
    }
    public class IndexModel
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
    }
}
