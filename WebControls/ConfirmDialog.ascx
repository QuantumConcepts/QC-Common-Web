<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ConfirmDialog.ascx.cs" Inherits="QuantumConcepts.Common.Web.WebControls.ConfirmDialog" %>

<div class="ModalPopupBackground">
    <asp:Button ID="fakeButton" runat="server" style="display: none;" />
    <ajax:ModalPopupExtender ID="popupPanelExtender" runat="server" TargetControlID="fakeButton" PopupControlID="popupPanel" BackgroundCssClass="ModalPopupBackground" />
    <asp:Panel ID="popupPanel" runat="server" CssClass="ConfirmDialog" style="display: none;">
        <asp:Panel ID="titlePanel" runat="server" CssClass="Header2 Title" />
        <asp:Panel ID="messagePanel" runat="server" class="Message" />
        <div class="Footer">
            <asp:LinkButton ID="yesLinkButton" runat="server" OnClick="yesLinkButton_Click" Text="Yes" Width="75px" />
            <asp:LinkButton ID="noLinkButton" runat="server" OnClick="noLinkButton_Click" Text="No" Width="75px" />
            <asp:LinkButton ID="okLinkButton" runat="server" OnClick="okLinkButton_Click" Text="Ok" Width="75px" />
            <asp:LinkButton ID="cancelLinkButton" runat="server" OnClick="cancelLinkButton_Click" Text="Cancel" Width="75px" />
            <asp:LinkButton ID="closeLinkButton" runat="server" OnClick="closeLinkButton_Click" Text="Close" Width="75px" />
        </div>
    </asp:Panel>
</div>