<%@ Page Title="Error" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="FolioWebApplication.Error" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p class="Error">
        <asp:Literal ID="MessageLiteral" runat="server" />
    </p>
</asp:Content>
