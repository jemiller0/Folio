using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Telerik.Web.UI;

namespace FolioWebApplication.FundType2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FundType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FundType2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ft2 = folioServiceContext.FindFundType2(id, true);
            if (ft2 == null) Response.Redirect("Default.aspx");
            FundType2FormView.DataSource = new[] { ft2 };
            Title = $"Fund Type {ft2.Name}";
        }

        protected void Fund2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fund2sPermission"] == null) return;
            var id = (Guid?)FundType2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Description", "description" }, { "AccountNumber", "externalAccountNo" }, { "FundStatus", "fundStatus" }, { "FundTypeId", "fundTypeId" }, { "LedgerId", "ledgerId" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Fund2sRadGrid.DataSource = folioServiceContext.Fund2s(out var i, Global.GetCqlFilter(Fund2sRadGrid, d, $"fundTypeId == \"{id}\""), Fund2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fund2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fund2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fund2sRadGrid.PageSize * Fund2sRadGrid.CurrentPageIndex, Fund2sRadGrid.PageSize, true);
            Fund2sRadGrid.VirtualItemCount = i;
            if (Fund2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Fund2sRadGrid.AllowFilteringByColumn = Fund2sRadGrid.VirtualItemCount > 10;
                Fund2sPanel.Visible = FundType2FormView.DataKey.Value != null && Session["Fund2sPermission"] != null && Fund2sRadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
