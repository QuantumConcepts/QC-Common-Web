using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Web.WebControls
{
    public enum PopupHorizontalAlignment
    {
        Left, Right
    }

    public enum PopupVerticalAlignment
    {
        Above, Below
    }

    public class Popup : Panel
    {
        private string _targetControlID;
        private PopupHorizontalAlignment _horizontalAlign = PopupHorizontalAlignment.Left;
        private PopupVerticalAlignment _verticalAlign = PopupVerticalAlignment.Below;
        private bool _usePositioning = true;
        private string _onShow = null;
        private string _onHide = null;

        public string TargetControlID { get { return _targetControlID; } set { _targetControlID = value; } }
        public new PopupHorizontalAlignment HorizontalAlign { get { return _horizontalAlign; } set { _horizontalAlign = value; } }
        public PopupVerticalAlignment VerticalAlign { get { return _verticalAlign; } set { _verticalAlign = value; } }
        public bool UsePositioning { get { return _usePositioning; } set { _usePositioning = value; } }
        public string OnShow { get { return _onShow; } set { _onShow = value; } }
        public string OnHide { get { return _onHide; } set { _onHide = value; } }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                Control targetControl = this.Parent.FindControl(_targetControlID);
                string onShowFunction = "null";
                string onHideFunction = "null";

                if (string.IsNullOrEmpty(_targetControlID) || targetControl == null)
                    throw new ApplicationException("Could not locate target control \"{0}.\"".FormatString(_targetControlID ?? "(null)"));

                if (!string.IsNullOrEmpty(_onShow))
                    onShowFunction = "function() { " + _onShow + " }";

                if (!string.IsNullOrEmpty(_onHide))
                    onHideFunction = "function() { " + _onHide + " }";

                this.Style.Add("display", "none");
                ScriptManager.RegisterStartupScript(this, typeof(Popup), this.ClientID, "Global.PopupManager.Create(document.getElementById(\"{0}\"), document.getElementById(\"{1}\"), \"{2}\", \"{3}\", {4}, {5}, {6});".FormatString(targetControl.ClientID, this.ClientID, _horizontalAlign.ToString(), _verticalAlign.ToString(), _usePositioning.ToString().ToLower(), onShowFunction, onHideFunction), true);
            }

            base.Render(writer);
        }
    }
}
