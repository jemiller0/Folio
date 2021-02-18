using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.FiscalYear2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FiscalYear2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void FiscalYear2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Currency", "currency" }, { "Description", "description" }, { "StartDate", "periodStart" }, { "EndDate", "periodEnd" }, { "Series", "series" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            FiscalYear2sRadGrid.DataSource = folioServiceContext.FiscalYear2s(out var i, Global.GetCqlFilter(FiscalYear2sRadGrid, d), FiscalYear2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FiscalYear2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FiscalYear2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, FiscalYear2sRadGrid.PageSize * FiscalYear2sRadGrid.CurrentPageIndex, FiscalYear2sRadGrid.PageSize, true);
            FiscalYear2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"FiscalYear2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Currency", "currency" }, { "Description", "description" }, { "StartDate", "periodStart" }, { "EndDate", "periodEnd" }, { "Series", "series" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tCurrency\tDescription\tStartDate\tEndDate\tSeries\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var fy2 in folioServiceContext.FiscalYear2s(Global.GetCqlFilter(FiscalYear2sRadGrid, d), FiscalYear2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[FiscalYear2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(FiscalYear2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{fy2.Id}\t{Global.TextEncode(fy2.Name)}\t{Global.TextEncode(fy2.Code)}\t{Global.TextEncode(fy2.Currency)}\t{Global.TextEncode(fy2.Description)}\t{fy2.StartDate:M/d/yyyy}\t{fy2.EndDate:M/d/yyyy}\t{Global.TextEncode(fy2.Series)}\t{fy2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(fy2.CreationUser?.Username)}\t{fy2.CreationUserId}\t{fy2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(fy2.LastWriteUser?.Username)}\t{fy2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
