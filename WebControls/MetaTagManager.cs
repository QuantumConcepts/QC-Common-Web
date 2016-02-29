using System.Configuration;
using System;
using System.Web.UI.WebControls;
using System.Collections;
using Microsoft.VisualBasic;
using System.Web.UI;
using System.Web;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Drawing;
using QuantumConcepts.Common.Extensions;
using QuantumConcepts.Common.Utils;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace QuantumConcepts.Common.Web
{
    namespace WebControls
    {
        public class MetaTagManager : System.Web.UI.Control
        {
            private string _description;
            private List<string> _keywords = new List<string>();
            private string _robots = "INDEX,FOLLOW,ARCHIVE";

            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }

            public string Keywords
            {
                get { return _keywords.ToCommaDelimitedString(); }
                set { _keywords.Parse(value); }
            }

            public List<string> KeywordCollection
            {
                get { return _keywords; }
                set { _keywords = value; }
            }

            public string Robots
            {
                get { return _robots; }
                set { _robots = value; }
            }

            protected override void Render(System.Web.UI.HtmlTextWriter output)
            {
                if (string.IsNullOrEmpty(_robots))
                    _robots = "INDEX,FOLLOW,ARCHIVE";

                output.Write("<meta name=\"description\" content=\"" + _description.Replace("\"", "\'").Replace("\r", "").Replace("\n", "") + "\">");
                output.Write("<meta name=\"keywords\" content=\"" + _keywords.ToCommaDelimitedString() + "\">");
                output.Write("<meta name=\"robots\" content=\"" + _robots + "\">");
            }
        }
    }
}
