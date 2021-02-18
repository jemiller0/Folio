using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.Owner2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Owner2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Owner2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var o2 = folioServiceContext.FindOwner2(id, true);
            if (o2 == null) Response.Redirect("Default.aspx");
            Owner2FormView.DataSource = new[] { o2 };
            Title = $"Owner {o2.Name}";
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, Global.GetCqlFilter(Fee2sRadGrid, d, $"ownerId == \"{id}\""), Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Owner2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void FeeType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FeeType2sPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Automatic", "automatic" }, { "Name", "feeFineType" }, { "DefaultAmount", "defaultAmount" }, { "ChargeNoticeId", "chargeNoticeId" }, { "ActionNoticeId", "actionNoticeId" }, { "OwnerId", "ownerId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            FeeType2sRadGrid.DataSource = folioServiceContext.FeeType2s(out var i, Global.GetCqlFilter(FeeType2sRadGrid, d, $"ownerId == \"{id}\""), FeeType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FeeType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FeeType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FeeType2sRadGrid.PageSize * FeeType2sRadGrid.CurrentPageIndex, FeeType2sRadGrid.PageSize, true);
            FeeType2sRadGrid.VirtualItemCount = i;
            if (FeeType2sRadGrid.MasterTableView.FilterExpression == "")
            {
                FeeType2sRadGrid.AllowFilteringByColumn = FeeType2sRadGrid.VirtualItemCount > 10;
                FeeType2sPanel.Visible = Owner2FormView.DataKey.Value != null && Session["FeeType2sPermission"] != null && FeeType2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void PaymentMethod2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PaymentMethod2sPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameMethod" }, { "AllowedRefundMethod", "allowedRefundMethod" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "OwnerId", "ownerId" } };
            PaymentMethod2sRadGrid.DataSource = folioServiceContext.PaymentMethod2s(out var i, Global.GetCqlFilter(PaymentMethod2sRadGrid, d, $"ownerId == \"{id}\""), PaymentMethod2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PaymentMethod2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PaymentMethod2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PaymentMethod2sRadGrid.PageSize * PaymentMethod2sRadGrid.CurrentPageIndex, PaymentMethod2sRadGrid.PageSize, true);
            PaymentMethod2sRadGrid.VirtualItemCount = i;
            if (PaymentMethod2sRadGrid.MasterTableView.FilterExpression == "")
            {
                PaymentMethod2sRadGrid.AllowFilteringByColumn = PaymentMethod2sRadGrid.VirtualItemCount > 10;
                PaymentMethod2sPanel.Visible = Owner2FormView.DataKey.Value != null && Session["PaymentMethod2sPermission"] != null && PaymentMethod2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void TransferAccount2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["TransferAccount2sPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "accountName" }, { "Desc", "desc" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "OwnerId", "ownerId" } };
            TransferAccount2sRadGrid.DataSource = folioServiceContext.TransferAccount2s(out var i, Global.GetCqlFilter(TransferAccount2sRadGrid, d, $"ownerId == \"{id}\""), TransferAccount2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[TransferAccount2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(TransferAccount2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, TransferAccount2sRadGrid.PageSize * TransferAccount2sRadGrid.CurrentPageIndex, TransferAccount2sRadGrid.PageSize, true);
            TransferAccount2sRadGrid.VirtualItemCount = i;
            if (TransferAccount2sRadGrid.MasterTableView.FilterExpression == "")
            {
                TransferAccount2sRadGrid.AllowFilteringByColumn = TransferAccount2sRadGrid.VirtualItemCount > 10;
                TransferAccount2sPanel.Visible = Owner2FormView.DataKey.Value != null && Session["TransferAccount2sPermission"] != null && TransferAccount2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
