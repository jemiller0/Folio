using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.StatisticalCodeType2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StatisticalCodeType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void StatisticalCodeType2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var sct2 = folioServiceContext.FindStatisticalCodeType2(id, true);
            if (sct2 == null) Response.Redirect("Default.aspx");
            sct2.Content = sct2.Content != null ? JsonConvert.DeserializeObject<JToken>(sct2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            StatisticalCodeType2FormView.DataSource = new[] { sct2 };
            Title = $"Statistical Code Type {sct2.Name}";
        }

        protected void StatisticalCode2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["StatisticalCode2sPermission"] == null) return;
            var id = (Guid?)StatisticalCodeType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Name", "name" }, { "StatisticalCodeTypeId", "statisticalCodeTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"statisticalCodeTypeId == \"{id}\"",
                Global.GetCqlFilter(StatisticalCode2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(StatisticalCode2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(StatisticalCode2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(StatisticalCode2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(StatisticalCode2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(StatisticalCode2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(StatisticalCode2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            StatisticalCode2sRadGrid.DataSource = folioServiceContext.StatisticalCode2s(where, StatisticalCode2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StatisticalCode2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StatisticalCode2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, StatisticalCode2sRadGrid.PageSize * StatisticalCode2sRadGrid.CurrentPageIndex, StatisticalCode2sRadGrid.PageSize, true);
            StatisticalCode2sRadGrid.VirtualItemCount = folioServiceContext.CountStatisticalCode2s(where);
            if (StatisticalCode2sRadGrid.MasterTableView.FilterExpression == "")
            {
                StatisticalCode2sRadGrid.AllowFilteringByColumn = StatisticalCode2sRadGrid.VirtualItemCount > 10;
                StatisticalCode2sPanel.Visible = StatisticalCodeType2FormView.DataKey.Value != null && Session["StatisticalCode2sPermission"] != null && StatisticalCode2sRadGrid.VirtualItemCount > 0;
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
