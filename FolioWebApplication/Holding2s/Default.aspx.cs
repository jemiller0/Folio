using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Holding2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Holding2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Holding2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void Holding2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "SourceId", "sourceId" } };
            Holding2sRadGrid.DataSource = folioServiceContext.Holding2s(out var i, Global.GetCqlFilter(Holding2sRadGrid, d), Holding2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Holding2sRadGrid.PageSize * Holding2sRadGrid.CurrentPageIndex, Holding2sRadGrid.PageSize, true);
            Holding2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Holding2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "ShortId", "hrid" }, { "HoldingTypeId", "holdingsTypeId" }, { "InstanceId", "instanceId" }, { "LocationId", "permanentLocationId" }, { "TemporaryLocationId", "temporaryLocationId" }, { "CallNumberTypeId", "callNumberTypeId" }, { "CallNumberPrefix", "callNumberPrefix" }, { "CallNumber", "callNumber" }, { "CallNumberSuffix", "callNumberSuffix" }, { "ShelvingTitle", "shelvingTitle" }, { "AcquisitionFormat", "acquisitionFormat" }, { "AcquisitionMethod", "acquisitionMethod" }, { "ReceiptStatus", "receiptStatus" }, { "IllPolicyId", "illPolicyId" }, { "RetentionPolicy", "retentionPolicy" }, { "DigitizationPolicy", "digitizationPolicy" }, { "CopyNumber", "copyNumber" }, { "ItemCount", "numberOfItems" }, { "ReceivingHistoryDisplayType", "receivingHistory.displayType" }, { "DiscoverySuppress", "discoverySuppress" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "SourceId", "sourceId" } };
            Response.Write("Id\tShortId\tHoldingType\tHoldingTypeId\tInstance\tInstanceId\tLocation\tLocationId\tTemporaryLocation\tTemporaryLocationId\tCallNumberType\tCallNumberTypeId\tCallNumberPrefix\tCallNumber\tCallNumberSuffix\tShelvingTitle\tAcquisitionFormat\tAcquisitionMethod\tReceiptStatus\tIllPolicy\tIllPolicyId\tRetentionPolicy\tDigitizationPolicy\tCopyNumber\tItemCount\tReceivingHistoryDisplayType\tDiscoverySuppress\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\tSource\tSourceId\r\n");
            foreach (var h2 in folioServiceContext.Holding2s(Global.GetCqlFilter(Holding2sRadGrid, d), Holding2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Holding2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Holding2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{h2.Id}\t{h2.ShortId}\t{Global.TextEncode(h2.HoldingType?.Name)}\t{h2.HoldingTypeId}\t{Global.TextEncode(h2.Instance?.Title)}\t{h2.InstanceId}\t{Global.TextEncode(h2.Location?.Name)}\t{h2.LocationId}\t{Global.TextEncode(h2.TemporaryLocation?.Name)}\t{h2.TemporaryLocationId}\t{Global.TextEncode(h2.CallNumberType?.Name)}\t{h2.CallNumberTypeId}\t{Global.TextEncode(h2.CallNumberPrefix)}\t{Global.TextEncode(h2.CallNumber)}\t{Global.TextEncode(h2.CallNumberSuffix)}\t{Global.TextEncode(h2.ShelvingTitle)}\t{Global.TextEncode(h2.AcquisitionFormat)}\t{Global.TextEncode(h2.AcquisitionMethod)}\t{Global.TextEncode(h2.ReceiptStatus)}\t{Global.TextEncode(h2.IllPolicy?.Name)}\t{h2.IllPolicyId}\t{Global.TextEncode(h2.RetentionPolicy)}\t{Global.TextEncode(h2.DigitizationPolicy)}\t{Global.TextEncode(h2.CopyNumber)}\t{Global.TextEncode(h2.ItemCount)}\t{Global.TextEncode(h2.ReceivingHistoryDisplayType)}\t{h2.DiscoverySuppress}\t{h2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(h2.CreationUser?.Username)}\t{h2.CreationUserId}\t{h2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(h2.LastWriteUser?.Username)}\t{h2.LastWriteUserId}\t{Global.TextEncode(h2.Source?.Name)}\t{h2.SourceId}\r\n");
            Response.End();
        }

        protected void Holding2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                if (folioServiceContext.AnyFee2s($"holdingsRecordId == \"{id}\"")) throw new Exception("Holding cannot be deleted because it is being referenced by a fee");
                if (folioServiceContext.AnyItem2s($"holdingsRecordId == \"{id}\"")) throw new Exception("Holding cannot be deleted because it is being referenced by a item");
                folioServiceContext.DeleteHolding2(id);
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
