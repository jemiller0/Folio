using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ExpenseClass2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ExpenseClass2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ExpenseClass2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "AccountNumberExtension", "externalAccountNumberExt" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "AccountNumberExtension", "externalAccountNumberExt"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ExpenseClass2sRadGrid.DataSource = folioServiceContext.ExpenseClass2s(out var i, where, ExpenseClass2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ExpenseClass2sRadGrid.PageSize * ExpenseClass2sRadGrid.CurrentPageIndex, ExpenseClass2sRadGrid.PageSize, true);
            ExpenseClass2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ExpenseClass2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tCode\tAccountNumberExtension\tName\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Code", "code" }, { "AccountNumberExtension", "externalAccountNumberExt" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "AccountNumberExtension", "externalAccountNumberExt"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ExpenseClass2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var ec2 in folioServiceContext.ExpenseClass2s(where, ExpenseClass2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ExpenseClass2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ec2.Id}\t{Global.TextEncode(ec2.Code)}\t{Global.TextEncode(ec2.AccountNumberExtension)}\t{Global.TextEncode(ec2.Name)}\t{ec2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ec2.CreationUser?.Username)}\t{ec2.CreationUserId}\t{ec2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ec2.LastWriteUser?.Username)}\t{ec2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
