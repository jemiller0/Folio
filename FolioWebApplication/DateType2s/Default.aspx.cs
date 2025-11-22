using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.DateType2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["DateType2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void DateType2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "DisplayFormatDelimiter", "displayFormat.delimiter" }, { "DisplayFormatKeepDelimiter", "displayFormat.keepDelimiter" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(DateType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(DateType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(DateType2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(DateType2sRadGrid, "DisplayFormatDelimiter", "displayFormat.delimiter"),
                Global.GetCqlFilter(DateType2sRadGrid, "DisplayFormatKeepDelimiter", "displayFormat.keepDelimiter"),
                Global.GetCqlFilter(DateType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(DateType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(DateType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(DateType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(DateType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            DateType2sRadGrid.DataSource = folioServiceContext.DateType2s(where, DateType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[DateType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(DateType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, DateType2sRadGrid.PageSize * DateType2sRadGrid.CurrentPageIndex, DateType2sRadGrid.PageSize, true);
            DateType2sRadGrid.VirtualItemCount = folioServiceContext.CountDateType2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"DateType2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tCode\tDisplayFormatDelimiter\tDisplayFormatKeepDelimiter\tSource\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "DisplayFormatDelimiter", "displayFormat.delimiter" }, { "DisplayFormatKeepDelimiter", "displayFormat.keepDelimiter" }, { "Source", "source" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(DateType2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(DateType2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(DateType2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(DateType2sRadGrid, "DisplayFormatDelimiter", "displayFormat.delimiter"),
                Global.GetCqlFilter(DateType2sRadGrid, "DisplayFormatKeepDelimiter", "displayFormat.keepDelimiter"),
                Global.GetCqlFilter(DateType2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(DateType2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(DateType2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(DateType2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(DateType2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var dt2 in folioServiceContext.DateType2s(where, DateType2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[DateType2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(DateType2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{dt2.Id}\t{Global.TextEncode(dt2.Name)}\t{Global.TextEncode(dt2.Code)}\t{Global.TextEncode(dt2.DisplayFormatDelimiter)}\t{dt2.DisplayFormatKeepDelimiter}\t{Global.TextEncode(dt2.Source)}\t{dt2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(dt2.CreationUser?.Username)}\t{dt2.CreationUserId}\t{dt2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(dt2.LastWriteUser?.Username)}\t{dt2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
