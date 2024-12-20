using FolioLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.LocationSettings
{
    public partial class Edit : System.Web.UI.Page
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

        protected void LocationSettingFormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var ls = id == null && (string)Session["LocationSettingsPermission"] == "Edit" ? new LocationSetting { Enabled = true } : folioServiceContext.FindLocationSetting(id, true);
            if (ls == null) Response.Redirect("Default.aspx");
            LocationSettingFormView.DataSource = new[] { ls };
            Title = $"Location Setting {ls.Id}";
        }

        protected void LocationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Location2s(orderBy: "name").ToArray();
        }

        protected void SettingsRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Settings().OrderBy(s => s.Name).ToArray();
        }

        protected void LocationSettingFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)LocationSettingFormView.DataKey.Value;
            var ls = id != null ? folioServiceContext.FindLocationSetting(id) : new LocationSetting { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            ls.LocationId = (Guid?)Guid.Parse((string)e.NewValues["LocationId"]);
            ls.SettingsId = (Guid?)Guid.Parse((string)e.NewValues["SettingsId"]);
            ls.Enabled = (bool?)e.NewValues["Enabled"];
            ls.LastWriteTime = DateTime.Now;
            ls.LastWriteUserId = (Guid?)Session["UserId"];
            var vr = LocationSetting.ValidateLocationSetting(ls, new ValidationContext(folioServiceContext));
            if (vr != null)
            {
                var cv = (CustomValidator)LocationSettingFormView.FindControl("LocationSettingCustomValidator");
                cv.IsValid = false;
                cv.ErrorMessage = vr.ErrorMessage;
                e.Cancel = true;
                return;
            }
            if (id == null) folioServiceContext.Insert(ls); else folioServiceContext.Update(ls);
            if (id == null) Response.Redirect($"Edit.aspx?Id={ls.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void LocationSettingFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void LocationSettingFormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)LocationSettingFormView.DataKey.Value;
            try
            {
                folioServiceContext.DeleteLocationSetting(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)((FormView)sender).FindControl("DeleteCustomValidator");
                cv.IsValid = false;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
