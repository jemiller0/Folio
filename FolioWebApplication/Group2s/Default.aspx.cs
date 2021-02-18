using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Group2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Group2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Group2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void Group2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "group" }, { "Description", "desc" }, { "ExpirationOffsetInDays", "expirationOffsetInDays" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Group2sRadGrid.DataSource = folioServiceContext.Group2s(out var i, Global.GetCqlFilter(Group2sRadGrid, d), Group2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Group2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Group2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Group2sRadGrid.PageSize * Group2sRadGrid.CurrentPageIndex, Group2sRadGrid.PageSize, true);
            Group2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Group2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "group" }, { "Description", "desc" }, { "ExpirationOffsetInDays", "expirationOffsetInDays" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tExpirationOffsetInDays\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var g2 in folioServiceContext.Group2s(Global.GetCqlFilter(Group2sRadGrid, d), Group2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Group2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Group2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{g2.Id}\t{Global.TextEncode(g2.Name)}\t{Global.TextEncode(g2.Description)}\t{g2.ExpirationOffsetInDays}\t{g2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(g2.CreationUser?.Username)}\t{g2.CreationUserId}\t{g2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(g2.LastWriteUser?.Username)}\t{g2.LastWriteUserId}\r\n");
            Response.End();
        }

        protected void Group2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                if (folioServiceContext.AnyBlockLimit2s($"patronGroupId == \"{id}\"")) throw new Exception("Group cannot be deleted because it is being referenced by a block limit");
                if (folioServiceContext.AnyLoan2s($"patronGroupIdAtCheckout == \"{id}\"")) throw new Exception("Group cannot be deleted because it is being referenced by a loan");
                if (folioServiceContext.AnyUser2s($"patronGroup == \"{id}\"")) throw new Exception("Group cannot be deleted because it is being referenced by a user");
                folioServiceContext.DeleteGroup2(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)gei.FindControl("DeleteCustomValidator");
                cv.IsValid = false;
                e.Canceled = true;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
