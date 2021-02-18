using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ServicePointUser2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ServicePointUser2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ServicePointUser2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "DefaultServicePointId", "defaultServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ServicePointUser2sRadGrid.DataSource = folioServiceContext.ServicePointUser2s(out var i, Global.GetCqlFilter(ServicePointUser2sRadGrid, d), ServicePointUser2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ServicePointUser2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ServicePointUser2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ServicePointUser2sRadGrid.PageSize * ServicePointUser2sRadGrid.CurrentPageIndex, ServicePointUser2sRadGrid.PageSize, true);
            ServicePointUser2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ServicePointUser2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "DefaultServicePointId", "defaultServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tUser\tUserId\tDefaultServicePoint\tDefaultServicePointId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var spu2 in folioServiceContext.ServicePointUser2s(Global.GetCqlFilter(ServicePointUser2sRadGrid, d), ServicePointUser2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ServicePointUser2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ServicePointUser2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{spu2.Id}\t{Global.TextEncode(spu2.User?.Username)}\t{spu2.UserId}\t{Global.TextEncode(spu2.DefaultServicePoint?.Name)}\t{spu2.DefaultServicePointId}\t{spu2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(spu2.CreationUser?.Username)}\t{spu2.CreationUserId}\t{spu2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(spu2.LastWriteUser?.Username)}\t{spu2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
