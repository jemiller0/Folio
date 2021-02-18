<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Printers.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="PrinterPanel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="PrinterHyperLink" runat="server" Text="Printer" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="PrinterFormView" runat="server" DataKeyNames="Id" DefaultMode='<%# (string)Session["PrintersPermission"] == "Edit" ? FormViewMode.Edit : FormViewMode.ReadOnly %>' RenderOuterTable="false" 
                OnDataBinding="PrinterFormView_DataBinding" OnItemUpdating="PrinterFormView_ItemUpdating" OnItemDeleting="PrinterFormView_ItemDeleting"
                OnItemCommand="PrinterFormView_ItemCommand" Enabled='<%# true || (string)Session["PrintersPermission"] == "Edit" %>'>
                <ItemTemplate>
                    <asp:Panel ID="ViewPrinterPanel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("ComputerName") != null %>'>
                                <td>
                                    <asp:Label ID="ComputerNameLabel" runat="server" Text="Computer Name:" AssociatedControlID="ComputerNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ComputerNameLiteral" runat="server" Text='<%#: Eval("ComputerName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Name") != null %>'>
                                <td>
                                    <asp:Label ID="NameLabel" runat="server" Text="Name:" AssociatedControlID="NameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NameLiteral" runat="server" Text='<%#: Eval("Name") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Left") != null %>'>
                                <td>
                                    <asp:Label ID="LeftLabel" runat="server" Text="Left:" AssociatedControlID="LeftLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LeftLiteral" runat="server" Text='<%#: Eval("Left") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Top") != null %>'>
                                <td>
                                    <asp:Label ID="TopLabel" runat="server" Text="Top:" AssociatedControlID="TopLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TopLiteral" runat="server" Text='<%#: Eval("Top") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Width") != null %>'>
                                <td>
                                    <asp:Label ID="WidthLabel" runat="server" Text="Width:" AssociatedControlID="WidthLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="WidthLiteral" runat="server" Text='<%#: Eval("Width") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Height") != null %>'>
                                <td>
                                    <asp:Label ID="HeightLabel" runat="server" Text="Height:" AssociatedControlID="HeightLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="HeightLiteral" runat="server" Text='<%#: Eval("Height") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Enabled") != null %>'>
                                <td>
                                    <asp:Label ID="EnabledLabel" runat="server" Text="Enabled:" AssociatedControlID="EnabledLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EnabledLiteral" runat="server" Text='<%#: Eval("Enabled") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CreationTime") != null %>'>
                                <td>
                                    <asp:Label ID="CreationTimeLabel" runat="server" Text="Creation Time:" AssociatedControlID="CreationTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CreationTimeLiteral" runat="server" Text='<%# Eval("CreationTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CreationUser") != null %>'>
                                <td>
                                    <asp:Label ID="CreationUserLabel" runat="server" Text="Creation User:" AssociatedControlID="CreationUserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUser.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="LastWriteTimeLabel" runat="server" Text="Last Write Time:" AssociatedControlID="LastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LastWriteTimeLiteral" runat="server" Text='<%# Eval("LastWriteTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LastWriteUser") != null %>'>
                                <td>
                                    <asp:Label ID="LastWriteUserLabel" runat="server" Text="Last Write User:" AssociatedControlID="LastWriteUserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUser.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Panel ID="EditPrinterPanel" runat="server" DefaultButton="InsertUpdateRadButton">
                        <div>
                            <asp:ValidationSummary ID="PrinterValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Printer" Visible="false" />
                        </div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="ComputerNameLabel" runat="server" Text="Computer Name:" AssociatedControlID="ComputerNameRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="ComputerNameRadTextBox" runat="server" Text='<%# Bind("ComputerName") %>' MaxLength="1024" Width="500px" OnTextChanged="ComputerNameRadTextBox_TextChanged" AutoPostBack="true" />
                                    <asp:RequiredFieldValidator ID="ComputerNameRequiredFieldValidator" runat="server" ControlToValidate="ComputerNameRadTextBox" ErrorMessage="The Computer Name field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Printer" />
                                    <asp:CustomValidator ID="ComputerNameCustomValidator" runat="server" ErrorMessage="The Computer Name field is invalid." ControlToValidate="ComputerNameRadTextBox" Display="Dynamic" CssClass="Error" OnServerValidate="ComputerNameCustomValidator_ServerValidate" ValidationGroup="Printer" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="NameLabel" runat="server" Text="Name:" AssociatedControlID="NameRadComboBox" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="NameRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" SelectedValue='<%# Bind("Name") %>' OnDataBinding="NameRadComboBox_DataBinding" OnSelectedIndexChanged="NameRadComboBox_SelectedIndexChanged" AutoPostBack="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" ControlToValidate="NameRadComboBox" ErrorMessage="The Name field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Printer" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="LeftLabel" runat="server" Text="Left:" AssociatedControlID="LeftRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="LeftRadNumericTextBox" runat="server" DbValue='<%# Bind("Left") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="-100" MaxValue="100" />
                                    <asp:RequiredFieldValidator ID="LeftRequiredFieldValidator" runat="server" ControlToValidate="LeftRadNumericTextBox" ErrorMessage="The Left field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Printer" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="TopLabel" runat="server" Text="Top:" AssociatedControlID="TopRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="TopRadNumericTextBox" runat="server" DbValue='<%# Bind("Top") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="-100" MaxValue="100" />
                                    <asp:RequiredFieldValidator ID="TopRequiredFieldValidator" runat="server" ControlToValidate="TopRadNumericTextBox" ErrorMessage="The Top field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Printer" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="WidthLabel" runat="server" Text="Width:" AssociatedControlID="WidthRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="WidthRadNumericTextBox" runat="server" DbValue='<%# Bind("Width") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="25" MaxValue="1100" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="HeightLabel" runat="server" Text="Height:" AssociatedControlID="HeightRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="HeightRadNumericTextBox" runat="server" DbValue='<%# Bind("Height") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MinValue="25" MaxValue="1100" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="EnabledRadCheckBox" runat="server" Text="Enabled" Checked='<%# Bind("Enabled") %>' AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CreationTime") != null %>'>
                                <td>
                                    <asp:Label ID="CreationTimeLabel" runat="server" Text="Creation Time:" AssociatedControlID="CreationTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CreationTimeLiteral" runat="server" Text='<%# Eval("CreationTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CreationUser") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Creation User:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%# Eval("CreationUser.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("CreationUserId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="LastWriteTimeLabel" runat="server" Text="Last Write Time:" AssociatedControlID="LastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LastWriteTimeLiteral" runat="server" Text='<%# Eval("LastWriteTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LastWriteUser") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Last Write User:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%# Eval("LastWriteUser.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("LastWriteUserId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# PrinterFormView.DataKey.Value == null  ? "Insert" : "Update" %>' CommandName="Update" ValidationGroup="Printer" />
                                    <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                    <script type="text/javascript">
                                        function DeleteRadButton_OnClientClicked(button, args) {
                                            button.set_autoPostBack(confirm('Are you sure you want to delete this printer?'));
                                        }
                                    </script>
                                    <telerik:RadButton ID="DeleteRadButton" runat="server" Text="Delete" Visible='<%# PrinterFormView.DataKey.Value != null %>' OnClientClicked="DeleteRadButton_OnClientClicked" CausesValidation="false" CommandName="Delete" />
                                    <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Printer cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" ValidationGroup="Printer" />
                                    <asp:CustomValidator ID="PrinterCustomValidator" runat="server" Display="Dynamic" CssClass="Error" ValidationGroup="Printer" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </EditItemTemplate>
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ComputerNameRadTextBox">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PrinterPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="NameRadComboBox">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PrinterPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InsertUpdateRadButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PrinterPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
