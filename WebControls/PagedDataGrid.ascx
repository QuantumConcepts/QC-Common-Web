<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="PagedDataGrid.ascx.cs" Inherits="QuantumConcepts.Common.Web.WebControls.PagedDataGrid" %>
<%@ Register TagPrefix="Common" TagName="PageSelector" Src="~/WebControls/PageSelector.ascx" %>

<h2 id="header" runat="server">
    <asp:Literal ID="titleLiteral" runat="server" />
</h2>

<asp:Panel runat="server" CssClass="Table">
    <asp:Panel ID="optionsPanel" runat="server" CssClass="Options">
        <span id="pageSelectorSpan" runat="server" style="float: right;">
            <Common:PageSelector id="topPageSelector" runat="server" OnSelectionChanged="pageSelector_SelectionChanged" />
        </span>
    
        <asp:Panel id="sortPanel" class="Inline" runat="server"/>
    
        <div class="Inline" style="float: left;">
            <asp:Literal ID="topResultsLiteral" runat="server" />
        </div>
    
        <asp:Panel id="filterPanel" CssClass="Inline" runat="server" />
    </asp:Panel>

    <asp:DataGrid ID="dataGrid" runat="server" OnItemCommand="dataGrid_ItemCommand" />

    <asp:Label ID="noResultsLabel" runat="server" Text="There are no results to display." Font-Italic="true" Visible="false" />

    <asp:Panel ID="bottomPageSelectorPanel" runat="server" CssClass="Options" style="clear: both;">
        <div class="Inline">
            <asp:Literal ID="bottomResultsLiteral" runat="server" />
        </div>
    
        <span style="float: right;">
            <Common:PageSelector id="bottomPageSelector" runat="server" ShowPageSizeSelector="false" OnSelectionChanged="pageSelector_SelectionChanged" />
        </span>
    </asp:Panel>
</asp:Panel>