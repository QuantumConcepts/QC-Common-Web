using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantumConcepts.Common.Utils;
using System.Web.UI.WebControls;

namespace QuantumConcepts.Common.Web.WebControls
{
    internal class PageLink : HyperLink
    {
        public ResourceUrl Url { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.Url != null)
                this.NavigateUrl = this.Url.RelativeUrl;
        }
    }
}