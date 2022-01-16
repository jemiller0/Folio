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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Caption", "caption" }, { "Comment", "comment" }, { "Format", "format" }, { "ItemId", "itemId" }, { "LocationId", "locationId" }, { "OrderItemId", "poLineId" }, { "TitleId", "titleId" }, { "HoldingId", "holdingId" }, { "DisplayOnHolding", "displayOnHolding" }, { "Enumeration", "enumeration" }, { "Chronology", "chronology" }, { "DiscoverySuppress", "discoverySuppress" }, { "ReceivingStatus", "receivingStatus" }, { "Supplement", "supplement" }, { "ReceiptTime", "receiptDate" }, { "ReceiveTime", "receivedDate" } };
            Receiving2sRadGrid.DataSource = folioServiceContext.Receiving2s(out var i, Global.GetCqlFilter(Receiving2sRadGrid, d, $"titleId == \"{id}\""), Receiving2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Receiving2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Receiving2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Receiving2sRadGrid.PageSize * Receiving2sRadGrid.CurrentPageIndex, Receiving2sRadGrid.PageSize, true);
            Receiving2sRadGrid.VirtualItemCount = i;
            if (Receiving2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Receiving2sRadGrid.AllowFilteringByColumn = Receiving2sRadGrid.VirtualItemCount > 10;
                Receiving2sPanel.Visible = Title2FormView.DataKey.Value != null && Session["Receiving2sPermission"] != null && Receiving2sRadGrid.VirtualItemCount > 0;
            }
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
