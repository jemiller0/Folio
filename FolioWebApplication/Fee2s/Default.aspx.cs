using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Fee2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Fee2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, Global.GetCqlFilter(Fee2sRadGrid, d), Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Fee2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            Response.Write("Id\tAmount\tRemainingAmount\tStatusName\tPaymentStatusName\tTitle\tCallNumber\tBarcode\tMaterialType\tItemStatusName\tLocation\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tDueTime\tReturnedTime\tLoan\tLoanId\tUser\tUserId\tItem\tItemId\tMaterialType1\tMaterialTypeId\tFeeType\tFeeTypeId\tOwner\tOwnerId\tHolding\tHoldingId\tInstance\tInstanceId\r\n");
            foreach (var f2 in folioServiceContext.Fee2s(Global.GetCqlFilter(Fee2sRadGrid, d), Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{f2.Id}\t{f2.Amount}\t{f2.RemainingAmount}\t{Global.TextEncode(f2.StatusName)}\t{Global.TextEncode(f2.PaymentStatusName)}\t{Global.TextEncode(f2.Title)}\t{Global.TextEncode(f2.CallNumber)}\t{Global.TextEncode(f2.Barcode)}\t{Global.TextEncode(f2.MaterialType)}\t{Global.TextEncode(f2.ItemStatusName)}\t{Global.TextEncode(f2.Location)}\t{f2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(f2.CreationUser?.Username)}\t{f2.CreationUserId}\t{f2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(f2.LastWriteUser?.Username)}\t{f2.LastWriteUserId}\t{f2.DueTime:M/d/yyyy HH:mm:ss}\t{f2.ReturnedTime:M/d/yyyy HH:mm:ss}\t{f2.Loan?.Id}\t{f2.LoanId}\t{Global.TextEncode(f2.User?.Username)}\t{f2.UserId}\t{f2.Item?.ShortId}\t{f2.ItemId}\t{Global.TextEncode(f2.MaterialType1?.Name)}\t{f2.MaterialTypeId}\t{Global.TextEncode(f2.FeeType?.Name)}\t{f2.FeeTypeId}\t{Global.TextEncode(f2.Owner?.Name)}\t{f2.OwnerId}\t{f2.Holding?.ShortId}\t{f2.HoldingId}\t{Global.TextEncode(f2.Instance?.Title)}\t{f2.InstanceId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
