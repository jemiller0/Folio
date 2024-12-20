using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Title2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Title2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Title2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var t2 = folioServiceContext.FindTitle2(id, true);
            if (t2 == null) Response.Redirect("Default.aspx");
            t2.Content = t2.Content != null ? JsonConvert.DeserializeObject<JToken>(t2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Title2FormView.DataSource = new[] { t2 };
            Title = $"Title {t2.Title}";
        }

        protected void Receiving2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Receiving2sPermission"] == null) return;
            var id = (Guid?)Title2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "DisplaySummary", "displaySummary" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "BindItemId", "bindItemId" }, { "BindItemTenantId", "bindItemTenantId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "ReceivingTenantId", "receivingTenantId" }, { "DisplayOnHolding", "displayOnHolding" }, { "DisplayToPublic", "displayToPublic" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "Barcode", "barcode" }, { "AccessionNumber", "accessionNumber" }, { "CallNumber", "callNumber" }, { "DiscoverySuppress", "discoverySuppress" }, { "CopyNumber", "copyNumber" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "IsBound", "isBound" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" }, { "StatusUpdatedDate", "statusUpdatedDate" }, { "ClaimingInterval", "claimingInterval" }, { "InternalNote", "internalNote" }, { "ExternalNote", "externalNote" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"titleId == \"{id}\"",
                Global.GetCqlFilter(Receiving2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplaySummary", "displaySummary"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Comment", "comment"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Format", "format"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Receiving2sRadGrid, "BindItemId", "bindItemId"),
                Global.GetCqlFilter(Receiving2sRadGrid, "BindItemTenantId", "bindItemTenantId"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Location.Name", "locationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Receiving2sRadGrid, "OrderItem.Number", "poLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(Receiving2sRadGrid, "Holding.ShortId", "holdingId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceivingTenantId", "receivingTenantId"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplayOnHolding", "displayOnHolding"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DisplayToPublic", "displayToPublic"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Enumeration", "enumeration"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Chronology", "chronology"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Receiving2sRadGrid, "AccessionNumber", "accessionNumber"),
                Global.GetCqlFilter(Receiving2sRadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Receiving2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Receiving2sRadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceivingStatus", "receivingStatus"),
                Global.GetCqlFilter(Receiving2sRadGrid, "Supplement", "supplement"),
                Global.GetCqlFilter(Receiving2sRadGrid, "IsBound", "isBound"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceiptTime", "receiptDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ReceiveTime", "receivedDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "StatusUpdatedDate", "statusUpdatedDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ClaimingInterval", "claimingInterval"),
                Global.GetCqlFilter(Receiving2sRadGrid, "InternalNote", "internalNote"),
                Global.GetCqlFilter(Receiving2sRadGrid, "ExternalNote", "externalNote"),
                Global.GetCqlFilter(Receiving2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Receiving2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Receiving2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(where, Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = folioServiceContext.CountReceiving2s(where);
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = Title2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void TitleAcquisitionsUnitsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["TitleAcquisitionsUnitsPermission"] == null) return;
            var id = (Guid?)Title2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindTitle2(id, true).TitleAcquisitionsUnits ?? new TitleAcquisitionsUnit[] { };
            TitleAcquisitionsUnitsRadGrid.DataSource = l;
            TitleAcquisitionsUnitsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            TitleAcquisitionsUnitsPanel.Visible = Title2FormView.DataKey.Value != null && ((string)Session["TitleAcquisitionsUnitsPermission"] == "Edit" || Session["TitleAcquisitionsUnitsPermission"] != null && l.Any());
        }

        protected void TitleBindItemIdsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["TitleBindItemIdsPermission"] == null) return;
            var id = (Guid?)Title2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindTitle2(id, true).TitleBindItemIds ?? new TitleBindItemId[] { };
            TitleBindItemIdsRadGrid.DataSource = l;
            TitleBindItemIdsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            TitleBindItemIdsPanel.Visible = Title2FormView.DataKey.Value != null && ((string)Session["TitleBindItemIdsPermission"] == "Edit" || Session["TitleBindItemIdsPermission"] != null && l.Any());
        }

        protected void TitleContributorsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["TitleContributorsPermission"] == null) return;
            var id = (Guid?)Title2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindTitle2(id, true).TitleContributors ?? new TitleContributor[] { };
            TitleContributorsRadGrid.DataSource = l;
            TitleContributorsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            TitleContributorsPanel.Visible = Title2FormView.DataKey.Value != null && ((string)Session["TitleContributorsPermission"] == "Edit" || Session["TitleContributorsPermission"] != null && l.Any());
        }

        protected void TitleProductIdsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["TitleProductIdsPermission"] == null) return;
            var id = (Guid?)Title2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindTitle2(id, true).TitleProductIds ?? new TitleProductId[] { };
            TitleProductIdsRadGrid.DataSource = l;
            TitleProductIdsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            TitleProductIdsPanel.Visible = Title2FormView.DataKey.Value != null && ((string)Session["TitleProductIdsPermission"] == "Edit" || Session["TitleProductIdsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
