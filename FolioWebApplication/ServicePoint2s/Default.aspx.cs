using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ServicePoint2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ServicePoint2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ServicePoint2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "Description", "description" }, { "ShelvingLagTime", "shelvingLagTime" }, { "PickupLocation", "pickupLocation" }, { "HoldShelfExpiryPeriodDuration", "holdShelfExpiryPeriod.duration" }, { "HoldShelfExpiryPeriodInterval", "holdShelfExpiryPeriod.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ServicePoint2sRadGrid.DataSource = folioServiceContext.ServicePoint2s(out var i, Global.GetCqlFilter(ServicePoint2sRadGrid, d), ServicePoint2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ServicePoint2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ServicePoint2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ServicePoint2sRadGrid.PageSize * ServicePoint2sRadGrid.CurrentPageIndex, ServicePoint2sRadGrid.PageSize, true);
            ServicePoint2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ServicePoint2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "Description", "description" }, { "ShelvingLagTime", "shelvingLagTime" }, { "PickupLocation", "pickupLocation" }, { "HoldShelfExpiryPeriodDuration", "holdShelfExpiryPeriod.duration" }, { "HoldShelfExpiryPeriodInterval", "holdShelfExpiryPeriod.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tDiscoveryDisplayName\tDescription\tShelvingLagTime\tPickupLocation\tHoldShelfExpiryPeriodDuration\tHoldShelfExpiryPeriodInterval\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var sp2 in folioServiceContext.ServicePoint2s(Global.GetCqlFilter(ServicePoint2sRadGrid, d), ServicePoint2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ServicePoint2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ServicePoint2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{sp2.Id}\t{Global.TextEncode(sp2.Name)}\t{Global.TextEncode(sp2.Code)}\t{Global.TextEncode(sp2.DiscoveryDisplayName)}\t{Global.TextEncode(sp2.Description)}\t{sp2.ShelvingLagTime}\t{sp2.PickupLocation}\t{sp2.HoldShelfExpiryPeriodDuration}\t{Global.TextEncode(sp2.HoldShelfExpiryPeriodInterval)}\t{sp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sp2.CreationUser?.Username)}\t{sp2.CreationUserId}\t{sp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(sp2.LastWriteUser?.Username)}\t{sp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
