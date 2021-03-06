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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            Interface2sRadGrid.DataSource = folioServiceContext.Interface2s(out var i, Global.GetCqlFilter(Interface2sRadGrid, d), Interface2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Interface2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Interface2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Interface2sRadGrid.PageSize * Interface2sRadGrid.CurrentPageIndex, Interface2sRadGrid.PageSize, true);
            Interface2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Interface2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Uri", "uri" }, { "Notes", "notes" }, { "Available", "available" }, { "DeliveryMethod", "deliveryMethod" }, { "StatisticsFormat", "statisticsFormat" }, { "LocallyStored", "locallyStored" }, { "OnlineLocation", "onlineLocation" }, { "StatisticsNotes", "statisticsNotes" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tUri\tNotes\tAvailable\tDeliveryMethod\tStatisticsFormat\tLocallyStored\tOnlineLocation\tStatisticsNotes\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var i2 in folioServiceContext.Interface2s(Global.GetCqlFilter(Interface2sRadGrid, d), Interface2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Interface2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Interface2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
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
