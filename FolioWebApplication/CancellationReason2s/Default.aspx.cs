using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.CancellationReason2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CancellationReason2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void CancellationReason2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "PublicDescription", "publicDescription" }, { "RequiresAdditionalInformation", "requiresAdditionalInformation" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            CancellationReason2sRadGrid.DataSource = folioServiceContext.CancellationReason2s(out var i, Global.GetCqlFilter(CancellationReason2sRadGrid, d), CancellationReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CancellationReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CancellationReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CancellationReason2sRadGrid.PageSize * CancellationReason2sRadGrid.CurrentPageIndex, CancellationReason2sRadGrid.PageSize, true);
            CancellationReason2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"CancellationReason2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "PublicDescription", "publicDescription" }, { "RequiresAdditionalInformation", "requiresAdditionalInformation" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tPublicDescription\tRequiresAdditionalInformation\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var cr2 in folioServiceContext.CancellationReason2s(Global.GetCqlFilter(CancellationReason2sRadGrid, d), CancellationReason2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CancellationReason2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CancellationReason2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{cr2.Id}\t{Global.TextEncode(cr2.Name)}\t{Global.TextEncode(cr2.Description)}\t{Global.TextEncode(cr2.PublicDescription)}\t{cr2.RequiresAdditionalInformation}\t{cr2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(cr2.CreationUser?.Username)}\t{cr2.CreationUserId}\t{cr2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(cr2.LastWriteUser?.Username)}\t{cr2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
