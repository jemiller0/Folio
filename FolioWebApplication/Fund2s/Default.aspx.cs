using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Fund2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Fund2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Fund2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Description", "description" }, { "AccountNumber", "externalAccountNo" }, { "FundStatus", "fundStatus" }, { "FundTypeId", "fundTypeId" }, { "LedgerId", "ledgerId" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Fund2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Fund2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Fund2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Fund2sRadGrid, "AccountNumber", "externalAccountNo"),
                Global.GetCqlFilter(Fund2sRadGrid, "FundStatus", "fundStatus"),
                Global.GetCqlFilter(Fund2sRadGrid, "FundType.Name", "fundTypeId", "name", folioServiceContext.FolioServiceClient.FundTypes),
                Global.GetCqlFilter(Fund2sRadGrid, "Ledger.Name", "ledgerId", "name", folioServiceContext.FolioServiceClient.Ledgers),
                Global.GetCqlFilter(Fund2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Fund2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Fund2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fund2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Fund2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Fund2sRadGrid.DataSource = folioServiceContext.Fund2s(out var i, where, Fund2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fund2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fund2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fund2sRadGrid.PageSize * Fund2sRadGrid.CurrentPageIndex, Fund2sRadGrid.PageSize, true);
            Fund2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Fund2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "Description", "description" }, { "AccountNumber", "externalAccountNo" }, { "FundStatus", "fundStatus" }, { "FundTypeId", "fundTypeId" }, { "LedgerId", "ledgerId" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tCode\tDescription\tAccountNumber\tFundStatus\tFundType\tFundTypeId\tLedger\tLedgerId\tName\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var f2 in folioServiceContext.Fund2s(Global.GetCqlFilter(Fund2sRadGrid, d), Fund2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fund2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fund2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{f2.Id}\t{Global.TextEncode(f2.Code)}\t{Global.TextEncode(f2.Description)}\t{Global.TextEncode(f2.AccountNumber)}\t{Global.TextEncode(f2.FundStatus)}\t{Global.TextEncode(f2.FundType?.Name)}\t{f2.FundTypeId}\t{Global.TextEncode(f2.Ledger?.Name)}\t{f2.LedgerId}\t{Global.TextEncode(f2.Name)}\t{f2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(f2.CreationUser?.Username)}\t{f2.CreationUserId}\t{f2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(f2.LastWriteUser?.Username)}\t{f2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
