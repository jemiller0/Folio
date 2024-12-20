using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Orientation = FolioLibrary.Orientation;

namespace FolioWebApplication.Settings
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SettingsPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void SettingsRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void SettingsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            SettingsRadGrid.DataSource = folioServiceContext.Settings(load: true).ToArray();
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"Settings.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tName\tOrientation\tFontFamily\tFontSize\tFontWeight\tEnabled\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var s in folioServiceContext.Settings(load: true))
                Response.Write($"{s.Id}\t{Global.TextEncode(s.Name)}\t{s.Orientation}\t{Global.TextEncode(s.FontFamily)}\t{s.FontSize}\t{s.FontWeight}\t{s.Enabled}\t{s.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(s.CreationUser?.Username)}\t{s.CreationUserId}\t{s.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(s.LastWriteUser?.Username)}\t{s.LastWriteUserId}\r\n");
            Response.End();
        }

        protected void SettingsRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                if (folioServiceContext.LocationSettings().Any(s => s.SettingsId == id)) throw new Exception("Setting cannot be deleted because it is being referenced by a location setting");
                folioServiceContext.DeleteSetting(id);
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
