using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.IssuanceModes
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IssuanceModesPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void IssuanceModesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            IssuanceModesRadGrid.DataSource = folioServiceContext.IssuanceModes(out var i, Global.GetCqlFilter(IssuanceModesRadGrid, d), IssuanceModesRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[IssuanceModesRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(IssuanceModesRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, IssuanceModesRadGrid.PageSize * IssuanceModesRadGrid.CurrentPageIndex, IssuanceModesRadGrid.PageSize, true);
            IssuanceModesRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"IssuanceModes.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var im in folioServiceContext.IssuanceModes(Global.GetCqlFilter(IssuanceModesRadGrid, d), IssuanceModesRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[IssuanceModesRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(IssuanceModesRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{im.Id}\t{Global.TextEncode(im.Name)}\t{Global.TextEncode(im.Source)}\t{im.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(im.CreationUser?.Username)}\t{im.CreationUserId}\t{im.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(im.LastWriteUser?.Username)}\t{im.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
