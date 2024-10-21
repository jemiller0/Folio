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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "IsFloatingCollection", "isFloatingCollection" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Location2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Location2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Location2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Location2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Location2sRadGrid, "DiscoveryDisplayName", "discoveryDisplayName"),
                Global.GetCqlFilter(Location2sRadGrid, "IsActive", "isActive"),
                Global.GetCqlFilter(Location2sRadGrid, "Institution.Name", "institutionId", "name", folioServiceContext.FolioServiceClient.Institutions),
                Global.GetCqlFilter(Location2sRadGrid, "Campus.Name", "campusId", "name", folioServiceContext.FolioServiceClient.Campuses),
                Global.GetCqlFilter(Location2sRadGrid, "Library.Name", "libraryId", "name", folioServiceContext.FolioServiceClient.Libraries),
                Global.GetCqlFilter(Location2sRadGrid, "PrimaryServicePoint.Name", "primaryServicePoint", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Location2sRadGrid, "IsFloatingCollection", "isFloatingCollection"),
                Global.GetCqlFilter(Location2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Location2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Location2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Location2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Location2sRadGrid.DataSource = folioServiceContext.Location2s(out var i, where, Location2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Location2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Location2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Location2sRadGrid.PageSize * Location2sRadGrid.CurrentPageIndex, Location2sRadGrid.PageSize, true);
            Location2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Location2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tCode\tDescription\tDiscoveryDisplayName\tIsActive\tInstitution\tInstitutionId\tCampus\tCampusId\tLibrary\tLibraryId\tPrimaryServicePoint\tPrimaryServicePointId\tIsFloatingCollection\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "description" }, { "DiscoveryDisplayName", "discoveryDisplayName" }, { "IsActive", "isActive" }, { "InstitutionId", "institutionId" }, { "CampusId", "campusId" }, { "LibraryId", "libraryId" }, { "PrimaryServicePointId", "primaryServicePoint" }, { "IsFloatingCollection", "isFloatingCollection" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(Location2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Location2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(Location2sRadGrid, "Code", "code"),
                Global.GetCqlFilter(Location2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(Location2sRadGrid, "DiscoveryDisplayName", "discoveryDisplayName"),
                Global.GetCqlFilter(Location2sRadGrid, "IsActive", "isActive"),
                Global.GetCqlFilter(Location2sRadGrid, "Institution.Name", "institutionId", "name", folioServiceContext.FolioServiceClient.Institutions),
                Global.GetCqlFilter(Location2sRadGrid, "Campus.Name", "campusId", "name", folioServiceContext.FolioServiceClient.Campuses),
                Global.GetCqlFilter(Location2sRadGrid, "Library.Name", "libraryId", "name", folioServiceContext.FolioServiceClient.Libraries),
                Global.GetCqlFilter(Location2sRadGrid, "PrimaryServicePoint.Name", "primaryServicePoint", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Location2sRadGrid, "IsFloatingCollection", "isFloatingCollection"),
                Global.GetCqlFilter(Location2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Location2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Location2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Location2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var l2 in folioServiceContext.Location2s(where, Location2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Location2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Location2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{l2.Id}\t{Global.TextEncode(l2.Name)}\t{Global.TextEncode(l2.Code)}\t{Global.TextEncode(l2.Description)}\t{Global.TextEncode(l2.DiscoveryDisplayName)}\t{l2.IsActive}\t{Global.TextEncode(l2.Institution?.Name)}\t{l2.InstitutionId}\t{Global.TextEncode(l2.Campus?.Name)}\t{l2.CampusId}\t{Global.TextEncode(l2.Library?.Name)}\t{l2.LibraryId}\t{Global.TextEncode(l2.PrimaryServicePoint?.Name)}\t{l2.PrimaryServicePointId}\t{l2.IsFloatingCollection}\t{l2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.CreationUser?.Username)}\t{l2.CreationUserId}\t{l2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(l2.LastWriteUser?.Username)}\t{l2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
