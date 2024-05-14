<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Invoice2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Invoice2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Invoice2HyperLink" runat="server" Text="Invoice" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Invoice2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Invoice2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewInvoice2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AccountingCode") != null %>'>
                                <td>
                                    <asp:Label ID="AccountingCodeLabel" runat="server" Text="Accounting Code:" AssociatedControlID="AccountingCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AccountingCodeLiteral" runat="server" Text='<%#: Eval("AccountingCode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AdjustmentsTotal") != null %>'>
                                <td>
                                    <asp:Label ID="AdjustmentsTotalLabel" runat="server" Text="Adjustments Total:" AssociatedControlID="AdjustmentsTotalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AdjustmentsTotalLiteral" runat="server" Text='<%# Eval("AdjustmentsTotal", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ApprovedBy") != null %>'>
                                <td>
                                    <asp:Label ID="ApprovedByLabel" runat="server" Text="Approved By:" AssociatedControlID="ApprovedByHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="ApprovedByHyperLink" runat="server" Text='<%#: Eval("ApprovedBy.Username") %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ApprovedById")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ApprovalDate") != null %>'>
                                <td>
                                    <asp:Label ID="ApprovalDateLabel" runat="server" Text="Approval Date:" AssociatedControlID="ApprovalDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ApprovalDateLiteral" runat="server" Text='<%# Eval("ApprovalDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BatchGroup") != null %>'>
                                <td>
                                    <asp:Label ID="BatchGroupLabel" runat="server" Text="Batch Group:" AssociatedControlID="BatchGroupHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("BillTo") != null %>'>
                                <td>
                                    <asp:Label ID="BillToLabel" runat="server" Text="Bill To:" AssociatedControlID="BillToHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="BillToHyperLink" runat="server" Text='<%# Eval("BillTo.Id") %>' NavigateUrl='<%# $"~/Configuration2s/Edit.aspx?Id={Eval("BillToId")}" %>' Enabled='<%# Session["Configuration2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CheckSubscriptionOverlap") != null %>'>
                                <td>
                                    <asp:Label ID="CheckSubscriptionOverlapLabel" runat="server" Text="Check Subscription Overlap:" AssociatedControlID="CheckSubscriptionOverlapLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CheckSubscriptionOverlapLiteral" runat="server" Text='<%#: Eval("CheckSubscriptionOverlap") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("CancellationNote") != null %>'>
                                <td>
                                    <asp:Label ID="CancellationNoteLabel" runat="server" Text="Cancellation Note:" AssociatedControlID="CancellationNoteLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CancellationNoteLiteral" runat="server" Text='<%#: Eval("CancellationNote") %>' />
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
                            <tr runat="server" visible='<%# Eval("Enclosure") != null %>'>
                                <td>
                                    <asp:Label ID="EnclosureLabel" runat="server" Text="Enclosure:" AssociatedControlID="EnclosureLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EnclosureLiteral" runat="server" Text='<%#: Eval("Enclosure") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExchangeRate") != null %>'>
                                <td>
                                    <asp:Label ID="ExchangeRateLabel" runat="server" Text="Exchange Rate:" AssociatedControlID="ExchangeRateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExchangeRateLiteral" runat="server" Text='<%#: Eval("ExchangeRate") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExportToAccounting") != null %>'>
                                <td>
                                    <asp:Label ID="ExportToAccountingLabel" runat="server" Text="Export To Accounting:" AssociatedControlID="ExportToAccountingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExportToAccountingLiteral" runat="server" Text='<%#: Eval("ExportToAccounting") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Number") != null %>'>
                                <td>
                                    <asp:Label ID="NumberLabel" runat="server" Text="Number:" AssociatedControlID="NumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NumberLiteral" runat="server" Text='<%#: Eval("Number") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("InvoiceDate") != null %>'>
                                <td>
                                    <asp:Label ID="InvoiceDateLabel" runat="server" Text="Invoice Date:" AssociatedControlID="InvoiceDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="InvoiceDateLiteral" runat="server" Text='<%# Eval("InvoiceDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LockTotal") != null %>'>
                                <td>
                                    <asp:Label ID="LockTotalLabel" runat="server" Text="Lock Total:" AssociatedControlID="LockTotalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LockTotalLiteral" runat="server" Text='<%# Eval("LockTotal", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Note") != null %>'>
                                <td>
                                    <asp:Label ID="NoteLabel" runat="server" Text="Note:" AssociatedControlID="NoteLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NoteLiteral" runat="server" Text='<%#: Eval("Note") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PaymentDueDate") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentDueDateLabel" runat="server" Text="Payment Due Date:" AssociatedControlID="PaymentDueDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaymentDueDateLiteral" runat="server" Text='<%# Eval("PaymentDueDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PaymentDate") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentDateLabel" runat="server" Text="Payment Date:" AssociatedControlID="PaymentDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaymentDateLiteral" runat="server" Text='<%# Eval("PaymentDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PaymentTerms") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentTermsLabel" runat="server" Text="Payment Terms:" AssociatedControlID="PaymentTermsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaymentTermsLiteral" runat="server" Text='<%#: Eval("PaymentTerms") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("PaymentMethod") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentMethodLabel" runat="server" Text="Payment Method:" AssociatedControlID="PaymentMethodLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaymentMethodLiteral" runat="server" Text='<%#: Eval("PaymentMethod") %>' />
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
                            <tr runat="server" visible='<%# Eval("Source") != null %>'>
                                <td>
                                    <asp:Label ID="SourceLabel" runat="server" Text="Source:" AssociatedControlID="SourceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SourceLiteral" runat="server" Text='<%#: Eval("Source") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubTotal") != null %>'>
                                <td>
                                    <asp:Label ID="SubTotalLabel" runat="server" Text="Sub Total:" AssociatedControlID="SubTotalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubTotalLiteral" runat="server" Text='<%# Eval("SubTotal", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Total") != null %>'>
                                <td>
                                    <asp:Label ID="TotalLabel" runat="server" Text="Total:" AssociatedControlID="TotalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TotalLiteral" runat="server" Text='<%# Eval("Total", "{0:c}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VendorInvoiceNumber") != null %>'>
                                <td>
                                    <asp:Label ID="VendorInvoiceNumberLabel" runat="server" Text="Vendor Invoice Number:" AssociatedControlID="VendorInvoiceNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VendorInvoiceNumberLiteral" runat="server" Text='<%#: Eval("VendorInvoiceNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DisbursementNumber") != null %>'>
                                <td>
                                    <asp:Label ID="DisbursementNumberLabel" runat="server" Text="Disbursement Number:" AssociatedControlID="DisbursementNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DisbursementNumberLiteral" runat="server" Text='<%#: Eval("DisbursementNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("VoucherNumber") != null %>'>
                                <td>
                                    <asp:Label ID="VoucherNumberLabel" runat="server" Text="Voucher Number:" AssociatedControlID="VoucherNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="VoucherNumberLiteral" runat="server" Text='<%#: Eval("VoucherNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Payment") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentLabel" runat="server" Text="Payment:" AssociatedControlID="PaymentHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="PaymentHyperLink" runat="server" Text='<%# Eval("Payment.Amount", "{0:c}") %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("PaymentId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DisbursementDate") != null %>'>
                                <td>
                                    <asp:Label ID="DisbursementDateLabel" runat="server" Text="Disbursement Date:" AssociatedControlID="DisbursementDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DisbursementDateLiteral" runat="server" Text='<%# Eval("DisbursementDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Vendor") != null %>'>
                                <td>
                                    <asp:Label ID="VendorLabel" runat="server" Text="Vendor:" AssociatedControlID="VendorHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="VendorHyperLink" runat="server" Text='<%#: Eval("Vendor.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("VendorId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
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
                            <tr runat="server" visible='<%# Eval("AccountNumber") != null %>'>
                                <td>
                                    <asp:Label ID="AccountNumberLabel" runat="server" Text="Account Number:" AssociatedControlID="AccountNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AccountNumberLiteral" runat="server" Text='<%#: Eval("AccountNumber") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ManualPayment") != null %>'>
                                <td>
                                    <asp:Label ID="ManualPaymentLabel" runat="server" Text="Manual Payment:" AssociatedControlID="ManualPaymentLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ManualPaymentLiteral" runat="server" Text='<%#: Eval("ManualPayment") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("NextInvoiceLineNumber") != null %>'>
                                <td>
                                    <asp:Label ID="NextInvoiceLineNumberLabel" runat="server" Text="Next Invoice Line Number:" AssociatedControlID="NextInvoiceLineNumberLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="NextInvoiceLineNumberLiteral" runat="server" Text='<%#: Eval("NextInvoiceLineNumber") %>' />
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
    <asp:Panel ID="InvoiceAcquisitionsUnitsPanel" runat="server" Visible='<%# (string)Session["InvoiceAcquisitionsUnitsPermission"] != null && Invoice2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InvoiceAcquisitionsUnitsHyperLink" runat="server" Text="Invoice Acquisitions Units" NavigateUrl="~/InvoiceAcquisitionsUnits/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InvoiceAcquisitionsUnitsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InvoiceAcquisitionsUnitsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No invoice acquisitions units found">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Acquisitions Unit" DataField="AcquisitionsUnit.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="AcquisitionsUnitHyperLink" runat="server" Text='<%#: Eval("AcquisitionsUnit.Name") %>' NavigateUrl='<%# $"~/AcquisitionsUnit2s/Edit.aspx?Id={Eval("AcquisitionsUnitId")}" %>' Enabled='<%# Session["AcquisitionsUnit2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="InvoiceAdjustmentsPanel" runat="server" Visible='<%# (string)Session["InvoiceAdjustmentsPermission"] != null && Invoice2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InvoiceAdjustmentsHyperLink" runat="server" Text="Invoice Adjustments" NavigateUrl="~/InvoiceAdjustments/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InvoiceAdjustmentsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InvoiceAdjustmentsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No invoice adjustments found">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Export To Accounting" DataField="ExportToAccounting" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Prorate" DataField="Prorate" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Relation To Total" DataField="RelationToTotal" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Type" DataField="Type" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Value" DataField="Value" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Total Amount" DataField="TotalAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="InvoiceItem2sPanel" runat="server" Visible='<%# (string)Session["InvoiceItem2sPermission"] != null && Invoice2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InvoiceItem2sHyperLink" runat="server" Text="Invoice Items" NavigateUrl="~/InvoiceItem2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="InvoiceItem2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" ShowFooter="true" EnableLinqExpressions="false" OnNeedDataSource="InvoiceItem2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No invoice items found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Accounting Code" DataField="AccountingCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Account Number" DataField="AccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Adjustments Total" DataField="AdjustmentsTotal" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Comment" DataField="Comment" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Description" DataField="Description" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") ?? "&nbsp;" %>' NavigateUrl='<%# $"~/InvoiceItem2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Order Item" DataField="OrderItem.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderItemHyperLink" runat="server" Text='<%#: Eval("OrderItemId") != null ? Eval("OrderItem.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/OrderItem2s/Edit.aspx?Id={Eval("OrderItemId")}" %>' Enabled='<%# Session["OrderItem2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Product Id" DataField="ProductId" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn HeaderText="Product Id Type" DataField="ProductIdType.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ProductIdTypeHyperLink" runat="server" Text='<%#: Eval("ProductIdType.Name") %>' NavigateUrl='<%# $"~/IdType2s/Edit.aspx?Id={Eval("ProductIdTypeId")}" %>' Enabled='<%# Session["IdType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Quantity" DataField="Quantity" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Release Encumbrance" DataField="ReleaseEncumbrance" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Subscription Info" DataField="SubscriptionInfo" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Subscription Start Date" DataField="SubscriptionStartDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Subscription End Date" DataField="SubscriptionEndDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Sub Total" DataField="SubTotal" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
                        <telerik:GridBoundColumn HeaderText="Total" DataField="Total" AutoPostBackOnFilter="true" DataFormatString="{0:c}" Aggregate="Sum" />
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
    <asp:Panel ID="InvoiceOrderNumbersPanel" runat="server" Visible='<%# (string)Session["InvoiceOrderNumbersPermission"] != null && Invoice2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InvoiceOrderNumbersHyperLink" runat="server" Text="Invoice Order Numbers" NavigateUrl="~/InvoiceOrderNumbers/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InvoiceOrderNumbersRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InvoiceOrderNumbersRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No invoice order numbers found">
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
    <asp:Panel ID="InvoiceTagsPanel" runat="server" Visible='<%# (string)Session["InvoiceTagsPermission"] != null && Invoice2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="InvoiceTagsHyperLink" runat="server" Text="Invoice Tags" NavigateUrl="~/InvoiceTags/Default.aspx" Enabled="false" /></legend>
            <telerik:RadGrid ID="InvoiceTagsRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="InvoiceTagsRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No invoice tags found">
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
    <asp:Panel ID="OrderInvoice2sPanel" runat="server" Visible='<%# (string)Session["OrderInvoice2sPermission"] != null && Invoice2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="OrderInvoice2sHyperLink" runat="server" Text="Order Invoices" NavigateUrl="~/OrderInvoice2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="OrderInvoice2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="OrderInvoice2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No order invoices found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="Id" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/OrderInvoice2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/OrderInvoice2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order" DataField="Order.Number" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="OrderHyperLink" runat="server" Text='<%#: Eval("Order.Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("OrderId")}" %>' Enabled='<%# Session["Order2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Transaction2sPanel" runat="server" Visible='<%# (string)Session["Transaction2sPermission"] != null && Invoice2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridBoundColumn HeaderText="Voided Amount" DataField="VoidedAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
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
    <asp:Panel ID="Voucher2sPanel" runat="server" Visible='<%# (string)Session["Voucher2sPermission"] != null && Invoice2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Voucher2sHyperLink" runat="server" Text="Vouchers" NavigateUrl="~/Voucher2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Voucher2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Voucher2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No vouchers found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Voucher2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="FolioHyperLink" runat="server" Text="FOLIO" NavigateUrl='<%# $"https://uchicago.folio.indexdata.com/invoice/view/{Eval("InvoiceId")}/voucher/{Eval("Id")}/view" %>' Target="_blank" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Voucher2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Accounting Code" DataField="AccountingCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Account Number" DataField="AccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridTemplateColumn HeaderText="Batch Group" DataField="BatchGroup.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Disbursement Number" DataField="DisbursementNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Date" DataField="DisbursementDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Amount" DataField="DisbursementAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Enclosure" DataField="Enclosure" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Invoice Currency" DataField="InvoiceCurrency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Exchange Rate" DataField="ExchangeRate" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Export To Accounting" DataField="ExportToAccounting" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="System Currency" DataField="SystemCurrency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Type" DataField="Type" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Voucher Date" DataField="VoucherDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") %>' NavigateUrl='<%# $"~/Voucher2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vendor" DataField="Vendor.Name" AllowSorting="false" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="VendorHyperLink" runat="server" Text='<%#: Eval("Vendor.Name") %>' NavigateUrl='<%# $"~/Organization2s/Edit.aspx?Id={Eval("VendorId")}" %>' Enabled='<%# Session["Organization2sPermission"] != null %>' />
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
            <telerik:AjaxSetting AjaxControlID="InvoiceAcquisitionsUnitsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InvoiceAcquisitionsUnitsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InvoiceAdjustmentsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InvoiceAdjustmentsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InvoiceItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InvoiceItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InvoiceOrderNumbersRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InvoiceOrderNumbersPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="InvoiceTagsRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="InvoiceTagsPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="OrderInvoice2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="OrderInvoice2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Transaction2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Transaction2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Voucher2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Voucher2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
</asp:Content>
