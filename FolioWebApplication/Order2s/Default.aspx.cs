using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Order2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Order2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Order2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "NumberPrefix", "poNumberPrefix" }, { "NumberSuffix", "poNumberSuffix" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "WorkflowStatus", "workflowStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Order2sRadGrid.DataSource = folioServiceContext.Order2s(out var i, Global.GetCqlFilter(Order2sRadGrid, d), Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2sRadGrid.PageSize * Order2sRadGrid.CurrentPageIndex, Order2sRadGrid.PageSize, true);
            Order2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Order2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "NumberPrefix", "poNumberPrefix" }, { "NumberSuffix", "poNumberSuffix" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "WorkflowStatus", "workflowStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tApproved\tApprovedBy\tApprovedById\tApprovalDate\tAssignedTo\tAssignedToId\tBillTo\tBillToId\tCloseReasonReason\tCloseReasonNote\tOrderDate\tManual\tNumber\tNumberPrefix\tNumberSuffix\tOrderType\tReencumber\tOngoingInterval\tOngoingIsSubscription\tOngoingManualRenewal\tOngoingNotes\tOngoingReviewPeriod\tOngoingRenewalDate\tOngoingReviewDate\tShipTo\tShipToId\tTemplate\tTemplateId\tVendor\tVendorId\tWorkflowStatus\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tOrderTransactionSummary2\r\n");
            foreach (var o2 in folioServiceContext.Order2s(Global.GetCqlFilter(Order2sRadGrid, d), Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{o2.Id}\t{o2.Approved}\t{Global.TextEncode(o2.ApprovedBy?.Username)}\t{o2.ApprovedById}\t{o2.ApprovalDate:M/d/yyyy}\t{Global.TextEncode(o2.AssignedTo?.Username)}\t{o2.AssignedToId}\t{Global.TextEncode(o2.BillTo?.Name)}\t{o2.BillToId}\t{Global.TextEncode(o2.CloseReasonReason)}\t{Global.TextEncode(o2.CloseReasonNote)}\t{o2.OrderDate:M/d/yyyy}\t{o2.Manual}\t{Global.TextEncode(o2.Number)}\t{Global.TextEncode(o2.NumberPrefix)}\t{Global.TextEncode(o2.NumberSuffix)}\t{Global.TextEncode(o2.OrderType)}\t{o2.Reencumber}\t{o2.OngoingInterval}\t{o2.OngoingIsSubscription}\t{o2.OngoingManualRenewal}\t{Global.TextEncode(o2.OngoingNotes)}\t{o2.OngoingReviewPeriod}\t{o2.OngoingRenewalDate:M/d/yyyy}\t{o2.OngoingReviewDate:M/d/yyyy}\t{Global.TextEncode(o2.ShipTo?.Name)}\t{o2.ShipToId}\t{o2.Template?.Id}\t{o2.TemplateId}\t{Global.TextEncode(o2.Vendor?.Name)}\t{o2.VendorId}\t{Global.TextEncode(o2.WorkflowStatus)}\t{o2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(o2.CreationUser?.Username)}\t{o2.CreationUserId}\t{o2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(o2.LastWriteUser?.Username)}\t{o2.LastWriteUserId}\t{o2.OrderTransactionSummary2?.Id}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
