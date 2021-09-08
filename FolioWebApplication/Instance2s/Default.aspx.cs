using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Instance2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Instance2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Instance2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void Instance2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Version", "_version" }, { "ShortId", "hrid" }, { "MatchKey", "matchKey" }, { "Source", "source" }, { "Title", "title" }, { "Author", "contributors.0.name" }, { "PublicationYear", "publication.0.dateOfPublication" }, { "PublicationPeriodStart", "publicationPeriod.start" }, { "PublicationPeriodEnd", "publicationPeriod.end" }, { "InstanceTypeId", "instanceTypeId" }, { "IssuanceModeId", "modeOfIssuanceId" }, { "CatalogedDate", "catalogedDate" }, { "PreviouslyHeld", "previouslyHeld" }, { "StaffSuppress", "staffSuppress" }, { "DiscoverySuppress", "discoverySuppress" }, { "SourceRecordFormat", "sourceRecordFormat" }, { "StatusId", "statusId" }, { "StatusLastWriteTime", "statusUpdatedDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Instance2sRadGrid.DataSource = folioServiceContext.Instance2s(out var i, Global.GetCqlFilter(Instance2sRadGrid, d), Instance2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Instance2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Instance2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Instance2sRadGrid.PageSize * Instance2sRadGrid.CurrentPageIndex, Instance2sRadGrid.PageSize, true);
            Instance2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Instance2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Version", "_version" }, { "ShortId", "hrid" }, { "MatchKey", "matchKey" }, { "Source", "source" }, { "Title", "title" }, { "Author", "contributors.0.name" }, { "PublicationYear", "publication.0.dateOfPublication" }, { "PublicationPeriodStart", "publicationPeriod.start" }, { "PublicationPeriodEnd", "publicationPeriod.end" }, { "InstanceTypeId", "instanceTypeId" }, { "IssuanceModeId", "modeOfIssuanceId" }, { "CatalogedDate", "catalogedDate" }, { "PreviouslyHeld", "previouslyHeld" }, { "StaffSuppress", "staffSuppress" }, { "DiscoverySuppress", "discoverySuppress" }, { "SourceRecordFormat", "sourceRecordFormat" }, { "StatusId", "statusId" }, { "StatusLastWriteTime", "statusUpdatedDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tVersion\tShortId\tMatchKey\tSource\tTitle\tAuthor\tPublicationYear\tPublicationPeriodStart\tPublicationPeriodEnd\tInstanceType\tInstanceTypeId\tIssuanceMode\tIssuanceModeId\tCatalogedDate\tPreviouslyHeld\tStaffSuppress\tDiscoverySuppress\tSourceRecordFormat\tStatus\tStatusId\tStatusLastWriteTime\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var i2 in folioServiceContext.Instance2s(Global.GetCqlFilter(Instance2sRadGrid, d), Instance2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Instance2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Instance2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{i2.Id}\t{i2.Version}\t{i2.ShortId}\t{Global.TextEncode(i2.MatchKey)}\t{Global.TextEncode(i2.Source)}\t{Global.TextEncode(i2.Title)}\t{Global.TextEncode(i2.Author)}\t{Global.TextEncode(i2.PublicationYear)}\t{i2.PublicationPeriodStart}\t{i2.PublicationPeriodEnd}\t{Global.TextEncode(i2.InstanceType?.Name)}\t{i2.InstanceTypeId}\t{Global.TextEncode(i2.IssuanceMode?.Name)}\t{i2.IssuanceModeId}\t{i2.CatalogedDate:M/d/yyyy}\t{i2.PreviouslyHeld}\t{i2.StaffSuppress}\t{i2.DiscoverySuppress}\t{Global.TextEncode(i2.SourceRecordFormat)}\t{Global.TextEncode(i2.Status?.Name)}\t{i2.StatusId}\t{i2.StatusLastWriteTime:M/d/yyyy HH:mm:ss}\t{i2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.CreationUser?.Username)}\t{i2.CreationUserId}\t{i2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(i2.LastWriteUser?.Username)}\t{i2.LastWriteUserId}\r\n");
            Response.End();
        }

        protected void Instance2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                if (folioServiceContext.AnyFee2s($"instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a fee");
                if (folioServiceContext.AnyHolding2s($"instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a holding");
                if (folioServiceContext.AnyOrderItem2s($"instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a order item");
                if (folioServiceContext.AnyPrecedingSucceedingTitle2s($"precedingInstanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a preceding succeeding title");
                if (folioServiceContext.AnyPrecedingSucceedingTitle2s($"succeedingInstanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a preceding succeeding title");
                if (folioServiceContext.AnyRecord2s($"externalIdsHolder.instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a record");
                if (folioServiceContext.AnyRelationships($"subInstanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a relationship");
                if (folioServiceContext.AnyRelationships($"superInstanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a relationship");
                if (folioServiceContext.AnyTitle2s($"instanceId == \"{id}\"")) throw new Exception("Instance cannot be deleted because it is being referenced by a title");
                folioServiceContext.DeleteInstance2(id);
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
