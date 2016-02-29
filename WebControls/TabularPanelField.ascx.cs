using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuantumConcepts.Common.Models;

namespace QuantumConcepts.Common.Web.WebControls
{
    [ParseChildren(true)]
    public partial class TabularPanelField : System.Web.UI.UserControl
    {
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel Label { get { return labelPanel; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel Value { get { return valuePanel; } }
    }
}