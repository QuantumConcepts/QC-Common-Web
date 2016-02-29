<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabularPanel.ascx.cs" Inherits="QuantumConcepts.Common.Web.WebControls.TabularPanel" %>

<asp:Panel ID="outerPanel" runat="server" CssClass="InputTable">
    <asp:Panel ID="headerPanel" runat="server" CssClass="Header" />

    <div class="Fields">
        <asp:PlaceHolder ID="placeHolder" runat="server" />
    </div>
    
    <asp:Panel ID="actionsPanel" runat="server" CssClass="Actions" />
</asp:Panel>