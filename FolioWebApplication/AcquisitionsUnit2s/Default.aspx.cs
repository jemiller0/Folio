using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.AcquisitionsUnit2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AcquisitionsUnit2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void AcquisitionsUnit2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "IsDeleted", "isDeleted" }, { "ProtectCreate", "protectCreate" }, { "ProtectRead", "protectRead" }, { "ProtectUpdate", "protectUpdate" }, { "ProtectDelete", "protectDelete" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "IsDeleted", "isDeleted"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "ProtectCreate", "protectCreate"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "ProtectRead", "protectRead"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "ProtectUpdate", "protectUpdate"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "ProtectDelete", "protectDelete"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            AcquisitionsUnit2sRadGrid.DataSource = folioServiceContext.AcquisitionsUnit2s(out var i, where, AcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, AcquisitionsUnit2sRadGrid.PageSize * AcquisitionsUnit2sRadGrid.CurrentPageIndex, AcquisitionsUnit2sRadGrid.PageSize, true);
            AcquisitionsUnit2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"AcquisitionsUnit2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "IsDeleted", "isDeleted" }, { "ProtectCreate", "protectCreate" }, { "ProtectRead", "protectRead" }, { "ProtectUpdate", "protectUpdate" }, { "ProtectDelete", "protectDelete" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tIsDeleted\tProtectCreate\tProtectRead\tProtectUpdate\tProtectDelete\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var au2 in folioServiceContext.AcquisitionsUnit2s(Global.GetCqlFilter(AcquisitionsUnit2sRadGrid, d), AcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{au2.Id}\t{Global.TextEncode(au2.Name)}\t{au2.IsDeleted}\t{au2.ProtectCreate}\t{au2.ProtectRead}\t{au2.ProtectUpdate}\t{au2.ProtectDelete}\t{au2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(au2.CreationUser?.Username)}\t{au2.CreationUserId}\t{au2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(au2.LastWriteUser?.Username)}\t{au2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
