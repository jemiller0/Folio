using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.StatisticalCodeType2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            StatisticalCodeType2FormView.DataSource = new[] { sct2 };
            Title = $"Statistical Code Type {sct2.Name}";
        }

        protected void StatisticalCode2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["StatisticalCode2sPermission"] == null) return;
            var id = (Guid?)StatisticalCodeType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Name", "name" }, { "StatisticalCodeTypeId", "statisticalCodeTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            StatisticalCode2sRadGrid.DataSource = folioServiceContext.StatisticalCode2s(out var i, Global.GetCqlFilter(StatisticalCode2sRadGrid, d, $"statisticalCodeTypeId == \"{id}\""), StatisticalCode2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StatisticalCode2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StatisticalCode2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, StatisticalCode2sRadGrid.PageSize * StatisticalCode2sRadGrid.CurrentPageIndex, StatisticalCode2sRadGrid.PageSize, true);
            StatisticalCode2sRadGrid.VirtualItemCount = i;
            if (StatisticalCode2sRadGrid.MasterTableView.FilterExpression == "")
            {
                StatisticalCode2sRadGrid.AllowFilteringByColumn = StatisticalCode2sRadGrid.VirtualItemCount > 10;
                StatisticalCode2sPanel.Visible = StatisticalCodeType2FormView.DataKey.Value != null && Session["StatisticalCode2sPermission"] != null && StatisticalCode2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
