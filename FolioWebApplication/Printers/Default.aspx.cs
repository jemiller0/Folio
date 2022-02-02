using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Printers
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PrintersPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PrintersRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void PrintersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            PrintersRadGrid.DataSource = folioServiceContext.Printers(load: true).ToArray();
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Printers.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" } };
            Response.Write("Id\tComputerName\tName\tLeft\tTop\tWidth\tHeight\tEnabled\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var p in folioServiceContext.Printers(Global.GetCqlFilter(PrintersRadGrid, d), PrintersRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PrintersRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PrintersRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{p.Id}\t{Global.TextEncode(p.ComputerName)}\t{Global.TextEncode(p.Name)}\t{p.Left}\t{p.Top}\t{p.Width}\t{p.Height}\t{p.Enabled}\t{p.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(p.CreationUser?.Username)}\t{p.CreationUserId}\t{p.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(p.LastWriteUser?.Username)}\t{p.LastWriteUserId}\r\n");
            Response.End();
        }

        protected void PrintersRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                folioServiceContext.DeletePrinter(id);
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
