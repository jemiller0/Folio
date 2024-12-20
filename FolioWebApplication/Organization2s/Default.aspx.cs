using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Organization2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Organization2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Organization2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "Language", "language" }, { "AccountingCode", "erpCode" }, { "PaymentMethod", "paymentMethod" }, { "AccessProvider", "accessProvider" }, { "Governmental", "governmental" }, { "Licensor", "licensor" }, { "MaterialSupplier", "materialSupplier" }, { "ClaimingInterval", "claimingInterval" }, { "DiscountPercent", "discountPercent" }, { "ExpectedActivationInterval", "expectedActivationInterval" }, { "ExpectedInvoiceInterval", "expectedInvoiceInterval" }, { "RenewalActivationInterval", "renewalActivationInterval" }, { "SubscriptionInterval", "subscriptionInterval" }, { "ExpectedReceiptInterval", "expectedReceiptInterval" }, { "TaxId", "taxId" }, { "LiableForVat", "liableForVat" }, { "TaxPercentage", "taxPercentage" }, { "EdiVendorEdiCode", "edi.vendorEdiCode" }, { "EdiVendorEdiType", "edi.vendorEdiType" }, { "EdiLibEdiCode", "edi.libEdiCode" }, { "EdiLibEdiType", "edi.libEdiType" }, { "EdiProrateTax", "edi.prorateTax" }, { "EdiProrateFees", "edi.prorateFees" }, { "EdiNamingConvention", "edi.ediNamingConvention" }, { "EdiSendAcctNum", "edi.sendAcctNum" }, { "EdiSupportOrder", "edi.supportOrder" }, { "EdiSupportInvoice", "edi.supportInvoice" }, { "EdiNotes", "edi.notes" }, { "EdiFtpFtpFormat", "edi.ediFtp.ftpFormat" }, { "EdiFtpServerAddress", "edi.ediFtp.serverAddress" }, { "EdiFtpUsername", "edi.ediFtp.username" }, { "EdiFtpPassword", "edi.ediFtp.password" }, { "EdiFtpFtpMode", "edi.ediFtp.ftpMode" }, { "EdiFtpFtpConnMode", "edi.ediFtp.ftpConnMode" }, { "EdiFtpFtpPort", "edi.ediFtp.ftpPort" }, { "EdiFtpOrderDirectory", "edi.ediFtp.orderDirectory" }, { "EdiFtpInvoiceDirectory", "edi.ediFtp.invoiceDirectory" }, { "EdiFtpNotes", "edi.ediFtp.notes" }, { "EdiJobScheduleEdi", "edi.ediJob.scheduleEdi" }, { "EdiJobSchedulingDate", "edi.ediJob.schedulingDate" }, { "EdiJobTime", "edi.ediJob.time" }, { "EdiJobIsMonday", "edi.ediJob.isMonday" }, { "EdiJobIsTuesday", "edi.ediJob.isTuesday" }, { "EdiJobIsWednesday", "edi.ediJob.isWednesday" }, { "EdiJobIsThursday", "edi.ediJob.isThursday" }, { "EdiJobIsFriday", "edi.ediJob.isFriday" }, { "EdiJobIsSaturday", "edi.ediJob.isSaturday" }, { "EdiJobIsSunday", "edi.ediJob.isSunday" }, { "EdiJobSendToEmails", "edi.ediJob.sendToEmails" }, { "EdiJobNotifyAllEdi", "edi.ediJob.notifyAllEdi" }, { "EdiJobNotifyInvoiceOnly", "edi.ediJob.notifyInvoiceOnly" }, { "EdiJobNotifyErrorOnly", "edi.ediJob.notifyErrorOnly" }, { "EdiJobSchedulingNotes", "edi.ediJob.schedulingNotes" }, { "IsVendor", "isVendor" }, { "IsDonor", "isDonor" }, { "SanCode", "sanCode" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Organization2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Organization2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Organization2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Organization2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Organization2sRadGrid, "ExportToAccounting", "exportToAccounting"),
                Global.GetCqlFilter(Organization2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Organization2sRadGrid, "Language", "language"),
                Global.GetCqlFilter(Organization2sRadGrid, "AccountingCode", "erpCode"),
                Global.GetCqlFilter(Organization2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Organization2sRadGrid, "AccessProvider", "accessProvider"),
                Global.GetCqlFilter(Organization2sRadGrid, "Governmental", "governmental"),
                Global.GetCqlFilter(Organization2sRadGrid, "Licensor", "licensor"),
                Global.GetCqlFilter(Organization2sRadGrid, "MaterialSupplier", "materialSupplier"),
                Global.GetCqlFilter(Organization2sRadGrid, "ClaimingInterval", "claimingInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "DiscountPercent", "discountPercent"),
                Global.GetCqlFilter(Organization2sRadGrid, "ExpectedActivationInterval", "expectedActivationInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "ExpectedInvoiceInterval", "expectedInvoiceInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "RenewalActivationInterval", "renewalActivationInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "SubscriptionInterval", "subscriptionInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "ExpectedReceiptInterval", "expectedReceiptInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "TaxId", "taxId"),
                Global.GetCqlFilter(Organization2sRadGrid, "LiableForVat", "liableForVat"),
                Global.GetCqlFilter(Organization2sRadGrid, "TaxPercentage", "taxPercentage"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiVendorEdiCode", "edi.vendorEdiCode"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiVendorEdiType", "edi.vendorEdiType"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiLibEdiCode", "edi.libEdiCode"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiLibEdiType", "edi.libEdiType"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiProrateTax", "edi.prorateTax"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiProrateFees", "edi.prorateFees"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiNamingConvention", "edi.ediNamingConvention"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiSendAcctNum", "edi.sendAcctNum"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiSupportOrder", "edi.supportOrder"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiSupportInvoice", "edi.supportInvoice"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiNotes", "edi.notes"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpFtpFormat", "edi.ediFtp.ftpFormat"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpServerAddress", "edi.ediFtp.serverAddress"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpUsername", "edi.ediFtp.username"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpPassword", "edi.ediFtp.password"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpFtpMode", "edi.ediFtp.ftpMode"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpFtpConnMode", "edi.ediFtp.ftpConnMode"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpFtpPort", "edi.ediFtp.ftpPort"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpOrderDirectory", "edi.ediFtp.orderDirectory"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpInvoiceDirectory", "edi.ediFtp.invoiceDirectory"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpNotes", "edi.ediFtp.notes"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobScheduleEdi", "edi.ediJob.scheduleEdi"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobSchedulingDate", "edi.ediJob.schedulingDate"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobTime", "edi.ediJob.time"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsMonday", "edi.ediJob.isMonday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsTuesday", "edi.ediJob.isTuesday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsWednesday", "edi.ediJob.isWednesday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsThursday", "edi.ediJob.isThursday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsFriday", "edi.ediJob.isFriday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsSaturday", "edi.ediJob.isSaturday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsSunday", "edi.ediJob.isSunday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobSendToEmails", "edi.ediJob.sendToEmails"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobNotifyAllEdi", "edi.ediJob.notifyAllEdi"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobNotifyInvoiceOnly", "edi.ediJob.notifyInvoiceOnly"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobNotifyErrorOnly", "edi.ediJob.notifyErrorOnly"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobSchedulingNotes", "edi.ediJob.schedulingNotes"),
                Global.GetCqlFilter(Organization2sRadGrid, "IsVendor", "isVendor"),
                Global.GetCqlFilter(Organization2sRadGrid, "IsDonor", "isDonor"),
                Global.GetCqlFilter(Organization2sRadGrid, "SanCode", "sanCode"),
                Global.GetCqlFilter(Organization2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Organization2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Organization2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Organization2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Organization2sRadGrid.DataSource = folioServiceContext.Organization2s(where, Organization2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Organization2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Organization2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Organization2sRadGrid.PageSize * Organization2sRadGrid.CurrentPageIndex, Organization2sRadGrid.PageSize, true);
            Organization2sRadGrid.VirtualItemCount = folioServiceContext.CountOrganization2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Organization2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tCode\tDescription\tExportToAccounting\tStatus\tLanguage\tAccountingCode\tPaymentMethod\tAccessProvider\tGovernmental\tLicensor\tMaterialSupplier\tClaimingInterval\tDiscountPercent\tExpectedActivationInterval\tExpectedInvoiceInterval\tRenewalActivationInterval\tSubscriptionInterval\tExpectedReceiptInterval\tTaxId\tLiableForVat\tTaxPercentage\tEdiVendorEdiCode\tEdiVendorEdiType\tEdiLibEdiCode\tEdiLibEdiType\tEdiProrateTax\tEdiProrateFees\tEdiNamingConvention\tEdiSendAcctNum\tEdiSupportOrder\tEdiSupportInvoice\tEdiNotes\tEdiFtpFtpFormat\tEdiFtpServerAddress\tEdiFtpUsername\tEdiFtpPassword\tEdiFtpFtpMode\tEdiFtpFtpConnMode\tEdiFtpFtpPort\tEdiFtpOrderDirectory\tEdiFtpInvoiceDirectory\tEdiFtpNotes\tEdiJobScheduleEdi\tEdiJobSchedulingDate\tEdiJobTime\tEdiJobIsMonday\tEdiJobIsTuesday\tEdiJobIsWednesday\tEdiJobIsThursday\tEdiJobIsFriday\tEdiJobIsSaturday\tEdiJobIsSunday\tEdiJobSendToEmails\tEdiJobNotifyAllEdi\tEdiJobNotifyInvoiceOnly\tEdiJobNotifyErrorOnly\tEdiJobSchedulingNotes\tIsVendor\tIsDonor\tSanCode\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "ExportToAccounting", "exportToAccounting" }, { "Status", "status" }, { "Language", "language" }, { "AccountingCode", "erpCode" }, { "PaymentMethod", "paymentMethod" }, { "AccessProvider", "accessProvider" }, { "Governmental", "governmental" }, { "Licensor", "licensor" }, { "MaterialSupplier", "materialSupplier" }, { "ClaimingInterval", "claimingInterval" }, { "DiscountPercent", "discountPercent" }, { "ExpectedActivationInterval", "expectedActivationInterval" }, { "ExpectedInvoiceInterval", "expectedInvoiceInterval" }, { "RenewalActivationInterval", "renewalActivationInterval" }, { "SubscriptionInterval", "subscriptionInterval" }, { "ExpectedReceiptInterval", "expectedReceiptInterval" }, { "TaxId", "taxId" }, { "LiableForVat", "liableForVat" }, { "TaxPercentage", "taxPercentage" }, { "EdiVendorEdiCode", "edi.vendorEdiCode" }, { "EdiVendorEdiType", "edi.vendorEdiType" }, { "EdiLibEdiCode", "edi.libEdiCode" }, { "EdiLibEdiType", "edi.libEdiType" }, { "EdiProrateTax", "edi.prorateTax" }, { "EdiProrateFees", "edi.prorateFees" }, { "EdiNamingConvention", "edi.ediNamingConvention" }, { "EdiSendAcctNum", "edi.sendAcctNum" }, { "EdiSupportOrder", "edi.supportOrder" }, { "EdiSupportInvoice", "edi.supportInvoice" }, { "EdiNotes", "edi.notes" }, { "EdiFtpFtpFormat", "edi.ediFtp.ftpFormat" }, { "EdiFtpServerAddress", "edi.ediFtp.serverAddress" }, { "EdiFtpUsername", "edi.ediFtp.username" }, { "EdiFtpPassword", "edi.ediFtp.password" }, { "EdiFtpFtpMode", "edi.ediFtp.ftpMode" }, { "EdiFtpFtpConnMode", "edi.ediFtp.ftpConnMode" }, { "EdiFtpFtpPort", "edi.ediFtp.ftpPort" }, { "EdiFtpOrderDirectory", "edi.ediFtp.orderDirectory" }, { "EdiFtpInvoiceDirectory", "edi.ediFtp.invoiceDirectory" }, { "EdiFtpNotes", "edi.ediFtp.notes" }, { "EdiJobScheduleEdi", "edi.ediJob.scheduleEdi" }, { "EdiJobSchedulingDate", "edi.ediJob.schedulingDate" }, { "EdiJobTime", "edi.ediJob.time" }, { "EdiJobIsMonday", "edi.ediJob.isMonday" }, { "EdiJobIsTuesday", "edi.ediJob.isTuesday" }, { "EdiJobIsWednesday", "edi.ediJob.isWednesday" }, { "EdiJobIsThursday", "edi.ediJob.isThursday" }, { "EdiJobIsFriday", "edi.ediJob.isFriday" }, { "EdiJobIsSaturday", "edi.ediJob.isSaturday" }, { "EdiJobIsSunday", "edi.ediJob.isSunday" }, { "EdiJobSendToEmails", "edi.ediJob.sendToEmails" }, { "EdiJobNotifyAllEdi", "edi.ediJob.notifyAllEdi" }, { "EdiJobNotifyInvoiceOnly", "edi.ediJob.notifyInvoiceOnly" }, { "EdiJobNotifyErrorOnly", "edi.ediJob.notifyErrorOnly" }, { "EdiJobSchedulingNotes", "edi.ediJob.schedulingNotes" }, { "IsVendor", "isVendor" }, { "IsDonor", "isDonor" }, { "SanCode", "sanCode" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Organization2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Organization2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Organization2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Organization2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Organization2sRadGrid, "ExportToAccounting", "exportToAccounting"),
                Global.GetCqlFilter(Organization2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Organization2sRadGrid, "Language", "language"),
                Global.GetCqlFilter(Organization2sRadGrid, "AccountingCode", "erpCode"),
                Global.GetCqlFilter(Organization2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Organization2sRadGrid, "AccessProvider", "accessProvider"),
                Global.GetCqlFilter(Organization2sRadGrid, "Governmental", "governmental"),
                Global.GetCqlFilter(Organization2sRadGrid, "Licensor", "licensor"),
                Global.GetCqlFilter(Organization2sRadGrid, "MaterialSupplier", "materialSupplier"),
                Global.GetCqlFilter(Organization2sRadGrid, "ClaimingInterval", "claimingInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "DiscountPercent", "discountPercent"),
                Global.GetCqlFilter(Organization2sRadGrid, "ExpectedActivationInterval", "expectedActivationInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "ExpectedInvoiceInterval", "expectedInvoiceInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "RenewalActivationInterval", "renewalActivationInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "SubscriptionInterval", "subscriptionInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "ExpectedReceiptInterval", "expectedReceiptInterval"),
                Global.GetCqlFilter(Organization2sRadGrid, "TaxId", "taxId"),
                Global.GetCqlFilter(Organization2sRadGrid, "LiableForVat", "liableForVat"),
                Global.GetCqlFilter(Organization2sRadGrid, "TaxPercentage", "taxPercentage"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiVendorEdiCode", "edi.vendorEdiCode"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiVendorEdiType", "edi.vendorEdiType"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiLibEdiCode", "edi.libEdiCode"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiLibEdiType", "edi.libEdiType"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiProrateTax", "edi.prorateTax"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiProrateFees", "edi.prorateFees"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiNamingConvention", "edi.ediNamingConvention"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiSendAcctNum", "edi.sendAcctNum"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiSupportOrder", "edi.supportOrder"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiSupportInvoice", "edi.supportInvoice"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiNotes", "edi.notes"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpFtpFormat", "edi.ediFtp.ftpFormat"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpServerAddress", "edi.ediFtp.serverAddress"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpUsername", "edi.ediFtp.username"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpPassword", "edi.ediFtp.password"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpFtpMode", "edi.ediFtp.ftpMode"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpFtpConnMode", "edi.ediFtp.ftpConnMode"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpFtpPort", "edi.ediFtp.ftpPort"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpOrderDirectory", "edi.ediFtp.orderDirectory"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpInvoiceDirectory", "edi.ediFtp.invoiceDirectory"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiFtpNotes", "edi.ediFtp.notes"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobScheduleEdi", "edi.ediJob.scheduleEdi"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobSchedulingDate", "edi.ediJob.schedulingDate"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobTime", "edi.ediJob.time"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsMonday", "edi.ediJob.isMonday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsTuesday", "edi.ediJob.isTuesday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsWednesday", "edi.ediJob.isWednesday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsThursday", "edi.ediJob.isThursday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsFriday", "edi.ediJob.isFriday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsSaturday", "edi.ediJob.isSaturday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobIsSunday", "edi.ediJob.isSunday"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobSendToEmails", "edi.ediJob.sendToEmails"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobNotifyAllEdi", "edi.ediJob.notifyAllEdi"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobNotifyInvoiceOnly", "edi.ediJob.notifyInvoiceOnly"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobNotifyErrorOnly", "edi.ediJob.notifyErrorOnly"),
                Global.GetCqlFilter(Organization2sRadGrid, "EdiJobSchedulingNotes", "edi.ediJob.schedulingNotes"),
                Global.GetCqlFilter(Organization2sRadGrid, "IsVendor", "isVendor"),
                Global.GetCqlFilter(Organization2sRadGrid, "IsDonor", "isDonor"),
                Global.GetCqlFilter(Organization2sRadGrid, "SanCode", "sanCode"),
                Global.GetCqlFilter(Organization2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Organization2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Organization2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Organization2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var o2 in folioServiceContext.Organization2s(where, Organization2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Organization2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Organization2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{o2.Id}\t{Global.TextEncode(o2.Name)}\t{Global.TextEncode(o2.Code)}\t{Global.TextEncode(o2.Description)}\t{o2.ExportToAccounting}\t{Global.TextEncode(o2.Status)}\t{Global.TextEncode(o2.Language)}\t{Global.TextEncode(o2.AccountingCode)}\t{Global.TextEncode(o2.PaymentMethod)}\t{o2.AccessProvider}\t{o2.Governmental}\t{o2.Licensor}\t{o2.MaterialSupplier}\t{o2.ClaimingInterval}\t{o2.DiscountPercent}\t{o2.ExpectedActivationInterval}\t{o2.ExpectedInvoiceInterval}\t{o2.RenewalActivationInterval}\t{o2.SubscriptionInterval}\t{o2.ExpectedReceiptInterval}\t{Global.TextEncode(o2.TaxId)}\t{o2.LiableForVat}\t{o2.TaxPercentage}\t{Global.TextEncode(o2.EdiVendorEdiCode)}\t{Global.TextEncode(o2.EdiVendorEdiType)}\t{Global.TextEncode(o2.EdiLibEdiCode)}\t{Global.TextEncode(o2.EdiLibEdiType)}\t{o2.EdiProrateTax}\t{o2.EdiProrateFees}\t{Global.TextEncode(o2.EdiNamingConvention)}\t{o2.EdiSendAcctNum}\t{o2.EdiSupportOrder}\t{o2.EdiSupportInvoice}\t{Global.TextEncode(o2.EdiNotes)}\t{Global.TextEncode(o2.EdiFtpFtpFormat)}\t{Global.TextEncode(o2.EdiFtpServerAddress)}\t{Global.TextEncode(o2.EdiFtpUsername)}\t{Global.TextEncode(o2.EdiFtpPassword)}\t{Global.TextEncode(o2.EdiFtpFtpMode)}\t{Global.TextEncode(o2.EdiFtpFtpConnMode)}\t{o2.EdiFtpFtpPort}\t{Global.TextEncode(o2.EdiFtpOrderDirectory)}\t{Global.TextEncode(o2.EdiFtpInvoiceDirectory)}\t{Global.TextEncode(o2.EdiFtpNotes)}\t{o2.EdiJobScheduleEdi}\t{o2.EdiJobSchedulingDate:M/d/yyyy}\t{Global.TextEncode(o2.EdiJobTime)}\t{o2.EdiJobIsMonday}\t{o2.EdiJobIsTuesday}\t{o2.EdiJobIsWednesday}\t{o2.EdiJobIsThursday}\t{o2.EdiJobIsFriday}\t{o2.EdiJobIsSaturday}\t{o2.EdiJobIsSunday}\t{Global.TextEncode(o2.EdiJobSendToEmails)}\t{o2.EdiJobNotifyAllEdi}\t{o2.EdiJobNotifyInvoiceOnly}\t{o2.EdiJobNotifyErrorOnly}\t{Global.TextEncode(o2.EdiJobSchedulingNotes)}\t{o2.IsVendor}\t{o2.IsDonor}\t{Global.TextEncode(o2.SanCode)}\t{o2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(o2.CreationUser?.Username)}\t{o2.CreationUserId}\t{o2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(o2.LastWriteUser?.Username)}\t{o2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
