using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.ManualBlockTemplate2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ManualBlockTemplate2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void ManualBlockTemplate2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "desc" }, { "BlockTemplateDescription", "blockTemplate.desc" }, { "BlockTemplatePatronMessage", "blockTemplate.patronMessage" }, { "BlockTemplateBorrowing", "blockTemplate.borrowing" }, { "BlockTemplateRenewals", "blockTemplate.renewals" }, { "BlockTemplateRequests", "blockTemplate.requests" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ManualBlockTemplate2sRadGrid.DataSource = folioServiceContext.ManualBlockTemplate2s(out var i, Global.GetCqlFilter(ManualBlockTemplate2sRadGrid, d), ManualBlockTemplate2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ManualBlockTemplate2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ManualBlockTemplate2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ManualBlockTemplate2sRadGrid.PageSize * ManualBlockTemplate2sRadGrid.CurrentPageIndex, ManualBlockTemplate2sRadGrid.PageSize, true);
            ManualBlockTemplate2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"ManualBlockTemplate2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Name", "name" }, { "Code", "code" }, { "Description", "desc" }, { "BlockTemplateDescription", "blockTemplate.desc" }, { "BlockTemplatePatronMessage", "blockTemplate.patronMessage" }, { "BlockTemplateBorrowing", "blockTemplate.borrowing" }, { "BlockTemplateRenewals", "blockTemplate.renewals" }, { "BlockTemplateRequests", "blockTemplate.requests" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tName\tCode\tDescription\tBlockTemplateDescription\tBlockTemplatePatronMessage\tBlockTemplateBorrowing\tBlockTemplateRenewals\tBlockTemplateRequests\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var mbt2 in folioServiceContext.ManualBlockTemplate2s(Global.GetCqlFilter(ManualBlockTemplate2sRadGrid, d), ManualBlockTemplate2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ManualBlockTemplate2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ManualBlockTemplate2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{mbt2.Id}\t{Global.TextEncode(mbt2.Name)}\t{Global.TextEncode(mbt2.Code)}\t{Global.TextEncode(mbt2.Description)}\t{Global.TextEncode(mbt2.BlockTemplateDescription)}\t{Global.TextEncode(mbt2.BlockTemplatePatronMessage)}\t{mbt2.BlockTemplateBorrowing}\t{mbt2.BlockTemplateRenewals}\t{mbt2.BlockTemplateRequests}\t{mbt2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(mbt2.CreationUser?.Username)}\t{mbt2.CreationUserId}\t{mbt2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(mbt2.LastWriteUser?.Username)}\t{mbt2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
