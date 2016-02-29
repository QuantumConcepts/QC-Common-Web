using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using QuantumConcepts.Common.Extensions;
using QuantumConcepts.Common.Utils;

namespace QuantumConcepts.Common.Web.WebControls
{
    public class PageAlert : Control
    {
        public enum PageAlertDisplayMode
        {
            Inline,
            Popup
        }

        public enum PageAlertSeverity
        {
            Information,
            Exclamation,
            Error
        }

        private string _title;
        private string _body = null;
        private PageAlertDisplayMode _method;
        private PageAlertSeverity _mode;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public PageAlertDisplayMode Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public PageAlertSeverity Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public PageAlert() {}

        public PageAlert(string title, string body, PageAlertDisplayMode method, PageAlertSeverity mode)
            : this(title, body, null, method, mode)
        { }

        public PageAlert(string title, Exception exception, PageAlertDisplayMode method, PageAlertSeverity mode)
            : this(title, null, exception, method, mode)
        { }

        public PageAlert(string title, string body, Exception exception, PageAlertDisplayMode method, PageAlertSeverity mode)
        {
            _title = title;
            _method = method;
            _mode = mode;
            _body = GetMessage(body, exception);
        }

        protected override void Render(HtmlTextWriter htmlWriter)
        {
            if (!string.IsNullOrEmpty(_body))
            {
                if (_method == PageAlertDisplayMode.Inline)
                {
                    string cssClass = null;
                    HtmlGenericControl outerDiv = new HtmlGenericControl("div");
                    HtmlGenericControl textDiv = new HtmlGenericControl("div");

                    if (_mode == PageAlertSeverity.Exclamation)
                        cssClass = "Exclamation";
                    else if (_mode == PageAlertSeverity.Error)
                        cssClass = "Error";
                    else
                        cssClass = "Information";

                    outerDiv.Attributes.Add("class", "PageAlert");

                    if (!string.IsNullOrEmpty(_title))
                    {
                        HtmlGenericControl titleDiv = new HtmlGenericControl("div");

                        titleDiv.Attributes.Add("class", "Title " + cssClass);
                        titleDiv.InnerHtml = _title;
                        outerDiv.Controls.Add(titleDiv);
                    }

                    textDiv.Attributes.Add("class", "Body " + cssClass);
                    textDiv.InnerHtml = _body;
                    outerDiv.Controls.Add(textDiv);
                    this.Controls.Add(outerDiv);
                }
                else
                    htmlWriter.Write("<script language=\"javascript\">alert(\"" + (string.IsNullOrEmpty(_title) ? "" : Escape(_title) + "\n\n") + Escape(_body) + "\"); </script>");

                this.Visible = true;
            }
            else
                this.Visible = false;

            base.Render(htmlWriter);
        }

        private static string Escape(string textToEscape)
        {
            return textToEscape.Replace("\"", "\\\"");
        }

        private string GetMessage(string body, Exception exception)
        {
            string breakText = (_method == PageAlertDisplayMode.Inline ? "<br/>" : "\\n");
            StringBuilder message = new StringBuilder();

            if (!string.IsNullOrEmpty(body))
                message.Append(body);
#if (DEBUG)
            if (exception != null)
            {
                if (_method == PageAlertDisplayMode.Inline)
                    message.Append("<br/><br/><h3>Exception</h3>" + exception.ToString().Replace("\r\n", "<br />"));
                else
                    message.Append("\n\nException:\n" + exception.ToString());
            }
#endif
            return message.ToString();
        }

        private string GetExceptionText(Exception exception)
        {
            if (_method == PageAlertDisplayMode.Inline)
                return ExceptionUtil.GetExceptionText(exception, "<br /><br />");
            else
                return ExceptionUtil.GetExceptionText(exception, "\n\n");
        }

        public static PageAlert CreateForInvalidParameter(string parameterName)
        {
            return new PageAlert("Invald Parameter", "The parameter \"{0}\" was either not supplied or is invalid.".FormatString(parameterName), PageAlertDisplayMode.Inline, PageAlertSeverity.Error);
        }

        public static PageAlert CreateForEditError(string action, string objectType, Exception ex)
        {
            return new PageAlert("Unexpected Error", "An unexpected error occurred while {0} the \"{1}.\"".FormatString(action, objectType), ex, PageAlertDisplayMode.Inline, PageAlertSeverity.Error);
        }
    }
}
