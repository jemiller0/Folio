using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.InstanceType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["InstanceType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void InstanceType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            InstanceType2sRadGrid.DataSource = folioServiceContext.InstanceType2s(out var i, Global.GetCqlFilter(InstanceType2sRadGrid, d), InstanceType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InstanceType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InstanceType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, InstanceType2sRadGrid.PageSize * InstanceType2sRadGrid.CurrentPageIndex, InstanceType2sRadGrid.PageSize, true);
            InstanceType2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"InstanceType2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var it2 in folioServiceContext.InstanceType2s(Global.GetCqlFilter(InstanceType2sRadGrid, d), InstanceType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[InstanceType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(InstanceType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{it2.Id}\t{Global.TextEncode(it2.Name)}\t{Global.TextEncode(it2.Code)}\t{it2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(it2.CreationUser?.Username)}\t{it2.CreationUserId}\t{it2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(it2.LastWriteUser?.Username)}\t{it2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
