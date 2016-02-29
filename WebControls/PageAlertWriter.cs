using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using QuantumConcepts.Common.Web.Utils;

namespace QuantumConcepts.Common.Web.WebControls
{
    public class PageAlertWriter : PlaceHolder
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            List<PageAlert> pageAlerts = SessionUtil.Instance.PageAlerts;

            if (pageAlerts != null && pageAlerts.Count > 0)
            {
                foreach (PageAlert pageAlert in pageAlerts)
                    this.Controls.Add(pageAlert);

                SessionUtil.Instance.ClearPageAlerts();

                base.Render(writer);
            }
        }
    }
}
