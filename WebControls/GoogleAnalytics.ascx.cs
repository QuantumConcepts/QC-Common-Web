using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuantumConcepts.Common.Extensions;
using QuantumConcepts.Common.Utils;

namespace QuantumConcepts.Common.Web.WebControls
{
    public partial class GoogleAnalytics : UserControl
    {
        public string GoogleAnalyticsAccountNumber { get { return QuantumConcepts.Common.Utils.ConfigUtil.Instance.GoogleAnalyticsAccountNumber; } }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Visible = !ConfigUtil.Instance.GoogleAnalyticsAccountNumber.IsNullOrEmpty();
        }
    }
}