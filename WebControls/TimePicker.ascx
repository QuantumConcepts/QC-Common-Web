<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="TimePicker.ascx.cs" Inherits="QuantumConcepts.Common.Web.WebControls.TimePicker" %>

<asp:TextBox ID="hourTextBox" runat="server" Width="30px" Text="12" style="text-align: center;" /> :
<st:FieldValidator id="hourFieldValidator" runat="server" TargetControlID="hourTextBox" FieldName="Hour" Required="True" FieldFormatDescription="1 to 12" ValidationRegex="(1[0-2])|[1-9]" />

<asp:TextBox ID="minuteTextBox" runat="server" Width="30px" Text="00" style="text-align: center;" />
<st:FieldValidator id="minuteFieldValidator" runat="server" TargetControlID="minuteTextBox" FieldName="Minute" Required="True" FieldFormatDescription="0 to 59" ValidationRegex="([0-5][0-9])|[0-9]" />

<asp:DropDownList id="amPMDropDownList" runat="server">
    <asp:ListItem Value="AM" Text="AM" />
    <asp:ListItem Value="PM" Text="PM" Selected="True" />
</asp:DropDownList>