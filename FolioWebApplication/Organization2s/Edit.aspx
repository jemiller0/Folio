<%@ Page Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="FolioWebApplication.Organization2s.Edit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="Organization2Panel" runat="server">
        <fieldset>
            <legend>
                <asp:HyperLink ID="Organization2HyperLink" runat="server" Text="Organization" NavigateUrl="Default.aspx" /></legend>
            <asp:FormView ID="Organization2FormView" runat="server" DataKeyNames="Id" DefaultMode="ReadOnly" RenderOuterTable="false" OnDataBinding="Organization2FormView_DataBinding">
                <ItemTemplate>
                    <asp:Panel ID="ViewOrganization2Panel" runat="server">
                        <table>
                            <tr runat="server" visible='<%# Eval("Id") != null %>'>
                                <td>
                                    <asp:Label ID="IdLabel" runat="server" Text="Id:" AssociatedControlID="IdHyperLink" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"Edit.aspx?Id={Eval("Id")}" %>' />
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
                            <tr runat="server" visible='<%# Eval("Code") != null %>'>
                                <td>
                                    <asp:Label ID="CodeLabel" runat="server" Text="Code:" AssociatedControlID="CodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="CodeLiteral" runat="server" Text='<%#: Eval("Code") %>' />
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
                            <tr runat="server" visible='<%# Eval("ExportToAccounting") != null %>'>
                                <td>
                                    <asp:Label ID="ExportToAccountingLabel" runat="server" Text="Export To Accounting:" AssociatedControlID="ExportToAccountingLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExportToAccountingLiteral" runat="server" Text='<%#: Eval("ExportToAccounting") %>' />
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
                            <tr runat="server" visible='<%# Eval("Language") != null %>'>
                                <td>
                                    <asp:Label ID="LanguageLabel" runat="server" Text="Language:" AssociatedControlID="LanguageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LanguageLiteral" runat="server" Text='<%#: Eval("Language") %>' />
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
                            <tr runat="server" visible='<%# Eval("PaymentMethod") != null %>'>
                                <td>
                                    <asp:Label ID="PaymentMethodLabel" runat="server" Text="Payment Method:" AssociatedControlID="PaymentMethodLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="PaymentMethodLiteral" runat="server" Text='<%#: Eval("PaymentMethod") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("AccessProvider") != null %>'>
                                <td>
                                    <asp:Label ID="AccessProviderLabel" runat="server" Text="Access Provider:" AssociatedControlID="AccessProviderLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="AccessProviderLiteral" runat="server" Text='<%#: Eval("AccessProvider") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Governmental") != null %>'>
                                <td>
                                    <asp:Label ID="GovernmentalLabel" runat="server" Text="Governmental:" AssociatedControlID="GovernmentalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="GovernmentalLiteral" runat="server" Text='<%#: Eval("Governmental") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("Licensor") != null %>'>
                                <td>
                                    <asp:Label ID="LicensorLabel" runat="server" Text="Licensor:" AssociatedControlID="LicensorLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LicensorLiteral" runat="server" Text='<%#: Eval("Licensor") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("MaterialSupplier") != null %>'>
                                <td>
                                    <asp:Label ID="MaterialSupplierLabel" runat="server" Text="Material Supplier:" AssociatedControlID="MaterialSupplierLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="MaterialSupplierLiteral" runat="server" Text='<%#: Eval("MaterialSupplier") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ClaimingInterval") != null %>'>
                                <td>
                                    <asp:Label ID="ClaimingIntervalLabel" runat="server" Text="Claiming Interval:" AssociatedControlID="ClaimingIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ClaimingIntervalLiteral" runat="server" Text='<%#: Eval("ClaimingInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("DiscountPercent") != null %>'>
                                <td>
                                    <asp:Label ID="DiscountPercentLabel" runat="server" Text="Discount Percent:" AssociatedControlID="DiscountPercentLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="DiscountPercentLiteral" runat="server" Text='<%#: Eval("DiscountPercent") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExpectedActivationInterval") != null %>'>
                                <td>
                                    <asp:Label ID="ExpectedActivationIntervalLabel" runat="server" Text="Expected Activation Interval:" AssociatedControlID="ExpectedActivationIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpectedActivationIntervalLiteral" runat="server" Text='<%#: Eval("ExpectedActivationInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExpectedInvoiceInterval") != null %>'>
                                <td>
                                    <asp:Label ID="ExpectedInvoiceIntervalLabel" runat="server" Text="Expected Invoice Interval:" AssociatedControlID="ExpectedInvoiceIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpectedInvoiceIntervalLiteral" runat="server" Text='<%#: Eval("ExpectedInvoiceInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("RenewalActivationInterval") != null %>'>
                                <td>
                                    <asp:Label ID="RenewalActivationIntervalLabel" runat="server" Text="Renewal Activation Interval:" AssociatedControlID="RenewalActivationIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="RenewalActivationIntervalLiteral" runat="server" Text='<%#: Eval("RenewalActivationInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SubscriptionInterval") != null %>'>
                                <td>
                                    <asp:Label ID="SubscriptionIntervalLabel" runat="server" Text="Subscription Interval:" AssociatedControlID="SubscriptionIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SubscriptionIntervalLiteral" runat="server" Text='<%#: Eval("SubscriptionInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("ExpectedReceiptInterval") != null %>'>
                                <td>
                                    <asp:Label ID="ExpectedReceiptIntervalLabel" runat="server" Text="Expected Receipt Interval:" AssociatedControlID="ExpectedReceiptIntervalLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="ExpectedReceiptIntervalLiteral" runat="server" Text='<%#: Eval("ExpectedReceiptInterval") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TaxId") != null %>'>
                                <td>
                                    <asp:Label ID="TaxIdLabel" runat="server" Text="Tax Id:" AssociatedControlID="TaxIdLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TaxIdLiteral" runat="server" Text='<%#: Eval("TaxId") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("LiableForVat") != null %>'>
                                <td>
                                    <asp:Label ID="LiableForVatLabel" runat="server" Text="Liable For Vat:" AssociatedControlID="LiableForVatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="LiableForVatLiteral" runat="server" Text='<%#: Eval("LiableForVat") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("TaxPercentage") != null %>'>
                                <td>
                                    <asp:Label ID="TaxPercentageLabel" runat="server" Text="Tax Percentage:" AssociatedControlID="TaxPercentageLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="TaxPercentageLiteral" runat="server" Text='<%#: Eval("TaxPercentage") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiVendorEdiCode") != null %>'>
                                <td>
                                    <asp:Label ID="EdiVendorEdiCodeLabel" runat="server" Text="EDI Vendor EDI Code:" AssociatedControlID="EdiVendorEdiCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiVendorEdiCodeLiteral" runat="server" Text='<%#: Eval("EdiVendorEdiCode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiVendorEdiType") != null %>'>
                                <td>
                                    <asp:Label ID="EdiVendorEdiTypeLabel" runat="server" Text="EDI Vendor EDI Type:" AssociatedControlID="EdiVendorEdiTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiVendorEdiTypeLiteral" runat="server" Text='<%#: Eval("EdiVendorEdiType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiLibEdiCode") != null %>'>
                                <td>
                                    <asp:Label ID="EdiLibEdiCodeLabel" runat="server" Text="EDI Lib EDI Code:" AssociatedControlID="EdiLibEdiCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiLibEdiCodeLiteral" runat="server" Text='<%#: Eval("EdiLibEdiCode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiLibEdiType") != null %>'>
                                <td>
                                    <asp:Label ID="EdiLibEdiTypeLabel" runat="server" Text="EDI Lib EDI Type:" AssociatedControlID="EdiLibEdiTypeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiLibEdiTypeLiteral" runat="server" Text='<%#: Eval("EdiLibEdiType") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiProrateTax") != null %>'>
                                <td>
                                    <asp:Label ID="EdiProrateTaxLabel" runat="server" Text="EDI Prorate Tax:" AssociatedControlID="EdiProrateTaxLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiProrateTaxLiteral" runat="server" Text='<%#: Eval("EdiProrateTax") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiProrateFees") != null %>'>
                                <td>
                                    <asp:Label ID="EdiProrateFeesLabel" runat="server" Text="EDI Prorate Fees:" AssociatedControlID="EdiProrateFeesLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiProrateFeesLiteral" runat="server" Text='<%#: Eval("EdiProrateFees") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiNamingConvention") != null %>'>
                                <td>
                                    <asp:Label ID="EdiNamingConventionLabel" runat="server" Text="EDI Naming Convention:" AssociatedControlID="EdiNamingConventionLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiNamingConventionLiteral" runat="server" Text='<%#: Eval("EdiNamingConvention") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiSendAcctNum") != null %>'>
                                <td>
                                    <asp:Label ID="EdiSendAcctNumLabel" runat="server" Text="EDI Send Acct Num:" AssociatedControlID="EdiSendAcctNumLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiSendAcctNumLiteral" runat="server" Text='<%#: Eval("EdiSendAcctNum") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiSupportOrder") != null %>'>
                                <td>
                                    <asp:Label ID="EdiSupportOrderLabel" runat="server" Text="EDI Support Order:" AssociatedControlID="EdiSupportOrderLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiSupportOrderLiteral" runat="server" Text='<%#: Eval("EdiSupportOrder") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiSupportInvoice") != null %>'>
                                <td>
                                    <asp:Label ID="EdiSupportInvoiceLabel" runat="server" Text="EDI Support Invoice:" AssociatedControlID="EdiSupportInvoiceLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiSupportInvoiceLiteral" runat="server" Text='<%#: Eval("EdiSupportInvoice") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiNotes") != null %>'>
                                <td>
                                    <asp:Label ID="EdiNotesLabel" runat="server" Text="EDI Notes:" AssociatedControlID="EdiNotesLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiNotesLiteral" runat="server" Text='<%#: Eval("EdiNotes") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpFtpFormat") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpFtpFormatLabel" runat="server" Text="EDI FTP FTP Format:" AssociatedControlID="EdiFtpFtpFormatLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpFtpFormatLiteral" runat="server" Text='<%#: Eval("EdiFtpFtpFormat") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpServerAddress") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpServerAddressLabel" runat="server" Text="EDI FTP Server Address:" AssociatedControlID="EdiFtpServerAddressLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpServerAddressLiteral" runat="server" Text='<%#: Eval("EdiFtpServerAddress") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpUsername") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpUsernameLabel" runat="server" Text="EDI FTP Username:" AssociatedControlID="EdiFtpUsernameLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpUsernameLiteral" runat="server" Text='<%#: Eval("EdiFtpUsername") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpPassword") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpPasswordLabel" runat="server" Text="EDI FTP Password:" AssociatedControlID="EdiFtpPasswordLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpPasswordLiteral" runat="server" Text='<%#: Eval("EdiFtpPassword") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpFtpMode") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpFtpModeLabel" runat="server" Text="EDI FTP FTP Mode:" AssociatedControlID="EdiFtpFtpModeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpFtpModeLiteral" runat="server" Text='<%#: Eval("EdiFtpFtpMode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpFtpConnMode") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpFtpConnModeLabel" runat="server" Text="EDI FTP FTP Conn Mode:" AssociatedControlID="EdiFtpFtpConnModeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpFtpConnModeLiteral" runat="server" Text='<%#: Eval("EdiFtpFtpConnMode") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpFtpPort") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpFtpPortLabel" runat="server" Text="EDI FTP FTP Port:" AssociatedControlID="EdiFtpFtpPortLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpFtpPortLiteral" runat="server" Text='<%#: Eval("EdiFtpFtpPort") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpOrderDirectory") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpOrderDirectoryLabel" runat="server" Text="EDI FTP Order Directory:" AssociatedControlID="EdiFtpOrderDirectoryLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpOrderDirectoryLiteral" runat="server" Text='<%#: Eval("EdiFtpOrderDirectory") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpInvoiceDirectory") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpInvoiceDirectoryLabel" runat="server" Text="EDI FTP Invoice Directory:" AssociatedControlID="EdiFtpInvoiceDirectoryLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpInvoiceDirectoryLiteral" runat="server" Text='<%#: Eval("EdiFtpInvoiceDirectory") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiFtpNotes") != null %>'>
                                <td>
                                    <asp:Label ID="EdiFtpNotesLabel" runat="server" Text="EDI FTP Notes:" AssociatedControlID="EdiFtpNotesLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiFtpNotesLiteral" runat="server" Text='<%#: Eval("EdiFtpNotes") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobScheduleEdi") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobScheduleEdiLabel" runat="server" Text="EDI Job Schedule Edi:" AssociatedControlID="EdiJobScheduleEdiLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobScheduleEdiLiteral" runat="server" Text='<%#: Eval("EdiJobScheduleEdi") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobSchedulingDate") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobSchedulingDateLabel" runat="server" Text="EDI Job Scheduling Date:" AssociatedControlID="EdiJobSchedulingDateLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobSchedulingDateLiteral" runat="server" Text='<%# Eval("EdiJobSchedulingDate", "{0:d}") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobTime") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobTimeLabel" runat="server" Text="EDI Job Time:" AssociatedControlID="EdiJobTimeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobTimeLiteral" runat="server" Text='<%#: Eval("EdiJobTime") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobIsMonday") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobIsMondayLabel" runat="server" Text="EDI Job Is Monday:" AssociatedControlID="EdiJobIsMondayLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobIsMondayLiteral" runat="server" Text='<%#: Eval("EdiJobIsMonday") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobIsTuesday") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobIsTuesdayLabel" runat="server" Text="EDI Job Is Tuesday:" AssociatedControlID="EdiJobIsTuesdayLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobIsTuesdayLiteral" runat="server" Text='<%#: Eval("EdiJobIsTuesday") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobIsWednesday") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobIsWednesdayLabel" runat="server" Text="EDI Job Is Wednesday:" AssociatedControlID="EdiJobIsWednesdayLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobIsWednesdayLiteral" runat="server" Text='<%#: Eval("EdiJobIsWednesday") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobIsThursday") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobIsThursdayLabel" runat="server" Text="EDI Job Is Thursday:" AssociatedControlID="EdiJobIsThursdayLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobIsThursdayLiteral" runat="server" Text='<%#: Eval("EdiJobIsThursday") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobIsFriday") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobIsFridayLabel" runat="server" Text="EDI Job Is Friday:" AssociatedControlID="EdiJobIsFridayLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobIsFridayLiteral" runat="server" Text='<%#: Eval("EdiJobIsFriday") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobIsSaturday") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobIsSaturdayLabel" runat="server" Text="EDI Job Is Saturday:" AssociatedControlID="EdiJobIsSaturdayLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobIsSaturdayLiteral" runat="server" Text='<%#: Eval("EdiJobIsSaturday") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobIsSunday") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobIsSundayLabel" runat="server" Text="EDI Job Is Sunday:" AssociatedControlID="EdiJobIsSundayLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobIsSundayLiteral" runat="server" Text='<%#: Eval("EdiJobIsSunday") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobSendToEmails") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobSendToEmailsLabel" runat="server" Text="EDI Job Send To Emails:" AssociatedControlID="EdiJobSendToEmailsLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobSendToEmailsLiteral" runat="server" Text='<%#: Eval("EdiJobSendToEmails") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobNotifyAllEdi") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobNotifyAllEdiLabel" runat="server" Text="EDI Job Notify All Edi:" AssociatedControlID="EdiJobNotifyAllEdiLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobNotifyAllEdiLiteral" runat="server" Text='<%#: Eval("EdiJobNotifyAllEdi") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobNotifyInvoiceOnly") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobNotifyInvoiceOnlyLabel" runat="server" Text="EDI Job Notify Invoice Only:" AssociatedControlID="EdiJobNotifyInvoiceOnlyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobNotifyInvoiceOnlyLiteral" runat="server" Text='<%#: Eval("EdiJobNotifyInvoiceOnly") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobNotifyErrorOnly") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobNotifyErrorOnlyLabel" runat="server" Text="EDI Job Notify Error Only:" AssociatedControlID="EdiJobNotifyErrorOnlyLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobNotifyErrorOnlyLiteral" runat="server" Text='<%#: Eval("EdiJobNotifyErrorOnly") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("EdiJobSchedulingNotes") != null %>'>
                                <td>
                                    <asp:Label ID="EdiJobSchedulingNotesLabel" runat="server" Text="EDI Job Scheduling Notes:" AssociatedControlID="EdiJobSchedulingNotesLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="EdiJobSchedulingNotesLiteral" runat="server" Text='<%#: Eval("EdiJobSchedulingNotes") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("IsVendor") != null %>'>
                                <td>
                                    <asp:Label ID="IsVendorLabel" runat="server" Text="Is Vendor:" AssociatedControlID="IsVendorLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="IsVendorLiteral" runat="server" Text='<%#: Eval("IsVendor") %>' />
                                </td>
                            </tr>
                            <tr runat="server" visible='<%# Eval("SanCode") != null %>'>
                                <td>
                                    <asp:Label ID="SanCodeLabel" runat="server" Text="San Code:" AssociatedControlID="SanCodeLiteral" />
                                </td>
                                <td>
                                    <asp:Literal ID="SanCodeLiteral" runat="server" Text='<%#: Eval("SanCode") %>' />
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
    <asp:Panel ID="Invoice2sPanel" runat="server" Visible='<%# (string)Session["Invoice2sPermission"] != null && Organization2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Accounting Code" DataField="AccountingCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Adjustments Total" DataField="AdjustmentsTotal" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Approved By" DataField="ApprovedBy.Username" SortExpression="ApprovedBy.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ApprovedByHyperLink" runat="server" Text='<%#: Eval("ApprovedById") != null ? Eval("ApprovedBy.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ApprovedById")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Approval Date" DataField="ApprovalDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Batch Group" DataField="BatchGroup.Name" SortExpression="BatchGroup.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Bill To" DataField="BillTo.Id" SortExpression="BillTo.Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="BillToHyperLink" runat="server" Text='<%# Eval("BillTo.Id") %>' NavigateUrl='<%# $"~/Configuration2s/Edit.aspx?Id={Eval("BillToId")}" %>' Enabled='<%# Session["Configuration2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Check Subscription Overlap" DataField="CheckSubscriptionOverlap" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Cancellation Note" DataField="CancellationNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Currency" DataField="Currency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Enclosure Needed" DataField="EnclosureNeeded" AutoPostBackOnFilter="true" />
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
                        <telerik:GridBoundColumn HeaderText="Vendor Invoice No" DataField="VendorInvoiceNo" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Number" DataField="DisbursementNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Voucher Number" DataField="VoucherNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Payment" DataField="Payment.Amount" SortExpression="Payment.Amount" AutoPostBackOnFilter="true">
                            <ItemTemplate>
                                <asp:HyperLink ID="PaymentHyperLink" runat="server" Text='<%# $"{Eval("Payment.Amount"):c}" %>' NavigateUrl='<%# $"~/Transaction2s/Edit.aspx?Id={Eval("PaymentId")}" %>' Enabled='<%# Session["Transaction2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Disbursement Date" DataField="DisbursementDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Account Number" DataField="AccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Manual Payment" DataField="ManualPayment" AutoPostBackOnFilter="true" />
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
                        <telerik:GridBoundColumn HeaderText="Invoice Transaction Summary 2" DataField="InvoiceTransactionSummary2" AutoPostBackOnFilter="true" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="Order2sPanel" runat="server" Visible='<%# (string)Session["Order2sPermission"] != null && Organization2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="Order2sHyperLink" runat="server" Text="Orders" NavigateUrl="~/Order2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="Order2sRadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="Order2sRadGrid_NeedDataSource">
                <MasterTableView DataKeyNames="Id" PagerStyle-Mode="NextPrevNumericAndAdvanced" NoMasterRecordsText="No orders found">
                    <SortExpressions>
                        <telerik:GridSortExpression FieldName="LastWriteTime" SortOrder="Descending" />
                    </SortExpressions>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="0px">
                            <ItemTemplate>
                                <asp:HyperLink ID="ViewHyperLink" Text="View" NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("Id")}" %>' runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Approved" DataField="Approved" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Approved By" DataField="ApprovedBy.Username" SortExpression="ApprovedBy.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="ApprovedByHyperLink" runat="server" Text='<%#: Eval("ApprovedById") != null ? Eval("ApprovedBy.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("ApprovedById")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Approval Date" DataField="ApprovalDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Assigned To" DataField="AssignedTo.Username" SortExpression="AssignedTo.Username" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="AssignedToHyperLink" runat="server" Text='<%#: Eval("AssignedToId") != null ? Eval("AssignedTo.Username") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/User2s/Edit.aspx?Id={Eval("AssignedToId")}" %>' Enabled='<%# Session["User2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Bill To" DataField="BillTo" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Close Reason Reason" DataField="CloseReasonReason" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Close Reason Note" DataField="CloseReasonNote" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Order Date" DataField="OrderDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Manual" DataField="Manual" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn HeaderText="Number" DataField="Number" SortExpression="Number" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="NumberHyperLink" runat="server" Text='<%#: Eval("Number") %>' NavigateUrl='<%# $"~/Order2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Order Type" DataField="OrderType" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Reencumber" DataField="Reencumber" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Ongoing Interval" DataField="OngoingInterval" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Ongoing Is Subscription" DataField="OngoingIsSubscription" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Ongoing Manual Renewal" DataField="OngoingManualRenewal" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Ongoing Notes" DataField="OngoingNotes" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Ongoing Review Period" DataField="OngoingReviewPeriod" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Ongoing Renewal Date" DataField="OngoingRenewalDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Ongoing Review Date" DataField="OngoingReviewDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Ship To" DataField="ShipTo" AutoPostBackOnFilter="true" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Template" DataField="Template.Id" SortExpression="Template.Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="TemplateHyperLink" runat="server" Text='<%# Eval("Template.Id") %>' NavigateUrl='<%# $"~/OrderTemplate2s/Edit.aspx?Id={Eval("TemplateId")}" %>' Enabled='<%# Session["OrderTemplate2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Status" DataField="Status" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
                        <telerik:GridBoundColumn HeaderText="Order Transaction Summary" DataField="OrderTransactionSummary2" AutoPostBackOnFilter="true" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="OrderItem2sPanel" runat="server" Visible='<%# (string)Session["OrderItem2sPermission"] != null && Organization2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridBoundColumn HeaderText="Eresource License Code" DataField="EresourceLicenseCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource License Description" DataField="EresourceLicenseDescription" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Eresource License Reference" DataField="EresourceLicenseReference" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Eresource Material Type" DataField="EresourceMaterialType.Name" SortExpression="EresourceMaterialType.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="EresourceMaterialTypeHyperLink" runat="server" Text='<%#: Eval("EresourceMaterialType.Name") %>' NavigateUrl='<%# $"~/MaterialType2s/Edit.aspx?Id={Eval("EresourceMaterialTypeId")}" %>' Enabled='<%# Session["MaterialType2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridHyperLinkColumn HeaderText="Eresource Resource URL" DataTextField="EresourceResourceUrl" DataNavigateUrlFields="EresourceResourceUrl" Target="_blank" SortExpression="EresourceResourceUrl" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Instance" DataField="Instance.Title" SortExpression="Instance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
    <asp:Panel ID="OrderItem2s1Panel" runat="server" Visible='<%# (string)Session["OrderItem2sPermission"] != null && Organization2FormView.DataKey.Value != null %>'>
        <fieldset>
            <legend>
                <asp:HyperLink ID="OrderItem2s1HyperLink" runat="server" Text="Order Items 1" NavigateUrl="~/OrderItem2s/Default.aspx" /></legend>
            <telerik:RadGrid ID="OrderItem2s1RadGrid" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowFilteringByColumn="false" GroupingSettings-CaseSensitive="false" AllowPaging="true" AllowCustomPaging="true" PageSize="10" EnableLinqExpressions="false" OnNeedDataSource="OrderItem2s1RadGrid_NeedDataSource">
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
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Instance" DataField="Instance.Title" SortExpression="Instance.Title" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InstanceHyperLink" runat="server" Text='<%#: Eval("Instance.Title") %>' NavigateUrl='<%# $"~/Instance2s/Edit.aspx?Id={Eval("InstanceId")}" %>' Enabled='<%# Session["Instance2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
    <asp:Panel ID="Voucher2sPanel" runat="server" Visible='<%# (string)Session["Voucher2sPermission"] != null && Organization2FormView.DataKey.Value != null %>'>
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
                        <telerik:GridTemplateColumn HeaderText="Id" DataField="Id" SortExpression="Id" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                            <ItemTemplate>
                                <asp:HyperLink ID="IdHyperLink" runat="server" Text='<%# Eval("Id") %>' NavigateUrl='<%# $"~/Voucher2s/Edit.aspx?Id={Eval("Id")}" %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Accounting Code" DataField="AccountingCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Account Number" DataField="AccountNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Amount" DataField="Amount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Batch Group" DataField="BatchGroup.Name" SortExpression="BatchGroup.Name" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="BatchGroupHyperLink" runat="server" Text='<%#: Eval("BatchGroup.Name") %>' NavigateUrl='<%# $"~/BatchGroup2s/Edit.aspx?Id={Eval("BatchGroupId")}" %>' Enabled='<%# Session["BatchGroup2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Disbursement Number" DataField="DisbursementNumber" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Date" DataField="DisbursementDate" AutoPostBackOnFilter="true" DataFormatString="{0:d}" />
                        <telerik:GridBoundColumn HeaderText="Disbursement Amount" DataField="DisbursementAmount" AutoPostBackOnFilter="true" DataFormatString="{0:c}" />
                        <telerik:GridBoundColumn HeaderText="Enclosure Needed" DataField="EnclosureNeeded" AutoPostBackOnFilter="true" />
                        <telerik:GridBoundColumn HeaderText="Invoice Currency" DataField="InvoiceCurrency" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridTemplateColumn AllowFiltering="false" AllowSorting="false" HeaderText="Invoice" DataField="Invoice.Number" SortExpression="Invoice.Number" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith">
                            <ItemTemplate>
                                <asp:HyperLink ID="InvoiceHyperLink" runat="server" Text='<%#: Eval("InvoiceId") != null ? Eval("Invoice.Number") ?? "&nbsp;" : "" %>' NavigateUrl='<%# $"~/Invoice2s/Edit.aspx?Id={Eval("InvoiceId")}" %>' Enabled='<%# Session["Invoice2sPermission"] != null %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
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
                        <telerik:GridBoundColumn HeaderText="Vendor Street Address 1" DataField="VendorStreetAddress1" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Street Address 2" DataField="VendorStreetAddress2" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor City" DataField="VendorCity" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor State" DataField="VendorState" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Postal Code" DataField="VendorPostalCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
                        <telerik:GridBoundColumn HeaderText="Vendor Country Code" DataField="VendorCountryCode" AutoPostBackOnFilter="true" HtmlEncode="true" CurrentFilterFunction="StartsWith" />
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
            <telerik:AjaxSetting AjaxControlID="Invoice2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Invoice2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="Order2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="Order2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="OrderItem2sRadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="OrderItem2sPanel" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="OrderItem2s1RadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="OrderItem2s1Panel" LoadingPanelID="RadAjaxLoadingPanel1" />
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
