<%@ Page Title="Labels" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="FolioWebApplication.Labels.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="FindPanel" runat="server" DefaultButton="FindRadButton">
        <asp:Label ID="BarcodeLabel" runat="server" Text="Barcode:" AssociatedControlID="BarcodeRadTextBox"></asp:Label>
        <telerik:RadTextBox ID="BarcodeRadTextBox" runat="server"></telerik:RadTextBox>
        <telerik:RadButton ID="FindRadButton" runat="server" Text="Find" OnClick="FindRadButton_Click">
        </telerik:RadButton>
    </asp:Panel>
</asp:Content>
