using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace QuantumConcepts.Common.Web.WebControls
{
    [ParseChildren(true)]
    public partial class TabularPanelRow : System.Web.UI.UserControl
    {
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<TabularPanelField> Fields { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            foreach (TabularPanelField field in this.Fields)
                fieldsPlaceHolder.Controls.Add(field);
        }
    }
}