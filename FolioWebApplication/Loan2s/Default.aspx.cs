using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Loan2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Loan2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Loan2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" } };
            Loan2sRadGrid.DataSource = folioServiceContext.Loan2s(out var i, Global.GetCqlFilter(Loan2sRadGrid, d), Loan2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2sRadGrid.PageSize * Loan2sRadGrid.CurrentPageIndex, Loan2sRadGrid.PageSize, true);
            Loan2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Loan2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" } };
            Response.Write("Id\tUser\tUserId\tProxyUser\tProxyUserId\tItem\tItemId\tItemEffectiveLocationAtCheckOut\tItemEffectiveLocationAtCheckOutId\tStatusName\tLoanTime\tDueTime\tReturnTime\tSystemReturnTime\tAction\tActionComment\tItemStatus\tRenewalCount\tLoanPolicy\tLoanPolicyId\tCheckoutServicePoint\tCheckoutServicePointId\tCheckinServicePoint\tCheckinServicePointId\tGroup\tGroupId\tDueDateChangedByRecall\tDeclaredLostDate\tClaimedReturnedDate\tOverdueFinePolicy\tOverdueFinePolicyId\tLostItemPolicy\tLostItemPolicyId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tAgedToLostDelayedBillingLostItemHasBeenBilled\tAgedToLostDelayedBillingDateLostItemShouldBeBilled\tAgedToLostDelayedBillingAgedToLostDate\r\n");
            foreach (var l2 in folioServiceContext.Loan2s(Global.GetCqlFilter(Loan2sRadGrid, d), Loan2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{l2.Id}\t{Global.TextEncode(l2.User?.Username)}\t{l2.UserId}\t{Global.TextEncode(l2.ProxyUser?.Username)}\t{l2.ProxyUserId}\t{l2.Item?.ShortId}\t{l2.ItemId}\t{Global.TextEncode(l2.ItemEffectiveLocationAtCheckOut?.Name)}\t{l2.ItemEffectiveLocationAtCheckOutId}\t{Global.TextEncode(l2.StatusName)}\t{l2.LoanTime:M/d/yyyy HH:mm:ss}\t{l2.DueTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.ReturnTime)}\t{l2.SystemReturnTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.Action)}\t{Global.TextEncode(l2.ActionComment)}\t{Global.TextEncode(l2.ItemStatus)}\t{l2.RenewalCount}\t{Global.TextEncode(l2.LoanPolicy?.Name)}\t{l2.LoanPolicyId}\t{Global.TextEncode(l2.CheckoutServicePoint?.Name)}\t{l2.CheckoutServicePointId}\t{Global.TextEncode(l2.CheckinServicePoint?.Name)}\t{l2.CheckinServicePointId}\t{Global.TextEncode(l2.Group?.Name)}\t{l2.GroupId}\t{l2.DueDateChangedByRecall}\t{l2.DeclaredLostDate:M/d/yyyy}\t{l2.ClaimedReturnedDate:M/d/yyyy}\t{Global.TextEncode(l2.OverdueFinePolicy?.Name)}\t{l2.OverdueFinePolicyId}\t{Global.TextEncode(l2.LostItemPolicy?.Name)}\t{l2.LostItemPolicyId}\t{l2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.CreationUser?.Username)}\t{l2.CreationUserId}\t{l2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.LastWriteUser?.Username)}\t{l2.LastWriteUserId}\t{l2.AgedToLostDelayedBillingLostItemHasBeenBilled}\t{l2.AgedToLostDelayedBillingDateLostItemShouldBeBilled:M/d/yyyy}\t{l2.AgedToLostDelayedBillingAgedToLostDate:M/d/yyyy}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
