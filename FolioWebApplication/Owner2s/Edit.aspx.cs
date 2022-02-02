using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Owner2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            o2.Content = o2.Content != null ? JsonConvert.DeserializeObject<JToken>(o2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Owner2FormView.DataSource = new[] { o2 };
            Title = $"Owner {o2.Name}";
        }

        protected void Fee2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ownerId == \"{id}\"",
                Global.GetCqlFilter(Fee2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Fee2sRadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Fee2sRadGrid, "RemainingAmount", "remaining"),
                Global.GetCqlFilter(Fee2sRadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "PaymentStatusName", "paymentStatus.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "Title", "title"),
                Global.GetCqlFilter(Fee2sRadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Fee2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Fee2sRadGrid, "MaterialType", "materialType"),
                Global.GetCqlFilter(Fee2sRadGrid, "ItemStatusName", "itemStatus.name"),
                Global.GetCqlFilter(Fee2sRadGrid, "Location", "location"),
                Global.GetCqlFilter(Fee2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "DueTime", "dueDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "ReturnedTime", "returnedDate"),
                Global.GetCqlFilter(Fee2sRadGrid, "Loan.Id", "loanId", "id", folioServiceContext.FolioServiceClient.Loans),
                Global.GetCqlFilter(Fee2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Fee2sRadGrid, "MaterialType1.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(Fee2sRadGrid, "FeeType.Name", "feeFineId", "feeFineType", folioServiceContext.FolioServiceClient.FeeTypes),
                Global.GetCqlFilter(Fee2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Fee2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances)
            }.Where(s => s != null)));
            Fee2sRadGrid.DataSource = folioServiceContext.Fee2s(out var i, where, Fee2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2sRadGrid.PageSize * Fee2sRadGrid.CurrentPageIndex, Fee2sRadGrid.PageSize, true);
            Fee2sRadGrid.VirtualItemCount = i;
            if (Fee2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2sRadGrid.AllowFilteringByColumn = Fee2sRadGrid.VirtualItemCount > 10;
                Fee2sPanel.Visible = Owner2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void FeeType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["FeeType2sPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Automatic", "automatic" }, { "Name", "feeFineType" }, { "DefaultAmount", "defaultAmount" }, { "ChargeNoticeId", "chargeNoticeId" }, { "ActionNoticeId", "actionNoticeId" }, { "OwnerId", "ownerId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ownerId == \"{id}\"",
                Global.GetCqlFilter(FeeType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(FeeType2sRadGrid, "Automatic", "automatic"),
                Global.GetCqlFilter(FeeType2sRadGrid, "Name", "feeFineType"),
                Global.GetCqlFilter(FeeType2sRadGrid, "DefaultAmount", "defaultAmount"),
                Global.GetCqlFilter(FeeType2sRadGrid, "ChargeNotice.Name", "chargeNoticeId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(FeeType2sRadGrid, "ActionNotice.Name", "actionNoticeId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(FeeType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(FeeType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(FeeType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(FeeType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            FeeType2sRadGrid.DataSource = folioServiceContext.FeeType2s(out var i, where, FeeType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FeeType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FeeType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FeeType2sRadGrid.PageSize * FeeType2sRadGrid.CurrentPageIndex, FeeType2sRadGrid.PageSize, true);
            FeeType2sRadGrid.VirtualItemCount = i;
            if (FeeType2sRadGrid.MasterTableView.FilterExpression == "")
            {
                FeeType2sRadGrid.AllowFilteringByColumn = FeeType2sRadGrid.VirtualItemCount > 10;
                FeeType2sPanel.Visible = Owner2FormView.DataKey.Value != null && Session["FeeType2sPermission"] != null && FeeType2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void PaymentMethod2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PaymentMethod2sPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "nameMethod" }, { "AllowedRefundMethod", "allowedRefundMethod" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "OwnerId", "ownerId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ownerId == \"{id}\"",
                Global.GetCqlFilter(PaymentMethod2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(PaymentMethod2sRadGrid, "Name", "nameMethod"),
                Global.GetCqlFilter(PaymentMethod2sRadGrid, "AllowedRefundMethod", "allowedRefundMethod"),
                Global.GetCqlFilter(PaymentMethod2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(PaymentMethod2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PaymentMethod2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(PaymentMethod2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            PaymentMethod2sRadGrid.DataSource = folioServiceContext.PaymentMethod2s(out var i, where, PaymentMethod2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PaymentMethod2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PaymentMethod2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PaymentMethod2sRadGrid.PageSize * PaymentMethod2sRadGrid.CurrentPageIndex, PaymentMethod2sRadGrid.PageSize, true);
            PaymentMethod2sRadGrid.VirtualItemCount = i;
            if (PaymentMethod2sRadGrid.MasterTableView.FilterExpression == "")
            {
                PaymentMethod2sRadGrid.AllowFilteringByColumn = PaymentMethod2sRadGrid.VirtualItemCount > 10;
                PaymentMethod2sPanel.Visible = Owner2FormView.DataKey.Value != null && Session["PaymentMethod2sPermission"] != null && PaymentMethod2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ServicePointOwnersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ServicePointOwnersPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindOwner2(id, true).ServicePointOwners ?? new ServicePointOwner[] { };
            ServicePointOwnersRadGrid.DataSource = l;
            ServicePointOwnersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ServicePointOwnersPanel.Visible = Owner2FormView.DataKey.Value != null && ((string)Session["ServicePointOwnersPermission"] == "Edit" || Session["ServicePointOwnersPermission"] != null && l.Any());
        }

        protected void TransferAccount2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["TransferAccount2sPermission"] == null) return;
            var id = (Guid?)Owner2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "accountName" }, { "Description", "desc" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "OwnerId", "ownerId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"ownerId == \"{id}\"",
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Name", "accountName"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "Description", "desc"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(TransferAccount2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            TransferAccount2sRadGrid.DataSource = folioServiceContext.TransferAccount2s(out var i, where, TransferAccount2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[TransferAccount2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(TransferAccount2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, TransferAccount2sRadGrid.PageSize * TransferAccount2sRadGrid.CurrentPageIndex, TransferAccount2sRadGrid.PageSize, true);
            TransferAccount2sRadGrid.VirtualItemCount = i;
            if (TransferAccount2sRadGrid.MasterTableView.FilterExpression == "")
            {
                TransferAccount2sRadGrid.AllowFilteringByColumn = TransferAccount2sRadGrid.VirtualItemCount > 10;
                TransferAccount2sPanel.Visible = Owner2FormView.DataKey.Value != null && Session["TransferAccount2sPermission"] != null && TransferAccount2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
