using FolioLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Permission2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Permission2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Permission2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var p2 = id == null && (string)Session["Permission2sPermission"] == "Edit" ? new Permission2 { Editable = true, Visible = true } : folioServiceContext.FindPermission2(id, true);
            if (p2 == null) Response.Redirect("Default.aspx");
            Permission2FormView.DataSource = new[] { p2 };
            Title = $"Permission {p2.Name}";
        }

        protected void Permission2FormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)Permission2FormView.DataKey.Value;
            var p2 = id != null ? folioServiceContext.FindPermission2(id) : new Permission2 { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            p2.Code = Global.Trim((string)e.NewValues["Code"]);
            p2.Name = Global.Trim((string)e.NewValues["Name"]);
            p2.Description = Global.Trim((string)e.NewValues["Description"]);
            p2.Editable = (bool?)e.NewValues["Editable"];
            p2.Visible = (bool?)e.NewValues["Visible"];
            p2.Dummy = (bool?)e.NewValues["Dummy"];
            p2.LastWriteTime = DateTime.Now;
            p2.LastWriteUserId = (Guid?)Session["UserId"];
            var vr = Permission2.ValidatePermission2(p2, new ValidationContext(folioServiceContext));
            if (vr != null)
            {
                var cv = (CustomValidator)Permission2FormView.FindControl("Permission2CustomValidator");
                cv.IsValid = false;
                cv.ErrorMessage = vr.ErrorMessage;
                e.Cancel = true;
                return;
            }
            if (id == null) folioServiceContext.Insert(p2); else folioServiceContext.Update(p2);
            if (id == null) Response.Redirect($"Edit.aspx?Id={p2.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void Permission2FormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void Permission2FormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)Permission2FormView.DataKey.Value;
            try
            {
                folioServiceContext.DeletePermission2(id);
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
