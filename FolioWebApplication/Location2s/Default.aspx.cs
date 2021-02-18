using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Location2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Location2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Location2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Location2sRadGrid.DataSource = folioServiceContext.Location2s(out var i, Global.GetCqlFilter(Location2sRadGrid, d), Location2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Location2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Location2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Location2sRadGrid.PageSize * Location2sRadGrid.CurrentPageIndex, Location2sRadGrid.PageSize, true);
            Location2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Location2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tDescription\tDiscoveryDisplayName\tIsActive\tInstitution\tInstitutionId\tCampus\tCampusId\tLibrary\tLibraryId\tPrimaryServicePoint\tPrimaryServicePointId\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var l2 in folioServiceContext.Location2s(Global.GetCqlFilter(Location2sRadGrid, d), Location2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Location2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Location2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{l2.Id}\t{Global.TextEncode(l2.Name)}\t{Global.TextEncode(l2.Code)}\t{Global.TextEncode(l2.Description)}\t{Global.TextEncode(l2.DiscoveryDisplayName)}\t{l2.IsActive}\t{Global.TextEncode(l2.Institution?.Name)}\t{l2.InstitutionId}\t{Global.TextEncode(l2.Campus?.Name)}\t{l2.CampusId}\t{Global.TextEncode(l2.Library?.Name)}\t{l2.LibraryId}\t{Global.TextEncode(l2.PrimaryServicePoint?.Name)}\t{l2.PrimaryServicePointId}\t{l2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.CreationUser?.Username)}\t{l2.CreationUserId}\t{l2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.LastWriteUser?.Username)}\t{l2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
