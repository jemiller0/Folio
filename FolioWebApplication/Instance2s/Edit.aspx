<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Instance2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Instance2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Instance2HyperLink" runat="server" Text="Instance" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Instance2FormView" runat="server" DataKeyNames="Id" DefaultMode='<%# (string)Session["Instance2sPermission"] == "Edit" ? FormViewMode.Edit : FormViewMode.ReadOnly %>' RenderOuterTable="false" 
                OnDataBinding="Instance2FormView_DataBinding" OnItemUpdating="Instance2FormView_ItemUpdating" OnItemDeleting="Instance2FormView_ItemDeleting"
                OnItemCommand="Instance2FormView_ItemCommand" Enabled='<%# true || (string)Session["Instance2sPermission"] == "Edit" %>'>
                <ItemTemplate>
                    <asp:Panel ID="ViewInstance2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Version") != null %>'>
                                <td>
                                    <asp:Label ID="VersionLabel" runat="server" Text="Version:" AssociatedControlID="VersionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VersionLiteral" runat="server" Text='<%#: Eval("Version") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ShortId") != null %>'>
                                <td>
                                    <asp:Label ID="ShortIdLabel" runat="server" Text="Short Id:" AssociatedControlID="ShortIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ShortIdLiteral" runat="server" Text='<%#: Eval("ShortId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("MatchKey") != null %>'>
                                <td>
                                    <asp:Label ID="MatchKeyLabel" runat="server" Text="Match Key:" AssociatedControlID="MatchKeyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="MatchKeyLiteral" runat="server" Text='<%#: Eval("MatchKey") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Source") != null %>'>
                                <td>
                                    <asp:Label ID="SourceLabel" runat="server" Text="Source:" AssociatedControlID="SourceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SourceLiteral" runat="server" Text='<%#: Eval("Source") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Title") != null %>'>
                                <td>
                                    <asp:Label ID="TitleLabel" runat="server" Text="Title:" AssociatedControlID="TitleLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TitleLiteral" runat="server" Text='<%#: Eval("Title") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Author") != null %>'>
                                <td>
                                    <asp:Label ID="AuthorLabel" runat="server" Text="Author:" AssociatedControlID="AuthorLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AuthorLiteral" runat="server" Text='<%#: Eval("Author") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PublicationYear") != null %>'>
                                <td>
                                    <asp:Label ID="PublicationYearLabel" runat="server" Text="Publication Year:" AssociatedControlID="PublicationYearLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublicationYearLiteral" runat="server" Text='<%#: Eval("PublicationYear") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PublicationPeriodStart") != null %>'>
                                <td>
                                    <asp:Label ID="PublicationPeriodStartLabel" runat="server" Text="Publication Period Start:" AssociatedControlID="PublicationPeriodStartLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublicationPeriodStartLiteral" runat="server" Text='<%#: Eval("PublicationPeriodStart") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PublicationPeriodEnd") != null %>'>
                                <td>
                                    <asp:Label ID="PublicationPeriodEndLabel" runat="server" Text="Publication Period End:" AssociatedControlID="PublicationPeriodEndLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublicationPeriodEndLiteral" runat="server" Text='<%#: Eval("PublicationPeriodEnd") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InstanceType") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceTypeLabel" runat="server" Text="Instance Type:" AssociatedControlID="InstanceTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InstanceTypeHyperLink" runat="server" Text='<%#: Eval("InstanceType.Name") %>' NavigateUrl='<%# $"~/InstanceType2s/Edit.aspx?Id={Eval("InstanceTypeId")}" %>' Enabled='<%# Session["InstanceType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("IssuanceMode") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Issuance Mode:" NavigateUrl="~/IssuanceModes/Default.aspx" Enabled='<%# Session["IssuanceModesPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IssuanceModeHyperLink" runat="server" Text='<%#: Eval("IssuanceMode.Name") %>' NavigateUrl='<%# $"~/IssuanceModes/Edit.aspx?Id={Eval("IssuanceModeId")}" %>' Enabled='<%# Session["IssuanceModesPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CatalogedDate") != null %>'>
                                <td>
                                    <asp:Label ID="CatalogedDateLabel" runat="server" Text="Cataloged Date:" AssociatedControlID="CatalogedDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CatalogedDateLiteral" runat="server" Text='<%# Eval("CatalogedDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PreviouslyHeld") != null %>'>
                                <td>
                                    <asp:Label ID="PreviouslyHeldLabel" runat="server" Text="Previously Held:" AssociatedControlID="PreviouslyHeldLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PreviouslyHeldLiteral" runat="server" Text='<%#: Eval("PreviouslyHeld") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StaffSuppress") != null %>'>
                                <td>
                                    <asp:Label ID="StaffSuppressLabel" runat="server" Text="Staff Suppress:" AssociatedControlID="StaffSuppressLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StaffSuppressLiteral" runat="server" Text='<%#: Eval("StaffSuppress") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DiscoverySuppress") != null %>'>
                                <td>
                                    <asp:Label ID="DiscoverySuppressLabel" runat="server" Text="Discovery Suppress:" AssociatedControlID="DiscoverySuppressLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DiscoverySuppressLiteral" runat="server" Text='<%#: Eval("DiscoverySuppress") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SourceRecordFormat") != null %>'>
                                <td>
                                    <asp:Label ID="SourceRecordFormatLabel" runat="server" Text="Source Record Format:" AssociatedControlID="SourceRecordFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SourceRecordFormatLiteral" runat="server" Text='<%#: Eval("SourceRecordFormat") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Status") != null %>'>
                                <td>
                                    <asp:HyperLink runat="server" Text="Status:" NavigateUrl="~/Statuses/Default.aspx" Enabled='<%# Session["StatusesPermission"] != null %>' />
                                </td>
                                <td>
                                    <asp:HyperLink ID="StatusHyperLink" runat="server" Text='<%#: Eval("Status.Name") %>' NavigateUrl='<%# $"~/Statuses/Edit.aspx?Id={Eval("StatusId")}" %>' Enabled='<%# Session["StatusesPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StatusLastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="StatusLastWriteTimeLabel" runat="server" Text="Status Last Write Time:" AssociatedControlID="StatusLastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatusLastWriteTimeLiteral" runat="server" Text='<%# Eval("StatusLastWriteTime", "{0:g}") %>' />
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
                            <tr runat="server" visible='<%# Eval("Content") != null %>'>
                                <td>
                                    <asp:Label ID="ContentLabel" runat="server" Text="Content:" AssociatedControlID="ContentLiteral" />
                                </td>
                                <td>
                                    <pre><asp:Literal ID="ContentLiteral" runat="server" Text='<%#: Eval("Content") %>' /></pre>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Panel ID="EditInstance2Panel" runat="server" DefaultButton="InsertUpdateRadButton">
                        <div>
                            <asp:ValidationSummary ID="Instance2ValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Instance2" Visible="false" />
                        </div>
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="VersionLabel" runat="server" Text="Version:" AssociatedControlID="VersionRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="VersionRadNumericTextBox" runat="server" DbValue='<%# Bind("Version") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MaxValue="2147483647" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ShortId") != null %>'>
                                <td>
                                    <asp:Label ID="ShortIdLabel" runat="server" Text="Short Id:" AssociatedControlID="ShortIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ShortIdLiteral" runat="server" Text='<%# Eval("ShortId") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="MatchKeyLabel" runat="server" Text="Match Key:" AssociatedControlID="MatchKeyRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="MatchKeyRadTextBox" runat="server" Text='<%# Bind("MatchKey") %>' MaxLength="1024" Width="500px" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Source") != null %>'>
                                <td>
                                    <asp:Label ID="SourceLabel" runat="server" Text="Source:" AssociatedControlID="SourceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SourceLiteral" runat="server" Text='<%#: Eval("Source") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="TitleLabel" runat="server" Text="Title:" AssociatedControlID="TitleRadTextBox" />
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="TitleRadTextBox" runat="server" Text='<%# Bind("Title") %>' MaxLength="1024" Width="500px" />
                                    <asp:RequiredFieldValidator ID="TitleRequiredFieldValidator" runat="server" ControlToValidate="TitleRadTextBox" ErrorMessage="The Title field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Instance2" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Author") != null %>'>
                                <td>
                                    <asp:Label ID="AuthorLabel" runat="server" Text="Author:" AssociatedControlID="AuthorLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AuthorLiteral" runat="server" Text='<%#: Eval("Author") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PublicationYear") != null %>'>
                                <td>
                                    <asp:Label ID="PublicationYearLabel" runat="server" Text="Publication Year:" AssociatedControlID="PublicationYearLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PublicationYearLiteral" runat="server" Text='<%#: Eval("PublicationYear") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="PublicationPeriodStartLabel" runat="server" Text="Publication Period Start:" AssociatedControlID="PublicationPeriodStartRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="PublicationPeriodStartRadNumericTextBox" runat="server" DbValue='<%# Bind("PublicationPeriodStart") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MaxValue="2147483647" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="PublicationPeriodEndLabel" runat="server" Text="Publication Period End:" AssociatedControlID="PublicationPeriodEndRadNumericTextBox" />
                                </td>
                                <td>
                                    <telerik:RadNumericTextBox ID="PublicationPeriodEndRadNumericTextBox" runat="server" DbValue='<%# Bind("PublicationPeriodEnd") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MaxValue="2147483647" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Instance Type:" NavigateUrl="~/InstanceType2s/Default.aspx" Enabled='<%# Session["InstanceType2sPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="InstanceTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("InstanceTypeId") %>' OnDataBinding="InstanceTypeRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:RequiredFieldValidator ID="InstanceTypeRequiredFieldValidator" runat="server" ControlToValidate="InstanceTypeRadComboBox" ErrorMessage="The Instance Type field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Instance2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Issuance Mode:" NavigateUrl="~/IssuanceModes/Default.aspx" Enabled='<%# Session["IssuanceModesPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="IssuanceModeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("IssuanceModeId") %>' OnDataBinding="IssuanceModeRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="CatalogedDateLabel" runat="server" Text="Cataloged Date:" AssociatedControlID="CatalogedDateRadDatePicker" />
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="CatalogedDateRadDatePicker" runat="server" DbSelectedDate='<%# Bind("CatalogedDate") %>' Width="500px" MinDate="1/1/1900" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="PreviouslyHeldRadCheckBox" runat="server" Text="Previously Held" Checked='<%# Bind("PreviouslyHeld") %>' AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="StaffSuppressRadCheckBox" runat="server" Text="Staff Suppress" Checked='<%# Bind("StaffSuppress") %>' AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="DiscoverySuppressRadCheckBox" runat="server" Text="Discovery Suppress" Checked='<%# Bind("DiscoverySuppress") %>' AutoPostBack="false" />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SourceRecordFormat") != null %>'>
                                <td>
                                    <asp:Label ID="SourceRecordFormatLabel" runat="server" Text="Source Record Format:" AssociatedControlID="SourceRecordFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SourceRecordFormatLiteral" runat="server" Text='<%#: Eval("SourceRecordFormat") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HyperLink runat="server" Text="Status:" NavigateUrl="~/Statuses/Default.aspx" Enabled='<%# Session["StatusesPermission"] != null %>' />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="StatusRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("StatusId") %>' OnDataBinding="StatusRadComboBox_DataBinding">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="" Text="" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StatusLastWriteTime") != null %>'>
                                <td>
                                    <asp:Label ID="StatusLastWriteTimeLabel" runat="server" Text="Status Last Write Time:" AssociatedControlID="StatusLastWriteTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatusLastWriteTimeLiteral" runat="server" Text='<%# Eval("StatusLastWriteTime", "{0:g}") %>' />
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
                            <tr runat="server" visible='<%# Eval("Content") != null %>'>
                                <td>
                                    <asp:Label ID="ContentLabel" runat="server" Text="Content:" AssociatedControlID="ContentLiteral" />
                                </td>
                                <td>
                                    <pre><asp:Literal ID="ContentLiteral" runat="server" Text='<%#: Eval("Content") %>' /></pre>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# Instance2FormView.DataKey.Value == null  ? "Insert" : "Update" %>' CommandName="Update" ValidationGroup="Instance2" />
                                    <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                    <script type="text/javascript">
                                        function DeleteRadButton_OnClientClicked(button, args) {
                                            button.set_autoPostBack(confirm('Are you sure you want to delete this instance?'));
                                        }
                                    </script>
                                    <telerik:RadButton ID="DeleteRadButton" runat="server" Text="Delete" Visible='<%# Instance2FormView.DataKey.Value != null %>' OnClientClicked="DeleteRadButton_OnClientClicked" CausesValidation="false" CommandName="Delete" />
                                    <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Instance cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" ValidationGroup="Instance2" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </EditItemTemplate>
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Fee2sPanel" runat="server" Visible='<%# (string)Session["Fee2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Fee2sHyperLink" runat="server" Text="Fees" NavigateUrl="~/Fee2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Fee2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Fee2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No fees found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Remaining Amount" DataField="RemainingAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Payment Status Name" DataField="PaymentStatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Fee2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Barcode" DataField="Barcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Material Type" DataField="MaterialType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Status Name" DataField="ItemStatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Location" DataField="Location" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Due Time" DataField="DueTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Returned Time" DataField="ReturnedTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Loan" DataField="Loan.Id" SortExpression="Loan.Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="LoanHyperLink" runat="server" Text='<%# Eval("Loan.Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("LoanId")}" %>' Enabled='<%# Session["Loan2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="User" DataField="User.Username" SortExpression="User.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("UserId") != null ? Eval("User.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Item" DataField="Item.ShortId" SortExpression="Item.ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("ItemId") != null ? Eval("Item.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Material Type 1" DataField="MaterialType1.Name" SortExpression="MaterialType1.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="MaterialType1HyperLink" runat="server" Text='<%#: Eval("MaterialType1.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Fee Type" DataField="FeeType.Name" SortExpression="FeeType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FeeTypeHyperLink" runat="server" Text='<%#: Eval("FeeType.Name") %>' NavigateUrl='<%# $"~/FeeType2s/Edit.aspx?Id={Eval("FeeTypeId")}" %>' Enabled='<%# Session["FeeType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Owner" DataField="Owner.Name" SortExpression="Owner.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="OwnerHyperLink" runat="server" Text='<%#: Eval("OwnerId") != null ? Eval("Owner.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Owner2s/Edit.aspx?Id={Eval("OwnerId")}" %>' Enabled='<%# Session["Owner2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Holding" DataField="Holding.ShortId" SortExpression="Holding.ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="HoldingHyperLink" runat="server" Text='<%# Eval("HoldingId") != null ? Eval("Holding.ShortId") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("HoldingId")}" %>' Enabled='<%# Session["Holding2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Holding2sPanel" runat="server" Visible='<%# (string)Session["Holding2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Holding2sHyperLink" runat="server" Text="Holdings" NavigateUrl="~/Holding2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Holding2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Holding2sRadGrid_NeedDataSource" OnItemCommand="Holding2sRadGrid_ItemCommand" OnInsertCommand="Holding2sRadGrid_UpdateCommand" OnUpdateCommand="Holding2sRadGrid_UpdateCommand" OnDeleteCommand="Holding2sRadGrid_DeleteCommand">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No holdings found" CommandItemDisplay='<%# (string)Session["Holding2sPermission"] == "Edit" ? GridCommandItemDisplay.Top : GridCommandItemDisplay.None %>' CommandItemSettings-ShowAddNewRecordButton='<%# (string)Session["Holding2sPermission"] == "Edit" %>' InsertItemPageIndexAction="ShowItemOnFirstPage">
                    <CommandItemSettings AddNewRecordText="New Holding" />
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:LinkButton ID="EditLinkButton" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false" Visible='<%# (string)Session["Holding2sPermission"] == "Edit" %>' />
                                <asp:HyperLink ID="ViewHyperLink" runat="server" Text="View" NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("Id")}" %>' Visible='<%# (string)Session["Holding2sPermission"] == "View" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:LinkButton ID="DeleteLinkButton" runat="server" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this holding?")' Visible='<%# (string)Session["Holding2sPermission"] == "Edit" %>' />
                                <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Holding cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Version" DataField="Version" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Short Id" DataField="ShortId" SortExpression="ShortId" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="ShortIdHyperLink" runat="server" Text='<%# Eval("ShortId") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Holding Type" DataField="HoldingType.Name" SortExpression="HoldingType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="HoldingTypeHyperLink" runat="server" Text='<%#: Eval("HoldingType.Name") %>' NavigateUrl='<%# $"~/HoldingType2s/Edit.aspx?Id={Eval("HoldingTypeId")}" %>' Enabled='<%# Session["HoldingType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Location" DataField="Location.Name" SortExpression="Location.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LocationHyperLink" runat="server" Text='<%#: Eval("Location.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("LocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Temporary Location" DataField="TemporaryLocation.Name" SortExpression="TemporaryLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemporaryLocationHyperLink" runat="server" Text='<%#: Eval("TemporaryLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("TemporaryLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Effective Location" DataField="EffectiveLocation.Name" SortExpression="EffectiveLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%#: Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("EffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Call Number Type" DataField="CallNumberType.Name" SortExpression="CallNumberType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CallNumberTypeHyperLink" runat="server" Text='<%#: Eval("CallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("CallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Call Number Prefix" DataField="CallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Call Number Suffix" DataField="CallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Shelving Title" DataField="ShelvingTitle" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Acquisition Format" DataField="AcquisitionFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Acquisition Method" DataField="AcquisitionMethod" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receipt Status" DataField="ReceiptStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Ill Policy" DataField="IllPolicy.Name" SortExpression="IllPolicy.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="IllPolicyHyperLink" runat="server" Text='<%#: Eval("IllPolicy.Name") %>' NavigateUrl='<%# $"~/IllPolicy2s/Edit.aspx?Id={Eval("IllPolicyId")}" %>' Enabled='<%# Session["IllPolicy2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Retention Policy" DataField="RetentionPolicy" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Digitization Policy" DataField="DigitizationPolicy" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Copy Number" DataField="CopyNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Item Count" DataField="ItemCount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receiving History Display Type" DataField="ReceivingHistoryDisplayType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Source" DataField="Source.Name" SortExpression="Source.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SourceHyperLink" runat="server" Text='<%#: Eval("SourceId") != null ? Eval("Source.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Source2s/Edit.aspx?Id={Eval("SourceId")}" %>' Enabled='<%# Session["Source2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Items">
                            <ItemTemplate>
                                <telerik:RadGrid ID="Holding2sItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Holding2sItem2sRadGrid_NeedDataSource" OnItemCommand="Holding2sItem2sRadGrid_ItemCommand" OnInsertCommand="Holding2sItem2sRadGrid_UpdateCommand" OnUpdateCommand="Holding2sItem2sRadGrid_UpdateCommand" OnDeleteCommand="Holding2sItem2sRadGrid_DeleteCommand">
                                    <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No items found" CommandItemDisplay='<%# (string)Session["Item2sPermission"] == "Edit" ? GridCommandItemDisplay.Top : GridCommandItemDisplay.None %>' CommandItemSettings-ShowAddNewRecordButton='<%# (string)Session["Item2sPermission"] == "Edit" %>' InsertItemPageIndexAction="ShowItemOnFirstPage">
                                        <CommandItemSettings AddNewRecordText="New Item" />
                                        <Columns>
                                            <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="EditLinkButton" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false" Visible='<%# (string)Session["Item2sPermission"] == "Edit" %>' />
                                                    <asp:HyperLink ID="ViewHyperLink" runat="server" Text="View" NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' Visible='<%# (string)Session["Item2sPermission"] == "View" %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="DeleteLinkButton" runat="server" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='return confirm("Are you sure you want to delete this item?")' Visible='<%# (string)Session["Item2sPermission"] == "Edit" %>' />
                                                    <asp:CustomValidator ID="DeleteCustomValidator" runat="server" ErrorMessage="Item cannot be deleted because it is being referenced" Display="Dynamic" CssClass="Error" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="NewLabelHyperLink" runat="server" Text="New Label" NavigateUrl='<%# $"~/Labels/Edit.aspx?Barcode={Eval("Barcode")}" %>' Visible='<%# !string.IsNullOrWhiteSpace((string)Eval("Barcode")) && Session["LabelsPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Version" DataField="Version" AutoPostBackOnFilter="true" />
                                            <telerik:GridTemplateColumn HeaderText="Short Id" DataField="ShortId" SortExpression="ShortId" AutoPostBackOnFilter="true">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="ShortIdHyperLink" runat="server" Text='<%# Eval("ShortId") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Discovery Suppress" DataField="DiscoverySuppress" AutoPostBackOnFilter="true" />
                                            <telerik:GridBoundColumn HeaderText="Accession Number" DataField="AccessionNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Barcode" DataField="Barcode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Effective Shelving Order" DataField="EffectiveShelvingOrder" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Call Number" DataField="CallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Call Number Prefix" DataField="CallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Call Number Suffix" DataField="CallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Call Number Type" DataField="CallNumberType.Name" SortExpression="CallNumberType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="CallNumberTypeHyperLink" runat="server" Text='<%#: Eval("CallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("CallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Effective Call Number" DataField="EffectiveCallNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Effective Call Number Prefix" DataField="EffectiveCallNumberPrefix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Effective Call Number Suffix" DataField="EffectiveCallNumberSuffix" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Effective Call Number Type" DataField="EffectiveCallNumberType.Name" SortExpression="EffectiveCallNumberType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="EffectiveCallNumberTypeHyperLink" runat="server" Text='<%#: Eval("EffectiveCallNumberType.Name") %>' NavigateUrl='<%# $"~/CallNumberType2s/Edit.aspx?Id={Eval("EffectiveCallNumberTypeId")}" %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Volume" DataField="Volume" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Enumeration" DataField="Enumeration" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Chronology" DataField="Chronology" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Item Identifier" DataField="ItemIdentifier" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Copy Number" DataField="CopyNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Pieces Count" DataField="PiecesCount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Pieces Description" DataField="PiecesDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Missing Pieces Count" DataField="MissingPiecesCount" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Missing Pieces Description" DataField="MissingPiecesDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Missing Pieces Time" DataField="MissingPiecesTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Damaged Status" DataField="DamagedStatus.Name" SortExpression="DamagedStatus.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="DamagedStatusHyperLink" runat="server" Text='<%#: Eval("DamagedStatus.Name") %>' NavigateUrl='<%# $"~/ItemDamagedStatus2s/Edit.aspx?Id={Eval("DamagedStatusId")}" %>' Enabled='<%# Session["ItemDamagedStatus2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Damaged Status Time" DataField="DamagedStatusTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridBoundColumn HeaderText="Status Name" DataField="StatusName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                                            <telerik:GridBoundColumn HeaderText="Status Date" DataField="StatusDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Material Type" DataField="MaterialType.Name" SortExpression="MaterialType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="MaterialTypeHyperLink" runat="server" Text='<%#: Eval("MaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Permanent Loan Type" DataField="PermanentLoanType.Name" SortExpression="PermanentLoanType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="PermanentLoanTypeHyperLink" runat="server" Text='<%#: Eval("PermanentLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("PermanentLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Temporary Loan Type" DataField="TemporaryLoanType.Name" SortExpression="TemporaryLoanType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="TemporaryLoanTypeHyperLink" runat="server" Text='<%#: Eval("TemporaryLoanType.Name") %>' NavigateUrl='<%# $"~/LoanType2s/Edit.aspx?Id={Eval("TemporaryLoanTypeId")}" %>' Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Permanent Location" DataField="PermanentLocation.Name" SortExpression="PermanentLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="PermanentLocationHyperLink" runat="server" Text='<%#: Eval("PermanentLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("PermanentLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Temporary Location" DataField="TemporaryLocation.Name" SortExpression="TemporaryLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="TemporaryLocationHyperLink" runat="server" Text='<%#: Eval("TemporaryLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("TemporaryLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Effective Location" DataField="EffectiveLocation.Name" SortExpression="EffectiveLocation.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%#: Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# $"~/Location2s/Edit.aspx?Id={Eval("EffectiveLocationId")}" %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="In Transit Destination Service Point" DataField="InTransitDestinationServicePoint.Name" SortExpression="InTransitDestinationServicePoint.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="InTransitDestinationServicePointHyperLink" runat="server" Text='<%#: Eval("InTransitDestinationServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("InTransitDestinationServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Order Item" DataField="OrderItem.Number" SortExpression="OrderItem.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn HeaderText="Last Check In Date Time" DataField="LastCheckInDateTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Check In Service Point" DataField="LastCheckInServicePoint.Name" SortExpression="LastCheckInServicePoint.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="LastCheckInServicePointHyperLink" runat="server" Text='<%#: Eval("LastCheckInServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("LastCheckInServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Check In Staff Member" DataField="LastCheckInStaffMember.Username" SortExpression="LastCheckInStaffMember.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="LastCheckInStaffMemberHyperLink" runat="server" Text='<%#: Eval("LastCheckInStaffMemberId") != null ? Eval("LastCheckInStaffMember.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastCheckInStaffMemberId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <EditFormSettings EditFormType="Template">
                                            <FormTemplate>
                                                <asp:Panel ID="EditItem2Panel" runat="server" DefaultButton="InsertUpdateRadButton">
                                                    <div>
                                                        <asp:ValidationSummary ID="Item2ValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Holding2sItem2s" Visible="false" />
                                                    </div>
                                                    <table>
                                                        <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                                            <td>
                                                                <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                                            </td>
                                                            <td>
                                                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="VersionLabel" runat="server" Text="Version:" AssociatedControlID="VersionRadNumericTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadNumericTextBox ID="VersionRadNumericTextBox" runat="server" DbValue='<%# Bind("Version") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MaxValue="2147483647" />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("ShortId") != null %>'>
                                                            <td>
                                                                <asp:Label ID="ShortIdLabel" runat="server" Text="Short Id:" AssociatedControlID="ShortIdLiteral" />
                                                            </td>
                                                            <td>
                                                                <asp:Literal ID="ShortIdLiteral" runat="server" Text='<%# Eval("ShortId") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="DiscoverySuppressRadCheckBox" runat="server" Text="Discovery Suppress" Checked='<%# Bind("DiscoverySuppress") %>' AutoPostBack="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="AccessionNumberLabel" runat="server" Text="Accession Number:" AssociatedControlID="AccessionNumberRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="AccessionNumberRadTextBox" runat="server" Text='<%# Bind("AccessionNumber") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="BarcodeLabel" runat="server" Text="Barcode:" AssociatedControlID="BarcodeRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="BarcodeRadTextBox" runat="server" Text='<%# Bind("Barcode") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("EffectiveShelvingOrder") != null %>'>
                                                            <td>
                                                                <asp:Label ID="EffectiveShelvingOrderLabel" runat="server" Text="Effective Shelving Order:" AssociatedControlID="EffectiveShelvingOrderLiteral" />
                                                            </td>
                                                            <td>
                                                                <asp:Literal ID="EffectiveShelvingOrderLiteral" runat="server" Text='<%#: Eval("EffectiveShelvingOrder") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CallNumberLabel" runat="server" Text="Call Number:" AssociatedControlID="CallNumberRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="CallNumberRadTextBox" runat="server" Text='<%# Bind("CallNumber") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CallNumberPrefixLabel" runat="server" Text="Call Number Prefix:" AssociatedControlID="CallNumberPrefixRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="CallNumberPrefixRadTextBox" runat="server" Text='<%# Bind("CallNumberPrefix") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CallNumberSuffixLabel" runat="server" Text="Call Number Suffix:" AssociatedControlID="CallNumberSuffixRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="CallNumberSuffixRadTextBox" runat="server" Text='<%# Bind("CallNumberSuffix") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Call Number Type:" NavigateUrl="~/CallNumberType2s/Default.aspx" Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="CallNumberTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("CallNumberTypeId") %>' OnDataBinding="Holding2sItem2sCallNumberTypeRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("EffectiveCallNumber") != null %>'>
                                                            <td>
                                                                <asp:Label ID="EffectiveCallNumberLabel" runat="server" Text="Effective Call Number:" AssociatedControlID="EffectiveCallNumberLiteral" />
                                                            </td>
                                                            <td>
                                                                <asp:Literal ID="EffectiveCallNumberLiteral" runat="server" Text='<%#: Eval("EffectiveCallNumber") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("EffectiveCallNumberPrefix") != null %>'>
                                                            <td>
                                                                <asp:Label ID="EffectiveCallNumberPrefixLabel" runat="server" Text="Effective Call Number Prefix:" AssociatedControlID="EffectiveCallNumberPrefixLiteral" />
                                                            </td>
                                                            <td>
                                                                <asp:Literal ID="EffectiveCallNumberPrefixLiteral" runat="server" Text='<%#: Eval("EffectiveCallNumberPrefix") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("EffectiveCallNumberSuffix") != null %>'>
                                                            <td>
                                                                <asp:Label ID="EffectiveCallNumberSuffixLabel" runat="server" Text="Effective Call Number Suffix:" AssociatedControlID="EffectiveCallNumberSuffixLiteral" />
                                                            </td>
                                                            <td>
                                                                <asp:Literal ID="EffectiveCallNumberSuffixLiteral" runat="server" Text='<%#: Eval("EffectiveCallNumberSuffix") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("EffectiveCallNumberType") != null %>'>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Effective Call Number Type:" NavigateUrl="~/CallNumberType2s/Default.aspx" Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <asp:HyperLink ID="EffectiveCallNumberTypeHyperLink" runat="server" Text='<%# Eval("EffectiveCallNumberType.Name") %>' NavigateUrl='<%# "~/CallNumberType2s/Edit.aspx?Id=" + Eval("EffectiveCallNumberTypeId") %>' Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="VolumeLabel" runat="server" Text="Volume:" AssociatedControlID="VolumeRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="VolumeRadTextBox" runat="server" Text='<%# Bind("Volume") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="EnumerationLabel" runat="server" Text="Enumeration:" AssociatedControlID="EnumerationRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="EnumerationRadTextBox" runat="server" Text='<%# Bind("Enumeration") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="ChronologyLabel" runat="server" Text="Chronology:" AssociatedControlID="ChronologyRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="ChronologyRadTextBox" runat="server" Text='<%# Bind("Chronology") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="ItemIdentifierLabel" runat="server" Text="Item Identifier:" AssociatedControlID="ItemIdentifierRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="ItemIdentifierRadTextBox" runat="server" Text='<%# Bind("ItemIdentifier") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="CopyNumberLabel" runat="server" Text="Copy Number:" AssociatedControlID="CopyNumberRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="CopyNumberRadTextBox" runat="server" Text='<%# Bind("CopyNumber") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="PiecesCountLabel" runat="server" Text="Pieces Count:" AssociatedControlID="PiecesCountRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="PiecesCountRadTextBox" runat="server" Text='<%# Bind("PiecesCount") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="PiecesDescriptionLabel" runat="server" Text="Pieces Description:" AssociatedControlID="PiecesDescriptionRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="PiecesDescriptionRadTextBox" runat="server" Text='<%# Bind("PiecesDescription") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="MissingPiecesCountLabel" runat="server" Text="Missing Pieces Count:" AssociatedControlID="MissingPiecesCountRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="MissingPiecesCountRadTextBox" runat="server" Text='<%# Bind("MissingPiecesCount") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="MissingPiecesDescriptionLabel" runat="server" Text="Missing Pieces Description:" AssociatedControlID="MissingPiecesDescriptionRadTextBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox ID="MissingPiecesDescriptionRadTextBox" runat="server" Text='<%# Bind("MissingPiecesDescription") %>' MaxLength="1024" Width="500px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="MissingPiecesTimeLabel" runat="server" Text="Missing Pieces Time:" AssociatedControlID="MissingPiecesTimeRadDateTimePicker" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadDateTimePicker ID="MissingPiecesTimeRadDateTimePicker" runat="server" DbSelectedDate='<%# Bind("MissingPiecesTime") %>' Width="500px" MinDate="1/1/1900" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Damaged Status:" NavigateUrl="~/ItemDamagedStatus2s/Default.aspx" Enabled='<%# Session["ItemDamagedStatus2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="DamagedStatusRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("DamagedStatusId") %>' OnDataBinding="Holding2sItem2sDamagedStatusRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="DamagedStatusTimeLabel" runat="server" Text="Damaged Status Time:" AssociatedControlID="DamagedStatusTimeRadDateTimePicker" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadDateTimePicker ID="DamagedStatusTimeRadDateTimePicker" runat="server" DbSelectedDate='<%# Bind("DamagedStatusTime") %>' Width="500px" MinDate="1/1/1900" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="StatusNameLabel" runat="server" Text="Status Name:" AssociatedControlID="StatusNameRadComboBox" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="StatusNameRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" SelectedValue='<%# Bind("StatusName") %>' OnDataBinding="Holding2sItem2sStatusNameRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="StatusNameRequiredFieldValidator" runat="server" ControlToValidate="StatusNameRadComboBox" ErrorMessage="The Status Name field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Holding2sItem2s" />
                                                                <asp:RegularExpressionValidator ID="StatusNameRegularExpressionValidator" runat="server" ErrorMessage="The Status Name field must match the regular expression '^(Aged to lost|Available|Awaiting pickup|Awaiting delivery|Checked out|Claimed returned|Declared lost|In process|In process (non-requestable)|In transit|Intellectual item|Long missing|Lost and paid|Missing|On order|Paged|Restricted|Order closed|Unavailable|Unknown|Withdrawn)$'." ControlToValidate="StatusNameRadComboBox" Display="Dynamic" CssClass="Error" ValidationExpression="^(Aged to lost|Available|Awaiting pickup|Awaiting delivery|Checked out|Claimed returned|Declared lost|In process|In process (non-requestable)|In transit|Intellectual item|Long missing|Lost and paid|Missing|On order|Paged|Restricted|Order closed|Unavailable|Unknown|Withdrawn)$" ValidationGroup="Holding2sItem2s" />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("StatusDate") != null %>'>
                                                            <td>
                                                                <asp:Label ID="StatusDateLabel" runat="server" Text="Status Date:" AssociatedControlID="StatusDateLiteral" />
                                                            </td>
                                                            <td>
                                                                <asp:Literal ID="StatusDateLiteral" runat="server" Text='<%# Eval("StatusDate", "{0:d}") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Material Type:" NavigateUrl="~/MaterialType2s/Default.aspx" Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="MaterialTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("MaterialTypeId") %>' OnDataBinding="Holding2sItem2sMaterialTypeRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="MaterialTypeRequiredFieldValidator" runat="server" ControlToValidate="MaterialTypeRadComboBox" ErrorMessage="The Material Type field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Holding2sItem2s" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Permanent Loan Type:" NavigateUrl="~/LoanType2s/Default.aspx" Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="PermanentLoanTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("PermanentLoanTypeId") %>' OnDataBinding="Holding2sItem2sPermanentLoanTypeRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                                <asp:RequiredFieldValidator ID="PermanentLoanTypeRequiredFieldValidator" runat="server" ControlToValidate="PermanentLoanTypeRadComboBox" ErrorMessage="The Permanent Loan Type field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Holding2sItem2s" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Temporary Loan Type:" NavigateUrl="~/LoanType2s/Default.aspx" Enabled='<%# Session["LoanType2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="TemporaryLoanTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("TemporaryLoanTypeId") %>' OnDataBinding="Holding2sItem2sTemporaryLoanTypeRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Permanent Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="PermanentLocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("PermanentLocationId") %>' OnDataBinding="Holding2sItem2sPermanentLocationRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Temporary Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="TemporaryLocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("TemporaryLocationId") %>' OnDataBinding="Holding2sItem2sTemporaryLocationRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("EffectiveLocation") != null %>'>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Effective Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <asp:HyperLink ID="EffectiveLocationHyperLink" runat="server" Text='<%# Eval("EffectiveLocation.Name") %>' NavigateUrl='<%# "~/Location2s/Edit.aspx?Id=" + Eval("EffectiveLocationId") %>' Enabled='<%# Session["Location2sPermission"] != null %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="In Transit Destination Service Point:" NavigateUrl="~/ServicePoint2s/Default.aspx" Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="InTransitDestinationServicePointRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("InTransitDestinationServicePointId") %>' OnDataBinding="Holding2sItem2sInTransitDestinationServicePointRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Order Item:" NavigateUrl="~/OrderItem2s/Default.aspx" Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="OrderItemRadComboBox" runat="server" MaxHeight="500px" Width="500px" EnableLoadOnDemand="true" SelectedValue='<%# Bind("OrderItemId") %>' OnDataBinding="Holding2sItem2sOrderItemRadComboBox_DataBinding" OnItemsRequested="Holding2sItem2sOrderItemRadComboBox_ItemsRequested" />
                                                                <asp:CustomValidator ID="OrderItemCustomValidator" runat="server" ErrorMessage="The Order Item field is invalid." ControlToValidate="OrderItemRadComboBox" Display="Dynamic" CssClass="Error" OnServerValidate="Holding2sItem2sOrderItemCustomValidator_ServerValidate" ValidationGroup="Holding2sItem2s" />
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
                                                                <asp:Label ID="LastCheckInDateTimeLabel" runat="server" Text="Last Check In Date Time:" AssociatedControlID="LastCheckInDateTimeRadDateTimePicker" />
                                                            </td>
                                                            <td>
                                                                <telerik:RadDateTimePicker ID="LastCheckInDateTimeRadDateTimePicker" runat="server" DbSelectedDate='<%# Bind("LastCheckInDateTime") %>' Width="500px" MinDate="1/1/1900" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Last Check In Service Point:" NavigateUrl="~/ServicePoint2s/Default.aspx" Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="LastCheckInServicePointRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("LastCheckInServicePointId") %>' OnDataBinding="Holding2sItem2sLastCheckInServicePointRadComboBox_DataBinding">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="" Text="" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("LastCheckInStaffMember") != null %>'>
                                                            <td>
                                                                <asp:HyperLink runat="server" Text="Last Check In Staff Member:" NavigateUrl="~/User2s/Default.aspx" Enabled='<%# Session["User2sPermission"] != null %>' />
                                                            </td>
                                                            <td>
                                                                <asp:HyperLink ID="LastCheckInStaffMemberHyperLink" runat="server" Text='<%# Eval("LastCheckInStaffMember.Username") %>' NavigateUrl='<%# "~/User2s/Edit.aspx?Id=" + Eval("LastCheckInStaffMemberId") %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" visible='<%# Eval("Content") != null %>'>
                                                            <td>
                                                                <asp:Label ID="ContentLabel" runat="server" Text="Content:" AssociatedControlID="ContentLiteral" />
                                                            </td>
                                                            <td>
                                                                <pre><asp:Literal ID="ContentLiteral" runat="server" Text='<%#: Eval("Content") %>' /></pre>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' ValidationGroup="Holding2sItem2s" />
                                                                <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                                                <asp:CustomValidator ID="Item2CustomValidator" runat="server" Display="Dynamic" CssClass="Error" ValidationGroup="Holding2sItem2s" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </FormTemplate>
                                        </EditFormSettings>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <EditFormSettings EditFormType="Template">
                        <FormTemplate>
                            <asp:Panel ID="EditHolding2Panel" runat="server" DefaultButton="InsertUpdateRadButton">
                                <div>
                                    <asp:ValidationSummary ID="Holding2ValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="Error" ValidationGroup="Holding2s" Visible="false" />
                                </div>
                                <table>
                                    <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                        <td>
                                            <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("Id")}" %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="VersionLabel" runat="server" Text="Version:" AssociatedControlID="VersionRadNumericTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="VersionRadNumericTextBox" runat="server" DbValue='<%# Bind("Version") %>' Width="500px" DataType="System.Int32" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" MaxValue="2147483647" />
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("ShortId") != null %>'>
                                        <td>
                                            <asp:Label ID="ShortIdLabel" runat="server" Text="Short Id:" AssociatedControlID="ShortIdLiteral" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="ShortIdLiteral" runat="server" Text='<%# Eval("ShortId") %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Holding Type:" NavigateUrl="~/HoldingType2s/Default.aspx" Enabled='<%# Session["HoldingType2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="HoldingTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("HoldingTypeId") %>' OnDataBinding="Holding2sHoldingTypeRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="LocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("LocationId") %>' OnDataBinding="Holding2sLocationRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="LocationRequiredFieldValidator" runat="server" ControlToValidate="LocationRadComboBox" ErrorMessage="The Location field is required." Display="Dynamic" CssClass="Error" ValidationGroup="Holding2s" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Temporary Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="TemporaryLocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("TemporaryLocationId") %>' OnDataBinding="Holding2sTemporaryLocationRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Effective Location:" NavigateUrl="~/Location2s/Default.aspx" Enabled='<%# Session["Location2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="EffectiveLocationRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("EffectiveLocationId") %>' OnDataBinding="Holding2sEffectiveLocationRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Call Number Type:" NavigateUrl="~/CallNumberType2s/Default.aspx" Enabled='<%# Session["CallNumberType2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="CallNumberTypeRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("CallNumberTypeId") %>' OnDataBinding="Holding2sCallNumberTypeRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CallNumberPrefixLabel" runat="server" Text="Call Number Prefix:" AssociatedControlID="CallNumberPrefixRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="CallNumberPrefixRadTextBox" runat="server" Text='<%# Bind("CallNumberPrefix") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CallNumberLabel" runat="server" Text="Call Number:" AssociatedControlID="CallNumberRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="CallNumberRadTextBox" runat="server" Text='<%# Bind("CallNumber") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CallNumberSuffixLabel" runat="server" Text="Call Number Suffix:" AssociatedControlID="CallNumberSuffixRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="CallNumberSuffixRadTextBox" runat="server" Text='<%# Bind("CallNumberSuffix") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="ShelvingTitleLabel" runat="server" Text="Shelving Title:" AssociatedControlID="ShelvingTitleRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="ShelvingTitleRadTextBox" runat="server" Text='<%# Bind("ShelvingTitle") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="AcquisitionFormatLabel" runat="server" Text="Acquisition Format:" AssociatedControlID="AcquisitionFormatRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="AcquisitionFormatRadTextBox" runat="server" Text='<%# Bind("AcquisitionFormat") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="AcquisitionMethodLabel" runat="server" Text="Acquisition Method:" AssociatedControlID="AcquisitionMethodRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="AcquisitionMethodRadTextBox" runat="server" Text='<%# Bind("AcquisitionMethod") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="ReceiptStatusLabel" runat="server" Text="Receipt Status:" AssociatedControlID="ReceiptStatusRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="ReceiptStatusRadTextBox" runat="server" Text='<%# Bind("ReceiptStatus") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HyperLink runat="server" Text="Ill Policy:" NavigateUrl="~/IllPolicy2s/Default.aspx" Enabled='<%# Session["IllPolicy2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="IllPolicyRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("IllPolicyId") %>' OnDataBinding="Holding2sIllPolicyRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="RetentionPolicyLabel" runat="server" Text="Retention Policy:" AssociatedControlID="RetentionPolicyRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="RetentionPolicyRadTextBox" runat="server" Text='<%# Bind("RetentionPolicy") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="DigitizationPolicyLabel" runat="server" Text="Digitization Policy:" AssociatedControlID="DigitizationPolicyRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="DigitizationPolicyRadTextBox" runat="server" Text='<%# Bind("DigitizationPolicy") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="CopyNumberLabel" runat="server" Text="Copy Number:" AssociatedControlID="CopyNumberRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="CopyNumberRadTextBox" runat="server" Text='<%# Bind("CopyNumber") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="ItemCountLabel" runat="server" Text="Item Count:" AssociatedControlID="ItemCountRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="ItemCountRadTextBox" runat="server" Text='<%# Bind("ItemCount") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="ReceivingHistoryDisplayTypeLabel" runat="server" Text="Receiving History Display Type:" AssociatedControlID="ReceivingHistoryDisplayTypeRadTextBox" />
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="ReceivingHistoryDisplayTypeRadTextBox" runat="server" Text='<%# Bind("ReceivingHistoryDisplayType") %>' MaxLength="1024" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="DiscoverySuppressRadCheckBox" runat="server" Text="Discovery Suppress" Checked='<%# Bind("DiscoverySuppress") %>' AutoPostBack="false" />
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
                                            <asp:HyperLink runat="server" Text="Source:" NavigateUrl="~/Source2s/Default.aspx" Enabled='<%# Session["Source2sPermission"] != null %>' />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="SourceRadComboBox" runat="server" MarkFirstMatch="true" MaxHeight="500px" Width="500px" AppendDataBoundItems="true" DataTextField="Name" DataValueField="Id" SelectedValue='<%# Bind("SourceId") %>' OnDataBinding="Holding2sSourceRadComboBox_DataBinding">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="" Text="" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" visible='<%# Eval("Content") != null %>'>
                                        <td>
                                            <asp:Label ID="ContentLabel" runat="server" Text="Content:" AssociatedControlID="ContentLiteral" />
                                        </td>
                                        <td>
                                            <pre><asp:Literal ID="ContentLiteral" runat="server" Text='<%#: Eval("Content") %>' /></pre>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <telerik:RadButton ID="InsertUpdateRadButton" runat="server" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' ValidationGroup="Holding2s" />
                                            <telerik:RadButton ID="CancelRadButton" runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                                            <asp:CustomValidator ID="Holding2CustomValidator" runat="server" Display="Dynamic" CssClass="Error" ValidationGroup="Holding2s" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </FormTemplate>
                    </EditFormSettings>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="OrderItem2sPanel" runat="server" Visible='<%# (string)Session["OrderItem2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="OrderItem2sHyperLink" runat="server" Text="Order Items" NavigateUrl="~/OrderItem2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="OrderItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="OrderItem2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No order items found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Edition" DataField="Edition" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Checkin Items" DataField="CheckinItems" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Agreement Id" DataField="AgreementId" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" />
                        <telerik:GridBoundColumn HeaderText="Acquisition Method" DataField="AcquisitionMethod" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Cancellation Restriction" DataField="CancellationRestriction" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Cancellation Restriction Note" DataField="CancellationRestrictionNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Collection" DataField="Collection" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Physical Unit List Price" DataField="PhysicalUnitListPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Electronic Unit List Price" DataField="ElectronicUnitListPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Additional Cost" DataField="AdditionalCost" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Discount" DataField="Discount" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Discount Type" DataField="DiscountType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Exchange Rate" DataField="ExchangeRate" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Physical Quantity" DataField="PhysicalQuantity" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Electronic Quantity" DataField="ElectronicQuantity" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Estimated Price" DataField="EstimatedPrice" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Fiscal Year Rollover Adjustment Amount" DataField="FiscalYearRolloverAdjustmentAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Internal Note" DataField="InternalNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receiving Note" DataField="ReceivingNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription From" DataField="SubscriptionFrom" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Subscription Interval" DataField="SubscriptionInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Subscription To" DataField="SubscriptionTo" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Donor" DataField="Donor" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource Activated" DataField="EresourceActivated" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Eresource Activation Due" DataField="EresourceActivationDue" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Eresource Create Inventory" DataField="EresourceCreateInventory" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource Trial" DataField="EresourceTrial" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Eresource Expected Activation Date" DataField="EresourceExpectedActivationDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Eresource User Limit" DataField="EresourceUserLimit" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Eresource Access Provider" DataField="EresourceAccessProvider.Name" SortExpression="EresourceAccessProvider.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EresourceAccessProviderHyperLink" runat="server" Text='<%#: Eval("EresourceAccessProvider.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("EresourceAccessProviderId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Eresource License Code" DataField="EresourceLicenseCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource License Description" DataField="EresourceLicenseDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource License Reference" DataField="EresourceLicenseReference" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Eresource Material Type" DataField="EresourceMaterialType.Name" SortExpression="EresourceMaterialType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EresourceMaterialTypeHyperLink" runat="server" Text='<%#: Eval("EresourceMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("EresourceMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridHyperLinkColumn HeaderText="Eresource Resource URL" DataTextField="EresourceResourceUrl" DataNavigateUrlFields="EresourceResourceUrl" Target="_blank" SortExpression="EresourceResourceUrl" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Is Package" DataField="IsPackage" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Order Format" DataField="OrderFormat" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Package Order Item" DataField="PackageOrderItem.Number" SortExpression="PackageOrderItem.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="PackageOrderItemHyperLink" runat="server" Text='<%#: Eval("PackageOrderItemId") != null ? Eval("PackageOrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("PackageOrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Payment Status" DataField="PaymentStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Physical Create Inventory" DataField="PhysicalCreateInventory" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Physical Material Type" DataField="PhysicalMaterialType.Name" SortExpression="PhysicalMaterialType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PhysicalMaterialTypeHyperLink" runat="server" Text='<%#: Eval("PhysicalMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("PhysicalMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Physical Material Supplier" DataField="PhysicalMaterialSupplier.Name" SortExpression="PhysicalMaterialSupplier.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PhysicalMaterialSupplierHyperLink" runat="server" Text='<%#: Eval("PhysicalMaterialSupplier.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("PhysicalMaterialSupplierId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Physical Expected Receipt Date" DataField="PhysicalExpectedReceiptDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Physical Receipt Due" DataField="PhysicalReceiptDue" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Publication Year" DataField="PublicationYear" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Publisher" DataField="Publisher" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Order" DataField="Order.Number" SortExpression="Order.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Receipt Date" DataField="ReceiptDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Receipt Status" DataField="ReceiptStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Requester" DataField="Requester" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Rush" DataField="Rush" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Selector" DataField="Selector" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Title Or Package" DataField="TitleOrPackage" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Instructions" DataField="VendorInstructions" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Note" DataField="VendorNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Customer Id" DataField="VendorCustomerId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="PrecedingSucceedingTitle2sPanel" runat="server" Visible='<%# (string)Session["PrecedingSucceedingTitle2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PrecedingSucceedingTitle2sHyperLink" runat="server" Text="Preceding Succeeding Titles" NavigateUrl="~/PrecedingSucceedingTitle2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="PrecedingSucceedingTitle2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PrecedingSucceedingTitle2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No preceding succeeding titles found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Succeeding Instance" DataField="SucceedingInstance.Title" SortExpression="SucceedingInstance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SucceedingInstanceHyperLink" runat="server" Text='<%#: Eval("SucceedingInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SucceedingInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Hrid" DataField="Hrid" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="PrecedingSucceedingTitle2s1Panel" runat="server" Visible='<%# (string)Session["PrecedingSucceedingTitle2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="PrecedingSucceedingTitle2s1HyperLink" runat="server" Text="Preceding Succeeding Titles 1" NavigateUrl="~/PrecedingSucceedingTitle2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="PrecedingSucceedingTitle2s1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="PrecedingSucceedingTitle2s1RadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No preceding succeeding titles found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Preceding Instance" DataField="PrecedingInstance.Title" SortExpression="PrecedingInstance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="PrecedingInstanceHyperLink" runat="server" Text='<%#: Eval("PrecedingInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("PrecedingInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/PrecedingSucceedingTitle2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Hrid" DataField="Hrid" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RelationshipsPanel" runat="server" Visible='<%# (string)Session["RelationshipsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RelationshipsHyperLink" runat="server" Text="Relationships" NavigateUrl="~/Relationships/Default.aspx" /></legend>
            <telerik:RadGrid ID="RelationshipsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RelationshipsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No relationships found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Relationships/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Relationships/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Super Instance" DataField="SuperInstance.Title" SortExpression="SuperInstance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SuperInstanceHyperLink" runat="server" Text='<%#: Eval("SuperInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SuperInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Instance Relationship Type" DataField="InstanceRelationshipType.Name" SortExpression="InstanceRelationshipType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceRelationshipTypeHyperLink" runat="server" Text='<%#: Eval("InstanceRelationshipType.Name") %>' NavigateUrl='<%# $"~/RelationshipTypes/Edit.aspx?Id={Eval("InstanceRelationshipTypeId")}" %>' Enabled='<%# Session["RelationshipTypesPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Relationships1Panel" runat="server" Visible='<%# (string)Session["RelationshipsPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Relationships1HyperLink" runat="server" Text="Relationships 1" NavigateUrl="~/Relationships/Default.aspx" /></legend>
            <telerik:RadGrid ID="Relationships1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Relationships1RadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No relationships found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Relationships/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Relationships/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Sub Instance" DataField="SubInstance.Title" SortExpression="SubInstance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SubInstanceHyperLink" runat="server" Text='<%#: Eval("SubInstance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("SubInstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Instance Relationship Type" DataField="InstanceRelationshipType.Name" SortExpression="InstanceRelationshipType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceRelationshipTypeHyperLink" runat="server" Text='<%#: Eval("InstanceRelationshipType.Name") %>' NavigateUrl='<%# $"~/RelationshipTypes/Edit.aspx?Id={Eval("InstanceRelationshipTypeId")}" %>' Enabled='<%# Session["RelationshipTypesPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Title2sPanel" runat="server" Visible='<%# (string)Session["Title2sPermission"] != null && Instance2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Title2sHyperLink" runat="server" Text="Titles" NavigateUrl="~/Title2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Title2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Title2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No titles found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Expected Receipt Date" DataField="ExpectedReceiptDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="TitleHyperLink" runat="server" Text='<%#: Eval("Title") %>' NavigateUrl='<%# $"~/Title2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Order Item" DataField="OrderItem.Number" SortExpression="OrderItem.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Publisher" DataField="Publisher" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Edition" DataField="Edition" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Package Name" DataField="PackageName" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Item Number" DataField="OrderItemNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Published Date" DataField="PublishedDate" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Receiving Note" DataField="ReceivingNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription From" DataField="SubscriptionFrom" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Subscription To" DataField="SubscriptionTo" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Subscription Interval" DataField="SubscriptionInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Is Acknowledged" DataField="IsAcknowledged" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Creation User" DataField="CreationUser.Username" SortExpression="CreationUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Last Write User" DataField="LastWriteUser.Username" SortExpression="LastWriteUser.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="InsertUpdateRadButton">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Instance2Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Fee2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Fee2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Holding2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Holding2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="OrderItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="OrderItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PrecedingSucceedingTitle2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PrecedingSucceedingTitle2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="PrecedingSucceedingTitle2s1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="PrecedingSucceedingTitle2s1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RelationshipsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RelationshipsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Relationships1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Relationships1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Title2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Title2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
