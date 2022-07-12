using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.IllPolicy2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IllPolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void IllPolicy2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ip2 = folioServiceContext.FindIllPolicy2(id, true);
            if (ip2 == null) Response.Redirect("Default.aspx");
            ip2.Content = ip2.Content != null ? JsonConvert.DeserializeObject<JToken>(ip2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            IllPolicy2FormView.DataSource = new[] { ip2 };
            Title = $"ILL Policy {ip2.Name}";
        }

        protected void Holding2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Holding2sPermission"] == null) return;
            var id = (Guid?)IllPolicy2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "EffectiveLocationId", "effectiveLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "SourceId", "sourceId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"illPolicyId == \"{id}\"",
                Global.GetCqlFilter(Holding2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Holding2sRadGrid, "ShortId", "hrid"),
                Global.GetCqlFilter(Holding2sRadGrid, "HoldingType.Name", "holdingsTypeId", "name", folioServiceContext.FolioServiceClient.HoldingTypes),
                Global.GetCqlFilter(Holding2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Holding2sRadGrid, "Location.Name", "permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2sRadGrid, "TemporaryLocation.Name", "temporaryLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2sRadGrid, "EffectiveLocation.Name", "effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberType.Name", "callNumberTypeId", "name", folioServiceContext.FolioServiceClient.CallNumberTypes),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberPrefix", "callNumberPrefix"),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Holding2sRadGrid, "CallNumberSuffix", "callNumberSuffix"),
                Global.GetCqlFilter(Holding2sRadGrid, "ShelvingTitle", "shelvingTitle"),
                Global.GetCqlFilter(Holding2sRadGrid, "AcquisitionFormat", "acquisitionFormat"),
                Global.GetCqlFilter(Holding2sRadGrid, "AcquisitionMethod", "acquisitionMethod"),
                Global.GetCqlFilter(Holding2sRadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(Holding2sRadGrid, "RetentionPolicy", "retentionPolicy"),
                Global.GetCqlFilter(Holding2sRadGrid, "DigitizationPolicy", "digitizationPolicy"),
                Global.GetCqlFilter(Holding2sRadGrid, "CopyNumber", "copyNumber"),
                Global.GetCqlFilter(Holding2sRadGrid, "ItemCount", "numberOfItems"),
                Global.GetCqlFilter(Holding2sRadGrid, "ReceivingHistoryDisplayType", "receivingHistory.displayType"),
                Global.GetCqlFilter(Holding2sRadGrid, "DiscoverySuppress", "discoverySuppress"),
                Global.GetCqlFilter(Holding2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Holding2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Holding2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Holding2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Holding2sRadGrid, "Source.Name", "sourceId", "name", folioServiceContext.FolioServiceClient.Sources)
            }.Where(s => s != null)));
            Holding2sRadGrid.DataSource = folioServiceContext.Holding2s(out var i, where, Holding2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2sRadGrid.PageSize * Holding2sRadGrid.CurrentPageIndex, Holding2sRadGrid.PageSize, true);
            Holding2sRadGrid.VirtualItemCount = i;
            if (Holding2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Holding2sRadGrid.AllowFilteringByColumn = Holding2sRadGrid.VirtualItemCount > 10;
                Holding2sPanel.Visible = IllPolicy2FormView.DataKey.Value != null && Session["Holding2sPermission"] != null && Holding2sRadGrid.VirtualItemCount > 0;
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
