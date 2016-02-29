<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="PagedRepeater.ascx.cs" Inherits="QuantumConcepts.Common.Web.WebControls.PagedRepeater" %>
<%@ Register TagPrefix="st" TagName="PageSelector" Src="~/WebControls/PageSelector.ascx" %>

<h2 id="header" runat="server">
    <asp:Literal ID="titleLiteral" runat="server" />
</h2>

<asp:Panel ID="optionsPanel" runat="server" CssClass="Submenu" style="font-size: 8pt;">
    <div style="float: left; padding-top: 2px;">
        <asp:Literal ID="topResultsLiteral" runat="server" />
    </div>
    
    <span id="pageSelectorSpan" runat="server" style="float: right;">
        <st:PageSelector id="topPageSelector" runat="server" OnSelectionChanged="pageSelector_SelectionChanged" />
    </span>
    
    <asp:Panel id="sortPanel" class="Inline" runat="server" style="float: right;"/>
    
    <asp:Panel id="filterPanel" CssClass="Inline" runat="server" />
</asp:Panel>

<asp:Repeater ID="repeater" runat="server" />

<asp:Label ID="noResultsLabel" runat="server" Text="There are no results to display." Font-Italic="true" Visible="false" />

<asp:Panel ID="bottomPageSelectorPanel" runat="server" CssClass="Submenu" style="clear: both; top: 0px; margin-top: 5px; border-top: solid 1px #999999;">
    <div class="Inline">
        <asp:Literal ID="bottomResultsLiteral" runat="server" />
    </div>
    
    <span style="float: right;">
        <st:PageSelector id="bottomPageSelector" runat="server" ShowPageSizeSelector="false" OnSelectionChanged="pageSelector_SelectionChanged" />
    </span>
</asp:Panel>