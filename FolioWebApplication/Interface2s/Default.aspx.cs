using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Interface2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Interface2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Interface2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Uri", "uri" }, { "Notes", "notes" }, { "Available", "available" }, { "DeliveryMethod", "deliveryMethod" }, { "StatisticsFormat", "statisticsFormat" }, { "LocallyStored", "locallyStored" }, { "OnlineLocation", "onlineLocation" }, { "StatisticsNotes", "statisticsNotes" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Interface2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Interface2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Interface2sRadGrid, "Uri", "uri"),
                Global.GetCqlFilter(Interface2sRadGrid, "Notes", "notes"),
                Global.GetCqlFilter(Interface2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(Interface2sRadGrid, "DeliveryMethod", "deliveryMethod"),
                Global.GetCqlFilter(Interface2sRadGrid, "StatisticsFormat", "statisticsFormat"),
                Global.GetCqlFilter(Interface2sRadGrid, "LocallyStored", "locallyStored"),
                Global.GetCqlFilter(Interface2sRadGrid, "OnlineLocation", "onlineLocation"),
                Global.GetCqlFilter(Interface2sRadGrid, "StatisticsNotes", "statisticsNotes"),
                Global.GetCqlFilter(Interface2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Interface2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Interface2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Interface2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Interface2sRadGrid.DataSource = folioServiceContext.Interface2s(where, Interface2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Interface2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Interface2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Interface2sRadGrid.PageSize * Interface2sRadGrid.CurrentPageIndex, Interface2sRadGrid.PageSize, true);
            Interface2sRadGrid.VirtualItemCount = folioServiceContext.CountInterface2s(where);
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Interface2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tUri\tNotes\tAvailable\tDeliveryMethod\tStatisticsFormat\tLocallyStored\tOnlineLocation\tStatisticsNotes\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Uri", "uri" }, { "Notes", "notes" }, { "Available", "available" }, { "DeliveryMethod", "deliveryMethod" }, { "StatisticsFormat", "statisticsFormat" }, { "LocallyStored", "locallyStored" }, { "OnlineLocation", "onlineLocation" }, { "StatisticsNotes", "statisticsNotes" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Interface2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Interface2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Interface2sRadGrid, "Uri", "uri"),
                Global.GetCqlFilter(Interface2sRadGrid, "Notes", "notes"),
                Global.GetCqlFilter(Interface2sRadGrid, "Available", "available"),
                Global.GetCqlFilter(Interface2sRadGrid, "DeliveryMethod", "deliveryMethod"),
                Global.GetCqlFilter(Interface2sRadGrid, "StatisticsFormat", "statisticsFormat"),
                Global.GetCqlFilter(Interface2sRadGrid, "LocallyStored", "locallyStored"),
                Global.GetCqlFilter(Interface2sRadGrid, "OnlineLocation", "onlineLocation"),
                Global.GetCqlFilter(Interface2sRadGrid, "StatisticsNotes", "statisticsNotes"),
                Global.GetCqlFilter(Interface2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Interface2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Interface2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Interface2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var i2 in folioServiceContext.Interface2s(where, Interface2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Interface2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Interface2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{i2.Id}\t{Global.TextEncode(i2.Name)}\t{Global.TextEncode(i2.Uri)}\t{Global.TextEncode(i2.Notes)}\t{i2.Available}\t{Global.TextEncode(i2.DeliveryMethod)}\t{Global.TextEncode(i2.StatisticsFormat)}\t{Global.TextEncode(i2.LocallyStored)}\t{Global.TextEncode(i2.OnlineLocation)}\t{Global.TextEncode(i2.StatisticsNotes)}\t{i2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.CreationUser?.Username)}\t{i2.CreationUserId}\t{i2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.LastWriteUser?.Username)}\t{i2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
