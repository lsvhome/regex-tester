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
#if DEBUG
            this.Pattern = "2(?<xx>.?)";
            this.Text = "dadf 3  2s fdsf 2a g";
            this.CsCode = "System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(\u0040\"abcd.*\");";
#endif
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
