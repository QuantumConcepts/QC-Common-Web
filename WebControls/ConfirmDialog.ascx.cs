using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace QuantumConcepts.Common.Web.WebControls
{
    [ParseChildren(true)]
    public partial class ConfirmDialog : System.Web.UI.UserControl
    {
        [Flags]
        public enum ConfirmDialogButton
        {
            Yes = 1,
            No = 2,
            Ok = 4,
            Cancel = 8,
            Close = 16
        }

        public delegate void DialogClosedEventHandler(ConfirmDialog sender, ConfirmDialogClosedEventArgs e);
        public event DialogClosedEventHandler DialogClosed;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel Title { get { return titlePanel; } }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Panel Message { get { return messagePanel; } }

        public ConfirmDialogButton Buttons { get; set; }
        public ConfirmDialogButton? DefaultButton { get; set; }
        public object PersistedData { get { return ViewState["PersistedData"]; } set { ViewState["PersistedData"] = value; } }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;

                if (value)
                    popupPanelExtender.Show();
                else
                    popupPanelExtender.Hide();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            yesLinkButton.Visible = IsButton(ConfirmDialogButton.Yes);
            noLinkButton.Visible = IsButton(ConfirmDialogButton.No);
            okLinkButton.Visible = IsButton(ConfirmDialogButton.Ok);
            cancelLinkButton.Visible = IsButton(ConfirmDialogButton.Cancel);
            closeLinkButton.Visible = IsButton(ConfirmDialogButton.Close);

            if (!this.DefaultButton.HasValue)
                popupPanel.DefaultButton = GetFirstVisibleButton(ConfirmDialogButton.Ok, ConfirmDialogButton.Yes, ConfirmDialogButton.No, ConfirmDialogButton.Cancel, ConfirmDialogButton.Close).ID;
            else
                popupPanel.DefaultButton = GetButton(this.DefaultButton).ID;

            base.OnPreRender(e);
        }

        protected void yesLinkButton_Click(object sender, EventArgs e)
        {
            OnDialogClosed(ConfirmDialogButton.Yes);
        }

        protected void noLinkButton_Click(object sender, EventArgs e)
        {
            OnDialogClosed(ConfirmDialogButton.No);
        }

        protected void okLinkButton_Click(object sender, EventArgs e)
        {
            OnDialogClosed(ConfirmDialogButton.Ok);
        }

        protected void cancelLinkButton_Click(object sender, EventArgs e)
        {
            OnDialogClosed(ConfirmDialogButton.Cancel);
        }

        protected void closeLinkButton_Click(object sender, EventArgs e)
        {
            OnDialogClosed(ConfirmDialogButton.Close);
        }

        private void OnDialogClosed(ConfirmDialogButton button)
        {
            if (DialogClosed != null)
                DialogClosed(this, new ConfirmDialogClosedEventArgs(button));
        }

        private bool IsButton(ConfirmDialogButton button)
        {
            return ((this.Buttons & button) == button);
        }

        private LinkButton GetFirstVisibleButton(params ConfirmDialogButton[] buttons)
        {
            foreach (ConfirmDialogButton button in buttons)
            {
                LinkButton linkButton = GetButton(button);

                if (linkButton.Visible)
                    return linkButton;
            }

            return null;
        }

        private LinkButton GetButton(ConfirmDialogButton? button)
        {
            if (!button.HasValue)
                return null;

            if (button.Value == ConfirmDialogButton.Cancel)
                return cancelLinkButton;
            else if (button.Value == ConfirmDialogButton.Close)
                return closeLinkButton;
            else if (button.Value == ConfirmDialogButton.No)
                return noLinkButton;
            else if (button.Value == ConfirmDialogButton.Ok)
                return okLinkButton;
            else if (button.Value == ConfirmDialogButton.Yes)
                return yesLinkButton;

            return null;
        }

        public class ConfirmDialogClosedEventArgs : EventArgs
        {
            private ConfirmDialogButton _button;

            public ConfirmDialogButton Button
            {
                get { return _button; }
                set { _button = value; }
            }

            public ConfirmDialogClosedEventArgs(ConfirmDialogButton button)
            {
                _button = button;
            }
        }
    }
}