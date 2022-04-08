<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Fee2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Fee2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Fee2HyperLink" runat="server" Text="Fee" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Fee2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Fee2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewFee2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Amount") != null %>'>
                                <td>
                                    <asp:Label ID="AmountLabel" runat="server" Text="Amount:" AssociatedControlID="AmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AmountLiteral" runat="server" Text='<%# Eval("Amount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RemainingAmount") != null %>'>
                                <td>
                                    <asp:Label ID="RemainingAmountLabel" runat="server" Text="Remaining Amount:" AssociatedControlID="RemainingAmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RemainingAmountLiteral" runat="server" Text='<%# Eval("RemainingAmount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("StatusName") != null %>'>
                                <td>
                                    <asp:Label ID="StatusNameLabel" runat="server" Text="Status Name:" AssociatedControlID="StatusNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatusNameLiteral" runat="server" Text='<%#: Eval("StatusName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PaymentStatusName") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentStatusNameLabel" runat="server" Text="Payment Status Name:" AssociatedControlID="PaymentStatusNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaymentStatusNameLiteral" runat="server" Text='<%#: Eval("PaymentStatusName") %>' />
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
                            <tr runat="server" visible='<%# Eval("CallNumber") != null %>'>
                                <td>
                                    <asp:Label ID="CallNumberLabel" runat="server" Text="Call Number:" AssociatedControlID="CallNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CallNumberLiteral" runat="server" Text='<%#: Eval("CallNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Barcode") != null %>'>
                                <td>
                                    <asp:Label ID="BarcodeLabel" runat="server" Text="Barcode:" AssociatedControlID="BarcodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="BarcodeLiteral" runat="server" Text='<%#: Eval("Barcode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("MaterialType") != null %>'>
                                <td>
                                    <asp:Label ID="MaterialTypeLabel" runat="server" Text="Material Type:" AssociatedControlID="MaterialTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="MaterialTypeLiteral" runat="server" Text='<%#: Eval("MaterialType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ItemStatusName") != null %>'>
                                <td>
                                    <asp:Label ID="ItemStatusNameLabel" runat="server" Text="Item Status Name:" AssociatedControlID="ItemStatusNameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ItemStatusNameLiteral" runat="server" Text='<%#: Eval("ItemStatusName") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Location") != null %>'>
                                <td>
                                    <asp:Label ID="LocationLabel" runat="server" Text="Location:" AssociatedControlID="LocationLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LocationLiteral" runat="server" Text='<%#: Eval("Location") %>' />
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
                            <tr runat="server" visible='<%# Eval("DueTime") != null %>'>
                                <td>
                                    <asp:Label ID="DueTimeLabel" runat="server" Text="Due Time:" AssociatedControlID="DueTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DueTimeLiteral" runat="server" Text='<%# Eval("DueTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReturnedTime") != null %>'>
                                <td>
                                    <asp:Label ID="ReturnedTimeLabel" runat="server" Text="Returned Time:" AssociatedControlID="ReturnedTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReturnedTimeLiteral" runat="server" Text='<%# Eval("ReturnedTime", "{0:g}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Loan") != null %>'>
                                <td>
                                    <asp:Label ID="LoanLabel" runat="server" Text="Loan:" AssociatedControlID="LoanHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="LoanHyperLink" runat="server" Text='<%# Eval("Loan.Id") %>' NavigateUrl='<%# $"~/Loan2s/Edit.aspx?Id={Eval("LoanId")}" %>' Enabled='<%# Session["Loan2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("User") != null %>'>
                                <td>
                                    <asp:Label ID="UserLabel" runat="server" Text="User:" AssociatedControlID="UserHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("User.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Item") != null %>'>
                                <td>
                                    <asp:Label ID="ItemLabel" runat="server" Text="Item:" AssociatedControlID="ItemHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ItemHyperLink" runat="server" Text='<%# Eval("Item.ShortId") %>' NavigateUrl='<%# $"~/Item2s/Edit.aspx?Id={Eval("ItemId")}" %>' Enabled='<%# Session["Item2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("MaterialType1") != null %>'>
                                <td>
                                    <asp:Label ID="MaterialType1Label" runat="server" Text="Material Type 1:" AssociatedControlID="MaterialType1HyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="MaterialType1HyperLink" runat="server" Text='<%#: Eval("MaterialType1.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("MaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FeeType") != null %>'>
                                <td>
                                    <asp:Label ID="FeeTypeLabel" runat="server" Text="Fee Type:" AssociatedControlID="FeeTypeHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FeeTypeHyperLink" runat="server" Text='<%#: Eval("FeeType.Name") %>' NavigateUrl='<%# $"~/FeeType2s/Edit.aspx?Id={Eval("FeeTypeId")}" %>' Enabled='<%# Session["FeeType2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Owner") != null %>'>
                                <td>
                                    <asp:Label ID="OwnerLabel" runat="server" Text="Owner:" AssociatedControlID="OwnerHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OwnerHyperLink" runat="server" Text='<%#: Eval("Owner.Name") %>' NavigateUrl='<%# $"~/Owner2s/Edit.aspx?Id={Eval("OwnerId")}" %>' Enabled='<%# Session["Owner2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Holding") != null %>'>
                                <td>
                                    <asp:Label ID="HoldingLabel" runat="server" Text="Holding:" AssociatedControlID="HoldingHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="HoldingHyperLink" runat="server" Text='<%# Eval("Holding.ShortId") %>' NavigateUrl='<%# $"~/Holding2s/Edit.aspx?Id={Eval("HoldingId")}" %>' Enabled='<%# Session["Holding2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Instance") != null %>'>
                                <td>
                                    <asp:Label ID="InstanceLabel" runat="server" Text="Instance:" AssociatedControlID="InstanceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
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
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Payment2sPanel" runat="server" Visible='<%# (string)Session["Payment2sPermission"] != null && Fee2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Payment2sHyperLink" runat="server" Text="Payments" NavigateUrl="~/Payment2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Payment2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Payment2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No payments found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="CreationTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Payment2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Payment2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridBoundColumn HeaderText="Type Action" DataField="TypeAction" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Comments" DataField="Comments" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Notify" DataField="Notify" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Remaining Amount" DataField="RemainingAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Transaction Information" DataField="TransactionInformation" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Service Point" DataField="ServicePoint.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ServicePointHyperLink" runat="server" Text='<%#: Eval("ServicePoint.Name") %>' NavigateUrl='<%# $"~/ServicePoint2s/Edit.aspx?Id={Eval("ServicePointId")}" %>' Enabled='<%# Session["ServicePoint2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Payment Method" DataField="PaymentMethod" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="User" DataField="User.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="UserHyperLink" runat="server" Text='<%#: Eval("UserId") != null ? Eval("User.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("UserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="RefundReason2sPanel" runat="server" Visible='<%# (string)Session["RefundReason2sPermission"] != null && Fee2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="RefundReason2sHyperLink" runat="server" Text="Refund Reasons" NavigateUrl="~/RefundReason2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="RefundReason2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="RefundReason2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No refund reasons found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/RefundReason2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/settings/users/refunds" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/RefundReason2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") %>' NavigateUrl='<%# $"~/RefundReason2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Creation User" DataField="CreationUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Last Write User" DataField="LastWriteUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="LastWriteUserHyperLink" runat="server" Text='<%#: Eval("LastWriteUserId") != null ? Eval("LastWriteUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("LastWriteUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="WaiveReason2sPanel" runat="server" Visible='<%# (string)Session["WaiveReason2sPermission"] != null && Fee2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="WaiveReason2sHyperLink" runat="server" Text="Waive Reasons" NavigateUrl="~/WaiveReason2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="WaiveReason2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="WaiveReason2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No waive reasons found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/WaiveReason2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/settings/users/waivereasons" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/WaiveReason2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NameHyperLink" runat="server" Text='<%#: Eval("Name") %>' NavigateUrl='<%# $"~/WaiveReason2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Creation Time" DataField="CreationTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Creation User" DataField="CreationUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="CreationUserHyperLink" runat="server" Text='<%#: Eval("CreationUserId") != null ? Eval("CreationUser.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("CreationUserId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Last Write Time" DataField="LastWriteTime" AutoPostBackOnFilter="true" DataFormatString="{0:g}" />
                        <telerik:GridTemplateColumn HeaderText="Last Write User" DataField="LastWriteUser.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
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
            <telerik:AjaxSetting AjaxControlID="Payment2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Payment2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RefundReason2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RefundReason2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="WaiveReason2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="WaiveReason2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
