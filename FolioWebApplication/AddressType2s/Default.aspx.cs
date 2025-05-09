using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.AddressType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AddressType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void AddressType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "addressType" }, { "Description", "desc" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AddressType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(AddressType2sRadGrid, "Name", "addressType"),
                Global.GetCqlFilter(AddressType2sRadGrid, "Description", "desc"),
                Global.GetCqlFilter(AddressType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(AddressType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(AddressType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(AddressType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            AddressType2sRadGrid.DataSource = folioServiceContext.AddressType2s(where, AddressType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AddressType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AddressType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, AddressType2sRadGrid.PageSize * AddressType2sRadGrid.CurrentPageIndex, AddressType2sRadGrid.PageSize, true);
            AddressType2sRadGrid.VirtualItemCount = folioServiceContext.CountAddressType2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"AddressType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tDescription\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "addressType" }, { "Description", "desc" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(AddressType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(AddressType2sRadGrid, "Name", "addressType"),
                Global.GetCqlFilter(AddressType2sRadGrid, "Description", "desc"),
                Global.GetCqlFilter(AddressType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(AddressType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(AddressType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(AddressType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var at2 in folioServiceContext.AddressType2s(where, AddressType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[AddressType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(AddressType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{at2.Id}\t{Global.TextEncode(at2.Name)}\t{Global.TextEncode(at2.Description)}\t{at2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(at2.CreationUser?.Username)}\t{at2.CreationUserId}\t{at2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(at2.LastWriteUser?.Username)}\t{at2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
