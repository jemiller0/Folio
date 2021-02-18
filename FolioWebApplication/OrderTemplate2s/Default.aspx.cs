using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.OrderTemplate2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["OrderTemplate2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void OrderTemplate2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "TemplateName", "templateName" }, { "TemplateCode", "templateCode" }, { "TemplateDescription", "templateDescription" } };
            OrderTemplate2sRadGrid.DataSource = folioServiceContext.OrderTemplate2s(out var i, Global.GetCqlFilter(OrderTemplate2sRadGrid, d), OrderTemplate2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderTemplate2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderTemplate2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderTemplate2sRadGrid.PageSize * OrderTemplate2sRadGrid.CurrentPageIndex, OrderTemplate2sRadGrid.PageSize, true);
            OrderTemplate2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"OrderTemplate2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "TemplateName", "templateName" }, { "TemplateCode", "templateCode" }, { "TemplateDescription", "templateDescription" } };
            Response.Write("Id\tTemplateName\tTemplateCode\tTemplateDescription\r\n");
            foreach (var ot2 in folioServiceContext.OrderTemplate2s(Global.GetCqlFilter(OrderTemplate2sRadGrid, d), OrderTemplate2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderTemplate2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderTemplate2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{ot2.Id}\t{Global.TextEncode(ot2.TemplateName)}\t{Global.TextEncode(ot2.TemplateCode)}\t{Global.TextEncode(ot2.TemplateDescription)}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
