using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.OverdueFinePolicy2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OverdueFinePolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void OverdueFinePolicy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "OverdueFineQuantity", "overdueFine.quantity" }, { "OverdueFineInterval", "overdueFine.intervalId" }, { "CountClosed", "countClosed" }, { "MaxOverdueFine", "maxOverdueFine" }, { "ForgiveOverdueFine", "forgiveOverdueFine" }, { "OverdueRecallFineQuantity", "overdueRecallFine.quantity" }, { "OverdueRecallFineInterval", "overdueRecallFine.intervalId" }, { "GracePeriodRecall", "gracePeriodRecall" }, { "MaxOverdueRecallFine", "maxOverdueRecallFine" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            OverdueFinePolicy2sRadGrid.DataSource = folioServiceContext.OverdueFinePolicy2s(out var i, Global.GetCqlFilter(OverdueFinePolicy2sRadGrid, d), OverdueFinePolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OverdueFinePolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OverdueFinePolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OverdueFinePolicy2sRadGrid.PageSize * OverdueFinePolicy2sRadGrid.CurrentPageIndex, OverdueFinePolicy2sRadGrid.PageSize, true);
            OverdueFinePolicy2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"OverdueFinePolicy2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "OverdueFineQuantity", "overdueFine.quantity" }, { "OverdueFineInterval", "overdueFine.intervalId" }, { "CountClosed", "countClosed" }, { "MaxOverdueFine", "maxOverdueFine" }, { "ForgiveOverdueFine", "forgiveOverdueFine" }, { "OverdueRecallFineQuantity", "overdueRecallFine.quantity" }, { "OverdueRecallFineInterval", "overdueRecallFine.intervalId" }, { "GracePeriodRecall", "gracePeriodRecall" }, { "MaxOverdueRecallFine", "maxOverdueRecallFine" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tOverdueFineQuantity\tOverdueFineInterval\tCountClosed\tMaxOverdueFine\tForgiveOverdueFine\tOverdueRecallFineQuantity\tOverdueRecallFineInterval\tGracePeriodRecall\tMaxOverdueRecallFine\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ofp2 in folioServiceContext.OverdueFinePolicy2s(Global.GetCqlFilter(OverdueFinePolicy2sRadGrid, d), OverdueFinePolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OverdueFinePolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OverdueFinePolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ofp2.Id}\t{Global.TextEncode(ofp2.Name)}\t{Global.TextEncode(ofp2.Description)}\t{ofp2.OverdueFineQuantity}\t{Global.TextEncode(ofp2.OverdueFineInterval)}\t{ofp2.CountClosed}\t{ofp2.MaxOverdueFine}\t{ofp2.ForgiveOverdueFine}\t{ofp2.OverdueRecallFineQuantity}\t{Global.TextEncode(ofp2.OverdueRecallFineInterval)}\t{ofp2.GracePeriodRecall}\t{ofp2.MaxOverdueRecallFine}\t{ofp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ofp2.CreationUser?.Username)}\t{ofp2.CreationUserId}\t{ofp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ofp2.LastWriteUser?.Username)}\t{ofp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
