using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI.Design;
using QuantumConcepts.Common.Utils;

namespace QuantumConcepts.Common.Web.WebControls
{
    public class FieldValidator : PlaceHolder
    {
        private string _targetControlID = null;
        private string _comparisonControlID = null;
        private string _fieldName = null;
        private string _fieldFormatDescription = null;
        private bool _required = true;
        private string _validationRegex = null;
        private RegexUtil.KnownRegex? _knownValidationRegex = null;
        private string _clientValidationFunction = null;
        private string _mask = null;
        private Unit _popupWidth = new Unit(200, UnitType.Pixel);
        private string _validationGroup = null;

        public string TargetControlID
        {
            get { return _targetControlID; }
            set { _targetControlID = value; }
        }

        public string ComparisonControlID
        {
            get { return _comparisonControlID; }
            set { _comparisonControlID = value; }
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public string FieldFormatDescription
        {
            get { return _fieldFormatDescription; }
            set { _fieldFormatDescription = value; }
        }

        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }

        public string ValidationRegex
        {
            get { return _validationRegex; }
            set { _validationRegex = value; }
        }

        public RegexUtil.KnownRegex? KnownValidationRegex
        {
            get { return _knownValidationRegex; }
            set { _knownValidationRegex = value; }
        }

        public string ClientValidationFunction
        {
            get { return _clientValidationFunction; }
            set { _clientValidationFunction = value; }
        }

        public string Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }

        public Unit PopupWidth
        {
            get { return _popupWidth; }
            set { _popupWidth = value; }
        }

        public string ValidationGroup
        {
            get { return _validationGroup; }
            set { _validationGroup = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            bool isComparison = !string.IsNullOrEmpty(_comparisonControlID);

            if (string.IsNullOrEmpty(_targetControlID))
                throw new ApplicationException("Required parameter TargetControlID not specified.");

            if (string.IsNullOrEmpty(_fieldName))
                throw new ApplicationException("Required parameter FieldName not specified.");

            if (_required || isComparison)
            {
                RequiredFieldValidator rfv = new RequiredFieldValidator();

                rfv.ID = this.ID + "RFV";
                rfv.ControlToValidate = _targetControlID;
                rfv.Display = ValidatorDisplay.None;
                rfv.ErrorMessage = "<b>Required Field</b><br/>" + _fieldName + " is a required field.";
                rfv.ValidationGroup = _validationGroup;

                this.Controls.Add(rfv);
                this.Controls.Add(GetExtender(rfv));
            }

            if (isComparison)
            {
                CompareValidator cv = new CompareValidator();

                cv.ID = this.ID + "CV";
                cv.ControlToValidate = _targetControlID;
                cv.ControlToCompare = _comparisonControlID;
                cv.Display = ValidatorDisplay.None;
                cv.ErrorMessage = "<b>Field Mismatch</b><br/>The " + _fieldName + " fields do not match.";
                cv.EnableClientScript = true;
                cv.ValidationGroup = _validationGroup;
                
                this.Controls.Add(cv);
                this.Controls.Add(GetExtender(cv));
            }
            else
            {
                if (!string.IsNullOrEmpty(_mask) && string.IsNullOrEmpty(_validationRegex) && !_knownValidationRegex.HasValue)
                    throw new ApplicationException("When the Mask parameter is set, the ValidationRegex or KnownValidationRegex must be set as well.");

                if (!string.IsNullOrEmpty(_mask))
                {
                    MaskedEditExtender mex = new MaskedEditExtender();

                    mex.ID = this.ID + "MEX";
                    mex.TargetControlID = _targetControlID;
                    mex.Mask = _mask;
                    mex.ClearMaskOnLostFocus = false;
                    mex.AutoComplete = false;
                    mex.PromptCharacter = "_";

                    this.Controls.Add(mex);
                }

                if (_knownValidationRegex.HasValue || !string.IsNullOrEmpty(_validationRegex))
                {
                    RegularExpressionValidator rev = new RegularExpressionValidator();

                    rev.ID = this.ID + "REV";
                    rev.ControlToValidate = _targetControlID;
                    rev.Display = ValidatorDisplay.None;
                    rev.ErrorMessage = GetInvalidFieldDescription();
                    rev.EnableClientScript = true;
                    rev.ValidationGroup = _validationGroup;

                    if (_knownValidationRegex.HasValue)
                        rev.ValidationExpression = RegexUtil.GetRegexStringForJavaScript(_knownValidationRegex.Value);
                    else
                        rev.ValidationExpression = _validationRegex;

                    this.Controls.Add(rev);
                    this.Controls.Add(GetExtender(rev));
                }
            }

            if (!string.IsNullOrEmpty(_clientValidationFunction))
            {
                CustomValidator cuv = new CustomValidator();

                cuv.ID = this.ID + "CUV";
                cuv.ControlToValidate = _targetControlID;
                cuv.ClientValidationFunction = _clientValidationFunction;
                cuv.Display = ValidatorDisplay.None;
                cuv.ErrorMessage = GetInvalidFieldDescription();
                cuv.EnableClientScript = true;
                cuv.ValidationGroup = _validationGroup;

                this.Controls.Add(cuv);
                this.Controls.Add(GetExtender(cuv));
            }

            base.OnLoad(e);
        }

        private string GetInvalidFieldDescription()
        {
            return "<b>Invalid Field</b><br/>The " + _fieldName + " field is not valid." + (string.IsNullOrEmpty(_fieldFormatDescription) ? "" : "<br/>The format for this field is: " + _fieldFormatDescription);
        }

        private ValidatorCalloutExtender GetExtender(BaseValidator target)
        {
            ValidatorCalloutExtender extender = new ValidatorCalloutExtender();

            extender.ID = target.ID + "Extender";
            extender.TargetControlID = target.ID;
            extender.Width = _popupWidth;
            extender.WarningIconImageUrl = "~/Resources/Images/Question-Button.png";
            extender.CloseImageUrl = "~/Resources/Images/X-Button-Small.png";

            return extender;
        }
    }
}