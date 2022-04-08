<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Transaction2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Transaction2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Transaction2HyperLink" runat="server" Text="Transaction" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Transaction2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Transaction2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewTransaction2Panel" runat="server">
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
                            <tr runat="server" visible='<%# Eval("AwaitingPaymentEncumbrance") != null %>'>
                                <td>
                                    <asp:Label ID="AwaitingPaymentEncumbranceLabel" runat="server" Text="Awaiting Payment Encumbrance:" AssociatedControlID="AwaitingPaymentEncumbranceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="AwaitingPaymentEncumbranceHyperLink" runat="server" Text='<%# Eval("AwaitingPaymentEncumbrance.Amount", "{0:c}") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("AwaitingPaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AwaitingPaymentReleaseEncumbrance") != null %>'>
                                <td>
                                    <asp:Label ID="AwaitingPaymentReleaseEncumbranceLabel" runat="server" Text="Awaiting Payment Release Encumbrance:" AssociatedControlID="AwaitingPaymentReleaseEncumbranceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AwaitingPaymentReleaseEncumbranceLiteral" runat="server" Text='<%#: Eval("AwaitingPaymentReleaseEncumbrance") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Currency") != null %>'>
                                <td>
                                    <asp:Label ID="CurrencyLabel" runat="server" Text="Currency:" AssociatedControlID="CurrencyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CurrencyLiteral" runat="server" Text='<%#: Eval("Currency") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Description") != null %>'>
                                <td>
                                    <asp:Label ID="DescriptionLabel" runat="server" Text="Description:" AssociatedControlID="DescriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DescriptionLiteral" runat="server" Text='<%#: Eval("Description") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AwaitingPaymentAmount") != null %>'>
                                <td>
                                    <asp:Label ID="AwaitingPaymentAmountLabel" runat="server" Text="Awaiting Payment Amount:" AssociatedControlID="AwaitingPaymentAmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AwaitingPaymentAmountLiteral" runat="server" Text='<%# Eval("AwaitingPaymentAmount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExpendedAmount") != null %>'>
                                <td>
                                    <asp:Label ID="ExpendedAmountLabel" runat="server" Text="Expended Amount:" AssociatedControlID="ExpendedAmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpendedAmountLiteral" runat="server" Text='<%# Eval("ExpendedAmount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InitialEncumberedAmount") != null %>'>
                                <td>
                                    <asp:Label ID="InitialEncumberedAmountLabel" runat="server" Text="Initial Encumbered Amount:" AssociatedControlID="InitialEncumberedAmountLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="InitialEncumberedAmountLiteral" runat="server" Text='<%# Eval("InitialEncumberedAmount", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Status") != null %>'>
                                <td>
                                    <asp:Label ID="StatusLabel" runat="server" Text="Status:" AssociatedControlID="StatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="StatusLiteral" runat="server" Text='<%#: Eval("Status") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OrderType") != null %>'>
                                <td>
                                    <asp:Label ID="OrderTypeLabel" runat="server" Text="Order Type:" AssociatedControlID="OrderTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OrderTypeLiteral" runat="server" Text='<%#: Eval("OrderType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OrderStatus") != null %>'>
                                <td>
                                    <asp:Label ID="OrderStatusLabel" runat="server" Text="Order Status:" AssociatedControlID="OrderStatusLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="OrderStatusLiteral" runat="server" Text='<%#: Eval("OrderStatus") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Subscription") != null %>'>
                                <td>
                                    <asp:Label ID="SubscriptionLabel" runat="server" Text="Subscription:" AssociatedControlID="SubscriptionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubscriptionLiteral" runat="server" Text='<%#: Eval("Subscription") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ReEncumber") != null %>'>
                                <td>
                                    <asp:Label ID="ReEncumberLabel" runat="server" Text="Re Encumber:" AssociatedControlID="ReEncumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ReEncumberLiteral" runat="server" Text='<%#: Eval("ReEncumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Order") != null %>'>
                                <td>
                                    <asp:Label ID="OrderLabel" runat="server" Text="Order:" AssociatedControlID="OrderHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("OrderItem") != null %>'>
                                <td>
                                    <asp:Label ID="OrderItemLabel" runat="server" Text="Order Item:" AssociatedControlID="OrderItemHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItem.Number") %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExpenseClass") != null %>'>
                                <td>
                                    <asp:Label ID="ExpenseClassLabel" runat="server" Text="Expense Class:" AssociatedControlID="ExpenseClassHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ExpenseClassHyperLink" runat="server" Text='<%#: Eval("ExpenseClass.Name") %>' NavigateUrl='<%# $"~/ExpenseClass2s/Edit.aspx?Id={Eval("ExpenseClassId")}" %>' Enabled='<%# Session["ExpenseClass2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FiscalYear") != null %>'>
                                <td>
                                    <asp:Label ID="FiscalYearLabel" runat="server" Text="Fiscal Year:" AssociatedControlID="FiscalYearHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("FromFund") != null %>'>
                                <td>
                                    <asp:Label ID="FromFundLabel" runat="server" Text="From Fund:" AssociatedControlID="FromFundHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="FromFundHyperLink" runat="server" Text='<%#: Eval("FromFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FromFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InvoiceCancelled") != null %>'>
                                <td>
                                    <asp:Label ID="InvoiceCancelledLabel" runat="server" Text="Invoice Cancelled:" AssociatedControlID="InvoiceCancelledLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="InvoiceCancelledLiteral" runat="server" Text='<%#: Eval("InvoiceCancelled") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PaymentEncumbrance") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentEncumbranceLabel" runat="server" Text="Payment Encumbrance:" AssociatedControlID="PaymentEncumbranceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PaymentEncumbranceHyperLink" runat="server" Text='<%# Eval("PaymentEncumbrance.Amount", "{0:c}") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("PaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
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
                            <tr runat="server" visible='<%# Eval("SourceFiscalYear") != null %>'>
                                <td>
                                    <asp:Label ID="SourceFiscalYearLabel" runat="server" Text="Source Fiscal Year:" AssociatedControlID="SourceFiscalYearHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="SourceFiscalYearHyperLink" runat="server" Text='<%#: Eval("SourceFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("SourceFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Invoice") != null %>'>
                                <td>
                                    <asp:Label ID="InvoiceLabel" runat="server" Text="Invoice:" AssociatedControlID="InvoiceHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("Invoice.Number") %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InvoiceItem") != null %>'>
                                <td>
                                    <asp:Label ID="InvoiceItemLabel" runat="server" Text="Invoice Item:" AssociatedControlID="InvoiceItemHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="InvoiceItemHyperLink" runat="server" Text='<%#: Eval("InvoiceItem.Number") %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("InvoiceItemId")}" %>' Enabled='<%# Session["InvoiceItem2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ToFund") != null %>'>
                                <td>
                                    <asp:Label ID="ToFundLabel" runat="server" Text="To Fund:" AssociatedControlID="ToFundHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ToFundHyperLink" runat="server" Text='<%#: Eval("ToFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("ToFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TransactionType") != null %>'>
                                <td>
                                    <asp:Label ID="TransactionTypeLabel" runat="server" Text="Transaction Type:" AssociatedControlID="TransactionTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TransactionTypeLiteral" runat="server" Text='<%#: Eval("TransactionType") %>' />
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
            </asp:FormView>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Invoice2sPanel" runat="server" Visible='<%# (string)Session["Invoice2sPermission"] != null && Transaction2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Invoice2sHyperLink" runat="server" Text="Invoices" NavigateUrl="~/Invoice2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Invoice2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Invoice2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No invoices found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/invoice/view/{Eval("Id")}" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Accounting Code" DataField="AccountingCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Adjustments Total" DataField="AdjustmentsTotal" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridTemplateColumn HeaderText="Approved By" DataField="ApprovedBy.Username" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ApprovedByHyperLink" runat="server" Text='<%#: Eval("ApprovedById") != null ? Eval("ApprovedBy.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ApprovedById")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Approval Date" DataField="ApprovalDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Batch Group" DataField="BatchGroup.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bill To" DataField="BillTo.Id" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Guid">
                            <ItemTemplate>
                                <asp:HyperLink ID="BillToHyperLink" runat="server" Text='<%# Eval("BillTo.Id") %>' NavigateUrl='<%# $"~/Configuration2s/Edit.aspx?Id={Eval("BillToId")}" %>' Enabled='<%# Session["Configuration2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Check Subscription Overlap" DataField="CheckSubscriptionOverlap" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Cancellation Note" DataField="CancellationNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Enclosure" DataField="Enclosure" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Exchange Rate" DataField="ExchangeRate" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Export To Accounting" DataField="ExportToAccounting" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Invoice Date" DataField="InvoiceDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Lock Total" DataField="LockTotal" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Note" DataField="Note" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Payment Due Date" DataField="PaymentDueDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Payment Date" DataField="PaymentDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Payment Terms" DataField="PaymentTerms" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Payment Method" DataField="PaymentMethod" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Sub Total" DataField="SubTotal" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Total" DataField="Total" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Vendor Invoice Number" DataField="VendorInvoiceNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Number" DataField="DisbursementNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Voucher Number" DataField="VoucherNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Date" DataField="DisbursementDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Vendor" DataField="Vendor.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="VendorHyperLink" runat="server" Text='<%#: Eval("Vendor.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("VendorId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Account Number" DataField="AccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Manual Payment" DataField="ManualPayment" AutoPostBackOnFilter="true" />
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
    <asp:Panel ID="Transaction2sPanel" runat="server" Visible='<%# (string)Session["Transaction2sPermission"] != null && Transaction2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Transaction2sHyperLink" runat="server" Text="Transactions" NavigateUrl="~/Transaction2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Transaction2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" ShowFooter="true" EnableLinqExpressions="false" OnNeedDataSource="Transaction2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No transactions found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true" Aggregate="Sum" FooterAggregateFormatString="{0:c}">
                            <ItemTemplate>
                                <asp:HyperLink ID="AmountHyperLink" runat="server" Text='<%# $"{Eval("Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Release Encumbrance" DataField="AwaitingPaymentReleaseEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Amount" DataField="AwaitingPaymentAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Expended Amount" DataField="ExpendedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Initial Encumbered Amount" DataField="InitialEncumberedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Type" DataField="OrderType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Status" DataField="OrderStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription" DataField="Subscription" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Re Encumber" DataField="ReEncumber" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Order" DataField="Order.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expense Class" DataField="ExpenseClass.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ExpenseClassHyperLink" runat="server" Text='<%#: Eval("ExpenseClassId") != null ? Eval("ExpenseClass.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/ExpenseClass2s/Edit.aspx?Id={Eval("ExpenseClassId")}" %>' Enabled='<%# Session["ExpenseClass2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fiscal Year" DataField="FiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Fund" DataField="FromFund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FromFundHyperLink" runat="server" Text='<%#: Eval("FromFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FromFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Invoice Cancelled" DataField="InvoiceCancelled" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Payment Encumbrance" DataField="PaymentEncumbrance.Amount" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Decimal">
                            <ItemTemplate>
                                <asp:HyperLink ID="PaymentEncumbranceHyperLink" runat="server" Text='<%# $"{Eval("PaymentEncumbrance.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("PaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Source Fiscal Year" DataField="SourceFiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SourceFiscalYearHyperLink" runat="server" Text='<%#: Eval("SourceFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("SourceFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice" DataField="Invoice.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("InvoiceId") != null ? Eval("Invoice.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Item" DataField="InvoiceItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceItemHyperLink" runat="server" Text='<%#: Eval("InvoiceItemId") != null ? Eval("InvoiceItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("InvoiceItemId")}" %>' Enabled='<%# Session["InvoiceItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Fund" DataField="ToFund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ToFundHyperLink" runat="server" Text='<%#: Eval("ToFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("ToFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Transaction Type" DataField="TransactionType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="Transaction2s1Panel" runat="server" Visible='<%# (string)Session["Transaction2sPermission"] != null && Transaction2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Transaction2s1HyperLink" runat="server" Text="Transactions 1" NavigateUrl="~/Transaction2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Transaction2s1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" ShowFooter="true" EnableLinqExpressions="false" OnNeedDataSource="Transaction2s1RadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No transactions found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true" Aggregate="Sum" FooterAggregateFormatString="{0:c}">
                            <ItemTemplate>
                                <asp:HyperLink ID="AmountHyperLink" runat="server" Text='<%# $"{Eval("Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Awaiting Payment Encumbrance" DataField="AwaitingPaymentEncumbrance.Amount" AllowSorting="false" AutoPostBackOnFilter="true" DataType="System.Decimal">
                            <ItemTemplate>
                                <asp:HyperLink ID="AwaitingPaymentEncumbranceHyperLink" runat="server" Text='<%# $"{Eval("AwaitingPaymentEncumbrance.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("AwaitingPaymentEncumbranceId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Release Encumbrance" DataField="AwaitingPaymentReleaseEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Awaiting Payment Amount" DataField="AwaitingPaymentAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Expended Amount" DataField="ExpendedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Initial Encumbered Amount" DataField="InitialEncumberedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Type" DataField="OrderType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Status" DataField="OrderStatus" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription" DataField="Subscription" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Re Encumber" DataField="ReEncumber" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Order" DataField="Order.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expense Class" DataField="ExpenseClass.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ExpenseClassHyperLink" runat="server" Text='<%#: Eval("ExpenseClassId") != null ? Eval("ExpenseClass.Name") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/ExpenseClass2s/Edit.aspx?Id={Eval("ExpenseClassId")}" %>' Enabled='<%# Session["ExpenseClass2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fiscal Year" DataField="FiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FiscalYearHyperLink" runat="server" Text='<%#: Eval("FiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("FiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Fund" DataField="FromFund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="FromFundHyperLink" runat="server" Text='<%#: Eval("FromFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("FromFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Invoice Cancelled" DataField="InvoiceCancelled" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Source" DataField="Source" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Source Fiscal Year" DataField="SourceFiscalYear.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="SourceFiscalYearHyperLink" runat="server" Text='<%#: Eval("SourceFiscalYear.Name") %>' NavigateUrl='<%# $"~/FiscalYear2s/Edit.aspx?Id={Eval("SourceFiscalYearId")}" %>' Enabled='<%# Session["FiscalYear2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice" DataField="Invoice.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("InvoiceId") != null ? Eval("Invoice.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Item" DataField="InvoiceItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceItemHyperLink" runat="server" Text='<%#: Eval("InvoiceItemId") != null ? Eval("InvoiceItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("InvoiceItemId")}" %>' Enabled='<%# Session["InvoiceItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Fund" DataField="ToFund.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ToFundHyperLink" runat="server" Text='<%#: Eval("ToFund.Name") %>' NavigateUrl='<%# $"~/Fund2s/Edit.aspx?Id={Eval("ToFundId")}" %>' Enabled='<%# Session["Fund2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Transaction Type" DataField="TransactionType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
    <asp:Panel ID="TransactionTagsPanel" runat="server" Visible='<%# (string)Session["TransactionTagsPermission"] != null && Transaction2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="TransactionTagsHyperLink" runat="server" Text="Transaction Tags" NavigateUrl="~/TransactionTags/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="TransactionTagsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="TransactionTagsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No transaction tags found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Content" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Content" DataField="Content" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="VoucherItem2sPanel" runat="server" Visible='<%# (string)Session["VoucherItem2sPermission"] != null && Transaction2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="VoucherItem2sHyperLink" runat="server" Text="Voucher Items" NavigateUrl="~/VoucherItem2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="VoucherItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" ShowFooter="true" EnableLinqExpressions="false" OnNeedDataSource="VoucherItem2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No voucher items found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/VoucherItem2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/VoucherItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridTemplateColumn HeaderText="Account Number" DataField="AccountNumber" SortExpression="AccountNumber" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="AccountNumberHyperLink" runat="server" Text='<%#: Eval("AccountNumber") %>' NavigateUrl='<%# $"~/VoucherItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voucher" DataField="Voucher.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="VoucherHyperLink" runat="server" Text='<%#: Eval("Voucher.Number") %>' NavigateUrl='<%# $"~/Voucher2s/Edit.aspx?Id={Eval("VoucherId")}" %>' Enabled='<%# Session["Voucher2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
            <telerik:AjaxSetting AjaxControlID="Invoice2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Invoice2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Transaction2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Transaction2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Transaction2s1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Transaction2s1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="TransactionTagsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="TransactionTagsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="VoucherItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="VoucherItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
