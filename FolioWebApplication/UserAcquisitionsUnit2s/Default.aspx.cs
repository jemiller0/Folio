using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.UserAcquisitionsUnit2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserAcquisitionsUnit2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void UserAcquisitionsUnit2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "AcquisitionsUnitId", "acquisitionsUnitId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "AcquisitionsUnit.Name", "acquisitionsUnitId", "name", folioServiceContext.FolioServiceClient.AcquisitionsUnits),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            UserAcquisitionsUnit2sRadGrid.DataSource = folioServiceContext.UserAcquisitionsUnit2s(out var i, where, UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, UserAcquisitionsUnit2sRadGrid.PageSize * UserAcquisitionsUnit2sRadGrid.CurrentPageIndex, UserAcquisitionsUnit2sRadGrid.PageSize, true);
            UserAcquisitionsUnit2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"UserAcquisitionsUnit2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "AcquisitionsUnitId", "acquisitionsUnitId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tUser\tUserId\tAcquisitionsUnit\tAcquisitionsUnitId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var uau2 in folioServiceContext.UserAcquisitionsUnit2s(Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, d), UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{uau2.Id}\t{Global.TextEncode(uau2.User?.Username)}\t{uau2.UserId}\t{Global.TextEncode(uau2.AcquisitionsUnit?.Name)}\t{uau2.AcquisitionsUnitId}\t{uau2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(uau2.CreationUser?.Username)}\t{uau2.CreationUserId}\t{uau2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(uau2.LastWriteUser?.Username)}\t{uau2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
