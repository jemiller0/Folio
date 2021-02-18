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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "ItemAgedLostOverdueDuration", "itemAgedLostOverdue.duration" }, { "ItemAgedLostOverdueInterval", "itemAgedLostOverdue.intervalId" }, { "PatronBilledAfterAgedLostDuration", "patronBilledAfterAgedLost.duration" }, { "PatronBilledAfterAgedLostInterval", "patronBilledAfterAgedLost.intervalId" }, { "ChargeAmountItemChargeType", "chargeAmountItem.chargeType" }, { "ChargeAmountItemAmount", "chargeAmountItem.amount" }, { "LostItemProcessingFee", "lostItemProcessingFee" }, { "ChargeAmountItemPatron", "chargeAmountItemPatron" }, { "ChargeAmountItemSystem", "chargeAmountItemSystem" }, { "LostItemChargeFeeFineDuration", "lostItemChargeFeeFine.duration" }, { "LostItemChargeFeeFineInterval", "lostItemChargeFeeFine.intervalId" }, { "ReturnedLostItemProcessingFee", "returnedLostItemProcessingFee" }, { "ReplacedLostItemProcessingFee", "replacedLostItemProcessingFee" }, { "ReplacementProcessingFee", "replacementProcessingFee" }, { "ReplacementAllowed", "replacementAllowed" }, { "LostItemReturned", "lostItemReturned" }, { "FeesFinesShallRefundedDuration", "feesFinesShallRefunded.duration" }, { "FeesFinesShallRefundedInterval", "feesFinesShallRefunded.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            LostItemFeePolicy2sRadGrid.DataSource = folioServiceContext.LostItemFeePolicy2s(out var i, Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, d), LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, LostItemFeePolicy2sRadGrid.PageSize * LostItemFeePolicy2sRadGrid.CurrentPageIndex, LostItemFeePolicy2sRadGrid.PageSize, true);
            LostItemFeePolicy2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LostItemFeePolicy2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Description", "description" }, { "ItemAgedLostOverdueDuration", "itemAgedLostOverdue.duration" }, { "ItemAgedLostOverdueInterval", "itemAgedLostOverdue.intervalId" }, { "PatronBilledAfterAgedLostDuration", "patronBilledAfterAgedLost.duration" }, { "PatronBilledAfterAgedLostInterval", "patronBilledAfterAgedLost.intervalId" }, { "ChargeAmountItemChargeType", "chargeAmountItem.chargeType" }, { "ChargeAmountItemAmount", "chargeAmountItem.amount" }, { "LostItemProcessingFee", "lostItemProcessingFee" }, { "ChargeAmountItemPatron", "chargeAmountItemPatron" }, { "ChargeAmountItemSystem", "chargeAmountItemSystem" }, { "LostItemChargeFeeFineDuration", "lostItemChargeFeeFine.duration" }, { "LostItemChargeFeeFineInterval", "lostItemChargeFeeFine.intervalId" }, { "ReturnedLostItemProcessingFee", "returnedLostItemProcessingFee" }, { "ReplacedLostItemProcessingFee", "replacedLostItemProcessingFee" }, { "ReplacementProcessingFee", "replacementProcessingFee" }, { "ReplacementAllowed", "replacementAllowed" }, { "LostItemReturned", "lostItemReturned" }, { "FeesFinesShallRefundedDuration", "feesFinesShallRefunded.duration" }, { "FeesFinesShallRefundedInterval", "feesFinesShallRefunded.intervalId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tDescription\tItemAgedLostOverdueDuration\tItemAgedLostOverdueInterval\tPatronBilledAfterAgedLostDuration\tPatronBilledAfterAgedLostInterval\tChargeAmountItemChargeType\tChargeAmountItemAmount\tLostItemProcessingFee\tChargeAmountItemPatron\tChargeAmountItemSystem\tLostItemChargeFeeFineDuration\tLostItemChargeFeeFineInterval\tReturnedLostItemProcessingFee\tReplacedLostItemProcessingFee\tReplacementProcessingFee\tReplacementAllowed\tLostItemReturned\tFeesFinesShallRefundedDuration\tFeesFinesShallRefundedInterval\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var lifp2 in folioServiceContext.LostItemFeePolicy2s(Global.GetCqlFilter(LostItemFeePolicy2sRadGrid, d), LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(LostItemFeePolicy2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{lifp2.Id}\t{Global.TextEncode(lifp2.Name)}\t{Global.TextEncode(lifp2.Description)}\t{lifp2.ItemAgedLostOverdueDuration}\t{Global.TextEncode(lifp2.ItemAgedLostOverdueInterval)}\t{lifp2.PatronBilledAfterAgedLostDuration}\t{Global.TextEncode(lifp2.PatronBilledAfterAgedLostInterval)}\t{Global.TextEncode(lifp2.ChargeAmountItemChargeType)}\t{lifp2.ChargeAmountItemAmount}\t{lifp2.LostItemProcessingFee}\t{lifp2.ChargeAmountItemPatron}\t{lifp2.ChargeAmountItemSystem}\t{lifp2.LostItemChargeFeeFineDuration}\t{Global.TextEncode(lifp2.LostItemChargeFeeFineInterval)}\t{lifp2.ReturnedLostItemProcessingFee}\t{lifp2.ReplacedLostItemProcessingFee}\t{lifp2.ReplacementProcessingFee}\t{lifp2.ReplacementAllowed}\t{Global.TextEncode(lifp2.LostItemReturned)}\t{lifp2.FeesFinesShallRefundedDuration}\t{Global.TextEncode(lifp2.FeesFinesShallRefundedInterval)}\t{lifp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lifp2.CreationUser?.Username)}\t{lifp2.CreationUserId}\t{lifp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(lifp2.LastWriteUser?.Username)}\t{lifp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
