using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.LostItemFeePolicy2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LostItemFeePolicy2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void LostItemFeePolicy2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "ItemAgedLostOverdueDuration", "itemAgedLostOverdue.duration" }, { "ItemAgedLostOverdueInterval", "itemAgedLostOverdue.intervalId" }, { "PatronBilledAfterAgedLostDuration", "patronBilledAfterAgedLost.duration" }, { "PatronBilledAfterAgedLostInterval", "patronBilledAfterAgedLost.intervalId" }, { "RecalledItemAgedLostOverdueDuration", "recalledItemAgedLostOverdue.duration" }, { "RecalledItemAgedLostOverdueInterval", "recalledItemAgedLostOverdue.intervalId" }, { "PatronBilledAfterRecalledItemAgedLostDuration", "patronBilledAfterRecalledItemAgedLost.duration" }, { "PatronBilledAfterRecalledItemAgedLostInterval", "patronBilledAfterRecalledItemAgedLost.intervalId" }, { "ChargeAmountItemChargeType", "chargeAmountItem.chargeType" }, { "ChargeAmountItemAmount", "chargeAmountItem.amount" }, { "LostItemProcessingFee", "lostItemProcessingFee" }, { "ChargeAmountItemPatron", "chargeAmountItemPatron" }, { "ChargeAmountItemSystem", "chargeAmountItemSystem" }, { "LostItemChargeFeeFineDuration", "lostItemChargeFeeFine.duration" }, { "LostItemChargeFeeFineInterval", "lostItemChargeFeeFine.intervalId" }, { "ReturnedLostItemProcessingFee", "returnedLostItemProcessingFee" }, { "ReplacedLostItemProcessingFee", "replacedLostItemProcessingFee" }, { "ReplacementProcessingFee", "replacementProcessingFee" }, { "ReplacementAllowed", "replacementAllowed" }, { "LostItemReturned", "lostItemReturned" }, { "FeesFinesShallRefundedDuration", "feesFinesShallRefunded.duration" }, { "FeesFinesShallRefundedInterval", "feesFinesShallRefunded.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ItemAgedLostOverdueDuration", "itemAgedLostOverdue.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ItemAgedLostOverdueInterval", "itemAgedLostOverdue.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "PatronBilledAfterAgedLostDuration", "patronBilledAfterAgedLost.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "PatronBilledAfterAgedLostInterval", "patronBilledAfterAgedLost.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "RecalledItemAgedLostOverdueDuration", "recalledItemAgedLostOverdue.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "RecalledItemAgedLostOverdueInterval", "recalledItemAgedLostOverdue.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "PatronBilledAfterRecalledItemAgedLostDuration", "patronBilledAfterRecalledItemAgedLost.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "PatronBilledAfterRecalledItemAgedLostInterval", "patronBilledAfterRecalledItemAgedLost.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ChargeAmountItemChargeType", "chargeAmountItem.chargeType"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ChargeAmountItemAmount", "chargeAmountItem.amount"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LostItemProcessingFee", "lostItemProcessingFee"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ChargeAmountItemPatron", "chargeAmountItemPatron"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ChargeAmountItemSystem", "chargeAmountItemSystem"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LostItemChargeFeeFineDuration", "lostItemChargeFeeFine.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LostItemChargeFeeFineInterval", "lostItemChargeFeeFine.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ReturnedLostItemProcessingFee", "returnedLostItemProcessingFee"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ReplacedLostItemProcessingFee", "replacedLostItemProcessingFee"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ReplacementProcessingFee", "replacementProcessingFee"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ReplacementAllowed", "replacementAllowed"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LostItemReturned", "lostItemReturned"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "FeesFinesShallRefundedDuration", "feesFinesShallRefunded.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "FeesFinesShallRefundedInterval", "feesFinesShallRefunded.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            LostItemFeePolicy2sRadGrid.DataSource = folioServiceContext.LostItemFeePolicy2s(out var i, where, LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LostItemFeePolicy2sRadGrid.PageSize * LostItemFeePolicy2sRadGrid.CurrentPageIndex, LostItemFeePolicy2sRadGrid.PageSize, true);
            LostItemFeePolicy2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LostItemFeePolicy2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tDescription\tItemAgedLostOverdueDuration\tItemAgedLostOverdueInterval\tPatronBilledAfterAgedLostDuration\tPatronBilledAfterAgedLostInterval\tRecalledItemAgedLostOverdueDuration\tRecalledItemAgedLostOverdueInterval\tPatronBilledAfterRecalledItemAgedLostDuration\tPatronBilledAfterRecalledItemAgedLostInterval\tChargeAmountItemChargeType\tChargeAmountItemAmount\tLostItemProcessingFee\tChargeAmountItemPatron\tChargeAmountItemSystem\tLostItemChargeFeeFineDuration\tLostItemChargeFeeFineInterval\tReturnedLostItemProcessingFee\tReplacedLostItemProcessingFee\tReplacementProcessingFee\tReplacementAllowed\tLostItemReturned\tFeesFinesShallRefundedDuration\tFeesFinesShallRefundedInterval\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "ItemAgedLostOverdueDuration", "itemAgedLostOverdue.duration" }, { "ItemAgedLostOverdueInterval", "itemAgedLostOverdue.intervalId" }, { "PatronBilledAfterAgedLostDuration", "patronBilledAfterAgedLost.duration" }, { "PatronBilledAfterAgedLostInterval", "patronBilledAfterAgedLost.intervalId" }, { "RecalledItemAgedLostOverdueDuration", "recalledItemAgedLostOverdue.duration" }, { "RecalledItemAgedLostOverdueInterval", "recalledItemAgedLostOverdue.intervalId" }, { "PatronBilledAfterRecalledItemAgedLostDuration", "patronBilledAfterRecalledItemAgedLost.duration" }, { "PatronBilledAfterRecalledItemAgedLostInterval", "patronBilledAfterRecalledItemAgedLost.intervalId" }, { "ChargeAmountItemChargeType", "chargeAmountItem.chargeType" }, { "ChargeAmountItemAmount", "chargeAmountItem.amount" }, { "LostItemProcessingFee", "lostItemProcessingFee" }, { "ChargeAmountItemPatron", "chargeAmountItemPatron" }, { "ChargeAmountItemSystem", "chargeAmountItemSystem" }, { "LostItemChargeFeeFineDuration", "lostItemChargeFeeFine.duration" }, { "LostItemChargeFeeFineInterval", "lostItemChargeFeeFine.intervalId" }, { "ReturnedLostItemProcessingFee", "returnedLostItemProcessingFee" }, { "ReplacedLostItemProcessingFee", "replacedLostItemProcessingFee" }, { "ReplacementProcessingFee", "replacementProcessingFee" }, { "ReplacementAllowed", "replacementAllowed" }, { "LostItemReturned", "lostItemReturned" }, { "FeesFinesShallRefundedDuration", "feesFinesShallRefunded.duration" }, { "FeesFinesShallRefundedInterval", "feesFinesShallRefunded.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "Name", "name"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "Description", "description"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ItemAgedLostOverdueDuration", "itemAgedLostOverdue.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ItemAgedLostOverdueInterval", "itemAgedLostOverdue.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "PatronBilledAfterAgedLostDuration", "patronBilledAfterAgedLost.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "PatronBilledAfterAgedLostInterval", "patronBilledAfterAgedLost.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "RecalledItemAgedLostOverdueDuration", "recalledItemAgedLostOverdue.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "RecalledItemAgedLostOverdueInterval", "recalledItemAgedLostOverdue.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "PatronBilledAfterRecalledItemAgedLostDuration", "patronBilledAfterRecalledItemAgedLost.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "PatronBilledAfterRecalledItemAgedLostInterval", "patronBilledAfterRecalledItemAgedLost.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ChargeAmountItemChargeType", "chargeAmountItem.chargeType"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ChargeAmountItemAmount", "chargeAmountItem.amount"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LostItemProcessingFee", "lostItemProcessingFee"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ChargeAmountItemPatron", "chargeAmountItemPatron"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ChargeAmountItemSystem", "chargeAmountItemSystem"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LostItemChargeFeeFineDuration", "lostItemChargeFeeFine.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LostItemChargeFeeFineInterval", "lostItemChargeFeeFine.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ReturnedLostItemProcessingFee", "returnedLostItemProcessingFee"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ReplacedLostItemProcessingFee", "replacedLostItemProcessingFee"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ReplacementProcessingFee", "replacementProcessingFee"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "ReplacementAllowed", "replacementAllowed"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LostItemReturned", "lostItemReturned"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "FeesFinesShallRefundedDuration", "feesFinesShallRefunded.duration"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "FeesFinesShallRefundedInterval", "feesFinesShallRefunded.intervalId"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var lifp2 in folioServiceContext.LostItemFeePolicy2s(where, LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{lifp2.Id}\t{Global.TextEncode(lifp2.Name)}\t{Global.TextEncode(lifp2.Description)}\t{lifp2.ItemAgedLostOverdueDuration}\t{Global.TextEncode(lifp2.ItemAgedLostOverdueInterval)}\t{lifp2.PatronBilledAfterAgedLostDuration}\t{Global.TextEncode(lifp2.PatronBilledAfterAgedLostInterval)}\t{lifp2.RecalledItemAgedLostOverdueDuration}\t{Global.TextEncode(lifp2.RecalledItemAgedLostOverdueInterval)}\t{lifp2.PatronBilledAfterRecalledItemAgedLostDuration}\t{Global.TextEncode(lifp2.PatronBilledAfterRecalledItemAgedLostInterval)}\t{Global.TextEncode(lifp2.ChargeAmountItemChargeType)}\t{lifp2.ChargeAmountItemAmount}\t{lifp2.LostItemProcessingFee}\t{lifp2.ChargeAmountItemPatron}\t{lifp2.ChargeAmountItemSystem}\t{lifp2.LostItemChargeFeeFineDuration}\t{Global.TextEncode(lifp2.LostItemChargeFeeFineInterval)}\t{lifp2.ReturnedLostItemProcessingFee}\t{lifp2.ReplacedLostItemProcessingFee}\t{lifp2.ReplacementProcessingFee}\t{lifp2.ReplacementAllowed}\t{Global.TextEncode(lifp2.LostItemReturned)}\t{lifp2.FeesFinesShallRefundedDuration}\t{Global.TextEncode(lifp2.FeesFinesShallRefundedInterval)}\t{lifp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lifp2.CreationUser?.Username)}\t{lifp2.CreationUserId}\t{lifp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lifp2.LastWriteUser?.Username)}\t{lifp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
