using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.LocationSettings
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LocationSettingsPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void LocationSettingsRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void LocationSettingsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            LocationSettingsRadGrid.DataSource = folioServiceContext.LocationSettings(load: true).ToArray();
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"LocationSettings.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tLocation\tLocationId\tSettings\tSettingsId\tEnabled\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var ls in folioServiceContext.LocationSettings(load: true))
                Response.Write($"{ls.Id}\t{Global.TextEncode(ls.Location?.Name)}\t{ls.LocationId}\t{Global.TextEncode(ls.Settings?.Name)}\t{ls.SettingsId}\t{ls.Enabled}\t{ls.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ls.CreationUser?.Username)}\t{ls.CreationUserId}\t{ls.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(ls.LastWriteUser?.Username)}\t{ls.LastWriteUserId}\r\n");
            Response.End();
        }

        protected void LocationSettingsRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                folioServiceContext.DeleteLocationSetting(id);
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
