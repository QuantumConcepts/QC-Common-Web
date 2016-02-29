using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using QuantumConcepts.Common.Extensions;
using QuantumConcepts.Common.DataObjects;
using QuantumConcepts.Common.Utils;
using QuantumConcepts.Common.Web.Utils;
using System.Collections;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace QuantumConcepts.Common.Web.WebControls
{
    public abstract class BasePage<UserType> : System.Web.UI.Page where UserType : IUser
    {
        private UserType _authenticatedUser;

        public UserType AuthenticatedUser { get { return _authenticatedUser; } protected set { _authenticatedUser = value; } }
        public virtual bool RequireSecurity { get { return false; } }
        public virtual bool RequireAuthentication { get { return false; } }
        protected virtual bool SSLEnabled { get { return false; } }
        public string ThemePath { get { return "~/App_Themes/" + this.Theme + "/"; } }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _authenticatedUser = GetAuthenticatedUser();

            if (this.RequireSecurity && !this.Request.IsSecureConnection && this.SSLEnabled)
            {
                //TODO
            }

            if (this.RequireAuthentication && _authenticatedUser == null)
            {
                //TODO
            }

            if (!ValidatePermission())
            {
                //TODO
            }

            ValidateAndExtractParameters();
        }

        protected virtual UserType GetAuthenticatedUser() { return default(UserType); }

        protected virtual bool ValidatePermission() { return true; }

        protected virtual void ValidateAndExtractParameters() { }

        public string GetParameter(string name)
        {
            return RequestUtil.GetParameter(HttpContext.Current, name);
        }

        public string[] GetParameters(string name)
        {
            return RequestUtil.GetParameters(HttpContext.Current, name);
        }

        protected bool ValidateAndExtractParameter(string name, bool required, ref string value)
        {
            if (!RequestUtil.ValidateAndExtractParameter(HttpContext.Current, name, ref value))
            {
                if (required)
                    SetMissingParameterPageAlert(name);

                return false;
            }

            return true;
        }

        protected bool ValidateAndExtractIntParameter(string name, bool required, ref int value)
        {
            if (!RequestUtil.ValidateAndExtractIntParameter(HttpContext.Current, name, ref value))
            {
                if (required)
                    SetMissingParameterPageAlert(name);

                return false;
            }

            return true;
        }

        protected bool ValidateAndExtractNullableIntParameter(string name, bool required, ref int? value)
        {
            if (!RequestUtil.ValidateAndExtractNullableIntParameter(HttpContext.Current, name, ref value))
            {
                if (required)
                    SetMissingParameterPageAlert(name);

                return false;
            }

            return true;
        }

        protected bool ValidateAndExtractBoolParameter(string name, bool required, ref bool value)
        {
            if (!RequestUtil.ValidateAndExtractBoolParameter(HttpContext.Current, name, ref value))
            {
                if (required)
                    SetMissingParameterPageAlert(name);

                return false;
            }

            return true;
        }

        protected bool ValidateAndExtractNullableBoolParameter(string name, bool required, ref bool? value)
        {
            if (!RequestUtil.ValidateAndExtractNullableBoolParameter(HttpContext.Current, name, ref value))
            {
                if (required)
                    SetMissingParameterPageAlert(name);

                return false;
            }

            return true;
        }

        protected bool ValidateAndExtractEnumParameter<T>(string name, bool required, ref T value)
            where T : struct
        {
            if (!RequestUtil.ValidateAndExtractEnumParameter<T>(HttpContext.Current, name, ref value))
            {
                if (required)
                    SetMissingParameterPageAlert(name);

                return false;
            }

            return true;
        }

        protected bool ValidateAndExtractNullableEnumParameter<T>(string name, bool required, ref T? value)
            where T : struct
        {
            if (!RequestUtil.ValidateAndExtractNullableEnumParameter(HttpContext.Current, name, ref value))
            {
                if (required)
                    SetMissingParameterPageAlert(name);

                return false;
            }

            return true;
        }

        protected void SetMissingParameterPageAlert(string name)
        {
            SessionUtil.Instance.AddPageAlert(new PageAlert("Invalid Parameter", "The required parameter \"{0}\" was not supplied.".FormatString(name), PageAlert.PageAlertDisplayMode.Inline, PageAlert.PageAlertSeverity.Error));
            UrlUtil.RedirectToSourceOrReferer();
        }

        protected void SetInvalidParameterPageAlert(string name)
        {
            SessionUtil.Instance.AddPageAlert(new PageAlert("Invalid Parameter", "The required parameter \"{0}\" is not valid.".FormatString(name), PageAlert.PageAlertDisplayMode.Inline, PageAlert.PageAlertSeverity.Error));
            UrlUtil.RedirectToSourceOrReferer();
        }

        /// <summary>Prepares the output stream to send a downloadable file.</summary>
        public void BeginSendDownloadFile(string filename, string contentType)
        {
            Response.AddHeader("Content-Disposition", "attachment; filename={0}".FormatString(filename));
            Response.ContentType = contentType;
        }

        /// <summary>Flushes and closes the output stream.</summary>
        public void EndSendDownloadFile()
        {
            Response.Flush();
            Response.Close();
        }

        public string MapThemePath(string virtualPath)
        {
            return "{0}{1}".FormatString(this.ThemePath, virtualPath);
        }

        protected void BindDropDownList(DropDownList dropDownList, IEnumerable dataSource)
        {
            if (!dataSource.IsNullOrEmpty())
            {
                dropDownList.DataSource = dataSource;
                dropDownList.DataBind();
            }
        }

        protected void SelectDropDownListItem(DropDownList dropDownList, string value)
        {
            if (dropDownList.Items.FindByValue(value) != null)
                dropDownList.SelectedValue = value;
        }
    }
}
