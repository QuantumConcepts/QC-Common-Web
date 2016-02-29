using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace QuantumConcepts.Common.Web.WebControls
{
    [ParseChildren(true)]
    public partial class TabularPanel : UserControl, INamingContainer
    {
        public Unit Width { get { return outerPanel.Width; } set { outerPanel.Width = value; } }
        public string DefaultButton { get { return outerPanel.DefaultButton; } set { outerPanel.DefaultButton = value; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel Header { get { return headerPanel; } }
        
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<TabularPanelRow> Rows { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel Actions { get { return actionsPanel; } }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            headerPanel.Visible = (headerPanel.Controls.Count > 0);

            if (this.Rows != null)
                foreach (TabularPanelRow row in this.Rows)
                    placeHolder.Controls.Add(row);

            actionsPanel.Visible = (actionsPanel.Controls.Count > 0);
        }
    }
}