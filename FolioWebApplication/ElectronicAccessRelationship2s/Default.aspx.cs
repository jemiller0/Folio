using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ElectronicAccessRelationship2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ElectronicAccessRelationship2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ElectronicAccessRelationship2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ElectronicAccessRelationship2sRadGrid.DataSource = folioServiceContext.ElectronicAccessRelationship2s(out var i, Global.GetCqlFilter(ElectronicAccessRelationship2sRadGrid, d), ElectronicAccessRelationship2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ElectronicAccessRelationship2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ElectronicAccessRelationship2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ElectronicAccessRelationship2sRadGrid.PageSize * ElectronicAccessRelationship2sRadGrid.CurrentPageIndex, ElectronicAccessRelationship2sRadGrid.PageSize, true);
            ElectronicAccessRelationship2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ElectronicAccessRelationship2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ear2 in folioServiceContext.ElectronicAccessRelationship2s(Global.GetCqlFilter(ElectronicAccessRelationship2sRadGrid, d), ElectronicAccessRelationship2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ElectronicAccessRelationship2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ElectronicAccessRelationship2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ear2.Id}\t{Global.TextEncode(ear2.Name)}\t{ear2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ear2.CreationUser?.Username)}\t{ear2.CreationUserId}\t{ear2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ear2.LastWriteUser?.Username)}\t{ear2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
