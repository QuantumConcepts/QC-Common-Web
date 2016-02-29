<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageSelector.ascx.cs" Inherits="QuantumConcepts.Common.Web.WebControls.PageSelector" %>

<asp:Panel ID="pageSizePanel" runat="server" style="display: inline;">
    Show: <asp:DropDownList ID="pageSizeDropDownList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="pageSizeDropDownList_SelectedIndexChanged" style="font-size: 8pt;"/>
</asp:Panel>

<asp:Panel ID="pagePanel" runat="server" style="display: inline;">
    <asp:Button ID="firstPageButton" runat="server" Text="First" OnClick="pageButton_Click" style="font-size: 8pt;" />
    <asp:Button ID="previousPageButton" runat="server" Text="Prev." OnClick="pageButton_Click" style="font-size: 8pt;" />
    <asp:DropDownList ID="viewPageDropDownList" runat="server" AutoPostBack="true" Width="75px" OnSelectedIndexChanged="viewPageDropDownList_SelectedIndexChanged" style="font-size: 8pt;" />
    <asp:Button ID="nextPageButton" runat="server" Text="Next" OnClick="pageButton_Click" style="font-size: 8pt;" />
    <asp:Button ID="lastPageButton" runat="server" Text="Last" OnClick="pageButton_Click" style="font-size: 8pt;" />
</asp:Panel>