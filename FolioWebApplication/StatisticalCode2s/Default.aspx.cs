using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.StatisticalCode2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StatisticalCode2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void StatisticalCode2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Name", "name" }, { "StatisticalCodeTypeId", "statisticalCodeTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            StatisticalCode2sRadGrid.DataSource = folioServiceContext.StatisticalCode2s(out var i, Global.GetCqlFilter(StatisticalCode2sRadGrid, d), StatisticalCode2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StatisticalCode2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StatisticalCode2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, StatisticalCode2sRadGrid.PageSize * StatisticalCode2sRadGrid.CurrentPageIndex, StatisticalCode2sRadGrid.PageSize, true);
            StatisticalCode2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"StatisticalCode2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Name", "name" }, { "StatisticalCodeTypeId", "statisticalCodeTypeId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tCode\tName\tStatisticalCodeType\tStatisticalCodeTypeId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var sc2 in folioServiceContext.StatisticalCode2s(Global.GetCqlFilter(StatisticalCode2sRadGrid, d), StatisticalCode2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[StatisticalCode2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(StatisticalCode2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{sc2.Id}\t{Global.TextEncode(sc2.Code)}\t{Global.TextEncode(sc2.Name)}\t{Global.TextEncode(sc2.StatisticalCodeType?.Name)}\t{sc2.StatisticalCodeTypeId}\t{sc2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sc2.CreationUser?.Username)}\t{sc2.CreationUserId}\t{sc2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sc2.LastWriteUser?.Username)}\t{sc2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
